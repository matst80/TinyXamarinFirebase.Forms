namespace TinyXamarinFirebase.Froms
{
    public interface IFirebaseAuth
    {
        void SignInWithCustomToken(string token, FirebaseLoginPromise completedPromise);
        void SignOut();
    }

}
