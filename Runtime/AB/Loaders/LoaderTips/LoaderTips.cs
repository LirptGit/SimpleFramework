﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace XFABManager
{
    internal abstract class LoaderTips
    {

        public virtual System.Type[] types { get; }

        public virtual string tips { get; }

        public virtual bool IsThrowException => false;

        private static Dictionary<Type, LoaderTips> allLoaderTips = null;

        public static Dictionary<Type, LoaderTips> AllLoaderTips
        {
            get
            {
                if (allLoaderTips == null)
                {
                    allLoaderTips = new Dictionary<System.Type, LoaderTips>();

#if UNITY_EDITOR 
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly assembly in assemblies)
                    { 
                        if (!assembly.FullName.StartsWith("XFABManager")) continue;
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (!XFABTools.IsBaseByClass(type, typeof(LoaderTips)) || type == typeof(LoaderTips)) continue;
                             
                            LoaderTips tip = Activator.CreateInstance(type) as LoaderTips;
                            if (tip == null) continue;
                            foreach (var item in tip.types)
                            {
                                if (allLoaderTips.ContainsKey(item)) continue;
                                allLoaderTips.Add(item, tip);
                            } 
                        } 
                    } 
#endif

                }
                return allLoaderTips;
            }
        } 
    }

}

