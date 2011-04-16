﻿using System;
using System.Collections.ObjectModel;

namespace Codeplex.Data
{
    /// <summary>Represents PropertyInfo delegate.</summary>
    internal interface IPropertyAccessor
    {
        string Name { get; }
        object GetValue(object target);
        void SetValue(object target, object value);
    }

    /// <summary>Represents PropertyInfo delegate.</summary>
    internal class PropertyAccessor<TTarget, TProperty> : IPropertyAccessor
    {
        readonly string name;
        readonly Func<TTarget, TProperty> getter;
        readonly Action<TTarget, TProperty> setter;

        public PropertyAccessor(string name, Func<TTarget, TProperty> getter, Action<TTarget, TProperty> setter)
        {
            this.name = name;
            this.getter = getter;
            this.setter = setter;
        }

        public string Name
        {
            get { return name; }
        }

        public object GetValue(object target)
        {
            return this.getter((TTarget)target);
        }

        public void SetValue(object target, object value)
        {
            this.setter((TTarget)target, (TProperty)value);
        }
    }
}