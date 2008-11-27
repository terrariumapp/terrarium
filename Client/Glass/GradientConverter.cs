using System;
using System.ComponentModel;

namespace Terrarium.Glass
{
    internal class GradientConverter : ExpandableObjectConverter
    {
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context,
                                                                   object value,
                                                                   Attribute[] filter)
        {
            return TypeDescriptor.GetProperties(value, filter);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}