#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
#endif

using Liamth99.Utils.Extensions;
using Shouldly;

namespace Utils.Tests;

public class EnumExtensionsTests
{
    private enum TestEnum
    {
#if NET8_0_OR_GREATER
        [Display(Name = "First Value")]
#endif
        [Test]
        First,

#if NET8_0_OR_GREATER
        [Display(Name = "Second Value")]
#endif
        [Test]
        Second,

        NoAttribute
    }

    private class TestAttribute : Attribute;

    [Fact]
    public void GetSingleAttributeOfType_ShouldReturnAttribute_WhenAttributeExists()
    {
        // Act
        var attribute = TestEnum.First.GetSingleAttributeOfType<TestAttribute>();

        // Assert
        attribute.ShouldNotBeNull();
    }

    [Fact]
    public void GetSingleAttributeOfType_ShouldReturnNull_WhenAttributeDoesNotExist()
    {
        // Act
        var attribute = TestEnum.NoAttribute.GetSingleAttributeOfType<TestAttribute>();

        // Assert
        attribute.ShouldBeNull();
    }

    [Fact]
    public void TryGetSingleAttributeOfType_ShouldReturnTrueAndSetAttribute_WhenAttributeExists()
    {
        // Act
        var result = TestEnum.Second.TryGetSingleAttributeOfType<TestAttribute>(out var attribute);

        // Assert
        result.ShouldBeTrue();
        attribute.ShouldNotBeNull();
    }

    [Fact]
    public void TryGetSingleAttributeOfType_ShouldReturnFalseAndSetNull_WhenAttributeDoesNotExist()
    {
        // Act
        var result = TestEnum.NoAttribute.TryGetSingleAttributeOfType<TestAttribute>(out var attribute);

        // Assert
        result.ShouldBeFalse();
        attribute.ShouldBeNull();
    }

#if NET8_0_OR_GREATER
    [Fact]
    public void DisplayName_ShouldReturnDisplayName_WhenDisplayAttributeExists()
    {
        // Act
        var displayName = TestEnum.First.DisplayName();

        // Assert
        displayName.ShouldBe("First Value");
    }

    [Fact]
    public void DisplayName_ShouldReturnEnumName_WhenDisplayAttributeDoesNotExist()
    {
        // Act
        var displayName = TestEnum.NoAttribute.DisplayName();

        // Assert
        displayName.ShouldBe(nameof(TestEnum.NoAttribute));
    }
#endif
}