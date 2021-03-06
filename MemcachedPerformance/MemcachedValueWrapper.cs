﻿using System;
using ServiceStack;
using ServiceStack.Text;

namespace MemcachedPerformance
{
    [Serializable]
    public class MemcachedValueWrapper
    {
        public Type ValueType { get; set; }
        public string JsonString { get; set; }

        [NonSerialized]
        private object _value;

        public object Value
        {
            get
            {
                if (_value == null && !string.IsNullOrEmpty(JsonString))
                    _value = JsonSerializer.DeserializeFromString(JsonString, ValueType);
                return _value;
            }
        }

        public MemcachedValueWrapper() { }

        public MemcachedValueWrapper(object value)
        {
            if (value == null) return;
            ValueType = value.GetType();
            _value = value;
            JsonString = value.ToJson();
        }
    }
}