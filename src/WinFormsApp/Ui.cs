namespace LTNet;
internal static class Ui
{
    public static void Columns(DataGridView grid, params (string Property, string Header, string Format)[] columns)
    {
        grid.Columns.Clear();
        foreach (var (property, header, format) in columns)
            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = property, DataPropertyName = property, HeaderText = header, DefaultCellStyle = new DataGridViewCellStyle { Format = format } });
    }
    public static void Error(IWin32Window owner, Exception ex) => MessageBox.Show(owner, ex.Message, "Không thể thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Error);
    public static void NotSpecified(IWin32Window owner, string feature) => MessageBox.Show(owner,
        $"Đề bài có nêu menu {feature} nhưng không mô tả luồng xử lý hoặc dữ liệu tương ứng. Chương trình giữ menu theo đề và không tự bổ sung nghiệp vụ.",
        feature, MessageBoxButtons.OK, MessageBoxIcon.Information);
}
