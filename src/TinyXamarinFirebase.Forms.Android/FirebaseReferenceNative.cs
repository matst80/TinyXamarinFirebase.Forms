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
            var listener = new TypeValueListener<T>(handler);
            query.AddValueEventListener(listener);
            return listener;
        }

        public void ObserveSingle<T>(FirebaseEventDelegate<T> handler)
        {
            query.AddListenerForSingleValueEvent(new TypeValueListener<T>(handler));
        }

        public object OnChildChange<T>(FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            var listener = new ChildTypeValueListener<T>(handler, changeType);
            query.AddChildEventListener(listener);
            return listener;
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
            var newNode = databaseReference.Push();
            return newNode.Key;
        }

        public string Push<T>(T data, FirebasePromise<bool> onCompletion = null)
        {
            var newNode = databaseReference.Push();
            var command = newNode.SetValue(ToNativeConverter.ToNative(data.GetType(), data));
            if (onCompletion != null)
                command.AddOnCompleteListener(new CommandCompletedListener(onCompletion));
            return newNode.Key;
        }

        public string Put<T>(T data, FirebasePromise<bool> onCompletion = null)
        {
            var newNode = databaseReference;
            var convertedData = ToNativeConverter.ToNative(data.GetType(), data);
            var command = newNode.SetValue(convertedData);
            if (onCompletion != null)
                command.AddOnCompleteListener(new CommandCompletedListener(onCompletion));
            return newNode.Key;
        }

        public void Remove(FirebasePromise<bool> onCompletion = null)
        {
            var command = databaseReference.RemoveValue();
            if (onCompletion != null)
                command.AddOnCompleteListener(new CommandCompletedListener(onCompletion));
        }

        public void RemoveObserver(object listener)
        {
            if (listener is IChildEventListener childEventListener)
            {
                databaseReference.RemoveEventListener(childEventListener);
            }
            else if (listener is IValueEventListener eventListener)
            {
                databaseReference.RemoveEventListener(eventListener);
            }
        }

        public void Transaction<T>(Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            databaseReference.RunTransaction(new FuncTransactionHandler<T>(transaction, onCompletion));
        }
    }
}