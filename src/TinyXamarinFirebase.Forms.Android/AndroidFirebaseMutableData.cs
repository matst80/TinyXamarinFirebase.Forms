using Firebase.Database;

namespace TinyXamarinFirebase.Froms.Droid
{
    public class AndroidFirebaseMutableData<T> : FirebaseMutableData<T>
    {
        private readonly MutableData mutableData;
        private readonly DataConverter converter;
        private readonly ToNativeConverter toNative;

        public AndroidFirebaseMutableData()
        {

        }

        internal AndroidFirebaseMutableData(MutableData mutableData)
        {
            this.mutableData = mutableData;
            this.converter = new DataConverter();
            this.toNative = new ToNativeConverter();
        }

        public override T GetValue()
        {
            return converter.Convert<T>(mutableData.Value);
        }

        public override void SetValue(T data)
        {
            mutableData.Value = toNative.ToNative(typeof(T), data);
        }
    }
}