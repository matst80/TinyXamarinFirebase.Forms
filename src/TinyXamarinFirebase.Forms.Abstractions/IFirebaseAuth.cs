using System;
using System.Threading.Tasks;

namespace TinyXamarinFirebase.Froms
{
    public interface IFirebaseMessaging
    {
        void SetReceiveRegistrationTokenHandler(Action<string> onToken);
        Task<string> FetchToken();
    }

    public interface IFirebaseAuth
    {
        void SignInWithCustomToken(string token, FirebaseLoginPromise completedPromise);
        void SignOut();
    }

}
