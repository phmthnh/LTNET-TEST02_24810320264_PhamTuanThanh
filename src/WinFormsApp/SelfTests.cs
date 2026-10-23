using System.Diagnostics;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace LTNet;
// Integration tests execute real controls on an STA thread with a live message loop.
internal static class SelfTests
{
    private static readonly List<string> passed = new();
    private static string output = "";
    private static void Check(bool condition, string description)
    {
        if (!condition) throw new InvalidOperationException(description);
        passed.Add(description);
    }
    public static void Run(string[] args)
    {
        output = args.Length > 1 ? args[1] : Path.Combine(AppContext.BaseDirectory, "test-output");
        Directory.CreateDirectory(output);
        using var host = new Form { Text = "LTNET Integration Tests", Width = 320, Height = 100 };
        host.Shown += async (_, _) =>
        {
            string? failure = null;
            try { await ExecuteAsync(); }
            catch (Exception ex) { failure = ex.ToString(); Environment.ExitCode = 1; }
            File.WriteAllText(Path.Combine(output, "test-results.json"), JsonSerializer.Serialize(new { passed = passed.Count, checks = passed, failure }, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }));
            host.Close();
        };
        Application.Run(host);
    }
    private static void Capture(Form form, string name)
    {
        form.Refresh();
        using var bitmap = new Bitmap(form.Width, form.Height);
        form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
        bitmap.Save(Path.Combine(output, name));
    }
    private static void FillBooking(frmBookingManager f, string name, string id, string price)
    {
        f.txtCustomerName.Text = name; f.txtIdentityCard.Text = id; f.txtPricePerNight.Text = price;
    }
    private static async Task ExecuteAsync()
    {
        Check(Rules.Identity("012345678"), "CCCD 9 chữ số, giữ số 0 đầu");
        Check(Rules.Identity("012345678901"), "CCCD 12 chữ số");
        foreach (var value in new[] { "", "12345678", "1234567890", "12345678901", "1234567890123", "12345678a", "１２３４５６７８９" }) Check(!Rules.Identity(value), $"Từ chối CCCD sai: {value}");
        Check(!Rules.Price("0", out _) && !Rules.Price("-1", out _) && !Rules.Price("abc", out _), "Từ chối giá 0, âm, chữ");
        using (var f = new frmBookingManager())
        {
            f.Show(); await Task.Delay(100);
            Check(f.dgvBookings.Columns.Count == 7 && !f.dgvBookings.AutoGenerateColumns && f.dgvBookings.ReadOnly && f.dgvBookings.SelectionMode == DataGridViewSelectionMode.FullRowSelect, "Grid đặt phòng đủ 7 cột, FullRowSelect, ReadOnly, AutoGenerateColumns=false");
            FillBooking(f, "", "abc", "0"); f.btnAdd.PerformClick();
            Check(f.Bookings.Count == 0 && f.errorProvider.GetError(f.txtCustomerName) != "" && f.errorProvider.GetError(f.txtIdentityCard) != "" && f.errorProvider.GetError(f.txtPricePerNight) != "", "ErrorProvider chặn 3 lỗi đặt phòng cùng lúc");
            Capture(f, "bai1_error.png");
            FillBooking(f, "Nguyễn Văn An", "012345678901", "850000"); f.btnAdd.PerformClick();
            Check(f.Bookings.Count == 1 && f.Bookings[0].BookingId == "BK001", "Thêm đặt phòng hợp lệ, mã BK001");
            f.txtCustomerName.Text = "Khách đã cập nhật"; f.txtPricePerNight.Text = "950000";
            Check(f.Bookings[0].CustomerName == "Nguyễn Văn An", "Không ghi dữ liệu chưa xác nhận xuống danh sách");
            f.btnUpdate.PerformClick();
            Check(f.Bookings[0].CustomerName == "Khách đã cập nhật" && f.Bookings[0].PricePerNight == 950000, "Cập nhật binding hai chiều xuống model và grid");
            f.NewBooking(); FillBooking(f, "Trần Bình", "012345678", "500000"); f.btnAdd.PerformClick();
            Check(f.Bookings.Count == 2 && f.Bookings[1].BookingId == "BK002", "Mã đặt phòng tăng BK002");
            f.dgvBookings.CurrentCell = f.dgvBookings.Rows[0].Cells[0];
            await Task.Delay(50);
            Check(f.txtCustomerName.Text == f.Bookings[0].CustomerName, "Chọn dòng grid đồng bộ lên ô nhập");
            f.Bookings[0].CustomerName = "Nguyễn Văn An";
            Check(f.txtCustomerName.Text == "Nguyễn Văn An", "INotifyPropertyChanged cập nhật model lên editor");
            Capture(f, "bai1_main.png");
            f.DeleteSelected(() => DialogResult.No);
            Check(f.Bookings.Count == 2, "Xác nhận xóa No giữ nguyên đặt phòng");
            f.DeleteSelected(() => DialogResult.Yes);
            Check(f.Bookings.Count == 1 && f.Bookings[0].BookingId == "BK002", "Xác nhận xóa Yes xóa đúng đặt phòng đang chọn");
            Check(f.txtBookingId.Text == "BK003", "Không tái dùng mã đã xóa trong phiên");
            f.Close();
        }
        var csv = Path.Combine(output, "nhan-su-mau.csv");
        using (var avatar = new Bitmap(180, 180))
        {
            using var graphics = Graphics.FromImage(avatar);
            graphics.Clear(Color.AliceBlue); graphics.FillEllipse(Brushes.SlateGray, 60, 25, 60, 60); graphics.FillEllipse(Brushes.SlateGray, 25, 100, 130, 140);
            avatar.Save(Path.Combine(output, "avatar-mau.png"));
        }
        var staff = new[] { new EmployeeModel { FullName = "Phạm Tuấn Thành", Salary = 15000000, AvatarPath = "avatar-mau.png" }, new EmployeeModel { FullName = "Nguyễn, \"Bình\"\nNhân viên", Salary = 12500000.5m, AvatarPath = @"C:\Ảnh\ảnh.png" } };
        EmployeeCsv.Save(csv, staff);
        var roundTrip = EmployeeCsv.Load(csv);
        Check(roundTrip.Count == 2 && roundTrip[1].FullName == staff[1].FullName && roundTrip[1].Salary == staff[1].Salary && roundTrip[1].AvatarPath == staff[1].AvatarPath, "CSV round-trip tiếng Việt, dấu phẩy, ngoặc kép, xuống dòng, số thập phân và đường dẫn ảnh");
        using (var f = new frmEmployeeManager())
        {
            f.Show(); f.ImportCsv(csv); await Task.Delay(100);
            Check(f.Employees.Count == 2 && f.StatusSummary.Contains("2"), "Import cập nhật danh sách và StatusStrip");
            f.dgvEmployees.CurrentCell = f.dgvEmployees.Rows[1].Cells[0];
            f.dgvEmployees.CurrentCell = f.dgvEmployees.Rows[0].Cells[0];
            Check(f.picAvatar.Image != null, "Ảnh đường dẫn tương đối được nạp theo thư mục CSV");
            using (File.Open(Path.Combine(output, "avatar-mau.png"), FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                Check(true, "Ảnh đang hiển thị không khóa file gốc");
            Capture(f, "bai2_csv.png");
            f.txtSearch.Text = "Phạm";
            Check(f.dgvEmployees.Rows.Count == 1 && f.StatusSummary.Contains("2"), "Tìm kiếm tên, tổng nhân viên vẫn tính toàn bộ danh sách");
            f.txtSearch.Clear(); f.ClearEditor(); f.txtEmployeeName.Text = "Nhân viên mới"; f.txtSalary.Text = "1000000"; f.btnAdd.PerformClick();
            Check(f.Employees.Count == 3, "Thêm nhân viên từ giao diện");
            f.txtSalary.Text = "2000000"; f.btnUpdate.PerformClick();
            Check(f.Employees.Last().Salary == 2000000, "Sửa lương nhân viên từ giao diện");
            f.btnDelete.PerformClick(); Check(f.Employees.Count == 2, "Xóa nhân viên từ giao diện");
            var bad = Path.Combine(output, "invalid.csv");
            File.WriteAllText(bad, "FullName,Salary,AvatarPath\nA,100,\nB,invalid,\n");
            bool rejected = false;
            try { f.ImportCsv(bad); } catch (InvalidDataException) { rejected = true; }
            Check(rejected && f.Employees.Count == 2, "CSV lỗi không ghi đè hoặc nhập dở danh sách hiện tại");
            f.dgvEmployees.CurrentCell = f.dgvEmployees.Rows[1].Cells[0];
            f.dgvEmployees.CurrentCell = f.dgvEmployees.Rows[0].Cells[0];
            Capture(f, "bai2_main.png");
            EmployeeCsv.Save(Path.Combine(output, "empty.csv"), []);
            f.ImportCsv(Path.Combine(output, "empty.csv"));
            Check(f.Employees.Count == 0 && f.dgvEmployees.Rows.Count == 0, "CSV rỗng có header nhập đúng danh sách rỗng");
            f.Close();
        }
        using (var f = new frmSchoolMain())
        {
            f.Show(); await Task.Delay(100);
            var first = f.OpenChild<frmStudentList>(); var again = f.OpenChild<frmStudentList>();
            Check(ReferenceEquals(first, again) && f.MdiChildren.Length == 1, "MDI chống mở trùng frmStudentList");
            first.WindowState = FormWindowState.Minimized; f.OpenChild<frmStudentList>();
            Check(first.WindowState == FormWindowState.Normal, "MDI khôi phục Form con thu nhỏ");
            f.OpenChild<frmCourseList>();
            foreach (var layout in new[] { MdiLayout.Cascade, MdiLayout.TileHorizontal, MdiLayout.TileVertical })
            { f.LayoutMdi(layout); Check(f.MdiChildren.Length == 2, $"MDI {layout} với 2 Form con"); }
            f.LayoutMdi(MdiLayout.Cascade); Capture(f, "bai3_mdi.png");
            foreach (var child in f.MdiChildren) child.Close();
            Check(f.MdiChildren.Length == 0, "Đóng tất cả Form con");
            f.OpenChild<frmStudentList>(); Check(f.MdiChildren.Length == 1, "Mở lại Form con sau khi đóng");
            f.Close();
        }
        using (var f = new frmWarehouseAnalytics { SuppressNotifications = true })
        {
            f.Show();
            int ticks = 0;
            using var timer = new System.Windows.Forms.Timer { Interval = 50 };
            timer.Tick += (_, _) => ticks++; timer.Start();
            var watch = Stopwatch.StartNew();
            var task = f.StartReportAsync();
            Check(ReferenceEquals(task, f.StartReportAsync()), "Không khởi chạy thống kê chồng nhau");
            await Task.Delay(600); Capture(f, "bai3_async.png");
            Check(f.pgbStatus.Value > 0 && f.pgbStatus.Value < 100, "Tiến độ thống kê cập nhật khi đang chạy");
            await task;
            Check(watch.Elapsed.TotalSeconds >= 4 && watch.Elapsed.TotalSeconds < 7 && ticks >= 35, "Thống kê khoảng 4–5 giây; UI timer tiếp tục chạy");
            Check(f.pgbStatus.Value == 100 && f.dgvReportData.Rows.Count == 20, "Thống kê hoàn thành 100%, grid có dữ liệu");
            Capture(f, "bai4_main.png");
            var canceled = f.StartReportAsync(); await Task.Delay(250); f.CancelReport(); await canceled;
            await Task.Delay(100);
            Check(f.pgbStatus.Value == 0 && f.lblPercent.Text == "0%" && f.lblStatus.Text.Contains("Đã hủy"), "Hủy thống kê reset tiến độ và báo đã hủy");
            var rerun = f.StartReportAsync(); await Task.Delay(150); f.CancelReport(); await rerun;
            Check(f.btnRun.Enabled && !f.btnCancel.Enabled, "Có thể chạy lại sau khi hủy");
            _ = f.StartReportAsync(); await Task.Delay(100); f.Close(); await Task.Delay(200);
            Check(f.IsDisposed, "Đóng Form khi đang thống kê hủy tác vụ an toàn");
        }
        using (var f = new frmIoTMonitor())
        {
            f.Show(); f.RecordTemperature(37);
            Check(f.lblTemperature.ForeColor == Color.Green, "IoT 37°C màu xanh");
            f.RecordTemperature(37.1);
            Check(f.lblTemperature.ForeColor == Color.Red, "IoT trên 37°C màu đỏ");
            int initial = f.ReceivedCount;
            f.StartMonitoring(); var worker = f.WorkerTask; f.StartMonitoring();
            Check(ReferenceEquals(worker, f.WorkerTask), "IoT chống tạo nhiều vòng lặp");
            await Task.Delay(4250);
            Check(f.ReceivedCount == initial + 2, "IoT nhận 2 chu kỳ sau khoảng 4 giây, không có lỗi cross-thread");
            Check(f.dgvLogs.Rows.Cast<DataGridViewRow>().All(r => (double)r.Cells[1].Value >= 30 && (double)r.Cells[1].Value <= 42), "Nhiệt độ trong khoảng 30–42°C");
            await f.StopMonitoringAsync(); int stopped = f.ReceivedCount;
            await Task.Delay(2100);
            Check(f.ReceivedCount == stopped && f.btnStart.Enabled, "Dừng IoT ngừng ghi nhật ký và cho phép chạy lại");
            Capture(f, "bai5_main.png");
            f.StartMonitoring(); await Task.Delay(100); f.Close(); await Task.Delay(150);
            Check(f.IsDisposed && f.WorkerTask!.IsCompleted, "Đóng IoT dừng tác vụ nền an toàn");
        }
    }
}
