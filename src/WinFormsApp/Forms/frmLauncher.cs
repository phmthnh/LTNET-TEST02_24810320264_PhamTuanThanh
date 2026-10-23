namespace LTNet;
public partial class frmLauncher : Form
{
    public frmLauncher()
    {
        InitializeComponent();
        btnBooking.Click += (_, _) => Open<frmBookingManager>();
        btnEmployee.Click += (_, _) => Open<frmEmployeeManager>();
        btnSchool.Click += (_, _) => Open<frmSchoolMain>();
        btnWarehouse.Click += (_, _) => Open<frmWarehouseAnalytics>();
        btnIoT.Click += (_, _) => Open<frmIoTMonitor>();
    }
    private void Open<T>() where T : Form, new()
    {
        var existing = OwnedForms.OfType<T>().FirstOrDefault();
        if (existing != null) { if (existing.WindowState == FormWindowState.Minimized) existing.WindowState = FormWindowState.Normal; existing.Activate(); }
        else new T().Show(this);
    }
    private void ReleaseResources() { }
}
