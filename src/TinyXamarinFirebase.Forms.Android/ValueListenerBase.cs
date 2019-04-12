namespace TinyXamarinFirebase.Forms.Droid
{
    public abstract class ValueListenerBase : Java.Lang.Object
    {
        internal void SetObjectFromHandler(IFirebaseEventDelegate handler, object snapData)
        {
            if (handler is IFirebaseEventDelegateWithObject withObject && snapData != null)
            {
                if (withObject.DataInstance != snapData)
                {
                    withObject.DataInstance = snapData;
                }
            }
        }

        internal object GetObjectFromHandler(IFirebaseEventDelegate handler)
        {
            if (handler is IFirebaseEventDelegateWithObject withObject)
            {
                return withObject.DataInstance;
            }
            return null;
        }
    }
}