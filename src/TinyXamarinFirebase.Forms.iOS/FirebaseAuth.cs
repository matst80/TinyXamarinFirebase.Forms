
using Firebase.Auth;
using Foundation;

namespace TinyXamarinFirebase.Froms.iOS
{

    public class FirebaseAuth : IFirebaseAuth
    {


        public FirebaseAuth()
        {

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