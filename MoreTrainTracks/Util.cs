using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace MoreTrainTracks
{
    public static class Util
    {
        public static string Remove(this string st, params string[] obj)
        {
            foreach (string s in obj)
            {
                st.Replace(s, "");
            }
            return st;
        }

        public static void Log(this object obj, string method = "debug")
        {
            if (method.ToUpper().Contains("DEBUG"))
                UnityEngine.Debug.Log(obj);
            else if (method.ToUpper().Contains("ERR"))
                UnityEngine.Debug.LogError(obj);
            else if (method.ToUpper().Contains("WAR"))
                UnityEngine.Debug.LogWarning(obj);
        }
        public static void WriteLine(this object obj, ref TextWriter tw)
        {
            tw.WriteLine(obj);
        }

        public static void SaveTexture(this Texture2D tex, string path, string method = "PNG")
        {
            byte[] b = new byte[] { };
            if (method == "PNG")
                b = tex.EncodeToPNG();
            else if (method == "JPG")
                b = tex.EncodeToJPG();
            else
                throw new ArgumentException("The given encoding method wasn't either PNG or JPG.");
            try
            {
                File.WriteAllBytes(path, b);
            }
            catch (Exception e)
            {
                throw new ArgumentException("The given path is not accessible : " + e.Message);
            }
        }


        public static List<T> RemoveNull<T>(this List<T> list)
        {
            foreach (T element in list)
            {
                if (element == null)
                    list.Remove(element);
            }
            return list;
        }
        public static List<T> MergeLists<T>(List<T> list1, List<T> list2)
        {
            foreach (T e in list2)
                list1.Add(e);
            return list1;
        }
    }
}
