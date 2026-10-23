using System.ComponentModel;

namespace LTNet;
public partial class frmIoTMonitor : Form
{
    private CancellationTokenSource? cancellation;
    private Task? worker;
    private readonly BindingList<SensorLog> logs = new();
    private int received;
    private bool closing;
    public frmIoTMonitor()
    {
        InitializeComponent();
        Ui.Columns(dgvLogs, ("Time", "Thời gian", "dd/MM/yyyy HH:mm:ss"), ("Temperature", "Nhiệt độ (°C)", "N1"), ("Status", "Trạng thái", ""));
        dgvLogs.DataSource = logs;
        btnStop.Enabled = false;
        btnStart.Click += (_, _) => StartMonitoring();
        btnStop.Click += async (_, _) => await StopMonitoringAsync();
        FormClosing += Closing_Async;
    }
    internal void StartMonitoring()
    {
        if (worker is { IsCompleted: false }) return;
        var cts = new CancellationTokenSource();
        cancellation = cts;
        btnStart.Enabled = false; btnStop.Enabled = true; connection.Text = "Đang kết nối / giám sát";
        // Capture Handle on the UI thread before the worker can call InvokeRequired.
        _ = Handle;
        worker = Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    await Task.Delay(2000, cts.Token);
                    var temperature = Math.Round(30 + Random.Shared.NextDouble() * 12, 1);
                    Publish(temperature, cts);
                }
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested) { }
        });
    }
    private void Publish(double temperature, CancellationTokenSource owner)
    {
        if (owner.IsCancellationRequested || IsDisposed || Disposing) return;
        if (InvokeRequired)
        {
            try { Invoke(new Action(() => Publish(temperature, owner))); }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) when (IsDisposed || Disposing || !IsHandleCreated) { }
            return;
        }
        if (cancellation != owner || owner.IsCancellationRequested) return;
        RecordTemperature(temperature);
    }
    internal void RecordTemperature(double temperature)
    {
        lblTemperature.Text = $"{temperature:N1} °C";
        lblTemperature.ForeColor = temperature > 37 ? Color.Red : Color.Green;
        logs.Add(new(DateTime.Now, temperature, temperature > 37 ? "Cảnh báo" : "Bình thường"));
        received++;
        cycles.Text = $"Chu kỳ: {received}";
        if (dgvLogs.Rows.Count > 0) dgvLogs.FirstDisplayedScrollingRowIndex = dgvLogs.Rows.Count - 1;
    }
    internal async Task StopMonitoringAsync()
    {
        var cts = cancellation;
        if (cts == null) return;
        btnStop.Enabled = false;
        cts.Cancel();
        if (worker != null) await worker;
        if (cancellation == cts) { cancellation = null; cts.Dispose(); }
        connection.Text = "Đã dừng giám sát";
        btnStart.Enabled = true;
    }
    private async void Closing_Async(object? sender, FormClosingEventArgs e)
    {
        if (cancellation == null) return;
        e.Cancel = true;
        if (closing) return;
        closing = true;
        await StopMonitoringAsync();
        Close();
    }
    private void ReleaseResources() { cancellation?.Cancel(); }
}
