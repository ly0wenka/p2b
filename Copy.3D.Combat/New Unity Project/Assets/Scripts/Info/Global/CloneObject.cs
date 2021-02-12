using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public static class CloneObject 
{
    public static object objCopy;

    public static object Clone(object target) 
        => ReflectionClone(target);

    public static object Clone(object target, bool serialized)
        => serialized ? SerializedClone(target) : ReflectionClone(target);

    public static object SerializedClone(object target)
    {
        if (target == null) 
        { 
            return null; 
        }

        using (Stream objectStream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, target);
            objectStream.Seek(0, SeekOrigin.Begin);
            return (object)formatter.Deserialize(objectStream);
        }
    }

    public static object ReflectionCloneArray(object[] target)
    {
        object[] arrayObj = (object[])Array.CreateInstance(target.GetType().GetElementType(), target.Length);

        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == null
                || target[i].GetType().IsEnum
                || target[i].GetType().IsValueType
                || target[i].GetType().IsGenericType
                || target[i].GetType().Equals(typeof(String))
                || target[i].GetType().IsSubclassOf(typeof(ScriptableObject)))
            {
                arrayObj[i] = target[i];
            }
            else
            {
                arrayObj[i] = ReflectionClone(target[i]);
            }
        }

        return arrayObj;
    }

    public static object ReflectionClone(object target)
    {
        Type typeSource = target.GetType();

        if (typeSource.IsArray) 
        { 
            return (object)ReflectionCloneArray((object[])target); 
        }

        object newObj = Activator.CreateInstance(typeSource);
        FieldInfo[] fields = typeSource.GetFields();

        foreach (FieldInfo field in fields)
        {
            object fieldValue = field.GetValue(target);

            if (fieldValue == null
                || field.FieldType.IsEnum
                || field.FieldType.IsValueType
                || field.FieldType.Equals(typeof(String))
                || field.FieldType.GetInterface("IClonable", true) == null
                || field.FieldType.GetInterface("ScriptableObject", true) != null
                || field.FieldType.IsSubclassOf(typeof(ScriptableObject))
                )
            {
                field.SetValue(newObj, fieldValue);
            }
            else
            {
                field.SetValue(newObj, ReflectionClone(fieldValue));
            }
        }

        return newObj;
    }
}
