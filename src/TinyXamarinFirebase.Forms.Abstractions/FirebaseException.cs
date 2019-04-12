using System;

namespace TinyXamarinFirebase.Forms
{
    public class FirebaseException : Exception
    {
        public FirebaseException()
        {

        }

        public FirebaseException(object nativeError)
        {
            NativeError = nativeError;
        }

        public object NativeError { get; set; }
    }
}
