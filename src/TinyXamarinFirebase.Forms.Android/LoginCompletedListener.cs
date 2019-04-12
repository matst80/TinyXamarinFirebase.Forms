using Android.Gms.Tasks;

namespace TinyXamarinFirebase.Forms.Droid
{
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