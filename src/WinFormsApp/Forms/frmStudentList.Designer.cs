#nullable enable
namespace LTNet;
partial class frmStudentList
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
lblTitle.Size = new Size(700, 36);
lblTitle.Text = "DANH SÁCH SINH VIÊN";
lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
Controls.Add(lblTitle);
dgvRecords = new DataGridView();
dgvRecords.Name = "dgvRecords";
dgvRecords.Location = new Point(24, 90);
dgvRecords.Size = new Size(690, 340);
dgvRecords.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
dgvRecords.AutoGenerateColumns = false;
dgvRecords.ReadOnly = true;
dgvRecords.AllowUserToAddRows = false;
dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
dgvRecords.BackgroundColor = Color.White;
Controls.Add(dgvRecords);
AutoScaleDimensions = new SizeF(7F, 15F);
AutoScaleMode = AutoScaleMode.Font;
ClientSize = new Size(760, 510);
MinimumSize = new Size(850, 580);
Name = "frmStudentList";
Text = "Danh sách Sinh viên";
StartPosition = FormStartPosition.CenterScreen;
MinimumSize = new Size(600, 400);
ResumeLayout(false);
PerformLayout();
    }
    internal Label lblTitle = null!;
internal DataGridView dgvRecords = null!;
}
