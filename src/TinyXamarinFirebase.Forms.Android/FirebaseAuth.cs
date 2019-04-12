using Firebase;
using Firebase.Auth;
using Firebase.Storage;

namespace TinyXamarinFirebase.Forms.Droid
{

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