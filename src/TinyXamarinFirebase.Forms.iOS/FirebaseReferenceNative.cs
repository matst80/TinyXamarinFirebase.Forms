using Firebase.Database;
using System;

namespace TinyXamarinFirebase.Forms.iOS
{


    internal class FirebaseReferenceNative : FirebaseReferenceBaseNative, IFirebaseReference
    {
        private DatabaseReference databaseReference;

        public FirebaseReferenceNative(DatabaseReference databaseReference) : base(databaseReference)
        {
            this.databaseReference = databaseReference;
        }

        public IFirebaseReference GetChild(string path)
        {
            return new FirebaseReferenceNative(Database.DefaultInstance.GetRootReference().GetChild(path));
        }

        public IFirebaseQuery LimitedToFirst(uint numberOfItems)
        {
            return new FirebaseQueryNative(databaseReference.GetQueryLimitedToFirst((nuint)numberOfItems));
        }

        public IFirebaseQuery LimitedToLast(uint numberOfItems)
        {
            return new FirebaseQueryNative(databaseReference.GetQueryLimitedToLast((nuint)numberOfItems));
        }

        public string Push()
        {
            return databaseReference.GetChildByAutoId().Key;
        }

        public string Push<T>(T data, FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = databaseReference;
            rootNode = rootNode.GetChildByAutoId();
            rootNode.SetValue(DataConverter.ToNative(data), (error, reference) =>
            {
                if (onCompletion != null)
                {
                    onCompletion.OnComplete(error == null);
                    if (error != null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
            return rootNode.Key;
        }

        public string Put<T>(T data, FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = databaseReference;
            rootNode.SetValue(DataConverter.ToNative(data), (error, reference) =>
            {
                if (onCompletion != null)
                {
                    onCompletion.OnComplete(error == null);
                    if (error != null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
            return rootNode.Key;
        }

        public void Remove(FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = databaseReference;
            rootNode.RemoveValue((error, reference) =>
            {
                if (onCompletion != null)
                {
                    onCompletion.OnComplete(error == null);
                    if (error != null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
        }

        public void RemoveObserver(object listener)
        {
            var rootNode = databaseReference;
            if (listener is uint nr)
            {
                rootNode.RemoveObserver(new nuint(nr));
            }
        }

        public void Transaction<T>(Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = databaseReference;
            rootNode.RunTransaction((currentData) =>
            {
                if (transaction != null)
                {
                    transaction.Invoke(new IosMutableData<T>(currentData));
                }
                return TransactionResult.Success(currentData);
            }, (error, commited, snapshot) => {
                if (onCompletion != null)
                {
                    onCompletion.OnComplete(commited);
                    if (error != null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
        }
    }
}