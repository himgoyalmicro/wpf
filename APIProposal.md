# Background and motivation

Based on discussion in 5612 and 1739 this proposal is to simplify the definition syntax of Rows and Columns by:

- Allowing rows and columns within a Grid to be defined by a collection that is delimited by commas and single quotes.
- Creating a Typeconvertor for ColumnDefinitionCollection and RowDefinitionCollection so they can process String as its input.

## Goal
The goal of this feature is to make Grid easier and more intuitive to write and learn and minimize the learning curve required to use Grid, one of the most widely used controls.

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
}

namespace System.Windows.Controls
{
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
```csharp
[TypeConverter(typeof(ColumnDefinitionCollectionConverter))]
[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
public ColumnDefinitionCollection ColumnDefinitions
{
    get
    {
        if (_data == null) { _data = new ExtendedData(); }
        if (_data.ColumnDefinitions == null) { _data.ColumnDefinitions = new ColumnDefinitionCollection(this); }

        return (_data.ColumnDefinitions);
    }
    set
    {
        if (value == null){
            _data.ColumnDefinitions = new ColumnDefinitionCollection(this);
            return;
        }
        _data ??= new ExtendedData();
        _data.ColumnDefinitions = value;
    }
}

[TypeConverter(typeof(RowDefinitionCollectionConverter))]
[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
public RowDefinitionCollection RowDefinitions
{
    get
    {
        if (_data == null) { _data = new ExtendedData(); }
        if (_data.RowDefinitions == null) { _data.RowDefinitions = new RowDefinitionCollection(this); }

        return (_data.RowDefinitions);
    }
    set
    {
        if (value == null){
             _data.RowDefinitions = new RowDefinitionCollection(this);
            return;
        }
        _data ??= new ExtendedData();
        _data.RowDefinitions = value;
    }
}

```

# API Usage
```xaml
<Grid ColumnDefinitions="1*, 2*, Auto, *, 300" RowDefinitions="1*, Auto, 25, 14, 20"> </Grid>
```

# Alternative design

# Risks
