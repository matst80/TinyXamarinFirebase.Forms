using System;
using System.Collections.Generic;

namespace TinyXamarinFirebase.Forms
{


    public class FirebaseEventDelegate<T> : IFirebaseEventDelegate
    {
        public FirebaseEventDelegate()
        {

        }

        public FirebaseEventDelegate(Action<T> onSnapshot)
        {
            OnSnapshot = onSnapshot;
        }

        public FirebaseEventDelegate(Action<T> onSnapshot, Action<FirebaseException> onError) : this(onSnapshot)
        {
            OnError = onError;
        }

        public Action<T> OnSnapshot { get; set; }
        public Action<FirebaseException> OnError { get; set; }
    }

    public class FirebaseChildEventDelegate<T> : IFirebaseEventDelegate
    {
        public FirebaseChildEventDelegate()
        {

        }

        public FirebaseChildEventDelegate(Action<FirebaseChildChangeEnum, string, KeyValuePair<string, T>> onSnapshot)
        {
            OnSnapshot = onSnapshot;
        }

        public FirebaseChildEventDelegate(Action<FirebaseChildChangeEnum, string, KeyValuePair<string, T>> onSnapshot, Action<FirebaseException> onError) : this(onSnapshot)
        {
            OnError = onError;
        }

        public Action<FirebaseChildChangeEnum, string, KeyValuePair<string,T>> OnSnapshot { get; set; }
        public Action<FirebaseException> OnError { get; set; }
    }

    public class FirebaseEventDelegate
    {
        public FirebaseEventDelegate()
        {

        }

        public FirebaseEventDelegate(Action<object> onSnapshot)
        {
            OnSnapshot = onSnapshot;
        }

        public FirebaseEventDelegate(Action<object> onSnapshot, Action<FirebaseException> onError) : this(onSnapshot)
        {
            OnError = onError;
        }

        public Action<object> OnSnapshot { get; set; }
        public Action<FirebaseException> OnError { get; set; }
    }
}
