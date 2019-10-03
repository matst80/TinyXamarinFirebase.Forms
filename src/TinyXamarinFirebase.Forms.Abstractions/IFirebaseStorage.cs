using System.IO;
using System;
using System.Threading.Tasks;

namespace TinyXamarinFirebase.Forms
{
    public interface IFirebaseStorage
    {
        void PutData(string path, Stream stream, FirebasePromise<FirebaseFileResult> onCompleted);
        void PutFile(string path, string uri, FirebasePromise<FirebaseFileResult> onCompleted);
        Task<bool> DeleteFile(string path);
    }

}
