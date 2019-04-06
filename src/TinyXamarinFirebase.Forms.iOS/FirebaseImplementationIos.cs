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

    public class FirebaseImplementationIos : IFirebaseDatabase
    {
        private DataConverter converter;

        public FirebaseImplementationIos()
        {
            converter = new DataConverter();
        }

        private DatabaseReference GetNodeFromPath(string path)
        {
            DatabaseReference rootNode = Database.DefaultInstance.GetRootReference();

            foreach (var part in path.Split('/'))
            {
                rootNode = rootNode.GetChild(part);
            }
            return rootNode;
        }

        public void GetData(string path, FirebaseEventDelegate handler)
        {
            var rootNode = GetNodeFromPath(path);

            rootNode.ObserveEvent(DataEventType.Value, (snapshot) =>
            {
                if (snapshot.Exists)
                {

                    var data = snapshot.GetValue();

                    //var jsonData = NSJsonSerialization.Serialize(data, NSJsonWritingOptions.PrettyPrinted, out NSError error);

                    handler.OnSnapshot(data);
                }
                else
                {
                    handler.OnError(new SnapshotNotFoundException());
                }

            }, (error) =>
            {
                handler.OnError(new FirebaseException(error));
            });

        }

        public void GetData<T>(string path, FirebaseEventDelegate<T> handler)
        {
            var rootNode = GetNodeFromPath(path);

            rootNode.ObserveEvent(DataEventType.Value, (snapshot) =>
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
                    handler.OnError(new SnapshotNotFoundException());
                }

            }, (error) =>
            {
                handler.OnError(new FirebaseException(error));
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

        public void OnChildChange<T>(string path, FirebaseChildChangeEnum changeType, FirebaseChildEventDelegate<T> handler)
        {
            var rootNode = GetNodeFromPath(path);
            int changeIdx = (int)changeType;
            rootNode.ObserveEvent((DataEventType)changeIdx, (snapshot) =>
            {
                HandleChildChange(handler, changeType, snapshot, null);
            });
        }

        public string Push<T>(string path, T data)
        {
            var rootNode = GetNodeFromPath(path);
            rootNode = rootNode.GetChildByAutoId();
            rootNode.SetValue(converter.ToNative(data));
            return rootNode.Key;
        }

        public string Put<T>(string path, T data)
        {
            var rootNode = GetNodeFromPath(path);
            rootNode.SetValue(converter.ToNative(data));
            return rootNode.Key;
        }

        public void Remove(string path)
        {
            var rootNode = GetNodeFromPath(path);
            rootNode.RemoveValue();
        }
    }
}