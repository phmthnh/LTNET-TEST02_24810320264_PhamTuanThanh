#nullable enable
namespace LTNet;
partial class frmSchoolMain
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
menuMain = new MenuStrip();
menuMain.Name = "menuMain";
menuMain.Location = new Point(0, 0);
menuMain.Size = new Size(1100, 24);
Controls.Add(menuMain);
statusMain = new StatusStrip();
statusMain.Name = "statusMain";
statusMain.Location = new Point(0, 678);
statusMain.Size = new Size(1100, 22);
Controls.Add(statusMain);
menuSystem = new ToolStripMenuItem();
menuSystem.Name = "menuSystem";
menuSystem.Text = "Hệ thống";
menuLogout = new ToolStripMenuItem();
menuLogout.Name = "menuLogout";
menuLogout.Text = "Đăng xuất";
menuPassword = new ToolStripMenuItem();
menuPassword.Name = "menuPassword";
menuPassword.Text = "Đổi mật khẩu";
menuExit = new ToolStripMenuItem();
menuExit.Name = "menuExit";
menuExit.Text = "Thoát";
menuManagement = new ToolStripMenuItem();
menuManagement.Name = "menuManagement";
menuManagement.Text = "Quản lý";
menuStudents = new ToolStripMenuItem();
menuStudents.Name = "menuStudents";
menuStudents.Text = "Danh sách Sinh viên";
menuLecturers = new ToolStripMenuItem();
menuLecturers.Name = "menuLecturers";
menuLecturers.Text = "Danh sách Giảng viên";
menuClasses = new ToolStripMenuItem();
menuClasses.Name = "menuClasses";
menuClasses.Text = "Danh mục Lớp học";
menuCourses = new ToolStripMenuItem();
menuCourses.Name = "menuCourses";
menuCourses.Text = "Danh mục môn học";
menuWindows = new ToolStripMenuItem();
menuWindows.Name = "menuWindows";
menuWindows.Text = "Cửa sổ";
menuCascade = new ToolStripMenuItem();
menuCascade.Name = "menuCascade";
menuCascade.Text = "Xếp tầng (Cascade)";
menuHorizontal = new ToolStripMenuItem();
menuHorizontal.Name = "menuHorizontal";
menuHorizontal.Text = "Ngang (Tile Horizontal)";
menuVertical = new ToolStripMenuItem();
menuVertical.Name = "menuVertical";
menuVertical.Text = "Dọc (Tile Vertical)";
menuCloseAll = new ToolStripMenuItem();
menuCloseAll.Name = "menuCloseAll";
menuCloseAll.Text = "Đóng tất cả";
managerStatus = new ToolStripStatusLabel();
managerStatus.Name = "managerStatus";
managerStatus.Text = "Cán bộ quản lý: Phạm Tuấn Thành";
progress = new ToolStripProgressBar();
progress.Name = "progress";
progress.Text = "";
menuSystem.DropDownItems.AddRange(new ToolStripItem[] { menuLogout, menuPassword, menuExit });
menuManagement.DropDownItems.AddRange(new ToolStripItem[] { menuStudents, menuLecturers, menuClasses, menuCourses });
menuWindows.DropDownItems.AddRange(new ToolStripItem[] { menuCascade, menuHorizontal, menuVertical, menuCloseAll });
menuMain.Items.AddRange(new ToolStripItem[] { menuSystem, menuManagement, menuWindows });
statusMain.Items.AddRange(new ToolStripItem[] { managerStatus, progress });
menuMain.MdiWindowListItem = menuWindows;
AutoScaleDimensions = new SizeF(7F, 15F);
AutoScaleMode = AutoScaleMode.Font;
ClientSize = new Size(1100, 700);
MinimumSize = new Size(850, 580);
Name = "frmSchoolMain";
Text = "Bài 3 - Hệ thống MDI quản lý trường học & đào tạo";
StartPosition = FormStartPosition.CenterScreen;
IsMdiContainer = true;
MainMenuStrip = menuMain;
ResumeLayout(false);
PerformLayout();
    }
    internal MenuStrip menuMain = null!;
internal StatusStrip statusMain = null!;

internal ToolStripMenuItem menuSystem = null!;
internal ToolStripMenuItem menuLogout = null!;
internal ToolStripMenuItem menuPassword = null!;
internal ToolStripMenuItem menuExit = null!;
internal ToolStripMenuItem menuManagement = null!;
internal ToolStripMenuItem menuStudents = null!;
internal ToolStripMenuItem menuLecturers = null!;
internal ToolStripMenuItem menuClasses = null!;
internal ToolStripMenuItem menuCourses = null!;
internal ToolStripMenuItem menuWindows = null!;
internal ToolStripMenuItem menuCascade = null!;
internal ToolStripMenuItem menuHorizontal = null!;
internal ToolStripMenuItem menuVertical = null!;
internal ToolStripMenuItem menuCloseAll = null!;
internal ToolStripStatusLabel managerStatus = null!;
internal ToolStripProgressBar progress = null!;
}
