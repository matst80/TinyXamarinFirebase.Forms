using System;

namespace TinyXamarinFirebase.Forms
{

    [AttributeUsage(AttributeTargets.Property)]
    public class FirebaseAttribute : Attribute
    {
        public FirebaseAttribute(string name)
        {
            Name = name;
        }

        public bool ReadOnly { get; set; }

        public string Name { get; set; }
    }
}
