using System;
namespace TinyXamarinFirebase.Froms
{
    public class FirebaseEventOnUIDelegate<T> : FirebaseEventDelegateWithPersistantObject<T>
    {
        public FirebaseEventOnUIDelegate(Action<T> onSnapshot)
        {
            OnSnapshot = (data) => 
                FirebaseXamarinHelper.RunOnUIThread(() => 
                    onSnapshot(data));
        }

        public FirebaseEventOnUIDelegate(Action<T> onSnapshot, Action<FirebaseException> onError) : this(onSnapshot)
        {
            OnError = onError;
        }
    }
}
