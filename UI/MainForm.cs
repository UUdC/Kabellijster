namespace Kabellijster;

public class MainForm : Form
{
    private readonly DropPanel _excelDrop = new("kabellijst.xlsx", ".xlsx", "Excel (*.xlsx)|*.xlsx");
    private readonly DropPanel _pdfDrop = new("schema.pdf", ".pdf", "PDF (*.pdf)|*.pdf");
    private readonly Button _start = new() { Text = "Controleer", Enabled = false, Height = 40, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11, FontStyle.Bold) };
    private readonly Button _readExcel = new() { Text = "Lees Excel", Enabled = false, Height = 40, Dock = DockStyle.Fill };
    private readonly Button _diag = new() { Text = "Lees PDF", Enabled = false, Height = 40, Dock = DockStyle.Fill };
    private readonly Button _open = new() { Text = "Open rapport", Enabled = false, Height = 40, Dock = DockStyle.Fill };
    private readonly Label _status = new() { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells, BackgroundColor = Color.White
    };
    private bool _reading;

    public MainForm()
    {
        Text = "Kabellijster - Excel vs. PDF";
        Size = new Size(1000, 700);
        MinimumSize = new Size(700, 500);
        StartPosition = FormStartPosition.CenterScreen;

        var drops = new TableLayoutPanel { Dock = DockStyle.Top, Height = 170, ColumnCount = 2, Padding = new Padding(10) };
        drops.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        drops.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        _excelDrop.Dock = DockStyle.Fill; _excelDrop.Margin = new Padding(0, 0, 5, 0);
        _pdfDrop.Dock = DockStyle.Fill; _pdfDrop.Margin = new Padding(5, 0, 0, 0);
        drops.Controls.Add(_excelDrop, 0, 0);
        drops.Controls.Add(_pdfDrop, 1, 0);

        var bar = new TableLayoutPanel { Dock = DockStyle.Top, Height = 56, ColumnCount = 5, Padding = new Padding(10, 5, 10, 5) };
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        bar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        bar.Controls.Add(_diag, 0, 0);
        bar.Controls.Add(_readExcel, 1, 0);
        bar.Controls.Add(_start, 2, 0);
        bar.Controls.Add(_open, 3, 0);
        bar.Controls.Add(_status, 4, 0);

        Controls.Add(_grid);
        Controls.Add(bar);
        Controls.Add(drops);

        _excelDrop.FileChanged += (_, _) =>
        {
            _grid.Columns.Clear();
            UpdateState();
            _status.Text = "Excel geselecteerd. PDF-vergelijking volgt later.";
        };
        _readExcel.Click += async (_, _) => await ShowCablesAsync();
        _status.Text = "Selecteer een Excel-kabellijst. PDF-vergelijking volgt later.";
    }

    private void UpdateState()
    {
        _readExcel.Enabled = !_reading && _excelDrop.FilePath != null;
        _excelDrop.Enabled = !_reading;
    }

    private async Task ShowCablesAsync()
    {
        var path = _excelDrop.FilePath!;
        _reading = true;
        UpdateState();
        UseWaitCursor = true;
        _status.Text = "Excel lezen...";
        _grid.Columns.Clear();
        try
        {
            var cables = await Task.Run(() =>
            {
                var settings = ExcelSettings.Load(Path.Combine(AppContext.BaseDirectory, "excel-instellingen.json"));
                return ExcelReader.Read(path, settings);
            });
            foreach (var header in new[] { "Kabelnaam", "Kabeltype", "Diameter", "Aders", "Bronkast", "Doelkast", "Tabblad", "Rij" })
                _grid.Columns.Add(header, header);
            foreach (var cable in cables)
                _grid.Rows.Add(cable.Kabelnaam, cable.Kabeltype, cable.Diameter, cable.Aders, cable.Bronkast, cable.Doelkast, cable.Tabblad, cable.Rij);
            _status.Text = $"{cables.Count} kabelrijen gelezen. PDF-vergelijking volgt later.";
        }
        catch (Exception ex)
        {
            _status.Text = "Fout: " + ex.Message;
            MessageBox.Show(this, ex.Message, "Fout bij lezen Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            _reading = false;
            UseWaitCursor = false;
            UpdateState();
        }
    }
}
