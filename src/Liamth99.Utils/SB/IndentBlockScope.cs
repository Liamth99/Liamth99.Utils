namespace Liamth99.Utils.SB;

internal sealed class IndentBlockScope : IDisposable
{
    private readonly IndentedStringBuilder _parent;
    private          bool                  _disposed;
    private readonly string                _close;

    public IndentBlockScope(IndentedStringBuilder parent, string header, string open = "{", string close = "}")
    {
        _parent = parent;
        _parent.AppendLine(header);
        _parent.AppendLine(open);
        _parent.IndentLevel++;
        _close = close;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _parent.IndentLevel--;
        _parent.AppendLine(_close);
        _disposed = true;
    }
}