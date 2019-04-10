using System;

namespace TinyXamarinFirebase.Froms
{

    public interface IFirebaseDatabase
    {
        void SetPersistenceEnabled(bool status);
        //void GetData(string path, FirebaseEventDelegate handler);
        void ObserveSingle<T>(string path, FirebaseEventDelegate<T> handler);
        object Observe<T>(string path, FirebaseEventDelegate<T> handler);
        object OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler);
        string Push<T>(string path, T data, FirebasePromise<bool> onCompletion = null);
        string Put<T>(string path, T data, FirebasePromise<bool> onCompletion = null);
        void Remove(string path, FirebasePromise<bool> onCompletion = null);
        void Transaction<T>(string path, Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null);
        void RemoveObserver(string path, object listener);
    }

}
