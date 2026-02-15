
using Liamth99.Utils.SB;
using Shouldly;

namespace Utils.Tests;

public class IndentedStringBuilderTests
{
    [Fact]
    public void Append_ShouldAddTextWithoutIndent_WhenIndentLevelIsZero()
    {
        var sb = new IndentedStringBuilder();

        sb.Append("Hello");

        sb.ToString().ShouldBe("Hello");
    }

    [Fact]
    public void AppendLine_ShouldAddNewLineAndResetLineState_WhenCalled()
    {
        var sb = new IndentedStringBuilder();

        sb.AppendLine("Hello");
        sb.Append("World");

        sb.ToString().ShouldBe($"Hello{Environment.NewLine}World");
    }

    [Fact]
    public void Append_ShouldIndentText_WhenIndentLevelIsGreaterThanZero()
    {
        var sb = new IndentedStringBuilder();
        sb.IndentLevel = 1;

        sb.Append("Hello");

        sb.ToString().ShouldBe("    Hello");
    }

    [Fact]
    public void AppendLine_ShouldIndentEachLine_WhenIndentLevelIsGreaterThanZero()
    {
        var sb = new IndentedStringBuilder();
        sb.IndentLevel = 1;

        sb.AppendLine("Hello");
        sb.AppendLine("World");

        var expected = $"    Hello{Environment.NewLine}    World{Environment.NewLine}";

        sb.ToString().ShouldBe(expected);
    }

    [Fact]
    public void Append_ShouldIndentEachLine_When_ValueContainsMultipleLines()
    {
        var sb = new IndentedStringBuilder();
        sb.IndentLevel = 1;

        sb.Append($"Hello{Environment.NewLine}World");

        var expected = $"    Hello{Environment.NewLine}    World";

        sb.ToString().ShouldBe(expected);
    }

    [Fact]
    public void Indent_ShouldIncreaseIndentLevelWithinScope_WhenUsedInUsingBlock()
    {
        var sb = new IndentedStringBuilder();

        using (sb.Indent())
        {
            sb.IndentLevel.ShouldBe(1);
            sb.Append("Hello");
        }

        sb.IndentLevel.ShouldBe(0);
    }

    [Fact]
    public void Indent_ShouldRestorePreviousIndentLevel_WhenNestedScopesAreDisposed()
    {
        var sb = new IndentedStringBuilder();

        using (sb.Indent())
        {
            sb.IndentLevel.ShouldBe(1);

            using (sb.Indent())
            {
                sb.IndentLevel.ShouldBe(2);
            }

            sb.IndentLevel.ShouldBe(1);
        }

        sb.IndentLevel.ShouldBe(0);
    }

    [Fact]
    public void Clear_ShouldResetIndentLevelAndContent_WhenCalled()
    {
        var sb = new IndentedStringBuilder();

        sb.IndentLevel = 2;
        sb.AppendLine("Test");

        sb.Clear();

        sb.ToString().ShouldBe(string.Empty);
        sb.IndentLevel.ShouldBe(0);
    }

    [Fact]
    public void Append_ShouldIgnoreValue_WhenStringIsNullOrEmpty()
    {
        var sb = new IndentedStringBuilder();

        sb.Append(null);
        sb.Append(string.Empty);

        sb.ToString().ShouldBe(string.Empty);
    }

    [Fact]
    public void Append_ShouldUseCustomIndentString_WhenProvidedInConstructor()
    {
        var sb = new IndentedStringBuilder("--");
        sb.IndentLevel = 2;

        sb.Append("Hello");

        sb.ToString().ShouldBe("----Hello");
    }

    [Fact]
    public void IndentBlock_ShouldWriteHeaderAndOpenBlockAndIncrementIndent_WhenCalled()
    {
        var sb = new IndentedStringBuilder();

        using var scope = sb.IndentBlock("if (true)");

        sb.IndentLevel.ShouldBe(1);

        var expected =
        """
        if (true)
        {

        """;

        sb.ToString().ShouldBe(expected);
    }

    [Fact]
    public void IndentBlock_ShouldIndentContentInsideBlock_WhenContentIsAppended()
    {
        var sb = new IndentedStringBuilder();

        using (sb.IndentBlock("if (true)"))
        {
            sb.AppendLine("Console.WriteLine(\"Hi\");");
        }

        var expected =
        """
        if (true)
        {
            Console.WriteLine("Hi");
        }

        """;

        sb.ToString().ShouldBe(expected);
    }

    [Fact]
    public void IndentBlock_ShouldUseCustomDelimiters_WhenProvided()
    {
        var sb = new IndentedStringBuilder();

        using (sb.IndentBlock("BEGIN", "[", "]"))
        {
            sb.AppendLine("Inside");
        }

        var expected =
        """
        BEGIN
        [
            Inside
        ]

        """;

        sb.ToString().ShouldBe(expected);
    }

    [Fact]
    public void IndentBlock_ShouldHandleNestedBlocks_WhenBlocksAreNested()
    {
        var sb = new IndentedStringBuilder();

        using (sb.IndentBlock("if (true)"))
        {
            using (sb.IndentBlock("while (false)"))
            {
                sb.AppendLine("DoWork();");
            }
        }

        var expected =
        """
        if (true)
        {
            while (false)
            {
                DoWork();
            }
        }

        """;

        sb.ToString().ShouldBe(expected);
    }
}
