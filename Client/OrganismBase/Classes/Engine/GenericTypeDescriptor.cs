//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace OrganismBase
{
    /// <summary>
    ///  A Custom Type Descriptor class used to expand the Organism,
    ///  OrganismState, and Species objects into the Property
    ///  Browser dialog.
    /// </summary>
    /// <internal/>
    [TypeConverter((typeof (ExpandableObjectConverter)))]
    public class GenericTypeDescriptor : ICustomTypeDescriptor
    {
        /// <summary>
        ///  The currently selected or visible object.
        /// </summary>
        private object currentObject;

        /// <summary>
        ///  Collection of property descriptors that define the visible
        ///  properties when the object is viewed in the property
        ///  grid.
        /// </summary>
        private PropertyDescriptorCollection propsCollection;

        /// <summary>
        ///  Creates a new type descriptor object given a currently
        ///  selected object.  The properties will be pulled off
        ///  via reflection.
        /// </summary>
        /// <param name="current">The object to retrieve properties from.</param>
        public GenericTypeDescriptor(object current)
        {
            currentObject = current;
        }

        #region ICustomTypeDescriptor Members

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection(null);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }


        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return null;
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor) this).GetProperties(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            if (propsCollection == null)
            {
                propsCollection = TypeDescriptor.GetProperties(currentObject.GetType(), attributes);
            }
            return propsCollection;
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return currentObject;
        }

        #endregion

        /// <summary>
        ///  Set a new object on the descriptor whose properties
        ///  will be enumerated and displayed.
        /// </summary>
        /// <param name="current">The object to retrieve properties from.</param>
        public void SetObject(object current)
        {
            currentObject = current;
        }
    }
}