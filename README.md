# BÁO CÁO BÀI TẬP THỰC HÀNH LẬP TRÌNH C# WINFORMS

## I. THÔNG TIN SINH VIÊN

* **Họ và tên:** Phạm Tuấn Thành
* **Mã số sinh viên:** 24810320264
* **Lớp học phần:** D19QTANM1
* **Link Repository:** https://github.com/phmthnh/LTNET-TEST02_24810320264_PhamTuanThanh
* **Đề:** D19QTANM1-LTNET-TEST02-02, TEST 02, gồm 5 bài.

---

## II. DANH SÁCH BÀI TẬP HOÀN THÀNH & TỰ ĐÁNH GIÁ

| Bài tập | Tiêu đề bài tập | Trạng thái | Mức độ hoàn thành (%) |
| :---: | :--- | :--- | :---: |
| **Bài 1** | Quản lý đặt phòng khách sạn | Hoàn thành | 100% |
| **Bài 2** | Quản lý hồ sơ nhân sự & thông tin lương | Hoàn thành | 100% |
| **Bài 3** | Hệ thống MDI quản lý trường học & đào tạo | Hoàn thành phần MDI; chưa đầy đủ nghiệp vụ quản lý | 70% |
| **Bài 4** | Bảng điều khiển tổng hợp báo cáo kho hàng | Hoàn thành | 100% |
| **Bài 5** | Mô phỏng giám sát hệ thống cảnh báo IoT real-time | Hoàn thành | 100% |

---

## III. MINH HỌA GIAO DIỆN & KỸ THUẬT NỔI BẬT

### 1. Bài 1: Quản lý đặt phòng khách sạn

**Form:** `frmBookingManager`.

**Kỹ thuật:** khai báo 7 cột bằng C#, `AutoGenerateColumns = false`, `DataPropertyName`, `BindingList<BookingModel>`, `BindingSource`, `INotifyPropertyChanged`, `ErrorProvider`. Grid chỉ đọc, chọn cả dòng, giá định dạng `N0`.

**Luồng:** Làm mới → nhập dữ liệu → Thêm đặt phòng. Chọn dòng → dữ liệu lên editor → sửa → Cập nhật. Xóa yêu cầu xác nhận Yes/No với biểu tượng cảnh báo. Mã tự tăng BK001, BK002... trong phiên, không tái sử dụng mã vừa xóa.

**Validation:** tên không trống, CCCD/CMND đúng 9 hoặc 12 chữ số, giá là số lớn hơn 0. CCCD lưu chuỗi để giữ số 0 đầu. Chỉnh sửa chưa bấm Cập nhật không làm thay đổi dữ liệu đã lưu.

![Giao diện chính Bài 1](docs/bai1_main.png)

![ErrorProvider Bài 1](docs/bai1_error.png)

### 2. Bài 2: Quản lý hồ sơ nhân sự & thông tin lương

**Form:** `frmEmployeeManager`.

**Kỹ thuật:** MenuStrip, ToolStrip có biểu tượng mở/lưu/thêm, StatusStrip tổng nhân viên/quỹ lương/đồng hồ, `OpenFileDialog`, `SaveFileDialog`, PictureBox `Zoom`, CSV UTF-8.

**Luồng:** Thêm mới → nhập tên, lương → Chọn ảnh JPG/PNG → Thêm. Chọn dòng để xem ảnh, sửa hoặc xóa. Tìm kiếm theo tên; tổng nhân viên và quỹ lương vẫn tính toàn bộ danh sách. Mở CSV thay toàn bộ danh sách sau khi parse thành công; CSV lỗi giữ nguyên dữ liệu cũ. Lưu CSV xuất toàn bộ danh sách, kể cả khi đang tìm kiếm.

Ảnh được sao chép vào bộ nhớ để không khóa file gốc. CSV chỉ lưu đường dẫn, không lưu dữ liệu nhị phân ảnh. Đường dẫn ảnh tương đối được giải theo thư mục chứa CSV. Ảnh bị mất không làm ứng dụng dừng.

![Giao diện và ảnh Bài 2](docs/bai2_main.png)

![Danh sách nhập từ CSV](docs/bai2_csv.png)

CSV hỗ trợ dấu phẩy, dấu ngoặc kép và xuống dòng trong trường được bao bằng dấu ngoặc kép; lương trong CSV dùng dấu chấm thập phân bất kể thiết lập vùng Windows.

