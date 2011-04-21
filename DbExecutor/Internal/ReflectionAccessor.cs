﻿using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Linq.Expressions;

namespace Codeplex.Data.Internal
{
    /// <summary>Reflection(PropertyInfo/MemberInfo) accessor.</summary>
    internal class ReflectionAccessor : IMemberAccessor
    {
        readonly IMemberAccessor accessor;

        public string Name { get { return accessor.Name; } }
        public Type DelaringType { get { return accessor.DelaringType; } }
        public bool IsReadable { get { return accessor.IsReadable; } }
        public bool IsWritable { get { return accessor.IsWritable; } }

        public ReflectionAccessor(FieldInfo memberInfo)
        {
            Contract.Requires<ArgumentNullException>(memberInfo != null);

            this.accessor = new FieldAccessor(memberInfo);
        }

        public ReflectionAccessor(PropertyInfo propertyInfo)
        {
            Contract.Requires<ArgumentNullException>(propertyInfo != null);

            this.accessor = new PropertyAccessor(propertyInfo);
        }

        public object GetValue(ref object target)
        {
            if (!IsReadable) throw new InvalidOperationException("is not readable member");

            return accessor.GetValue(ref target);
        }

        public void SetValue(ref object target, object value)
        {
            if (!IsWritable) throw new InvalidOperationException("is not writable member");

            accessor.SetValue(ref target, value);
        }

        class PropertyAccessor : IMemberAccessor
        {
            public string Name { get; private set; }
            public Type DelaringType { get; private set; }
            public bool IsReadable { get; private set; }
            public bool IsWritable { get; private set; }

            readonly PropertyInfo info;

            public PropertyAccessor(PropertyInfo info)
            {
                this.info = info;
                this.Name = info.Name;
                this.DelaringType = info.DeclaringType;
                this.IsReadable = (info.GetGetMethod(false) != null);
                this.IsWritable = (info.GetSetMethod(false) != null);
            }

            public object GetValue(ref object target)
            {
                return info.GetValue(target, null);
            }

            public void SetValue(ref object target, object value)
            {
                info.SetValue(target, value, null);
            }
        }

        class FieldAccessor : IMemberAccessor
        {
            public string Name { get; private set; }
            public Type DelaringType { get; private set; }
            public bool IsReadable { get; private set; }
            public bool IsWritable { get; private set; }

            readonly FieldInfo info;

            public FieldAccessor(FieldInfo info)
            {
                this.info = info;
                this.Name = info.Name;
                this.DelaringType = info.DeclaringType;
                this.IsReadable = true;
                this.IsWritable = !info.IsInitOnly;
            }

            public object GetValue(ref object target)
            {
                return info.GetValue(target);
            }

            public void SetValue(ref object target, object value)
            {
                info.SetValue(target, value);
            }
        }
    }
}