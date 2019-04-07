using System;

namespace TinyXamarinFirebase.Froms
{
    public interface IFirebaseStorage
    {

    }

    public interface IFirebaseAuth
    {
        void SignInWithCustomToken(string token, FirebaseLoginPromise completedPromise);
        void SignOut();
    }

    public interface IFirebaseDatabase
    {
        void SetPersistenceEnabled(bool status);
        void GetData(string path, FirebaseEventDelegate handler);
        void GetData<T>(string path, FirebaseEventDelegate<T> handler);
        void OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler);
        string Push<T>(string path, T data);
        string Put<T>(string path, T data);
        void Remove(string path);
    }

}
