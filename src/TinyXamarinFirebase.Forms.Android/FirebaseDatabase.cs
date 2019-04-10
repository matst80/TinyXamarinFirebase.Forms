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

        //public void GetData(string path, FirebaseEventDelegate handler)
        //{
        //    database.GetReference(path).AddValueEventListener(new StringValueListener(handler));
        //}

        public object Observe<T>(string path, FirebaseEventDelegate<T> handler)
        {
            var listener = new TypeValueListener<T>(handler);
            database.GetReference(path).AddValueEventListener(listener);
            return listener;
        }

        public void ObserveSingle<T>(string path, FirebaseEventDelegate<T> handler)
        {
            database.GetReference(path).AddListenerForSingleValueEvent(new TypeValueListener<T>(handler));
        }

        public object OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            var listener = new ChildTypeValueListener<T>(handler, changeType);
            database.GetReference(path).AddChildEventListener(listener);
            return listener;
        }

        public string Push<T>(string path, T data, FirebasePromise<bool> onCompletion = null)
        {
            var newNode = database.GetReference(path).Push();
            var command = newNode.SetValue(converter.ToNative(data.GetType(), data));
            if (onCompletion != null)
                command.AddOnCompleteListener(new CommandCompletedListener(onCompletion));
            return newNode.Key;
        }

        public string Put<T>(string path, T data, FirebasePromise<bool> onCompletion = null)
        {
            var newNode = database.GetReference(path);
            var convertedData = converter.ToNative(data.GetType(), data);
            var command = newNode.SetValue(convertedData);
            if (onCompletion!=null)
                command.AddOnCompleteListener(new CommandCompletedListener(onCompletion));
            return newNode.Key;
        }

        public void RemoveObserver(string path, object listener)
        {
            var newNode = database.GetReference(path);
            if (listener is IChildEventListener childEventListener)
            {
                newNode.RemoveEventListener(childEventListener);
            }
            else if (listener is IValueEventListener eventListener)
            {
                newNode.RemoveEventListener(eventListener);
            }
        }

        public void Transaction<T>(string path, Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            var newNode = database.GetReference(path);
            newNode.RunTransaction(new FuncTransactionHandler<T>(transaction, onCompletion));
        }

        public void Remove(string path, FirebasePromise<bool> onCompletion = null)
        {
            var command = database.GetReference(path).RemoveValue();
            if (onCompletion != null)
                command.AddOnCompleteListener(new CommandCompletedListener(onCompletion));
        }

        public void SetPersistenceEnabled(bool status)
        {
            database.SetPersistenceEnabled(status);
        }
    }
}