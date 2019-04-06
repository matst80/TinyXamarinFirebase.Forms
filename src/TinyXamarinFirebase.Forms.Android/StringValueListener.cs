using Firebase.Database;
using Android.Runtime;

namespace TinyXamarinFirebase.Froms.Droid
{
    public class StringValueListener : Java.Lang.Object, IValueEventListener
    {
        private readonly FirebaseEventDelegate eventDelegate;

        public StringValueListener(FirebaseEventDelegate eventDelegate)
        {
            this.eventDelegate = eventDelegate;
        }

        public void OnCancelled(DatabaseError error)
        {
            eventDelegate.OnError(new FirebaseException(error));
        }

        public void OnDataChange(DataSnapshot snapshot)
        {
            if (snapshot.Exists())
            {
                var jsonObject = new Org.Json.JSONObject();
                var val = snapshot.GetValue(jsonObject.Class);
                if (val is JavaDictionary map)
                {
                    eventDelegate.OnSnapshot(map.ToString());
                }
                else
                {

                    eventDelegate.OnSnapshot(val.ToString());
                }
            }
            else
            {
                eventDelegate.OnSnapshot(null);
            }
        }
    }
}