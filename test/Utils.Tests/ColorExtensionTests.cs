using System.Drawing;
using Liamth99.Utils.Extensions;
using Shouldly;

namespace Utils.Tests;

public class ColorExtensionTests
{
    private readonly Color _color = Color.FromArgb(255, 34, 128, 200);

    [Fact]
    public void RRGGBB_Format()
    {
        _color.ToHex("RRGGBB").ShouldBe("2280C8");
    }

    [Fact]
    public void AARRGGBB_Format()
    {
        _color.ToHex("AARRGGBB").ShouldBe("FF2280C8");
    }

    [Fact]
    public void Reverse_Order()
    {
        _color.ToHex("BBGGRR").ShouldBe("C88022");
    }

    [Fact]
    public void Single_Token()
    {
        _color.ToHex("BGR").ShouldBe("C82");
    }

    [Fact]
    public void Other_Is_Treated_As_Literal()
    {
        _color.ToHex("#RR GG BB").ShouldBe("#22 80 C8");
    }

    [Fact]
    public void Triple_Token_Throws()
    {
        Should.Throw<FormatException>(() => _color.ToHex("RRR"))
              .Message.ShouldBe("Invalid token length 'RRR'. Only single or double channel tokens are allowed.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData(null)]
    public void Empty_Format_Throws(string? f)
    {
        Should.Throw<ArgumentException>(() => _color.ToHex(f!))
              .Message.ShouldBe("Format cannot be null or empty. (Parameter 'format')");
    }
}