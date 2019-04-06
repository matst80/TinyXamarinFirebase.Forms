using System;
using Foundation;
using System.Collections;
using System.Linq;

namespace TinyXamarinFirebase.Froms.iOS
{
    public class DataConverter
    {
        public DataConverter()
        {

        }

        public object Convert(object platformData, Type resultType, object objectToModify = null)
        {
            if (platformData is NSString nsString)
            {
                return nsString.ToString();
            }
            else if (platformData is NSNumber value)
            {
                if (resultType == typeof(int))
                {
                    return (int)value;
                }
                else if (resultType == typeof(long))
                {
                    return (long)value;
                }
                else if (resultType == typeof(double))
                {
                    return (double)value;
                }
                else if (resultType == typeof(Single))
                {
                    return (float)value;
                }
                else if (resultType == typeof(bool))
                {
                    return (bool)value;
                }
            }
            else if (platformData is NSDictionary dict)
            {
                return Convert(dict, resultType, objectToModify);
            }
            return Activator.CreateInstance(resultType);
        }

        public object Convert(NSDictionary dict, Type returnType, object objectToModify = null)
        {
            var ret = objectToModify ?? Activator.CreateInstance(returnType);
            if (ret is IDictionary nodeDict)
            {
                var type = nodeDict.GetType();
                var keyType = GetItemType(type, 0);
                var dictType = GetItemType(type, 1);
                foreach (var itemData in dict)
                {
                    var key = Convert(itemData.Key, keyType);
                    if (nodeDict.Contains(key))
                    {
                        var item = Convert(itemData, dictType, nodeDict[key]);
                    }
                    else
                    {
                        var val = Convert(itemData.Value, dictType);
                        nodeDict.Add(key, val);
                    }
                }
            }
            foreach (var item in TypePropertyHelper.GetFirebaseProperties(returnType))
            {
                var prp = item.Value.Item1;
                var key = (NSString)item.Key;
                if (dict.ContainsKey(key))
                {
                    var data = dict[key];
                    try
                    {
                        var internalData = prp.GetValue(ret);
                        prp.SetValue(ret, Convert(data, prp.PropertyType, internalData));
                    }
                    catch (Exception ex)
                    {
                        var i = ex;
                    }
                }
            }
            return ret;
        }

        private Type GetItemType(Type type, int nr = 1)
        {
            if (!type.GenericTypeArguments.Any())
            {
                type = type.BaseType;
            }
            return type.GenericTypeArguments[nr];
        }

        public T Convert<T>(NSObject snapshot, object objectToModify = null)
        {

            var ret = objectToModify ?? Activator.CreateInstance<T>();
            if (snapshot is NSDictionary dict)
            {
                return (T)Convert(dict, typeof(T), ret);
            }
            return (T)ret;
        }

        public NSObject ToNative(object data)
        {
            if (data is int i)
            {
                return new NSNumber(i);
            }
            if (data is long lng)
            {
                return new NSNumber(lng);
            }
            if (data is long d)
            {
                return new NSNumber(d);
            }
            if (data is Single s)
            {
                return new NSNumber(s);
            }
            if (data is bool b)
            {
                return new NSNumber(b);
            }
            if (data is string str)
            {
                return new NSString(str);
            }
            if (data is IDictionary dict)
            {
                throw new NotImplementedException();
            }
            return null;
        }

        public NSDictionary ToNative<T>(T data)
        {
            var type = typeof(T);

            NSMutableDictionary ret = new NSMutableDictionary();
            foreach (var item in TypePropertyHelper.GetFirebaseProperties(type))
            {
                var prp = item.Value.Item1;
                var key = (NSString)item.Key;
                var objData = prp.GetValue(data);
                if (objData != null)
                {
                    var nativeData = ToNative(objData);
                    ret.Add(key, nativeData);
                }
            }
            return ret;
        }
    }
}