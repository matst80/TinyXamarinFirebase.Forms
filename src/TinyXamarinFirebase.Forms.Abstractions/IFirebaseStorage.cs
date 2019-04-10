using System.IO;
using System;
using System.Threading.Tasks;

namespace TinyXamarinFirebase.Froms
{
    public interface IFirebaseStorage
    {
        void PutData(string path, Stream stream, FirebasePromise<FirebaseFileResult> onCompleted);
        void PutFile(string path, Uri uri, FirebasePromise<FirebaseFileResult> onCompleted);
        Task<bool> DeleteFile(string path);
    }

}
