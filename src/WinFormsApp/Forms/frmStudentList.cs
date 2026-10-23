namespace LTNet;
public partial class frmStudentList : Form
{
    public frmStudentList()
    {
        InitializeComponent();
        Ui.Columns(dgvRecords, ("FullName", "Tên sinh viên", ""));
        dgvRecords.DataSource = new System.ComponentModel.BindingList<StudentModel>();
    }
    private void ReleaseResources() { }
}
