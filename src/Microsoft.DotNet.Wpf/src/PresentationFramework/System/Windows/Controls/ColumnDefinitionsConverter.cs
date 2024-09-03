// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// 
//
// Description: Contains the ThicknessConverter: TypeConverter for the Thicknessclass.
//
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Controls;
using System.Security;
using MS.Internal;
using MS.Utility;

#pragma warning disable 1634, 1691  // suppressing PreSharp warnings

namespace System.Windows.Controls
{
    /// <summary>
    /// ThicknessConverter - Converter class for converting instances of other types to and from Thickness instances.
    /// </summary> 
    public class ColumnDefinitionsConverter : TypeConverter
    {
        #region Public Methods

        /// <summary>
        /// CanConvertFrom - Returns whether or not this class can convert from a given type.
        /// </summary>
        /// <returns>
        /// bool - True if thie converter can convert from the provided type, false if not.
        /// </returns>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="sourceType"> The Type being queried for support. </param>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// CanConvertTo - Returns whether or not this class can convert to a given type.
        /// </summary>
        /// <returns>
        /// bool - True if this converter can convert to the provided type, false if not.
        /// </returns>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="destinationType"> The Type being queried for support. </param>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }


        /// <summary>
        /// ConvertFrom - Attempt to convert to a Thickness from the given object
        /// </summary>
        /// <returns>
        /// The Thickness which was constructed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the example object is not null and is not a valid type
        /// which can be converted to a Thickness.
        /// </exception>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="cultureInfo"> The CultureInfo which is respected when converting. </param>
        /// <param name="source"> The object to convert to a Thickness. </param>
        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value)
        {
            object k = typeDescriptorContext.Instance;
            if (value is string inputString)
            {
                return new ColumnDefinitionCollection(inputString);
            }

            return base.ConvertFrom(typeDescriptorContext, cultureInfo, value);
        }

        /// <summary>
        /// ConvertTo - Attempt to convert a Thickness to the given type
        /// </summary>
        /// <returns>
        /// The object which was constructoed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// An ArgumentNullException is thrown if the example object is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// An ArgumentException is thrown if the object is not null and is not a Thickness,
        /// or if the destinationType isn't one of the valid destination types.
        /// </exception>
        /// <param name="typeDescriptorContext"> The ITypeDescriptorContext for this call. </param>
        /// <param name="cultureInfo"> The CultureInfo which is respected when converting. </param>
        /// <param name="value"> The Thickness to convert. </param>
        /// <param name="destinationType">The type to which to convert the Thickness instance. </param>
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ColumnDefinitionCollection columnDefinitions)
            {
                return ConvertColumnDefinitionsToString(columnDefinitions);
            }

            return base.ConvertTo(typeDescriptorContext, cultureInfo, value, destinationType);

        }


        #endregion Public Methods

        //-------------------------------------------------------------------
        //
        //  Internal Methods
        //
        //-------------------------------------------------------------------

        #region Internal Methods

        static internal string ConvertColumnDefinitionsToString(ColumnDefinitionCollection columnDefinitions)
        {
            var parts = new string[columnDefinitions.Count];
            
            for (int i = 0; i < columnDefinitions.Count; i++)
            {
                parts[i] = columnDefinitions[i].Width.ToString();
            }

            return string.Join(",", parts);
        }

    #endregion

    }
}
