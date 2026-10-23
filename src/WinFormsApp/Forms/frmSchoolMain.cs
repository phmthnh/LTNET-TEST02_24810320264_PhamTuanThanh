namespace LTNet;
public partial class frmSchoolMain : Form
{
    public frmSchoolMain()
    {
        InitializeComponent();
        menuLogout.Click += (_, _) => Close();
        menuPassword.Click += (_, _) => Ui.NotSpecified(this, "Đổi mật khẩu");
        menuExit.Click += (_, _) => Close();
        menuStudents.Click += (_, _) => OpenChild<frmStudentList>();
        menuLecturers.Click += (_, _) => Ui.NotSpecified(this, "Danh sách Giảng viên");
        menuClasses.Click += (_, _) => Ui.NotSpecified(this, "Danh mục Lớp học");
        // The specification explicitly requires frmCourseList even though it omits its menu entry.
        menuCourses.Click += (_, _) => OpenChild<frmCourseList>();
        menuCascade.Click += (_, _) => LayoutMdi(MdiLayout.Cascade);
        menuHorizontal.Click += (_, _) => LayoutMdi(MdiLayout.TileHorizontal);
        menuVertical.Click += (_, _) => LayoutMdi(MdiLayout.TileVertical);
        menuCloseAll.Click += (_, _) => { foreach (var child in MdiChildren) child.Close(); };
        MdiChildActivate += (_, _) => progress.Value = MdiChildren.Length > 0 ? 100 : 0;
    }
    internal T OpenChild<T>() where T : Form, new()
    {
        var existing = MdiChildren.OfType<T>().FirstOrDefault();
        if (existing != null)
        {
            if (existing.WindowState == FormWindowState.Minimized) existing.WindowState = FormWindowState.Normal;
            existing.Activate(); return existing;
        }
        var child = new T { MdiParent = this };
        child.Show(); return child;
    }
    private void ReleaseResources() { }
}
