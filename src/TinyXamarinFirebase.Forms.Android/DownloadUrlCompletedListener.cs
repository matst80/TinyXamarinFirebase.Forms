using Android.Gms.Tasks;
using Firebase.Storage;

namespace TinyXamarinFirebase.Froms.Droid
{
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