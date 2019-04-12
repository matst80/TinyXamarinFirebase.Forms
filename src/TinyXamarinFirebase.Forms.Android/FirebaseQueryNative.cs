using System;
using Firebase.Database;

namespace TinyXamarinFirebase.Froms.Droid
{
    internal class FirebaseQueryNative : FirebaseReferencBaseNative, IFirebaseQuery
    {

        public FirebaseQueryNative(Query query) : base(query)
        {

        }

    }
}