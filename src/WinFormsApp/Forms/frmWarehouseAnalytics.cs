namespace LTNet;
public partial class frmWarehouseAnalytics : Form
{
    private CancellationTokenSource? cancellation;
    private Task? running;
    private bool closeAfterCancel;
    internal bool SuppressNotifications { get; set; }
    internal Task? RunningTask => running;
    public frmWarehouseAnalytics()
    {
        InitializeComponent();
        Ui.Columns(dgvReportData, ("ProductId", "Mã hàng", ""), ("ProductName", "Tên hàng", ""), ("Quantity", "Tồn kho", "N0"), ("UnitPrice", "Đơn giá", "N0"), ("Value", "Giá trị", "N0"));
        cboWarehouse.SelectedIndex = 0;
        dtpFrom.Value = DateTime.Today.AddDays(-30);
        btnCancel.Enabled = false;
        btnRun.Click += Run_Click;
        btnCancel.Click += (_, _) => CancelReport();
        FormClosing += Closing_Async;
    }
    private async void Run_Click(object? sender, EventArgs e) => await StartReportAsync();
    internal Task StartReportAsync()
    {
        if (running is { IsCompleted: false }) return running;
        if (dtpFrom.Value.Date > dtpTo.Value.Date)
        {
            lblStatus.Text = "Khoảng thời gian không hợp lệ: ngày bắt đầu sau ngày kết thúc.";
            return Task.CompletedTask;
        }
        running = GenerateAsync();
        return running;
    }
    private async Task GenerateAsync()
    {
        using var cts = new CancellationTokenSource();
        cancellation = cts;
        btnRun.Enabled = false; btnCancel.Enabled = true;
        pgbStatus.Value = 0; lblPercent.Text = "0%";
        dgvReportData.DataSource = null;
        lblStatus.Text = "Đang tổng hợp dữ liệu kho... Bạn vẫn có thể di chuyển và thu phóng cửa sổ.";
        var progress = new Progress<int>(value =>
        {
            if (IsDisposed || cts.IsCancellationRequested || cancellation != cts) return;
            pgbStatus.Value = value; lblPercent.Text = $"{value}%";
        });
        try
        {
            var report = await WarehouseService.GenerateAsync(cboWarehouse.Text, dtpFrom.Value, dtpTo.Value, progress, cts.Token);
            cts.Token.ThrowIfCancellationRequested();
            dgvReportData.DataSource = report;
            pgbStatus.Value = 100; lblPercent.Text = "100%";
            lblStatus.Text = $"Hoàn thành: {report.Count} mặt hàng. Tổng giá trị: {report.Sum(x => x.Value):N0}.";
        }
        catch (OperationCanceledException)
        {
            pgbStatus.Value = 0; lblPercent.Text = "0%"; lblStatus.Text = "Đã hủy tác vụ.";
            if (!SuppressNotifications && !closeAfterCancel) MessageBox.Show(this, "Đã hủy tác vụ thống kê kho.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) { lblStatus.Text = "Không thể hoàn thành thống kê."; if (!SuppressNotifications) Ui.Error(this, ex); }
        finally { cancellation = null; btnRun.Enabled = true; btnCancel.Enabled = false; }
    }
    internal void CancelReport() => cancellation?.Cancel();
    private async void Closing_Async(object? sender, FormClosingEventArgs e)
    {
        if (running is not { IsCompleted: false }) return;
        e.Cancel = true;
        if (closeAfterCancel) return;
        closeAfterCancel = true;
        CancelReport();
        await running;
        Close();
    }
    private void ReleaseResources() { cancellation?.Cancel(); }
}