### 3. Bài 3: Hệ thống MDI quản lý trường học & đào tạo

**Form:** `frmSchoolMain`, `frmStudentList`, `frmCourseList`.

**Kỹ thuật:** `IsMdiContainer = true`; tìm Form trong `MdiChildren` trước khi tạo; Form đã mở được khôi phục nếu thu nhỏ và gọi `Activate()`; Form mới được gán `MdiParent` và `Show()`. Menu Cửa sổ thực hiện `Cascade`, `TileHorizontal`, `TileVertical`, Đóng tất cả. StatusStrip hiển thị cán bộ quản lý và `ToolStripProgressBar`.

![Khung MDI và hai Form con](docs/bai3_mdi.png)

### 4. Bài 4: Bảng điều khiển tổng hợp báo cáo kho hàng

**Form:** `frmWarehouseAnalytics`.

**Kỹ thuật:** `await Task.Run(...)`, `CancellationTokenSource`, `IProgress<int>`. Tác vụ mô phỏng khoảng 4–5 giây, xử lý ngoài UI thread; `pgbStatus` và `lblPercent` cập nhật tiến độ.

**Luồng:** chọn kho/khoảng thời gian → Thống Kê Kho → tiến độ → kết quả tồn kho/giá trị. Hủy Tác Vụ ngắt tác vụ nền, reset về 0%, báo đã hủy và cho phép chạy lại. Đóng Form lúc chạy cũng hủy và chờ tác vụ kết thúc an toàn. Không tạo nhiều lượt chạy chồng nhau.

![Tác vụ nền đang chạy](docs/bai3_async.png)

![Kết quả thống kê kho](docs/bai4_main.png)

### 5. Bài 5: Mô phỏng giám sát hệ thống cảnh báo IoT real-time

**Form:** `frmIoTMonitor`.

**Kỹ thuật:** vòng lặp `Task.Run`, chu kỳ `Task.Delay(2000, token)`, nhiệt độ ngẫu nhiên 30–42°C. Kiểm tra `InvokeRequired`, gọi `Invoke(new Action(...))` trước khi cập nhật label, grid và bộ đếm. Bật `Control.CheckForIllegalCrossThreadCalls = true` khi chạy.

**Luồng:** Bắt đầu Giám sát → mỗi 2 giây nhận nhiệt độ → cập nhật label/nhật ký/số chu kỳ → Dừng Giám sát. Trên 37°C màu đỏ; đúng 37°C và thấp hơn màu xanh. Dừng không xóa nhật ký. Đóng Form dừng vòng lặp, không truy cập Control đã giải phóng.

![Giám sát IoT](docs/bai5_main.png)

---

## IV. HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN

### 1. Môi trường

- Windows, Visual Studio 2022 từ 17.8 trở lên hoặc Visual Studio 2026.
- Workload **.NET desktop development**, .NET 8 SDK và Windows Desktop Runtime 8.
- Dự án target `net8.0-windows`, không cần package NuGet bên thứ ba hay database.

### 2. Clone và chạy bằng Visual Studio

```bash
git clone https://github.com/phmthnh/LTNET-TEST02_24810320264_PhamTuanThanh.git
```

1. Mở `src/WinFormsAppSolution.sln`.
2. Chọn `WinFormsApp` làm Startup Project nếu cần.
3. Chọn Debug / Any CPU, nhấn **F5**.
4. Chọn bài ở màn hình chính.
5. Xem Designer: Solution Explorer → Forms → nhấp phải `frmBookingManager.cs` (hoặc Form khác) → **View Designer**, hoặc **Shift+F7**. Mỗi Form có `.Designer.cs` riêng; không chọn `.Designer.cs` để mở Designer.

### 3. Cấu trúc thư mục

```text
LTNET-TEST02_24810320264_PhamTuanThanh/
├── .gitignore
├── README.md
├── docs/                         # Ảnh minh họa báo cáo
└── src/
    ├── WinFormsAppSolution.sln
    └── WinFormsApp/
        ├── Forms/                # 8 Form và các file Designer tương ứng
        ├── Models.cs
        ├── Services.cs
        ├── Ui.cs
        ├── Program.cs
        └── WinFormsApp.csproj
```

`.gitignore` loại bỏ `.vs/`, `bin/`, `obj/`, `*.user`, `*.suo` và các file log.
