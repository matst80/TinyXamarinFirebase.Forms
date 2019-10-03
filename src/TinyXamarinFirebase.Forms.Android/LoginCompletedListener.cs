using Android.Gms.Tasks;
using Firebase.Auth;

namespace TinyXamarinFirebase.Forms.Droid
{
    //internal class AuthStateListener : Java.Lang.Object, Firebase.Auth.FirebaseAuth.IAuthStateListener
    //{
    //    private FirebasePromise<bool> completedListener;

    //    public AuthStateListener(FirebasePromise<bool> completedListener)
    //    {
    //        this.completedListener = completedListener;
    //    }

    //    public void OnAuthStateChanged(Firebase.Auth.FirebaseAuth auth)
    //    {

    //        auth.CurrentUser
    //    }

    //    public void OnComplete(Task task)
    //    {
    //        completedListener.OnComplete(task.IsSuccessful);
    //    }
    //}

    internal class LoginCompletedListener : Java.Lang.Object, IOnCompleteListener
    {
        private FirebasePromise<bool> completedListener;

        public LoginCompletedListener(FirebasePromise<bool> completedListener)
        {
            this.completedListener = completedListener;
        }


        public void OnComplete(Task task)
        {
            completedListener.OnComplete(task.IsSuccessful);
        }
    }

    internal class CommandCompletedListener : Java.Lang.Object, IOnCompleteListener
    {
        private FirebasePromise<bool> completedListener;

        public CommandCompletedListener(FirebasePromise<bool> completedListener)
        {
            this.completedListener = completedListener;
        }

        public void OnComplete(Task task)
        {
            completedListener.OnComplete(task.IsSuccessful);
        }
    }
}