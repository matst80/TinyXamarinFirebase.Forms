using Firebase.Database;

namespace TinyXamarinFirebase.Forms.Droid
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
                //FirebaseXamarinHelper.RunOnUIThread(() =>
                //{
                try
                {
                    var snapData = DataConverter.Convert<T>(snapshot.Value, GetObjectFromHandler(eventDelegate));
                    eventDelegate.OnSnapshot(snapData);
                    SetObjectFromHandler(eventDelegate, snapData);
                }
                catch (System.Exception ex)
                {
                    var i = ex;
                }

            }
            else
            {
                eventDelegate.OnSnapshot(default(T));
            }
        }


    }
}