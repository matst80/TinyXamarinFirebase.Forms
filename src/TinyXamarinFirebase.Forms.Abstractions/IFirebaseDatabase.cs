using System;

namespace TinyXamarinFirebase.Forms
{
    public interface IFirebaseReferenceBase
    {
        void ObserveSingle<T>(FirebaseEventDelegate<T> handler);
        object Observe<T>(FirebaseEventDelegate<T> handler);
        object OnChildChange<T>(FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler);
    }

    public interface IFirebaseQuery : IFirebaseReferenceBase
    {

        IFirebaseQuery OrderByKey();
        IFirebaseQuery OrderByChild(string key);
        IFirebaseQuery OrderByValue();
    }

    public interface IFirebaseReference : IFirebaseQuery
    {

        IFirebaseReference GetChild(string path);
        IFirebaseQuery LimitedToLast(uint numberOfItems);
        IFirebaseQuery LimitedToFirst(uint numberOfItems);

        string Push();

        string Push<T>(T data, FirebasePromise<bool> onCompletion = null);
        //string Push(string path);
        string Put<T>(T data, FirebasePromise<bool> onCompletion = null);
        void Remove(FirebasePromise<bool> onCompletion = null);
        void Transaction<T>(Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null);
        void RemoveObserver(object listener);
    }

    public interface IFirebaseDatabase
    {
        IFirebaseReference GetChild(string path);
        IFirebaseReference GetRoot();
        void SetPersistenceEnabled(bool status);
        //void GetData(string path, FirebaseEventDelegate handler);
        void ObserveSingle<T>(string path, FirebaseEventDelegate<T> handler);
        object Observe<T>(string path, FirebaseEventDelegate<T> handler);
        object OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler);
        string Push<T>(string path, T data, FirebasePromise<bool> onCompletion = null);
        //string Push(string path);
        string Put<T>(string path, T data, FirebasePromise<bool> onCompletion = null);
        void Remove(string path, FirebasePromise<bool> onCompletion = null);
        void Transaction<T>(string path, Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null);
        void RemoveObserver(string path, object listener);
    }

}
