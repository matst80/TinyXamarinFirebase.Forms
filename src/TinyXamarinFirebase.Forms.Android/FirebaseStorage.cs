using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Firebase;

namespace TinyXamarinFirebase.Forms.Droid
{
    public class FirebaseStorage : IFirebaseStorage
    {
        private readonly Firebase.Storage.FirebaseStorage storage;

        public FirebaseStorage(FirebaseApp app)
        {
            storage = Firebase.Storage.FirebaseStorage.GetInstance(app); 
        }

        public async Task<bool> DeleteFile(string path)
        {
            var reference = storage.GetReference(path);
            await reference.DeleteAsync();
            return true;
        }

        public void PutData(string path, Stream stream, FirebasePromise<FirebaseFileResult> onCompleted)
        {
            var reference = storage.GetReference(path);
            reference.PutStream(stream).AddOnCompleteListener(new FileCompletedListener(reference, onCompleted));
        }

        public void PutFile(string path, string uri, FirebasePromise<FirebaseFileResult> onCompleted)
        {
            var reference = storage.GetReference(path);
            reference.PutFile(Android.Net.Uri.FromFile(new Java.IO.File(uri))).AddOnCompleteListener(new FileCompletedListener(reference, onCompleted));
        }
    }
}