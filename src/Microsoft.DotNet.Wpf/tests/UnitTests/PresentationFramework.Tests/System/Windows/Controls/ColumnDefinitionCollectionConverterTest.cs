// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#pragma warning disable IDE0005
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Globalization;
using System.Text;
using MS.Internal;
using System.ComponentModel.Design.Serialization;
using System.Collections.Generic;
#pragma warning restore IDE0005

namespace System.Windows.Controls.Tests;

public class ColumnDefinitionCollectionConverterTests
{
    public static IEnumerable<object?[]> CanConvertTo_TestData()
    {
        yield return new object?[] { null, null, false };
        yield return new object?[] { null, typeof(object), false };
        yield return new object?[] { null, typeof(string), false };
        yield return new object?[] { new CustomTypeDescriptorContext(), null, false };
        yield return new object?[] { new CustomTypeDescriptorContext(), typeof(object), false };
        yield return new object?[] { new CustomTypeDescriptorContext(), typeof(string), false };
        yield return new object?[] { new CustomTypeDescriptorContext { Instance = new object() }, null, false };
        yield return new object?[] { new CustomTypeDescriptorContext { Instance = new object() }, typeof(object), false };
        yield return new object?[] { new CustomTypeDescriptorContext { Instance = new object() }, typeof(string), false };
    }

    [Theory]
    [MemberData(nameof(CanConvertTo_TestData))]
    public void CanConvertTo_Invoke_ReturnsExpected(ITypeDescriptorContext context, Type destinationType, bool expected)
    {
        var converter = new ColumnDefinitionCollectionConverter();
        Assert.Equal(expected, converter.CanConvertTo(context, destinationType));
    }

    public static IEnumerable<object?[]> CanConvertFrom_TestData()
    {
        yield return new object?[] { null, null, false };
        yield return new object?[] { null, typeof(object), false };
        yield return new object?[] { null, typeof(string), true };
        yield return new object?[] { new CustomTypeDescriptorContext(), null, false };
        yield return new object?[] { new CustomTypeDescriptorContext(), typeof(object), false };
        yield return new object?[] { new CustomTypeDescriptorContext(), typeof(string), true };
        yield return new object?[] { new CustomTypeDescriptorContext { Instance = new object() }, null, false };
        yield return new object?[] { new CustomTypeDescriptorContext { Instance = new object() }, typeof(object), false };
        yield return new object?[] { new CustomTypeDescriptorContext { Instance = new object() }, typeof(string), true };
    }

    [Theory]
    [MemberData(nameof(CanConvertFrom_TestData))]
    public void CanConvertFrom_Invoke_ReturnsExpected(ITypeDescriptorContext context, Type sourceType, bool expected)
    {
        var converter = new ColumnDefinitionCollectionConverter();
        Assert.Equal(expected, converter.CanConvertFrom(context, sourceType));
    }

    public static IEnumerable<object[]> ConvertFrom_TestData()
    {
        List<string> possibleInputs = new List<string> { "100", "Auto", "*", "2*" };
        int count = possibleInputs.Count;

        for (int i = 1; i < (1 << count); i++)
        {
            List<string> subset = new List<string>();
            ColumnDefinitionCollection expectedOutput = new ColumnDefinitionCollection();

            for (int j = 0; j < count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    expectedOutput.Add(new ColumnDefinition { Width = GridLengthConverter.FromString(possibleInputs[j], CultureInfo.InvariantCulture) });
                    subset.Add(possibleInputs[j]);
                }
            }

            if (subset.Count > 1)
            {
                yield return new object[] { string.Join(", ", subset), expectedOutput };
                yield return new object[] { string.Join(" ", subset), expectedOutput };
            }
            else
            {
                yield return new object[] { subset[0], expectedOutput };
            }
        }
    }

    private static bool CompareColumnDefinitions(ColumnDefinitionCollection col1, object? col)
    {
        if (col is not ColumnDefinitionCollection col2)
            return false;
        
        if (col1.Count != col2.Count)
            return false;

        for (int i = 0; i < col1.Count; i++)
        {
            if (col1[i].Width != col2[i].Width ||
                col1[i].MinWidth != col2[i].MinWidth ||
                col1[i].MaxWidth != col2[i].MaxWidth)
            {
                return false;
            }
        }
        return true;
    }

    [Theory]
    [MemberData(nameof(ConvertFrom_TestData))]
    public void ConvertFrom_ReturnsExpected(string value, ColumnDefinitionCollection expected)
    {
        var converter = new ColumnDefinitionCollectionConverter();
        bool result = CompareColumnDefinitions(expected, converter.ConvertFrom(value));
        Assert.True(result, "Expected Column Definitions to be same.");
        result = CompareColumnDefinitions(expected, converter.ConvertFrom(null, null, value));
        Assert.True(result, "Expected Column Definitions to be same.");
        result = CompareColumnDefinitions(expected, converter.ConvertFrom(new CustomTypeDescriptorContext(), CultureInfo.InvariantCulture, value));
        Assert.True(result, "Expected Column Definitions to be same.");
    }

    [Fact]
    public void ConvertFrom_NullValue_ThrowsNotSupportedException()
    {
        var converter = new ColumnDefinitionCollectionConverter();
        Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(null!));
        Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(null, null, null));
        Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(new CustomTypeDescriptorContext(), CultureInfo.InvariantCulture, null));
    }

    private class CustomTypeDescriptorContext : ITypeDescriptorContext
    {
        public IContainer Container => throw new NotImplementedException();

        private object? _instance;

        public object Instance
        {
            get => _instance!;
            set => _instance = value;
        }

        public PropertyDescriptor PropertyDescriptor => throw new NotImplementedException();

        public object? GetService(Type serviceType) => throw new NotImplementedException();

        public void OnComponentChanged() => throw new NotImplementedException();

        public bool OnComponentChanging() => throw new NotImplementedException();
    }
}