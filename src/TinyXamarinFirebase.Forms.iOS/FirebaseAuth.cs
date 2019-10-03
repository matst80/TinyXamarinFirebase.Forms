
using Firebase.Auth;
using Foundation;

namespace TinyXamarinFirebase.Forms.iOS
{

    public class FirebaseAuth : IFirebaseAuth
    {


        public FirebaseAuth()
        {

        }

        public string CurrentUserId
        {
            get
            {
                return Firebase.Auth.Auth.DefaultInstance?.CurrentUser?.Uid;
            }
        }

        public void SignInWithCustomToken(string token, FirebaseLoginPromise completedPromise)
        {
            Firebase.Auth.Auth.DefaultInstance.SignInWithCustomToken(token, (authResult, error) =>
            {
                completedPromise.OnComplete(error == null);
            });
        }

        public void SignOut()
        {
            NSError error = null;
            Firebase.Auth.Auth.DefaultInstance.SignOut(out error);
        }
    }
}