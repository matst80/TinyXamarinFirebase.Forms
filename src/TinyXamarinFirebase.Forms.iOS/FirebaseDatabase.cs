using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using Foundation;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System;

namespace TinyXamarinFirebase.Forms.iOS
{

    public class FirebaseDatabase : IFirebaseDatabase
    {

        private Database instance;

        public FirebaseDatabase()
        {
            instance = Database.DefaultInstance;
        }

        private DatabaseReference GetNodeFromPath(string path)
        {
            DatabaseReference rootNode = instance.GetRootReference();

            foreach (var part in path.Split('/'))
            {
                rootNode = rootNode.GetChild(part);
            }
            return rootNode;
        }


        public object Observe<T>(string path, FirebaseEventDelegate<T> handler)
        {
            return GetChild(path).Observe<T>(handler);
        }

        public void ObserveSingle<T>(string path, FirebaseEventDelegate<T> handler)
        {
            GetChild(path).ObserveSingle<T>(handler);
        }

        public object OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            return GetChild(path).OnChildChange<T>(changeType, handler);
        }

        public string Push<T>(string path, T data, FirebasePromise<bool> onCompletion = null)
        {
            return GetChild(path).Push(data, onCompletion);
        }

        public string Put<T>(string path, T data, FirebasePromise<bool> onCompletion = null)
        {
            return GetChild(path).Put(data, onCompletion);
        }

        public void Remove(string path, FirebasePromise<bool> onCompletion = null)
        {
            GetChild(path).Remove(onCompletion);
        }

        public void SetPersistenceEnabled(bool status)
        {
            instance.PersistenceEnabled = status;
        }

        public void Transaction<T>(string path, Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            GetChild(path).Transaction<T>(transaction, onCompletion);
        }

        public void RemoveObserver(string path, object listener)
        {
            GetChild(path).RemoveObserver(listener);
        }

        public IFirebaseReference GetChild(string path)
        {
            return new FirebaseReferenceNative(GetNodeFromPath(path));
        }

        public IFirebaseReference GetRoot()
        {
            return new FirebaseReferenceNative(instance.GetRootReference());
        }
    }
}