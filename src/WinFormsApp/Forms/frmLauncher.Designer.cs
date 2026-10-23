#nullable enable
namespace LTNet;
partial class frmLauncher
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
lblTitle.Text = "BÀI TẬP THỰC HÀNH C# WINFORMS";
lblTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
Controls.Add(lblTitle);
lblStudent = new Label();
lblStudent.UseMnemonic = false;
lblStudent.Name = "lblStudent";
lblStudent.Location = new Point(24, 70);
lblStudent.Size = new Size(1000, 25);
lblStudent.Text = "Phạm Tuấn Thành  •  24810320264  •  D19QTANM1";
Controls.Add(lblStudent);
btnBooking = new Button();
btnBooking.UseMnemonic = false;
btnBooking.Name = "btnBooking";
btnBooking.Location = new Point(24, 130);
btnBooking.Size = new Size(1000, 36);
btnBooking.Text = "Bài 1: Quản lý đặt phòng khách sạn";
btnBooking.UseVisualStyleBackColor = true;
Controls.Add(btnBooking);
btnEmployee = new Button();
btnEmployee.UseMnemonic = false;
btnEmployee.Name = "btnEmployee";
btnEmployee.Location = new Point(24, 190);
btnEmployee.Size = new Size(1000, 36);
btnEmployee.Text = "Bài 2: Quản lý hồ sơ nhân sự & thông tin lương";
btnEmployee.UseVisualStyleBackColor = true;
Controls.Add(btnEmployee);
btnSchool = new Button();
btnSchool.UseMnemonic = false;
btnSchool.Name = "btnSchool";
btnSchool.Location = new Point(24, 250);
btnSchool.Size = new Size(1000, 36);
btnSchool.Text = "Bài 3: Hệ thống MDI quản lý trường học & đào tạo";
btnSchool.UseVisualStyleBackColor = true;
Controls.Add(btnSchool);
btnWarehouse = new Button();
btnWarehouse.UseMnemonic = false;
btnWarehouse.Name = "btnWarehouse";
btnWarehouse.Location = new Point(24, 310);
btnWarehouse.Size = new Size(1000, 36);
btnWarehouse.Text = "Bài 4: Bảng điều khiển tổng hợp báo cáo kho hàng";
btnWarehouse.UseVisualStyleBackColor = true;
Controls.Add(btnWarehouse);
btnIoT = new Button();
btnIoT.UseMnemonic = false;
btnIoT.Name = "btnIoT";
btnIoT.Location = new Point(24, 370);
btnIoT.Size = new Size(1000, 36);
btnIoT.Text = "Bài 5: Mô phỏng giám sát hệ thống cảnh báo IoT real-time";
btnIoT.UseVisualStyleBackColor = true;
Controls.Add(btnIoT);
AutoScaleDimensions = new SizeF(7F, 15F);
AutoScaleMode = AutoScaleMode.Font;
ClientSize = new Size(1100, 700);
MinimumSize = new Size(850, 580);
Name = "frmLauncher";
Text = "LTNET TEST02 | Phạm Tuấn Thành | 24810320264";
StartPosition = FormStartPosition.CenterScreen;

ResumeLayout(false);
PerformLayout();
    }
    internal Label lblTitle = null!;
internal Label lblStudent = null!;
internal Button btnBooking = null!;
internal Button btnEmployee = null!;
internal Button btnSchool = null!;
internal Button btnWarehouse = null!;
internal Button btnIoT = null!;
}
