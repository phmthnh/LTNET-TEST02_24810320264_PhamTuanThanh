namespace LTNet;
public partial class frmCourseList : Form
{
    public frmCourseList()
    {
        InitializeComponent();
        Ui.Columns(dgvRecords, ("CourseName", "Tên môn học", ""));
        dgvRecords.DataSource = new System.ComponentModel.BindingList<CourseModel>();
    }
    private void ReleaseResources() { }
}
