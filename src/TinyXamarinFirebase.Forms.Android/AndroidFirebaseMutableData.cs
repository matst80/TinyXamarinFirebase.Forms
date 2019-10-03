using Firebase.Database;

namespace TinyXamarinFirebase.Forms.Droid
{
    public class AndroidFirebaseMutableData<T> : FirebaseMutableData<T>
    {
        private readonly MutableData mutableData;

        public AndroidFirebaseMutableData()
        {

        }

        internal AndroidFirebaseMutableData(MutableData mutableData)
        {
            this.mutableData = mutableData;

        }

        public override T GetValue()
        {
            return DataConverter.Convert<T>(mutableData.Value);
        }

        public override void SetValue(T data)
        {
            mutableData.Value = ToNativeConverter.ToNative(typeof(T), data);
        }
    }
}