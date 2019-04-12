using Java.Util;
using System.Linq;
using System.Collections.Generic;
using Javax.Crypto;
using System.Collections;
using Java.Interop;
using Org.Json;
using Java.Lang;
using Firebase;
using Android.App;
using System;
using Firebase.Database;

namespace TinyXamarinFirebase.Forms.Droid
{

    public class FirebaseDatabase : IFirebaseDatabase
    {
        private Firebase.Database.FirebaseDatabase database;


        public FirebaseDatabase(FirebaseApp app)
        {
            database = Firebase.Database.FirebaseDatabase.GetInstance(app);
        
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
            return GetChild(path).Push<T>(data, onCompletion);
        }

        public string Put<T>(string path, T data, FirebasePromise<bool> onCompletion = null)
        {
            return GetChild(path).Put(data, onCompletion);
        }

        public void RemoveObserver(string path, object listener)
        {
            GetChild(path).RemoveObserver(listener);
        }

        public void Transaction<T>(string path, Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            GetChild(path).Transaction(transaction, onCompletion);
        }

        public void Remove(string path, FirebasePromise<bool> onCompletion = null)
        {
            GetChild(path).Remove(onCompletion);
        }

        public void SetPersistenceEnabled(bool status)
        {
            database.SetPersistenceEnabled(status);
        }

        public IFirebaseReference GetChild(string path)
        {
            return new FirebaseReferenceNative(database.GetReference(path));
        }

        public IFirebaseReference GetRoot()
        {
            return new FirebaseReferenceNative(database.GetReference("/"));
        }
    }
}