using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaConnection.model
{
    public class WrapperModel<T>
    {
        public string Action { get; set; }
        public string Type { get; set; }
        public string CreatedAt;
        public string UpdatedAt;
        public string Publisher;
        public string CreatedBy;
        public string UpdatedBy;
        public T Child { get; set; }
        public WrapperModel(){}

        public WrapperModel(string action, string type, string CreatedAt, string UpdatedAt, string publisher, string createdBy, string updatedBy, T child)
        {
            this.Action = action;
            this.Type = type;
            this.Child = child;
            this.CreatedAt = CreatedAt;
            this.UpdatedAt = UpdatedAt;
            this.Publisher = publisher;
            this.CreatedBy = createdBy;
            this.UpdatedBy = updatedBy;
            
        }
        public override string ToString()
        {
            return $"{Action} => {Type} {Child.ToString()}";
        }

    }
}
