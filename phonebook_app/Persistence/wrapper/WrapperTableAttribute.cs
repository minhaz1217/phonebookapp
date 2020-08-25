using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookWrite.Persistence.wrapper
{
    [System.AttributeUsage(System.AttributeTargets.Property )]
    public class WrapperTableAttribute : Attribute
    {
        public string Name;
        public WrapperTableAttribute(string key)
        {
            this.Name = key;
        }
    }
}
