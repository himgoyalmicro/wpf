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

public class RowDefinitionCollectionTests
{
    [WpfFact]
    public void SetOwnerToNull_WhenOwnerIsAlreadyNull_DoesNothing()
    {
        RowDefinitionCollection RDC = new RowDefinitionCollection();
        Assert.Null(RDC.Owner);
        RDC.Owner = null;
        Assert.Null(RDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToNull_WhenOwnerIsNotNull_OwnerValueGetsUpdatedToNull()
    {
        Grid grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition());
        RowDefinitionCollection RDC = grid.RowDefinitions;
        Assert.Equal(grid, RDC.Owner);
        RDC.Owner = null;
        Assert.Null(RDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToNotNull_WhenOwnerIsAlreadyNull_OwnerValueGetsUpdated()
    {
        RowDefinitionCollection RDC = new RowDefinitionCollection();
        Assert.Null(RDC.Owner);
        Grid grid = new Grid();
        RDC.Owner = grid;
        Assert.Equal(grid, RDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToSameValueAsBefore_DoesNothing()
    {
        Grid grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition());
        RowDefinitionCollection RDC = grid.RowDefinitions;
        Assert.Equal(grid, RDC.Owner);
        RDC.Owner = grid;
        Assert.Equal(grid, RDC.Owner);
    }

    [WpfFact]
    public void SetOwnerToNotNull_WhenOwnerIsAlreadyNotNull_ThrowsException()
    {
        Grid grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition());
        RowDefinitionCollection RDC = grid.RowDefinitions;
        Assert.Equal(grid, RDC.Owner);
        Assert.Throws<ArgumentException>(() => RDC.Owner = new Grid());
    }

    [WpfFact]
    public void RowDefinitionCollections_ShouldNotShareSameOwningGrid()
    {
        Grid grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition());
        RowDefinitionCollection RDC = new RowDefinitionCollection()
        {
            Owner = grid
        };
        //RDC.Owner = grid;
        Assert.Equal(grid, RDC.Owner);
    }
}