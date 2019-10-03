using System;
namespace TinyXamarinFirebase.Forms
{
    public static class FirebaseXamarinHelper
    {
        public static Action<Action> RunOnUIThread { get; set; } = (action) => action?.Invoke();
    }
}
