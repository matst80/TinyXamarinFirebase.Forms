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

        public string Name { get; set; }
    }
}
