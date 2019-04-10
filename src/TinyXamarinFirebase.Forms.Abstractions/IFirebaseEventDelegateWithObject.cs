namespace TinyXamarinFirebase.Froms
{
    public interface IFirebaseEventDelegateWithObject : IFirebaseEventDelegate
    {
        object DataInstance { get; set; }
    }
}
