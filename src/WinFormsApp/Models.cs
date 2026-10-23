using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LTNet;
public abstract class ObservableModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    { if (EqualityComparer<T>.Default.Equals(field, value)) return; field = value; PropertyChanged?.Invoke(this, new(name)); }
}
public sealed class BookingModel : ObservableModel
{
    private string id = "", customer = "", identity = "", room = "Standard";
    private DateTime checkIn = DateTime.Today;
    private int days = 1;
    private decimal price = 500000;
    public string BookingId { get => id; set => Set(ref id, value); }
    public string CustomerName { get => customer; set => Set(ref customer, value); }
    public string IdentityCard { get => identity; set => Set(ref identity, value); }
    public string RoomType { get => room; set => Set(ref room, value); }
    public DateTime CheckIn { get => checkIn; set => Set(ref checkIn, value); }
    public int Days { get => days; set => Set(ref days, value); }
    public decimal PricePerNight { get => price; set => Set(ref price, value); }
}
public sealed class EmployeeModel : ObservableModel
{
    private string name = "", avatar = "";
    private decimal salary;
    public string FullName { get => name; set => Set(ref name, value); }
    public decimal Salary { get => salary; set => Set(ref salary, value); }
    public string AvatarPath { get => avatar; set => Set(ref avatar, value); }
}
public sealed record ReportRow(string ProductId, string ProductName, int Quantity, decimal UnitPrice)
{ public decimal Value => Quantity * UnitPrice; }
public sealed record SensorLog(DateTime Time, double Temperature, string Status);
public sealed record StudentModel(string FullName);
public sealed record CourseModel(string CourseName);
