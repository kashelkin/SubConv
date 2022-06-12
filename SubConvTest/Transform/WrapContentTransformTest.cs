using SubConv.Data;
using SubConv.Transform;
using System;
using System.Collections.Generic;
using Xunit;

namespace SubConvTest.Transform
{
    public class WrapContentTransformTest : BaseTransformTest
    {
        [Theory]
        [InlineData("Default entry", "Default", "Default entry")]
        [InlineData("Name entry", "Names", "[Name entry]")]
        [InlineData("Null entry", null, "Null entry")]
        public void Applies_Content_Transform_To_Single_Style(string content, string? style, string expected)
        {
            var entry = new SubtitleEntry(
                new TimeSpan(2, 10, 12),
                new TimeSpan(2, 10, 15),
                content,
                style);

            var sut = new WrapContentTransform("Names", "[", "]");

            var result = sut.Transform(ToEnumerable(entry));

            Assert.Collection(result, e => e
                .WithStart(2, 10, 12)
                .WithEnd(2, 10, 15)
                .WithStyle(style)
                .WithContent(expected));
        }

        [Theory]
        [InlineData("Default entry", "Default", "Default entry")]
        [InlineData("Name entry", "Names", "[Name entry]")]
        [InlineData("SmallName entry", "SmallNames", "[SmallName entry]")]
        [InlineData("Null entry", null, "Null entry")]
        public void Applies_Content_Transform_To_Multiple_Styles(string content, string? style, string expected)
        {
            var entry = new SubtitleEntry(
                new TimeSpan(2, 10, 12),
                new TimeSpan(2, 10, 15),
                content,
                style);

            var sut = new WrapContentTransform("Names,SmallNames", "[", "]");

            var result = sut.Transform(ToEnumerable(entry));

            Assert.Collection(result, e => e
                .WithStart(2, 10, 12)
                .WithEnd(2, 10, 15)
                .WithStyle(style)
                .WithContent(expected));
        }

        [Theory]
        [InlineData("Default entry", "Default", "[Default entry]")]
        [InlineData("Name entry", "Names", "[Name entry]")]
        [InlineData("SmallName entry", "SmallNames", "[SmallName entry]")]
        [InlineData("Null entry", null, "[Null entry]")]
        public void Applies_Content_Transform_To_All_Styles(string content, string? style, string expected)
        {
            var entry = new SubtitleEntry(
                new TimeSpan(2, 10, 12),
                new TimeSpan(2, 10, 15),
                content,
                style);

            var sut = new WrapContentTransform("*", "[", "]");

            var result = sut.Transform(ToEnumerable(entry));

            Assert.Collection(result, e => e
                .WithStart(2, 10, 12)
                .WithEnd(2, 10, 15)
                .WithStyle(style)
                .WithContent(expected));
        }

        [Theory]
        [InlineData("Default entry", "Default", "[Default entry]")]
        [InlineData("[Default entry", "Default", "[Default entry]")]
        [InlineData("Default entry]", "Default", "[Default entry]")]
        [InlineData("[Default entry]", "Default", "[Default entry]")]
        public void Checks_If_Content_Already_Enclosed(string content, string? style, string expected)
        {
            var entry = new SubtitleEntry(
                new TimeSpan(2, 10, 12),
                new TimeSpan(2, 10, 15),
                content,
                style);

            var sut = new WrapContentTransform("Default", "[", "]");

            var result = sut.Transform(ToEnumerable(entry));

            Assert.Collection(result, e => e
                .WithStart(2, 10, 12)
                .WithEnd(2, 10, 15)
                .WithStyle(style)
                .WithContent(expected));
        }
    }
}
