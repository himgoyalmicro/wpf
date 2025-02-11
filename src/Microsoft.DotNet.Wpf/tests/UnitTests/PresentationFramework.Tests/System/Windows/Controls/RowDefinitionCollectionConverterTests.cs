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

public class RowDefinitionCollectionConverterTests
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
        var converter = new RowDefinitionCollectionConverter();
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
        var converter = new RowDefinitionCollectionConverter();
        Assert.Equal(expected, converter.CanConvertFrom(context, sourceType));
    }

    public static IEnumerable<object[]> ConvertFrom_TestData()
    {
        // Valid possible inputs for Row Definition
        List<string> possibleInputs = new List<string> { "100", "Auto", "*", "2*" };
        int count = possibleInputs.Count;

        // Generate all possible combinations of inputs
        for (int i = 1; i < (1 << count); i++)
        {
            List<string> subset = new List<string>();
            RowDefinitionCollection expectedOutput = new RowDefinitionCollection();

            for (int j = 0; j < count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    // Add the Row Definition to the expected output
                    expectedOutput.Add(new RowDefinition { Height = GridLengthConverter.FromString(possibleInputs[j], CultureInfo.InvariantCulture) });
                    subset.Add(possibleInputs[j]);
                }
            }

            if (subset.Count > 1)
            {
                yield return new object[] { string.Join(", ", subset), expectedOutput }; // comma separated input string 
                yield return new object[] { string.Join(" ", subset), expectedOutput }; // space separated input string
            }
            else
            {
                yield return new object[] { subset[0], expectedOutput };
            }
        }
    }

    private static bool CompareRowDefinitions(RowDefinitionCollection col1, object? col)
    {
        if (col is not RowDefinitionCollection col2)
            return false;
        
        if (col1.Count != col2.Count)
            return false;

        for (int i = 0; i < col1.Count; i++)
        {
            if (col1[i].Height != col2[i].Height ||
                col1[i].MinHeight != col2[i].MinHeight ||
                col1[i].MaxHeight != col2[i].MaxHeight)
            {
                return false;
            }
        }
        return true;
    }

    [Theory]
    [MemberData(nameof(ConvertFrom_TestData))]
    public void ConvertFrom_ReturnsExpected(string value, RowDefinitionCollection expected)
    {
        var converter = new RowDefinitionCollectionConverter();
        bool result = CompareRowDefinitions(expected, converter.ConvertFrom(value));
        Assert.True(result, "Expected Row Definitions to be same.");
        result = CompareRowDefinitions(expected, converter.ConvertFrom(null, null, value));
        Assert.True(result, "Expected Row Definitions to be same.");
        result = CompareRowDefinitions(expected, converter.ConvertFrom(new CustomTypeDescriptorContext(), CultureInfo.InvariantCulture, value));
        Assert.True(result, "Expected Row Definitions to be same.");
    }

    [Fact]
    public void ConvertFrom_NullValue_ThrowsNotSupportedException()
    {
        var converter = new RowDefinitionCollectionConverter();
        Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(null!));
        Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(null, null, null));
        Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(new CustomTypeDescriptorContext(), CultureInfo.InvariantCulture, null));
    }

    public static IEnumerable<object[]> ConvertFrom_InvalidValue_TestData()
    {
        yield return new object[] { "$$$$" };
        yield return new object[] { "notValid" };
        yield return new object[] { "Auto $$" };
        yield return new object[] { "****" };
    }

    [Theory]
    [MemberData(nameof(ConvertFrom_InvalidValue_TestData))]
    public void ConvertFrom_InvokeInvalidValue_ThrowsNotSupportedException(string value)
    {
        var converter = new RowDefinitionCollectionConverter();
        Assert.Throws<FormatException>(() => converter.ConvertFrom(value));
        Assert.Throws<FormatException>(() => converter.ConvertFrom(null, null, value));
        Assert.Throws<FormatException>(() => converter.ConvertFrom(new CustomTypeDescriptorContext(), CultureInfo.InvariantCulture, value));
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