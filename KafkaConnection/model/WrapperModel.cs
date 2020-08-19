﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaConnection.model
{
    public class WrapperModel<T>
    {
        public string Action { get; set; }
        public string Type { get; set; }
        public T Child { get; set; }
        public WrapperModel(){}

        public WrapperModel(string action, string type, T child)
        {
            this.Action = action;
            this.Type = type;
            this.Child = child;
        }
        public override string ToString()
        {
            return $"{Action} => {Type} {Child.ToString()}";
        }

    }
}