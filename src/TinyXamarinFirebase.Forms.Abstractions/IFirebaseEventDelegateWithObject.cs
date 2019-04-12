namespace TinyXamarinFirebase.Forms
{
    public interface IFirebaseEventDelegateWithObject : IFirebaseEventDelegate
    {
        object DataInstance { get; set; }
    }
}
