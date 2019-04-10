using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using Foundation;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System;

namespace TinyXamarinFirebase.Froms.iOS
{

    public class FirebaseDatabase : IFirebaseDatabase
    {
        private DataConverter converter;
        private Database instance;

        public FirebaseDatabase()
        {
            converter = new DataConverter();
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
            var rootNode = GetNodeFromPath(path);

            return (uint)rootNode.ObserveEvent(DataEventType.Value, (snapshot) =>
            {
                if (snapshot.Exists)
                {
                    FirebaseXamarinHelper.RunOnUIThread(() =>
                    {
                        var data = snapshot.GetValue();
                        var snapData = converter.Convert<T>(data, GetObjectFromHandler(handler));
                        SetObjectFromHandler(handler, snapData);
                        handler.OnSnapshot(snapData);
                    });
                }
                else
                {
                    handler?.OnError?.Invoke(new SnapshotNotFoundException());
                }

            }, (error) =>
            {
                handler?.OnError?.Invoke(new FirebaseException(error));
            });
        }

        public void ObserveSingle<T>(string path, FirebaseEventDelegate<T> handler)
        {
            var rootNode = GetNodeFromPath(path);

            rootNode.ObserveSingleEvent(DataEventType.Value, (snapshot) =>
            {
                if (snapshot.Exists)
                {
                    FirebaseXamarinHelper.RunOnUIThread(() =>
                    {
                        var data = snapshot.GetValue();
                        var snapData = converter.Convert<T>(data, GetObjectFromHandler(handler));
                        SetObjectFromHandler(handler, snapData);
                        handler.OnSnapshot(snapData);
                    });
                }
                else
                {
                    handler?.OnError?.Invoke(new SnapshotNotFoundException());
                }

            }, (error) =>
            {
                handler?.OnError?.Invoke(new FirebaseException(error));
            });
        }

        private void SetObjectFromHandler(IFirebaseEventDelegate handler, object snapData)
        {
            if (handler is IFirebaseEventDelegateWithObject withObject && snapData != null)
            {
                if (withObject.DataInstance != snapData)
                {
                    withObject.DataInstance = snapData;
                }
            }
        }

        private object GetObjectFromHandler(IFirebaseEventDelegate handler)
        {
            if (handler is IFirebaseEventDelegateWithObject withObject)
            {
                return withObject.DataInstance;
            }
            return null;
        }

        private void HandleChildChange<T>(FirebaseChildEventDelegate<T> handler, FirebaseChildChangeEnum type, DataSnapshot snapshot, string previousChildName)
        {
            FirebaseXamarinHelper.RunOnUIThread(() =>
            {
                var snapData = converter.Convert<T>(snapshot.GetValue(), GetObjectFromHandler(handler));
                handler.OnSnapshot(type, previousChildName, new KeyValuePair<string, T>(snapshot.Key, snapData));
                SetObjectFromHandler(handler, snapData);
            });
        }

        public object OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            var rootNode = GetNodeFromPath(path);
            int changeIdx = (int)changeType;
            return (uint)rootNode.ObserveEvent((DataEventType)changeIdx, (snapshot) =>
            {
                HandleChildChange(handler, changeType, snapshot, null);
            });
        }

        public string Push<T>(string path, T data, FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = GetNodeFromPath(path);
            rootNode = rootNode.GetChildByAutoId();
            rootNode.SetValue(converter.ToNative(data), (error, reference) =>
            {
                if (onCompletion != null)
                {
                    onCompletion.OnComplete(error == null);
                    if (error != null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
            return rootNode.Key;
        }

        public string Put<T>(string path, T data, FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = GetNodeFromPath(path);
            rootNode.SetValue(converter.ToNative(data), (error, reference) =>
            {
                if (onCompletion!=null)
                {
                    onCompletion.OnComplete(error == null);
                    if (error!=null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
            return rootNode.Key;
        }

        public void Remove(string path, FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = GetNodeFromPath(path);
            rootNode.RemoveValue((error, reference) =>
            {
                if (onCompletion != null)
                {
                    onCompletion.OnComplete(error == null);
                    if (error != null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
        }

        public void SetPersistenceEnabled(bool status)
        {
            instance.PersistenceEnabled = status;
        }

        public void Transaction<T>(string path, Action<FirebaseMutableData<T>> transaction, FirebasePromise<bool> onCompletion = null)
        {
            var rootNode = GetNodeFromPath(path);
            rootNode.RunTransaction((currentData) =>
            {
                if (transaction != null)
                {
                    transaction.Invoke(new IosMutableData<T>(currentData));
                }
                return TransactionResult.Success(currentData);
            },(error, commited, snapshot) => {
                if (onCompletion != null)
                {
                    onCompletion.OnComplete(commited);
                    if (error != null)
                    {
                        onCompletion.OnError(new FirebaseException(error));
                    }
                }
            });
        }

        public void RemoveObserver(string path, object listener)
        {
            var rootNode = GetNodeFromPath(path);
            if (listener is uint nr) {
                rootNode.RemoveObserver(new nuint(nr));
            }
        }
    }
}