#nullable enable
namespace LTNet;
partial class frmIoTMonitor
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) { ReleaseResources(); components?.Dispose(); }
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
SuspendLayout();
lblTitle = new Label();
lblTitle.UseMnemonic = false;
lblTitle.Name = "lblTitle";
lblTitle.Location = new Point(24, 20);
lblTitle.Size = new Size(1050, 36);
lblTitle.Text = "BÀI 5 · GIÁM SÁT HỆ THỐNG CẢNH BÁO IoT";
lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
Controls.Add(lblTitle);
btnStart = new Button();
btnStart.UseMnemonic = false;
btnStart.Name = "btnStart";
btnStart.Location = new Point(24, 90);
btnStart.Size = new Size(200, 36);
btnStart.Text = "Bắt đầu Giám sát";
btnStart.UseVisualStyleBackColor = true;
Controls.Add(btnStart);
btnStop = new Button();
btnStop.UseMnemonic = false;
btnStop.Name = "btnStop";
btnStop.Location = new Point(240, 90);
btnStop.Size = new Size(200, 36);
btnStop.Text = "Dừng Giám sát";
btnStop.UseVisualStyleBackColor = true;
Controls.Add(btnStop);
lblTemperature = new Label();
lblTemperature.UseMnemonic = false;
lblTemperature.Name = "lblTemperature";
lblTemperature.Location = new Point(24, 155);
lblTemperature.Size = new Size(600, 75);
lblTemperature.Text = "-- °C";
lblTemperature.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
lblTemperature.ForeColor = Color.Green;
Controls.Add(lblTemperature);
dgvLogs = new DataGridView();
dgvLogs.Name = "dgvLogs";
dgvLogs.Location = new Point(24, 265);
dgvLogs.Size = new Size(1050, 380);
dgvLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
dgvLogs.ReadOnly = true;
dgvLogs.AllowUserToAddRows = false;
dgvLogs.AllowUserToDeleteRows = false;
dgvLogs.AutoGenerateColumns = false;
dgvLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
dgvLogs.MultiSelect = false;
dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
dgvLogs.RowHeadersVisible = false;
dgvLogs.BackgroundColor = Color.White;
Controls.Add(dgvLogs);
statusMain = new StatusStrip();
statusMain.Name = "statusMain";
statusMain.Location = new Point(0, 678);
statusMain.Size = new Size(1100, 22);
Controls.Add(statusMain);
connection = new ToolStripStatusLabel();
connection.Name = "connection";
connection.Text = "Chưa kết nối";
cycles = new ToolStripStatusLabel();
cycles.Name = "cycles";
cycles.Text = "Chu kỳ: 0";
statusMain.Items.AddRange(new ToolStripItem[] { connection, cycles });

AutoScaleDimensions = new SizeF(7F, 15F);
AutoScaleMode = AutoScaleMode.Font;
ClientSize = new Size(1100, 700);
MinimumSize = new Size(850, 580);
Name = "frmIoTMonitor";
Text = "Bài 5 - Mô phỏng giám sát hệ thống cảnh báo IoT real-time";
StartPosition = FormStartPosition.CenterScreen;

ResumeLayout(false);
PerformLayout();
    }
    internal Label lblTitle = null!;
internal Button btnStart = null!;
internal Button btnStop = null!;
internal Label lblTemperature = null!;
internal DataGridView dgvLogs = null!;
internal StatusStrip statusMain = null!;

internal ToolStripStatusLabel connection = null!;
internal ToolStripStatusLabel cycles = null!;
}
