using System;
using System.Threading.Tasks;

namespace TinyXamarinFirebase.Forms
{

    public interface IFirebaseAuth
    {
        void SignInWithCustomToken(string token, FirebaseLoginPromise completedPromise);
        void SignOut();
    }

}
