using Firebase.Database;
using System;
using System.Collections.Generic;

namespace TinyXamarinFirebase.Forms.iOS
{
    internal class FirebaseReferenceBaseNative : IFirebaseReferenceBase
    {


        private readonly DatabaseQuery databaseReference;

        public FirebaseReferenceBaseNative(DatabaseQuery databaseReference)
        {
            this.databaseReference = databaseReference;
        }

        public object Observe<T>(FirebaseEventDelegate<T> handler)
        {
            var rootNode = databaseReference;

            return (uint)rootNode.ObserveEvent(DataEventType.Value, (snapshot) =>
            {
                if (snapshot.Exists)
                {
                    //FirebaseXamarinHelper.RunOnUIThread(() =>
                    //{
                        var data = snapshot.GetValue();
                        var snapData = DataConverter.Convert<T>(data, GetObjectFromHandler(handler));
                        SetObjectFromHandler(handler, snapData);
                        handler.OnSnapshot(snapData);
                    //});
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

        public void ObserveSingle<T>(FirebaseEventDelegate<T> handler)
        {
            var rootNode = databaseReference;

            rootNode.ObserveSingleEvent(DataEventType.Value, (snapshot) =>
            {
                if (snapshot.Exists)
                {
                    FirebaseXamarinHelper.RunOnUIThread(() =>
                    {
                        var data = snapshot.GetValue();
                        var snapData = DataConverter.Convert<T>(data, GetObjectFromHandler(handler));
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

        public IFirebaseQuery OrderByChild(string key)
        {
            return new FirebaseQueryNative(databaseReference.GetQueryOrderedByChild(key));
        }

        public IFirebaseQuery OrderByKey()
        {
            return new FirebaseQueryNative(databaseReference.GetQueryOrderedByKey());
        }

        public IFirebaseQuery OrderByValue()
        {
            return new FirebaseQueryNative(databaseReference.GetQueryOrderedByValue());
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
                var snapData = DataConverter.Convert<T>(snapshot.GetValue(), GetObjectFromHandler(handler));
                handler.OnSnapshot(type, previousChildName, new KeyValuePair<string, T>(snapshot.Key, snapData));
                SetObjectFromHandler(handler, snapData);
            });
        }

        public object OnChildChange<T>(FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            //var rootNode = GetNodeFromPath(path);
            int changeIdx = (int)changeType;
            return (uint)databaseReference.ObserveEvent((DataEventType)changeIdx, (snapshot) =>
            {
                HandleChildChange(handler, changeType, snapshot, null);
            });
        }
    }

    internal class FirebaseQueryNative : FirebaseReferenceBaseNative, IFirebaseQuery
    {
        private DatabaseQuery queryReference;

        public FirebaseQueryNative(DatabaseQuery reference) : base(reference)
        {
            this.queryReference = reference;
        }

    }
}