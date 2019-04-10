using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Firebase;
using Android.Gms.Tasks;
using Firebase.Storage;

namespace TinyXamarinFirebase.Froms.Droid
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
            reference.PutStream(stream).AddOnCompleteListener(new FileCompletedListener(onCompleted));
        }

        public void PutFile(string path, Uri uri, FirebasePromise<FirebaseFileResult> onCompleted)
        {

        }
    }

    internal class FileCompletedListener : Java.Lang.Object, IOnCompleteListener
    {
        private readonly DownloadUrlCompletedListener downloadListener;
        private readonly StorageReference storageReference;

        public FileCompletedListener(StorageReference storageReference, FirebasePromise<FirebaseFileResult> onCompleted)
        {
            this.downloadListener = new DownloadUrlCompletedListener(storageReference, onCompleted);
            this.storageReference = storageReference;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsComplete)
            {
                storageReference.DownloadUrl.AddOnCompleteListener(downloadListener);
            }
        }
    }

    internal class DownloadUrlCompletedListener : Java.Lang.Object, IOnCompleteListener
    {
        private StorageReference storageReference;
        private FirebasePromise<FirebaseFileResult> onCompleted;

        public DownloadUrlCompletedListener(StorageReference storageReference, FirebasePromise<FirebaseFileResult> onCompleted)
        {
            this.storageReference = storageReference;
            this.onCompleted = onCompleted;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {

                onCompleted?.OnComplete(new FirebaseFileResult()
                {
                    IsSuccess = true,
                    DownloadUrl = ""
                });
            }
        }
    }
}