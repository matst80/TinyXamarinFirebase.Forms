using Firebase.Database;

namespace TinyXamarinFirebase.Froms.iOS
{
    public class IosMutableData<T> : FirebaseMutableData<T>
    {
        private MutableData currentData;
        private DataConverter converter;

        public IosMutableData(MutableData currentData)
        {
            this.currentData = currentData;
            this.converter = new DataConverter();
        }

        public override T GetValue()
        {
            return converter.Convert<T>(currentData.GetValue());
        }

        public override void SetValue(T data)
        {
            currentData.SetValue(converter.ToNative(data));
            //var i = data;
        }
    }
}