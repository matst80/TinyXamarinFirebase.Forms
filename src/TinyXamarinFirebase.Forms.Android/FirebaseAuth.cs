using System;
using System.IO;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
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

        }

        public void PutFile(string path, Uri uri, FirebasePromise<FirebaseFileResult> onCompleted)
        {

        }
    }

    public class FirebaseAuth : IFirebaseAuth
    {
        private readonly FirebaseApp app;
        private Firebase.Auth.FirebaseAuth instance;

        public FirebaseAuth(FirebaseApp app)
        {
            this.app = app;
            instance = Firebase.Auth.FirebaseAuth.GetInstance(app);
        }

        public void SignInWithCustomToken(string token, FirebaseLoginPromise completedListener)
        {
            instance.SignInWithCustomToken(token).AddOnCompleteListener(new LoginCompletedListener(completedListener));
        }

        public void SignOut()
        {
            instance.SignOut();
        }
    }
}