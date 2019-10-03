using System;
using Foundation;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace TinyXamarinFirebase.Forms.iOS
{
    public static class DataConverter
    {


        public static object Convert(object platformData, Type resultType, object objectToModify = null)
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

            if (resultType.IsValueType && resultType != typeof(string))
            {
                return Activator.CreateInstance(resultType);
            }
            else
            {
                return null;
            }
        }

        public static object Convert(NSDictionary dict, Type returnType, object objectToModify = null)
        {
            var ret = objectToModify ?? Activator.CreateInstance(returnType);
            if (ret is IDictionary nodeDict)
            {
                var type = nodeDict.GetType();
                var keyType = GetItemType(type, 0);
                var dictType = GetItemType(type, 1);
                var keysToRemove = new List<object>();
                foreach (var key in nodeDict.Keys)
                {
                    keysToRemove.Add(key);
                }
                foreach (var itemData in dict)
                {
                    var key = Convert(itemData.Key, keyType);
                    if (keysToRemove.Contains(key))
                    {
                        keysToRemove.Remove(key);
                    }
                    if (nodeDict.Contains(key))
                    {
                        var item = Convert(itemData.Value, dictType, nodeDict[key]);
                    }
                    else
                    {
                        var val = Convert(itemData.Value, dictType);
                        nodeDict.Add(key, val);
                    }
                }
                foreach (var keyToRemove in keysToRemove)
                {
                    if (nodeDict.Contains(keyToRemove))
                    {
                        nodeDict.Remove(keyToRemove);
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
                    //try
                    //{
                    var internalData = prp.GetValue(ret);
                    prp.SetValue(ret, Convert(data, prp.PropertyType, internalData));
                    //}
                    //catch (Exception ex)
                    //{
                    //    var i = ex;
                    //}
                }
                else
                {
                    if (prp.PropertyType.IsValueType && prp.PropertyType != typeof(string))
                    {
                        prp.SetValue(ret, Activator.CreateInstance(prp.PropertyType));
                    }
                    else
                    {
                        prp.SetValue(ret, null);
                    }
                }
            }
            return ret;
        }

        internal static NSDictionary ToDictionary<T>(T data)
        {
            return ToNative(data) as NSDictionary;
        }

        private static Type GetItemType(Type type, int nr = 1)
        {
            if (!type.GenericTypeArguments.Any())
            {
                type = type.BaseType;
            }
            return type.GenericTypeArguments[nr];
        }

        public static T Convert<T>(NSObject snapshot, object objectToModify = null)
        {
            var tType = typeof(T);
            if (tType.IsPrimitive || snapshot is NSString)
            {
                return (T)Convert(snapshot, tType, objectToModify);
            }
            var ret = objectToModify ?? Activator.CreateInstance<T>();
            if (snapshot is NSDictionary dict)
            {
                return (T)Convert(dict, typeof(T), ret);
            }
            return (T)ret;
        }

        public static NSObject ToNativePrimitive(object data)
        {
            if (data is int i)
            {
                return new NSNumber(i);
            }
            if (data is long lng)
            {
                return new NSNumber(lng);
            }
            if (data is double d)
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
            return null;
        }

        public static NSObject ToNative<T>(T data)
        {
            if (data == null)
                return null;
            var type = data.GetType();

            if (type.IsPrimitive || data is string)
            {
                if (data is T)
                {
                    return ToNativePrimitive(data);
                }
                return new NSObject();
            }
            if (data is IDictionary dict)
            {
                var ret = new NSMutableDictionary();
                foreach (var key in dict.Keys)
                {
                    var itemdata = dict[key];
                    if (itemdata != null)
                        ret.Add(new NSString(key.ToString()), ToNative(itemdata));
                }
                return ret;
            }
            else
            {
                var ret = new NSMutableDictionary();
                foreach (var item in TypePropertyHelper.GetFirebaseProperties(type))
                {
                    var prp = item.Value.Property;
                    if (!item.Value.CustomAttribute.ReadOnly)
                    {
                        var key = new NSString(item.Key);
                        var objData = prp.GetValue(data);
                        if (objData != null)
                        {
                            var nativeData = ToNative(objData);
                            ret.Add(key, nativeData);
                        }
                    }
                }
                return ret;
            }
        }
    }
}