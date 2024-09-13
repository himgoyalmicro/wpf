# Background and motivation

Based on discussion in 5612 and 1739 this proposal is to simplify the definition syntax of RowDefinitions and ColumnDefinitions by:

- Allowing rows and columns within a Grid to be defined by a collection that is delimited by commas.
- Creating a Typeconvertor for ColumnDefinitionCollection and RowDefinitionCollection so they can process String as its input.

## Goal
The goal of this feature is to make Grid easier and more intuitive to write and learn. 

## Example
### Current Syntax
Defining columns and rows is overly tedious, repetitive and not very productive, as is shown below:
```
<Grid>
    <Grid.ColumnDefinitions>
          <ColumnDefinition Width="1*" />
          <ColumnDefinition Width="2*" />
          <ColumnDefinition Width="Auto" />
          <ColumnDefinition Width="*" />
          <ColumnDefinition Width="300" />
    </Grid.ColumnDefinitions>
    <Grid.RowDefinitions>
          <RowDefinition Height="1*" />
          <RowDefinition Height="Auto" />
          <RowDefinition Height="25" />
          <RowDefinition Height= "14" />
          <RowDefinition Height="20" />
    </Grid.RowDefinitions>
</Grid>
```
### Proposed Syntax
The same functionality as above with the proposed succinct syntax is shown below:
```
<Grid ColumnDefinitions="1*, 2*, Auto, *, 300" RowDefinitions="1*, Auto, 25, 14, 20"> </Grid>
```

## Scope
| Feature     | Priority      |
| -------------- | -------------- |
| Ability for rows and columns to be defined as a collection or list of values | Must |
| Content properties of RowDefinition and ColumnDefinition are set to Height and Width, respectively | Must |
| Original syntax will still be fully supported and functional | Must |
| Include support for min/max height/width | Won't |
| Include support for data binding within definitions | Won't |


# API Proposal
We will implement Type Converter for ColumnDefinitionCollection and RowDefinitionCollection. This will allow us to convert string input into the corresponding collection.  
```csharp
namespace System.Windows.Controls
{
     public partial class ColumnDefinitionCollectionConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext typeDescriptorContext, System.Type sourceType) { throw null; }        
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType) { throw null; }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext typeDescriptorContext, System.Globalization.CultureInfo cultureInfo, object source) { throw null; }        
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType) { throw null; }
    }

     public partial class RowDefinitionCollectionConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext typeDescriptorContext, System.Type sourceType) { throw null; }        
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType) { throw null; }
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext typeDescriptorContext, System.Globalization.CultureInfo cultureInfo, object source) { throw null; }        
        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType) { throw null; }
    }
}
```
We will also introduce a setter method for ColumnDefinitons and RowDefinitions of Grid. 
```diff
+[TypeConverter(typeof(ColumnDefinitionCollectionConverter))]
[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
public ColumnDefinitionCollection ColumnDefinitions
{
    get { ... }
+    set { ... }
}

+[TypeConverter(typeof(RowDefinitionCollectionConverter))]
[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
public RowDefinitionCollection RowDefinitions
{
    get { ... }
+    set { ... }
}

```

# API Usage
```xaml
<Grid ColumnDefinitions="1*, 2*, Auto, *, 300" RowDefinitions="1*, Auto, 25, 14, 20"> </Grid>
```

# Alternative design
We can introduce a new public Dependency property ColumnDefinitionsInline and RowDefinitionsInline to update the RowDefinitions and ColumnDefinitions. 

```csharp
public string ColumnDefinitionsInline
{
    get { ... }
    set { ... }
}

public string RowDefinitionsInline
{
    get { ... }
    set { ... }
}

public static readonly DependencyProperty ColumnDefinitionsInlineProperty =
    DependencyProperty.Register(
        nameof(ColumnDefinitionsInline),
        typeof(string),
        typeof(Grid),
        new FrameworkPropertyMetadata(null, OnColumnDefinitionsInlineChanged));

public static readonly DependencyProperty RowDefinitionsInlineProperty =
    DependencyProperty.Register(
        nameof(RowDefinitionsInline),
        typeof(string),
        typeof(Grid),
        new FrameworkPropertyMetadata(null, OnRowDefinitionsInlineChanged));
```

# Risks
