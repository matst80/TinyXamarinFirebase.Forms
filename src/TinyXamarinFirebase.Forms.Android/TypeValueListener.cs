using Firebase.Database;

namespace TinyXamarinFirebase.Froms.Droid
{

    public class TypeValueListener<T> : ValueListenerBase, IValueEventListener
    {
        private readonly FirebaseEventDelegate<T> eventDelegate;
        private readonly DataConverter converter;

        public TypeValueListener(FirebaseEventDelegate<T> eventDelegate)
        {
            this.eventDelegate = eventDelegate;
            this.converter = new DataConverter();
        }

        public void OnCancelled(DatabaseError error)
        {
            eventDelegate.OnError(new FirebaseException(error));
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Exists())
            {
                FirebaseXamarinHelper.RunOnUIThread(() =>
                {
                    var snapData = converter.Convert<T>(snapshot.Value, GetObjectFromHandler(eventDelegate));
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