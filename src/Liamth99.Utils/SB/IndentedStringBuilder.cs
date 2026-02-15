using System.Text;

namespace Liamth99.Utils.SB;

/// <summary>
/// A utility class for building strings with support for custom indentation and line formatting.
/// </summary>
public sealed class IndentedStringBuilder
{
    private readonly StringBuilder _sb;
    private bool _atStartOfLine = true;

    /// Gets the string used for indentation at each level in the <see cref="IndentedStringBuilder"/>.
    public string IndentString { get; }

    /// Gets or sets the current indentation level for the <see cref="IndentedStringBuilder"/> instance.
    /// Each level corresponds to one repetition of the <see cref="IndentString"/> value.
    public int IndentLevel { get; set; }

    ///
    public IndentedStringBuilder(string indentString = "    ")
    {
        _sb          = new StringBuilder();
        IndentString = indentString ?? throw new ArgumentNullException(nameof(indentString));
    }

    ///
    public IndentedStringBuilder(StringBuilder stringBuilder, string indentString = "    ")
    {
        _sb          = stringBuilder;
        IndentString = indentString ?? throw new ArgumentNullException(nameof(indentString));
    }

    /// <summary>
    /// Increases the indentation level for the associated <see cref="IndentedStringBuilder"/>
    /// instance and returns a disposable object to manage the scope of the indentation.
    /// The indentation level is automatically reduced when the returned object is disposed.
    /// </summary>
    public IDisposable Indent()
    {
        return new IndentScope(this);
    }

    /// <summary>
    /// Creates and returns a disposable object that manages a block of text with custom
    /// indentation, adding a header and open/close delimiters around the block.
    /// </summary>
    /// <param name="header">The header text to prepend before the indented block.</param>
    /// <param name="open">The opening delimiter for the block. Defaults to "{".</param>
    /// <param name="close">The closing delimiter for the block. Defaults to "}".</param>
    public IDisposable IndentBlock(string header, string open = "{", string close = "}")
    {
        return new IndentBlockScope(this, header, open, close);
    }

    private void WriteIndentIfNeeded()
    {
        if (_atStartOfLine)
        {
            for (int i = 0; i < IndentLevel; i++)
            {
                _sb.Append(IndentString);
            }

            _atStartOfLine = false;
        }
    }

    /// <inheritdoc cref="StringBuilder.Append(string)"/>
    public IndentedStringBuilder Append(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return this;

        foreach (char c in value!)
        {
            WriteIndentIfNeeded();
            _sb.Append(c);

            if (c is '\n')
                _atStartOfLine = true;
        }

        return this;
    }

    /// <inheritdoc cref="StringBuilder.AppendLine()"/>
    public IndentedStringBuilder AppendLine()
    {
        _sb.AppendLine();
        _atStartOfLine = true;
        return this;
    }

    /// <inheritdoc cref="StringBuilder.AppendLine(string)"/>
    public IndentedStringBuilder AppendLine(string value)
    {
        Append(value);
        _sb.AppendLine();
        _atStartOfLine = true;
        return this;
    }

    /// <inheritdoc cref="StringBuilder.ToString()"/>
    public override string ToString()
    {
        return _sb.ToString();
    }

    /// <inheritdoc cref="StringBuilder.Clear()"/>
    public void Clear()
    {
        _sb.Clear();
        _atStartOfLine = true;
        IndentLevel    = 0;
    }
}