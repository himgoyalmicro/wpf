// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Windows.Markup.Tests;

public class TrimSurroundingWhitespaceAttributeTests
{
    [Fact]
    public void Ctor_Default()
    {
        new TrimSurroundingWhitespaceAttribute();
    }
}