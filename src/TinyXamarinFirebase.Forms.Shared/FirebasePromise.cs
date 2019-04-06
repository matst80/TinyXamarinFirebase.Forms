using System;

namespace TinyXamarinFirebase.Froms
{

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

        public Action<T> onComplete;

        public void OnComplete(T data)
        {
            onComplete?.Invoke(data);
        }
    }
}
