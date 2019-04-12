using Firebase.Database;

namespace TinyXamarinFirebase.Froms.iOS
{
    public class IosMutableData<T> : FirebaseMutableData<T>
    {
        private MutableData currentData;

        public IosMutableData(MutableData currentData)
        {
            this.currentData = currentData;
        }

        public override T GetValue()
        {
            return DataConverter.Convert<T>(currentData.GetValue());
        }

        public override void SetValue(T data)
        {
            currentData.SetValue(DataConverter.ToNative(data));
            //var i = data;
        }
    }
}