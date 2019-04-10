using System;
namespace TinyXamarinFirebase.Froms
{
    public static class FirebaseXamarinHelper
    {
        public static Action<Action> RunOnUIThread { get; set; } = (action) => action?.Invoke();
    }
}
