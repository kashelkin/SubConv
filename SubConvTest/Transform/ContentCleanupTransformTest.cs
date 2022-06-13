using SubConv.Data;
using SubConv.Transform;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace SubConvTest.Transform;

public class ContentCleanupTransformTest : BaseTransformTest
{
    [Theory]
    [InlineData("abc  def", "abc def")]
    [InlineData("abc    def", "abc def")]
    [InlineData(" abc  ", "abc")]
    [InlineData("   abc   def  ", "abc def")]
    [InlineData("abc\n def", "abc\ndef")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods",
        Justification = "Content value declared in InlineData attribute")]
    public void Trim_Redundant_Spaces(string content, string expected)
    {
        var sut = new ContentCleanupTransform();
        var entry = new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 15), content.EnvNewLine());

        var result = sut.Transform(ToEnumerable(entry));

        Assert.Collection(result, e => e
            .HasContent(expected.EnvNewLine()));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods",
        Justification = "Content value declared in InlineData attribute")]
    public void Remove_Empty_Entries(string content)
    {
        var sut = new ContentCleanupTransform();
        var entry = new SubtitleEntry(new TimeSpan(0, 1, 10), new TimeSpan(0, 1, 15), content.EnvNewLine());

        var result = sut.Transform(ToEnumerable(entry));

        Assert.Empty(result);
    }
}