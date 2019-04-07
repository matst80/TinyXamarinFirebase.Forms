using Firebase.Database;
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

namespace TinyXamarinFirebase.Froms.Droid
{

    public class FirebaseDatabase : IFirebaseDatabase
    {
        private Firebase.Database.FirebaseDatabase database;
        private ToNativeConverter converter;

        public FirebaseDatabase(FirebaseApp app)
        {
            database = Firebase.Database.FirebaseDatabase.GetInstance(app);
            converter = new ToNativeConverter();
        }

        public void GetData(string path, FirebaseEventDelegate handler)
        {
            database.GetReference(path).AddValueEventListener(new StringValueListener(handler));
        }

        public void GetData<T>(string path, FirebaseEventDelegate<T> handler)
        {
            database.GetReference(path).AddValueEventListener(new TypeValueListener<T>(handler));
        }

        public void OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            database.GetReference(path).AddChildEventListener(new ChildTypeValueListener<T>(handler, changeType));
        }

        public string Push<T>(string path, T data)
        {
            var newNode = database.GetReference(path).Push();
            newNode.SetValue(converter.ToNative(data.GetType(), data));
            return newNode.Key;
        }

        public string Put<T>(string path, T data)
        {
            var newNode = database.GetReference(path);
            newNode.SetValue(converter.ToNative(data.GetType(), data));
            return newNode.Key;
        }

        public void Remove(string path)
        {
            database.GetReference(path).RemoveValue();
        }

        public void SetPersistenceEnabled(bool status)
        {
            database.SetPersistenceEnabled(status);
        }
    }
}