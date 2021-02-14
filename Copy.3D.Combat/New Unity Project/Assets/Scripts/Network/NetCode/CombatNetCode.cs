﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CombatNetcode
{
    public interface MainScriptInterface
    {
    }

    public class CombatBehaviour : MonoBehaviour
    {
        public virtual void CombatFixedUpdate()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RecordVar : Attribute
    {
        public RecordVar()
        {
        }

        public static Dictionary<System.Reflection.MemberInfo, System.Object> SaveStateTrackers(MainScriptInterface sourceObj,
            Dictionary<System.Reflection.MemberInfo, System.Object> targetDictionary)
        {
            if (sourceObj == null) return null;

            MemberInfo[] members = sourceObj.GetType().GetMembers();
            foreach (var prop in members)
            {
                //System.Reflection.FieldInfo fieldInfo = sourceObj.GetType().GetField(prop.Name);
                //System.Reflection.PropertyInfo propertyInfo = sourceObj.GetType().GetProperty(prop.Name);
                System.Reflection.FieldInfo fieldInfo = prop as System.Reflection.FieldInfo; // Perfomance improvement
                System.Reflection.PropertyInfo
                    propertyInfo = prop as System.Reflection.PropertyInfo; // Perfomance improvement

                //RecordVar[] recordAttr = (RecordVar[])prop.GetCustomAttributes(typeof(RecordVar), false);
                bool isRecord = (fieldInfo != null || propertyInfo != null) &&
                                prop.IsDefined(typeof(RecordVar), false); //Perfomance improvement

                if (isRecord)
                {
                    System.Object objValue = null;
                    Type objType = sourceObj.GetType();

                    if (fieldInfo != null)
                    {
                        objValue = fieldInfo.GetValue(sourceObj);
                        objType = fieldInfo.FieldType;
                    }
                    else if (propertyInfo != null)
                    {
                        objValue = propertyInfo.GetValue(sourceObj, null);
                        objType = propertyInfo.PropertyType;
                    }

                    if (objValue is MainScriptInterface || (objType != null && IsMainScriptInterface(objType)))
                    {
                        // Object is MainScript Interface
                        Dictionary<System.Reflection.MemberInfo, System.Object> recursiveDictionary =
                            new Dictionary<System.Reflection.MemberInfo, System.Object>();

                        // Save the object reference itself
                        recursiveDictionary.Add(prop, objValue);

                        objValue = SaveStateTrackers(objValue as MainScriptInterface, recursiveDictionary);
                    }
                    else if (objValue != null && objType != null && objType.IsGenericType &&
                             (objType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) || objType is IList))
                    {
                        // Object is List Type
                        objValue = SaveListTracker(objValue as IList, prop);
                    }
                    else if (objValue != null && objType != null && objType.IsGenericType &&
                             objType.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)))
                    {
                        // Object is Dictionary Type
                        Type type1 = objValue.GetType().GetGenericArguments()[0]; // Key
                        Type type2 = objValue.GetType().GetGenericArguments()[1]; // Value
                        objValue = SaveDictionaryTracker(objValue as IDictionary, type1, type2, prop);
                    }
                    else if (objValue != null && objType != null && objType.IsArray)
                    {
                        // Object is Array Type
                        objValue = SaveArrayTracker(objValue, prop);
                    }

                    if (!targetDictionary.ContainsKey(prop))
                    {
                        targetDictionary.Add(prop, objValue);
                    }
                    else
                    {
                        targetDictionary[prop] = objValue;
                    }
                }
            }

            return targetDictionary;
        }

        public static MainScriptInterface LoadStateTrackers(MainScriptInterface targetObj,
            Dictionary<System.Reflection.MemberInfo, System.Object> sourceDictionary)
        {
            if (targetObj == null) return null;

            MemberInfo[] members = targetObj.GetType().GetMembers();
            foreach (var prop in members)
            {
                System.Reflection.FieldInfo fieldInfo = prop as System.Reflection.FieldInfo;
                System.Reflection.PropertyInfo propertyInfo = prop as System.Reflection.PropertyInfo;

                bool isRecord = (fieldInfo != null || propertyInfo != null) && prop.IsDefined(typeof(RecordVar), false);

                if (isRecord)
                {
                    var objValue = sourceDictionary[prop];
                    Type objType = null;

                    if (fieldInfo != null) objType = fieldInfo.FieldType;
                    if (propertyInfo != null) objType = propertyInfo.PropertyType;

                    if (objValue != null && objValue is Dictionary<System.Reflection.MemberInfo, System.Object>)
                    {
                        // Object is Recursive Dictionary
                        var recursiveObject =
                            (objValue as Dictionary<System.Reflection.MemberInfo, System.Object>)[prop];
                        if (fieldInfo != null) fieldInfo.SetValue(targetObj, recursiveObject);
                        if (propertyInfo != null) propertyInfo.SetValue(targetObj, recursiveObject, null);

                        objValue = LoadStateTrackers(recursiveObject as MainScriptInterface,
                            objValue as Dictionary<System.Reflection.MemberInfo, System.Object>);
                    }
                    else if (objValue != null && objType != null &&
                             (objType.IsGenericType &&
                              objType.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) ||
                              objType is IList))
                    {
                        // Object is List Type
                        objValue = LoadListTracker(objValue as IList, objType, prop);
                    }
                    else if (objValue != null && objType != null && objType.IsGenericType &&
                             objType.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>)))
                    {
                        // Object is Dictionary Type
                        Type type1 = objValue.GetType().GetGenericArguments()[0]; // Key
                        Type type2 = objValue.GetType().GetGenericArguments()[1]; // Value
                        objValue = LoadDictionaryTracker(objValue as IDictionary, type1, type2, prop);
                    }
                    else if (objValue != null && objType != null && objType.IsArray)
                    {
                        // Object is Array Tracker Type
                        objValue = LoadArrayTracker(
                            objValue as Dictionary<System.Reflection.MemberInfo, System.Object>[],
                            objType.GetElementType(), prop);
                    }

                    if (fieldInfo != null) fieldInfo.SetValue(targetObj, objValue);
                    if (propertyInfo != null) propertyInfo.SetValue(targetObj, objValue, null);
                }
            }

            return targetObj;
        }


        public static IList SaveListTracker(IList source, MemberInfo memberInfo)
        {
            List<System.Object> newList = new List<System.Object>();
            foreach (var entry in source as IList)
            {
                var newEntry = entry;
                if (entry is MainScriptInterface || IsMainScriptInterface(entry.GetType()))
                {
                    Dictionary<System.Reflection.MemberInfo, System.Object> recursiveDictionary =
                        new Dictionary<System.Reflection.MemberInfo, System.Object>();
                    recursiveDictionary.Add(memberInfo, newEntry);

                    newEntry = SaveStateTrackers(entry as MainScriptInterface, recursiveDictionary);
                }

                newList.Add(newEntry);
            }

            return newList;
        }

        public static IDictionary SaveDictionaryTracker(IDictionary source, Type key, Type value, MemberInfo memberInfo)
        {
            Type dictType = typeof(Dictionary<,>).MakeGenericType(key, value);
            IDictionary newDictionary = Activator.CreateInstance(dictType) as IDictionary;
            foreach (DictionaryEntry entry in source)
            {
                var newEntry = entry.Value;
                if (entry.Value is MainScriptInterface || IsMainScriptInterface(entry.Value.GetType()))
                {
                    Dictionary<System.Reflection.MemberInfo, System.Object> recursiveDictionary =
                        new Dictionary<System.Reflection.MemberInfo, System.Object>();
                    recursiveDictionary.Add(memberInfo, newEntry);

                    newEntry = SaveStateTrackers(entry.Value as MainScriptInterface, recursiveDictionary);
                }

                newDictionary.Add(entry.Key, newEntry);
            }

            return newDictionary;
        }

        public static Dictionary<System.Reflection.MemberInfo, System.Object>[] SaveArrayTracker(System.Object source,
            MemberInfo memberInfo)
        {
            Dictionary<System.Reflection.MemberInfo, System.Object>[] newArray =
                new Dictionary<System.Reflection.MemberInfo, System.Object>[(source as Array).Length];
            int i = 0;
            foreach (var entry in source as Array)
            {
                Dictionary<System.Reflection.MemberInfo, System.Object> newEntry =
                    new Dictionary<System.Reflection.MemberInfo, System.Object>();
                if (entry != null && (entry is MainScriptInterface || IsMainScriptInterface(entry.GetType())))
                {
                    Dictionary<System.Reflection.MemberInfo, System.Object> recursiveDictionary =
                        new Dictionary<System.Reflection.MemberInfo, System.Object>();
                    recursiveDictionary.Add(memberInfo, entry);

                    newEntry = SaveStateTrackers(entry as MainScriptInterface, recursiveDictionary);
                }
                else
                {
                    newEntry.Add(memberInfo, entry);
                }

                newArray[i] = newEntry;
                i++;
            }

            return newArray;
        }

        public static IList LoadListTracker(IList source, Type T, MemberInfo memberInfo)
        {
            IList newList = Activator.CreateInstance(T) as IList;
            foreach (var entry in source as IList)
            {
                var newEntry = entry;
                if (entry is Dictionary<System.Reflection.MemberInfo, System.Object>)
                {
                    var recursiveObject =
                        (entry as Dictionary<System.Reflection.MemberInfo, System.Object>)[memberInfo];
                    newEntry = LoadStateTrackers(recursiveObject as MainScriptInterface,
                        entry as Dictionary<System.Reflection.MemberInfo, System.Object>);
                }

                (newList as IList).Add(newEntry);
            }

            return newList;
        }

        public static IDictionary LoadDictionaryTracker(IDictionary source, Type key, Type value, MemberInfo memberInfo)
        {
            Type dictType = typeof(Dictionary<,>).MakeGenericType(key, value);
            IDictionary newDictionary = Activator.CreateInstance(dictType) as IDictionary;
            foreach (DictionaryEntry entry in source)
            {
                var newEntry = entry.Value;
                if (entry.Value is Dictionary<System.Reflection.MemberInfo, System.Object>)
                {
                    var recursiveObject =
                        (entry.Value as Dictionary<System.Reflection.MemberInfo, System.Object>)[memberInfo];
                    newEntry = LoadStateTrackers(recursiveObject as MainScriptInterface,
                        entry.Value as Dictionary<System.Reflection.MemberInfo, System.Object>);

                    newEntry = LoadStateTrackers(recursiveObject as MainScriptInterface,
                        entry.Value as Dictionary<System.Reflection.MemberInfo, System.Object>);
                }

                newDictionary.Add(entry.Key, newEntry);
            }

            return newDictionary;
        }

        public static object LoadArrayTracker(Dictionary<System.Reflection.MemberInfo, System.Object>[] source,
            Type elementType, MemberInfo memberInfo)
        {
            var newArray = Array.CreateInstance(elementType, (source as Array).Length);
            int i = 0;
            foreach (var entry in source)
            {
                if (entry is Dictionary<System.Reflection.MemberInfo, System.Object>)
                {
                    var recursiveObject =
                        (entry as Dictionary<System.Reflection.MemberInfo, System.Object>)[memberInfo];
                    newArray.SetValue(
                        LoadStateTrackers(recursiveObject as MainScriptInterface,
                            entry as Dictionary<System.Reflection.MemberInfo, System.Object>), i);
                }
                else
                {
                    newArray.SetValue(entry, i);
                }

                i++;
            }

            return newArray;
        }

        public static bool IsMainScriptInterface(Type T)
        {
            Type[] interfaceList = T.GetInterfaces();
            foreach (Type t in interfaceList)
            {
                if (t == typeof(MainScriptInterface)) return true;
            }

            return false;
        }
    }
}