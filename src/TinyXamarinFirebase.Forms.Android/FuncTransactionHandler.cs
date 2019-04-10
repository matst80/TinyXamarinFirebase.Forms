using Firebase.Database;
using System;

namespace TinyXamarinFirebase.Froms.Droid
{
    internal class FuncTransactionHandler<T> : Java.Lang.Object, Transaction.IHandler
    {
        private Action<FirebaseMutableData<T>> transaction;
        private readonly FirebasePromise<bool> onCompletion;

        public FuncTransactionHandler(Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            this.transaction = transaction;
            this.onCompletion = onCompletion;
        }

        public Transaction.Result DoTransaction(MutableData currentData)
        {
            if (transaction != null)
            {
                transaction.Invoke(new AndroidFirebaseMutableData<T>(currentData));
            }
            return Transaction.Success(currentData);
        }

        public void OnComplete(DatabaseError error, bool committed, DataSnapshot currentData)
        {
            onCompletion?.OnComplete(error == null);
            if (error != null)
            {
                onCompletion?.OnError(new FirebaseException(error));
            }
        }
    }
}