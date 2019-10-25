using Android.Gms.Tasks;
using Firebase.Storage;

namespace TinyXamarinFirebase.Forms.Droid
{
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
}