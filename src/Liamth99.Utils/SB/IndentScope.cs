using System;

namespace Liamth99.Utils.SB;

internal sealed class IndentScope : IDisposable
{
    private readonly IndentedStringBuilder _parent;
    private          bool                  _disposed;

    public IndentScope(IndentedStringBuilder parent)
    {
        _parent = parent;
        _parent.IndentLevel++;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _parent.IndentLevel--;
        _disposed = true;
    }
}