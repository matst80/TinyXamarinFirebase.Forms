using Firebase.Database;

namespace TinyXamarinFirebase.Froms.Droid
{

    public class TypeValueListener<T> : ValueListenerBase, IValueEventListener
    {
        private readonly FirebaseEventDelegate<T> eventDelegate;


        public TypeValueListener(FirebaseEventDelegate<T> eventDelegate)
        {
            this.eventDelegate = eventDelegate;
        }

        public void OnCancelled(DatabaseError error)
        {
            eventDelegate?.OnError?.Invoke(new FirebaseException(error));
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Exists())
            {
                FirebaseXamarinHelper.RunOnUIThread(() =>
                {
                    var snapData = DataConverter.Convert<T>(snapshot.Value, GetObjectFromHandler(eventDelegate));
                    eventDelegate.OnSnapshot(snapData);
                    SetObjectFromHandler(eventDelegate, snapData);
                });
            }
            else
            {
                eventDelegate.OnSnapshot(default(T));
            }
        }


    }
}