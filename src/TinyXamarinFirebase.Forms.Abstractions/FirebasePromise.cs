using System;

namespace TinyXamarinFirebase.Forms
{
    public abstract class FirebaseMutableData<T>
    {
        public abstract T GetValue();
        public abstract void SetValue(T data);
    }

    public class FirebaseLoginPromise : FirebasePromise<bool>
    {
        public FirebaseLoginPromise(Action<bool> onComplete) : base(onComplete)
        {

        }
    }

    public class FirebasePromise<T>
    {
        public FirebasePromise(Action<T> onComplete)
        {
            this.onComplete = onComplete;
        }

        public FirebasePromise(Action<T> onComplete, Action<FirebaseException> onError)
        {
            this.onComplete = onComplete;
            this.onError = onError;
        }

        public Action<T> onComplete;
        public Action<FirebaseException> onError;

        public void OnComplete(T data)
        {
            onComplete?.Invoke(data);
        }

        public void OnError(FirebaseException exception)
        {
            onError?.Invoke(exception);
        }
    }
}
