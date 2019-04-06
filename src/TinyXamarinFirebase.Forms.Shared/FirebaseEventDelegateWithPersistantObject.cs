using System.ComponentModel;
namespace TinyXamarinFirebase.Froms
{

    public class FirebaseEventDelegateWithPersistantObject<T> : FirebaseEventDelegate<T>, IFirebaseEventDelegateWithObject
    {
        private T objectToModify { get; set; }

        public object DataInstance
        {
            get
            {
                return objectToModify;
            }
            set
            {
                objectToModify = (T)value;
            }
        }

        public void SetInitalData(T data)
        {
            objectToModify = data;
        }
    }
}
