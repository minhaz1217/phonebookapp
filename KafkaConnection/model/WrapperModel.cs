using System;
using System.Collections.Generic;
using System.Text;

namespace MessageCarrier.model
{
    public class WrapperModel<T>
    {
        public string Action { get; set; }
        public string Type { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Publisher { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
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
