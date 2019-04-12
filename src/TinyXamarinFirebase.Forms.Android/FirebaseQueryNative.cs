using System;
using Firebase.Database;

namespace TinyXamarinFirebase.Forms.Droid
{
    internal class FirebaseQueryNative : FirebaseReferencBaseNative, IFirebaseQuery
    {

        public FirebaseQueryNative(Query query) : base(query)
        {

        }

    }
}