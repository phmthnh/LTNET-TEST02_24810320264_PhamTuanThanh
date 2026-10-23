using System.ComponentModel;

namespace LTNet;
public partial class frmBookingManager : Form
{
    internal readonly BindingList<BookingModel> Bookings = new();
    private readonly BindingSource listSource = new();
    private readonly BindingSource editorSource = new();
    private readonly List<Binding> editorBindings = new();
    private BookingModel? selected;
    private int nextId = 1;
    private bool changing;
    public frmBookingManager()
    {
        InitializeComponent();
        Ui.Columns(dgvBookings, ("BookingId", "Mã Đặt", ""), ("CustomerName", "Tên Khách", ""), ("IdentityCard", "CCCD", ""), ("RoomType", "Loại Phòng", ""), ("CheckIn", "Ngày Nhận", "dd/MM/yyyy"), ("Days", "Số Đêm", ""), ("PricePerNight", "Giá/Đêm", "N0"));
        listSource.DataSource = Bookings;
        dgvBookings.DataSource = listSource;
        editorSource.DataSource = typeof(BookingModel);
        Bind(txtBookingId, "Text", "BookingId");
        Bind(txtCustomerName, "Text", "CustomerName");
        Bind(txtIdentityCard, "Text", "IdentityCard");
        Bind(cboRoomType, "SelectedItem", "RoomType");
        Bind(dtpCheckIn, "Value", "CheckIn");
        Bind(numDays, "Value", "Days");
        Bind(txtPricePerNight, "Text", "PricePerNight");
        listSource.PositionChanged += (_, _) => SelectCurrent();
        dgvBookings.CellClick += (_, e) => { if (e.RowIndex >= 0) SelectCurrent(); };
        btnAdd.Click += Add_Click;
        btnUpdate.Click += Update_Click;
        btnDelete.Click += Delete_Click;
        btnReset.Click += (_, _) => NewBooking();
        NewBooking();
    }
    private void Bind(Control control, string property, string member)
    {
        // Explicit WriteValue after validation keeps invalid edits out of the live list.
        var binding = new Binding(property, editorSource, member, true, DataSourceUpdateMode.Never);
        control.DataBindings.Add(binding);
        editorBindings.Add(binding);
    }
    internal void NewBooking()
    {
        changing = true;
        selected = null;
        dgvBookings.ClearSelection();
        editorSource.DataSource = new BookingModel { BookingId = $"BK{nextId:000}" };
        errorProvider.Clear();
        btnUpdate.Enabled = btnDelete.Enabled = false;
        changing = false;
        txtCustomerName.Focus();
    }
    private void SelectCurrent()
    {
        if (changing || listSource.Current is not BookingModel model) return;
        selected = model;
        editorSource.DataSource = model;
        errorProvider.Clear();
        btnUpdate.Enabled = btnDelete.Enabled = true;
    }
    internal bool ValidateBooking()
    {
        errorProvider.Clear();
        bool valid = true;
        if (string.IsNullOrWhiteSpace(txtCustomerName.Text)) { errorProvider.SetError(txtCustomerName, "Tên khách hàng không được để trống."); valid = false; }
        if (!Rules.Identity(txtIdentityCard.Text.Trim())) { errorProvider.SetError(txtIdentityCard, "CCCD/CMND phải có đúng 9 hoặc 12 chữ số."); valid = false; }
        if (!Rules.Price(txtPricePerNight.Text, out _)) { errorProvider.SetError(txtPricePerNight, "Giá phòng phải là số lớn hơn 0."); valid = false; }
        return valid;
    }
    private void WriteEditor()
    {
        // A property notification must not refresh the other controls midway through a commit.
        editorSource.RaiseListChangedEvents = false;
        try { foreach (var binding in editorBindings) binding.WriteValue(); editorSource.EndEdit(); }
        finally { editorSource.RaiseListChangedEvents = true; editorSource.ResetBindings(false); }
    }
    private void Add_Click(object? sender, EventArgs e)
    {
        if (!ValidateBooking()) return;
        // Always create a fresh object: adding while a row is selected must not mutate it.
        var newBooking = new BookingModel { BookingId = $"BK{nextId:000}", CustomerName = txtCustomerName.Text.Trim(), IdentityCard = txtIdentityCard.Text.Trim(), RoomType = cboRoomType.Text, CheckIn = dtpCheckIn.Value.Date, Days = (int)numDays.Value, PricePerNight = decimal.Parse(txtPricePerNight.Text) };
        changing = true;
        Bookings.Add(newBooking);
        nextId++;
        changing = false;
        listSource.Position = Bookings.Count - 1;
        SelectCurrent();
    }
    private void Update_Click(object? sender, EventArgs e)
    {
        if (selected == null || !ValidateBooking()) return;
        WriteEditor();
        listSource.ResetCurrentItem();
    }
    private void Delete_Click(object? sender, EventArgs e)
    {
        if (selected == null) return;
        if (MessageBox.Show(this, $"Xóa đặt phòng {selected.BookingId}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        Bookings.Remove(selected);
        NewBooking();
    }
    private void ReleaseResources() { listSource.Dispose(); editorSource.Dispose(); }
}
