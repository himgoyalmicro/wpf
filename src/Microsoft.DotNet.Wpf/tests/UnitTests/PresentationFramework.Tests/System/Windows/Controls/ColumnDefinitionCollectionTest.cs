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

public class ColumnDefinitionCollectionTests
{
    [WpfFact]
    public void SetOwnerToNull_WhenOwnerIsAlreadyNull_DoesNothing()
    {
        ColumnDefinitionCollection CDC = new ColumnDefinitionCollection();
        Assert.Null(CDC.Owner);
        CDC.Owner = null;
        Assert.Null(CDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToNull_WhenOwnerIsNotNull_OwnerValueGetsUpdatedToNull()
    {
        Grid grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        ColumnDefinitionCollection CDC = grid.ColumnDefinitions;
        Assert.Equal(grid, CDC.Owner);
        CDC.Owner = null;
        Assert.Null(CDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToNotNull_WhenOwnerIsAlreadyNull_OwnerValueGetsUpdated()
    {
        ColumnDefinitionCollection CDC = new ColumnDefinitionCollection();
        Assert.Null(CDC.Owner);
        Grid grid = new Grid();
        CDC.Owner = grid;
        Assert.Equal(grid, CDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToSameValueAsBefore_DoesNothing()
    {
        Grid grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        ColumnDefinitionCollection CDC = grid.ColumnDefinitions;
        Assert.Equal(grid, CDC.Owner);
        CDC.Owner = grid;
        Assert.Equal(grid, CDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToNotNull_WhenOwnerIsAlreadyNotNull_ThrowsException()
    {
        Grid grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        ColumnDefinitionCollection CDC = grid.ColumnDefinitions;
        Assert.Equal(grid, CDC.Owner);
        Assert.Throws<ArgumentException>(() => CDC.Owner = new Grid());
    }

    [WpfFact]
    public void ColumnDefinitionCollections_ShouldNotShareSameOwningGrid()
    {
        Grid grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        ColumnDefinitionCollection CDC = new ColumnDefinitionCollection();
        CDC.Add(new ColumnDefinition());
        CDC.Owner = grid;
        Assert.Equal(grid, CDC.Owner);
    }
}