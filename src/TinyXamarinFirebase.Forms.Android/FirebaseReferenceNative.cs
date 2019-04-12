using System;
using Firebase.Database;

namespace TinyXamarinFirebase.Froms.Droid
{
    public class FirebaseReferencBaseNative : IFirebaseReferenceBase
    {
        internal Query query;

        public FirebaseReferencBaseNative(Query query)
        {
            this.query = query;
        }

        public object Observe<T>(FirebaseEventDelegate<T> handler)
        {
            throw new NotImplementedException();
        }

        public void ObserveSingle<T>(FirebaseEventDelegate<T> handler)
        {
            throw new NotImplementedException();
        }

        public object OnChildChange<T>(FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            throw new NotImplementedException();
        }

        public IFirebaseQuery OrderByChild(string key)
        {
            return new FirebaseQueryNative(query.OrderByChild(key));
        }

        public IFirebaseQuery OrderByKey()
        {
            return new FirebaseQueryNative(query.OrderByKey());
        }

        public IFirebaseQuery OrderByValue()
        {
            return new FirebaseQueryNative(query.OrderByValue());
        }
    }

    public class FirebaseReferenceNative : FirebaseReferencBaseNative, IFirebaseReference
    {
        internal DatabaseReference databaseReference;

        public FirebaseReferenceNative(DatabaseReference databaseReference) : base(databaseReference)
        {
            this.databaseReference = databaseReference;
        }

        public IFirebaseReference GetChild(string path)
        {
            return new FirebaseReferenceNative(databaseReference.Child(path));
        }

        public IFirebaseQuery LimitedToFirst(uint numberOfItems)
        {
            return new FirebaseQueryNative(databaseReference.LimitToFirst((int)numberOfItems));
        }

        public IFirebaseQuery LimitedToLast(uint numberOfItems)
        {
            return new FirebaseQueryNative(databaseReference.LimitToLast((int)numberOfItems));
        }

        public string Push()
        {
            throw new NotImplementedException();
        }

        public string Push<T>(T data, FirebasePromise<bool> onCompletion = null)
        {
            throw new NotImplementedException();
        }

        public string Put<T>(T data, FirebasePromise<bool> onCompletion = null)
        {
            throw new NotImplementedException();
        }

        public void Remove(FirebasePromise<bool> onCompletion = null)
        {
            throw new NotImplementedException();
        }

        public void RemoveObserver(object listener)
        {
            throw new NotImplementedException();
        }

        public void Transaction<T>(Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            throw new NotImplementedException();
        }
    }
}