using System;

namespace Ray.Infrastructure.AutoTask
{
    public class AutoTaskAttribute : Attribute
    {
        public AutoTaskAttribute(string code, string alias = "")
        {
            Code = code;
            Alias = alias;
        }

        public string Code { get; set; }

        public string Alias { get; set; }
    }
}
