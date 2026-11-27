namespace Kabellijster;

public class DropPanel : Panel
{
    private readonly Label _title = new() { Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.BottomCenter, Font = new Font("Segoe UI", 14, FontStyle.Bold) };
    private readonly Label _hint = new() { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10), AutoEllipsis = true };
    private readonly string _extension;
    private readonly string _filter;
    private readonly string _prompt;

    public string? FilePath { get; private set; }
    public event EventHandler? FileChanged;

    public DropPanel(string title, string extension, string filter)
    {
        _extension = extension; _filter = filter;
        _prompt = $"Sleep hier een {extension}-bestand naartoe\nof klik om te bladeren";
        AllowDrop = true;
        BorderStyle = BorderStyle.FixedSingle;
        Padding = new Padding(10);
        Cursor = Cursors.Hand;
        _title.Text = title;
        Controls.Add(_hint);
        Controls.Add(_title);
        foreach (Control c in new Control[] { _title, _hint }) { c.Click += (_, _) => Browse(); c.Cursor = Cursors.Hand; }
        Click += (_, _) => Browse();
        DragEnter += OnDragEnter;
        DragLeave += (_, _) => UpdateLook(false);
        DragDrop += OnDragDrop;
        UpdateLook(false);
    }

    private string? ExtractFile(DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files && files.Length == 1 &&
            string.Equals(Path.GetExtension(files[0]), _extension, StringComparison.OrdinalIgnoreCase))
            return files[0];
        return null;
    }

    private void OnDragEnter(object? s, DragEventArgs e)
    {
        var ok = ExtractFile(e) != null;
        e.Effect = ok ? DragDropEffects.Copy : DragDropEffects.None;
        UpdateLook(ok);
    }

    private void OnDragDrop(object? s, DragEventArgs e)
    {
        var f = ExtractFile(e);
        if (f != null) SetFile(f); else UpdateLook(false);
    }

    private void Browse()
    {
        using var dlg = new OpenFileDialog { Filter = _filter };
        if (dlg.ShowDialog() == DialogResult.OK) SetFile(dlg.FileName);
    }

    private void SetFile(string path)
    {
        FilePath = path;
        UpdateLook(false);
        FileChanged?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateLook(bool hover)
    {
        if (hover) BackColor = Color.FromArgb(200, 230, 255);
        else BackColor = FilePath == null ? Color.FromArgb(245, 245, 245) : Color.FromArgb(215, 240, 215);
        _hint.Text = FilePath == null ? _prompt : "✔ " + Path.GetFileName(FilePath);
    }
}
