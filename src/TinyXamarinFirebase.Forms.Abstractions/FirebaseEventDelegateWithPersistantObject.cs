using System;
using System.ComponentModel;
namespace TinyXamarinFirebase.Froms
{

    public class FirebaseEventDelegateWithPersistantObject<T> : FirebaseEventDelegate<T>, IFirebaseEventDelegateWithObject
    {
        public FirebaseEventDelegateWithPersistantObject() 
        {

        }

        public FirebaseEventDelegateWithPersistantObject(Action<T> onSnapshot, Action<FirebaseException> onError) : base(onSnapshot, onError)
        {

        }

        public FirebaseEventDelegateWithPersistantObject(Action<T> onSnapshot) : base(onSnapshot)
        {

        }

        public FirebaseEventDelegateWithPersistantObject(Action<T> onSnapshot, T initalData) : base(onSnapshot)
        {
            objectToModify = initalData;
        }

        public FirebaseEventDelegateWithPersistantObject(Action<T> onSnapshot, Action<FirebaseException> onError, T initalData) : base(onSnapshot, onError)
        {
            objectToModify = initalData;
        }

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
