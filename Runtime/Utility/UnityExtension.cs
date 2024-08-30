using System.IO;
using System.Text;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace SimpleFramework
{
    public static class UnityExtension
    {
        /// <summary>
        /// 获取或者添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        public static void SetPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.x = newValue;
            transform.position = v;
        }

        public static void SetPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.y = newValue;
            transform.position = v;
        }

        public static void SetPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.z = newValue;
            transform.position = v;
        }

        public static void SetLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.x = newValue;
            transform.localPosition = v;
        }

        public static void SetLocalPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.y = newValue;
            transform.localPosition = v;
        }

        public static void SetLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.z = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(this Transform transform)
        {
            if (transform == null) return;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 获取指定文件夹下所有特定类型的文件
        /// </summary>
        /// <param name="rootPath">根目录</param>
        /// <param name="filePathList">返回文件的数组</param>
        /// <param name="FileType">文件类型</param>
        public static void GetAllFile(string rootPath, ref List<FileInfo> filePathList, string FileType = "")
        {
            DirectoryInfo direction = new DirectoryInfo(rootPath);

            FileInfo[] files = direction.GetFiles("*");

            for (int i = 0; i < files.Length; i++)
            {
                if (FileType == "") //获取所有的文件
                {
                    if (files[i].Name.EndsWith(".mate"))
                    {
                        //剔除mate文件
                        continue;
                    }
                    filePathList.Add(files[i]);
                }
                else if (FileType != "" && files[i].Name.EndsWith(FileType)) //获取执行后缀的文件
                {
                    filePathList.Add(files[i]);
                }
            }

            DirectoryInfo[] directions = direction.GetDirectories("*");

            for (int i = 0; i < directions.Length; i++)
            {
                GetAllFile(directions[i].FullName, ref filePathList, FileType);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 时间文本格式
        /// </summary>
        /// <param name="time">单位：秒</param>
        /// <returns>格式 00:00:00</returns>
        public static string TimeFormat(float time)
        {
            return (((int)(time / 3600)) < 10 ? "0" + ((int)(time / 3600)) : ((int)(time / 3600)).ToString()) + ":" +
               (((int)(time % 3600 / 60) < 10) ? ("0" + (int)(time % 3600 / 60)) : ((int)(time % 3600 / 60)).ToString()) + ":" +
               (((int)(time % 3600 % 60) < 10) ? ("0" + (int)(time % 3600 % 60)) : ((int)(time % 3600 % 60)).ToString());
        }

        /// <summary>
        /// 时间文本格式
        /// </summary>
        /// <param name="time">单位：秒</param>
        /// <returns>格式 00:00:00</returns>
        public static string TimeFormatTwo(float time)
        {
            return ((((int)(time / 3600)) * 60 + ((int)(time % 3600 / 60)) < 10) ?
                ("0" + (int)(time % 3600 / 60)) : ((int)(time % 3600 / 60)).ToString()) + ":" +
               (((int)(time % 3600 % 60) < 10) ? ("0" + (int)(time % 3600 % 60)) : ((int)(time % 3600 % 60)).ToString());
        }

        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 根据时间戳获取时间
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <returns></returns>
        public static DateTime GetDateTime(double timeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}