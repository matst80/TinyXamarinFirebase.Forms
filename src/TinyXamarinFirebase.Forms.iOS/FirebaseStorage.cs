using System;
using System.IO;
using System.Threading.Tasks;
using Firebase.Storage;
using Foundation;

namespace TinyXamarinFirebase.Froms.iOS
{
    public class FirebaseStorage : IFirebaseStorage
    {
        private readonly Storage storage;

        public FirebaseStorage()
        {
            storage = Storage.DefaultInstance;
        }

        public async Task<bool> DeleteFile(string path)
        {
            var reference = storage.GetRootReference().GetChild(path);
            await reference.DeleteAsync();
            return true;
        }

        public void PutData(string path, Stream stream, FirebasePromise<FirebaseFileResult> onCompleted)
        {
            var uploadReference = storage.GetRootReference().GetChild(path);
            uploadReference.PutData(NSData.FromStream(stream), null, async (metadata, error) =>
            {
                var downloadUrl = await uploadReference.GetDownloadUrlAsync();
                onCompleted?.OnComplete(new FirebaseFileResult()
                {
                    IsSuccess = error == null,
                    DownloadUrl = downloadUrl.AbsoluteString
                });
            });
        }

        public void PutFile(string path, Uri uri, FirebasePromise<FirebaseFileResult> onCompleted)
        {
            var uploadReference = storage.GetRootReference().GetChild(path);
            uploadReference.PutFile(NSUrl.FromString(uri.AbsolutePath), null, async (metadata, error) =>
            {
                var downloadUrl = await uploadReference.GetDownloadUrlAsync();
                onCompleted?.OnComplete(new FirebaseFileResult()
                {
                    IsSuccess = error == null,
                    DownloadUrl = downloadUrl.AbsoluteString
                });
            });
        }
    }
}