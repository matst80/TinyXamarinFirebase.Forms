using Android.Runtime;
using System;
using System.Collections;
using System.Linq;
using Java.Lang;

namespace TinyXamarinFirebase.Froms.Droid
{

    public class ToNativeConverter
    {
        public Java.Lang.Object ToNative(Type type, object data)
        {
            var dataType = data.GetType();
            if (dataType.IsPrimitive || data is string)
            {
                if (data is bool b)
                {
                    return new Java.Lang.Boolean(b);
                }
                else if (data is float f)
                {
                    return new Java.Lang.Float(f);
                }
                else if (data is double d)
                {
                    return new Java.Lang.Double(d);
                }
                else if (data is long l)
                {
                    return new Java.Lang.Long(l);
                }
                else if (data is int i)
                {
                    return new Java.Lang.Integer(i);
                }
                else if (data is string s)
                {
                    return new Java.Lang.String(s);
                }
                return (Java.Lang.Object)data;
            }
            else
            {
                var ret = new JavaDictionary<string, Java.Lang.Object>();
                foreach (var item in TypePropertyHelper.GetFirebaseProperties(type))
                {
                    var prp = item.Value.Property;
                    var key = item.Key;
                    var objData = prp.GetValue(data);
                    if (objData != null)
                    {
                        ret.Add(key, objData);
                    }
                }
                return ret;
            }
            //return (Java.Lang.Object) data;
        }
    }

    public class DataConverter
    {
      
        public object Convert(object platformData, Type resultType, object objectToModify = null)
        {
            if (platformData is JavaDictionary dict)
            {
                return Convert(dict, resultType, objectToModify);
            }

            return System.Convert.ChangeType(platformData, resultType);

        }

        public object Convert(JavaDictionary dict, Type returnType, object objectToModify = null)
        {
            var ret = objectToModify ?? Activator.CreateInstance(returnType);
            if (ret is IDictionary nodeDict)
            {
                var type = nodeDict.GetType();
                var keyType = GetItemType(type, 0);
                var dictType = GetItemType(type, 1);
                foreach (DictionaryEntry itemData in dict)
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
                var prp = item.Value.Property;
                var key = (string)item.Key;
                if (dict.Contains(key))
                {
                    var data = dict[key];
                    try
                    {
                        var internalData = prp.GetValue(ret);
                        prp.SetValue(ret, Convert(data, prp.PropertyType, internalData));
                    }
                    catch (System.Exception ex)
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

        public T Convert<T>(Java.Lang.Object snapshot, object objectToModify = null)
        {
            var tType = typeof(T);
            if (tType.IsPrimitive || typeof(string).IsAssignableFrom(tType))
            {
                return (T)System.Convert.ChangeType(snapshot, tType);
            }
            var ret = objectToModify ?? Activator.CreateInstance<T>();
            if (snapshot is JavaDictionary dict)
            {
                return (T)Convert(dict, typeof(T), ret);
            }
            return (T)ret;
        }
    }
}