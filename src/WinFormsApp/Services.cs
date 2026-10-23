using System.Globalization;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace LTNet;
internal static class Rules
{
    public static bool Identity(string value) => (value.Length == 9 || value.Length == 12) && value.All(c => c >= '0' && c <= '9');
    public static bool Price(string value, out decimal price) => decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out price) && price > 0;
}
internal static class EmployeeCsv
{
    private static readonly string[] Header = ["FullName", "Salary", "AvatarPath"];
    private static string Escape(string value) => "\"" + value.Replace("\"", "\"\"") + "\"";
    public static void Save(string path, IEnumerable<EmployeeModel> employees)
    {
        using var writer = new StreamWriter(path, false, new UTF8Encoding(true));
        writer.WriteLine(string.Join(",", Header));
        foreach (var e in employees) writer.WriteLine(string.Join(",", new[] { e.FullName, e.Salary.ToString(CultureInfo.InvariantCulture), e.AvatarPath }.Select(Escape)));
    }
    public static List<EmployeeModel> Load(string path)
    {
        using var parser = new TextFieldParser(path, Encoding.UTF8) { TextFieldType = FieldType.Delimited, HasFieldsEnclosedInQuotes = true, TrimWhiteSpace = false };
        parser.SetDelimiters(",");
        if (!(parser.ReadFields() ?? []).SequenceEqual(Header)) throw new InvalidDataException("CSV phải có tiêu đề FullName,Salary,AvatarPath.");
        var result = new List<EmployeeModel>();
        while (!parser.EndOfData)
        {
            var row = parser.ReadFields() ?? [];
            if (row.Length != 3 || !decimal.TryParse(row[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var salary))
                throw new InvalidDataException($"Dữ liệu CSV không hợp lệ gần dòng {parser.LineNumber}. Không nhập một phần danh sách.");
            result.Add(new() { FullName = row[0], Salary = salary, AvatarPath = row[2] });
        }
        return result;
    }
}
internal static class WarehouseService
{
    public static async Task<List<ReportRow>> GenerateAsync(string warehouse, DateTime from, DateTime to, IProgress<int> progress, CancellationToken token)
    {
        return await Task.Run(() =>
        {
            for (int i = 1; i <= 50; i++)
            {
                token.ThrowIfCancellationRequested();
                if (token.WaitHandle.WaitOne(90)) token.ThrowIfCancellationRequested();
                progress.Report(i * 2);
            }
            var random = new Random(HashCode.Combine(warehouse, from.Date, to.Date));
            return Enumerable.Range(1, 20).Select(i => new ReportRow($"SP{i:000}", $"{warehouse} - Sản phẩm {i}", random.Next(10, 300), random.Next(10, 200) * 1000m)).ToList();
        }, token);
    }
}
