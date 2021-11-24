﻿using System;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace JotunnLib.Utils
{
    public class ReflectionUtils
    {
        public static object InvokePrivate(object instance, string name, object[] args = null)
        {
            MethodInfo method = instance.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
            {
                Type[] types = args == null ? new Type[0] : args.Select(arg => arg.GetType()).ToArray();
                method = instance.GetType().GetMethod(name, types);
            }

            if (method == null)
            {
                Debug.LogError("Method " + name + " does not exist on type: " + instance.GetType());
                return null;
            }

            return method.Invoke(instance, args);
        }

        public static T GetPrivateField<T>(object instance, string name)
        {
            FieldInfo var = instance.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

            if (var == null)
            {
                Debug.LogError("Variable " + name + " does not exist on type: " + instance.GetType());
                return default(T);
            }

            return (T)var.GetValue(instance);
        }

        public static void SetPrivateField(object instance, string name, object value)
        {
            FieldInfo var = instance.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

            if (var == null)
            {
                Debug.LogError("Variable " + name + " does not exist on type: " + instance.GetType());
                return;
            }

            var.SetValue(instance, value);
        }

        public static T GetPrivateStaticField<T>(Type type, string name)
        {
            FieldInfo var = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Static);

            if (var == null)
            {
                Debug.LogError("Variable " + name + " does not exist on static type: " + type);
                return default(T);
            }

            return (T)var.GetValue(null);
        }
    }
}
