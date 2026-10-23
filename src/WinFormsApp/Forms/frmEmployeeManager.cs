using System.ComponentModel;
using System.Globalization;

namespace LTNet;
public partial class frmEmployeeManager : Form
{
    internal readonly BindingList<EmployeeModel> Employees = new();
    private readonly BindingSource source = new();
    private EmployeeModel? selected;
    private bool filtering;
    public frmEmployeeManager()
    {
        InitializeComponent();
        Ui.Columns(dgvEmployees, ("FullName", "Tên nhân viên", ""), ("Salary", "Lương", "N0"), ("AvatarPath", "Đường dẫn ảnh", ""));
        dgvEmployees.DataSource = source;
        Employees.ListChanged += (_, _) => { ApplySearch(); UpdateStatus(); };
        source.PositionChanged += (_, _) => SelectCurrent();
        dgvEmployees.CellClick += (_, e) => { if (e.RowIndex >= 0) SelectCurrent(); };
        menuOpenCsv.Click += Open_Click; menuSaveCsv.Click += Save_Click;
        menuExit.Click += (_, _) => Close();
        menuAdd.Click += Add_Click; menuDelete.Click += Delete_Click; menuSearch.Click += (_, _) => txtSearch.Focus();
        toolOpen.Image = SystemIcons.Application.ToBitmap(); toolOpen.Click += Open_Click;
        toolSave.Image = SystemIcons.Information.ToBitmap(); toolSave.Click += Save_Click;
        toolNew.Image = SystemIcons.Asterisk.ToBitmap(); toolNew.Click += (_, _) => ClearEditor();
        btnAdd.Click += Add_Click; btnUpdate.Click += Update_Click; btnDelete.Click += Delete_Click;
        btnNew.Click += (_, _) => ClearEditor(); btnAvatar.Click += Avatar_Click;
        txtSearch.TextChanged += (_, _) => ApplySearch();
        clockTimer.Tick += (_, _) => UpdateStatus();
        clockTimer.Start();
        ApplySearch(); ClearEditor(); UpdateStatus();
    }
    internal void ApplySearch()
    {
        if (filtering) return;
        filtering = true;
        var keep = selected;
        source.DataSource = new BindingList<EmployeeModel>(Employees.Where(x => string.IsNullOrWhiteSpace(txtSearch.Text) || x.FullName.Contains(txtSearch.Text.Trim(), StringComparison.CurrentCultureIgnoreCase)).ToList());
        if (keep != null && source.IndexOf(keep) >= 0) source.Position = source.IndexOf(keep);
        filtering = false;
        SelectCurrent();
    }
    private void SelectCurrent()
    {
        if (filtering) return;
        selected = source.Position >= 0 && source.Position < source.Count ? source.Current as EmployeeModel : null;
        if (selected == null) { ClearEditor(); return; }
        txtEmployeeName.Text = selected.FullName;
        txtSalary.Text = selected.Salary.ToString(CultureInfo.CurrentCulture);
        txtAvatarPath.Text = selected.AvatarPath;
        LoadAvatar(selected.AvatarPath, false);
        btnUpdate.Enabled = btnDelete.Enabled = true;
    }
    internal void ClearEditor()
    {
        selected = null; dgvEmployees.ClearSelection();
        txtEmployeeName.Clear(); txtSalary.Text = "0"; txtAvatarPath.Clear();
        picAvatar.Image?.Dispose(); picAvatar.Image = null;
        btnUpdate.Enabled = btnDelete.Enabled = false;
        errorProvider.Clear(); txtEmployeeName.Focus();
    }
    private bool ReadSalary(out decimal salary)
    {
        errorProvider.Clear();
        if (decimal.TryParse(txtSalary.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out salary)) return true;
        errorProvider.SetError(txtSalary, "Lương phải là một giá trị số để tính tổng quỹ lương."); return false;
    }
    private void Add_Click(object? sender, EventArgs e)
    {
        if (!ReadSalary(out var salary)) return;
        var model = new EmployeeModel { FullName = txtEmployeeName.Text, Salary = salary, AvatarPath = txtAvatarPath.Text };
        Employees.Add(model);
        txtSearch.Clear();
        source.Position = source.IndexOf(model); SelectCurrent();
    }
    private void Update_Click(object? sender, EventArgs e)
    {
        if (selected == null || !ReadSalary(out var salary)) return;
        var model = selected;
        Employees.RaiseListChangedEvents = false;
        model.FullName = txtEmployeeName.Text; model.Salary = salary; model.AvatarPath = txtAvatarPath.Text;
        Employees.RaiseListChangedEvents = true;
        Employees.ResetBindings();
    }
    private void Delete_Click(object? sender, EventArgs e)
    {
        if (selected == null) return;
        var model = selected; selected = null; Employees.Remove(model);
    }
    private void Avatar_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog { Filter = "Ảnh JPG và PNG|*.jpg;*.png", CheckFileExists = true };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        if (LoadAvatar(dialog.FileName, true)) txtAvatarPath.Text = dialog.FileName;
    }
    private bool LoadAvatar(string path, bool reportError)
    {
        Image? next = null;
        try
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                using var stream = File.OpenRead(path);
                using var original = Image.FromStream(stream);
                next = new Bitmap(original);
            }
            picAvatar.Image?.Dispose(); picAvatar.Image = next; return true;
        }
        catch (Exception ex) when (ex is IOException or ArgumentException or OutOfMemoryException or UnauthorizedAccessException)
        {
            next?.Dispose();
            if (reportError) Ui.Error(this, ex);
            else { picAvatar.Image?.Dispose(); picAvatar.Image = null; }
            return false;
        }
    }
    private void Save_Click(object? sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog { Filter = "CSV UTF-8|*.csv", DefaultExt = "csv", AddExtension = true, FileName = "nhan-su.csv" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try { EmployeeCsv.Save(dialog.FileName, Employees); }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { Ui.Error(this, ex); }
    }
    private void Open_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog { Filter = "CSV UTF-8|*.csv", CheckFileExists = true };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try { ImportCsv(dialog.FileName); }
        catch (Exception ex) when (ex is IOException or InvalidDataException or UnauthorizedAccessException or Microsoft.VisualBasic.FileIO.MalformedLineException) { Ui.Error(this, ex); }
    }
    internal void ImportCsv(string path)
    {
        var imported = EmployeeCsv.Load(path); // Parse completely before replacing the live list.
        foreach (var model in imported)
            if (!string.IsNullOrWhiteSpace(model.AvatarPath) && !Path.IsPathRooted(model.AvatarPath))
                model.AvatarPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path))!, model.AvatarPath));
        Employees.RaiseListChangedEvents = false;
        Employees.Clear(); foreach (var model in imported) Employees.Add(model);
        Employees.RaiseListChangedEvents = true;
        txtSearch.Clear(); Employees.ResetBindings(); ClearEditor(); UpdateStatus();
    }
    internal void UpdateStatus()
    {
        countStatus.Text = $"Tổng số nhân viên: {Employees.Count}   |   ";
        salaryStatus.Text = $"Tổng quỹ lương: {Employees.Sum(x => x.Salary):N0}   |   ";
        timeStatus.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }
    private void ReleaseResources()
    {
        clockTimer?.Stop(); source.Dispose();
        if (picAvatar != null) { picAvatar.Image?.Dispose(); picAvatar.Image = null; }
        if (toolMain != null) foreach (ToolStripItem item in toolMain.Items) item.Image?.Dispose();
    }
}
