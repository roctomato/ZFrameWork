using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zby
{
    public static class OSTools
    {
        static private I_Os s_Os;

        static E_PlatForm GetPlatform()
        {
            E_PlatForm plat = E_PlatForm.E_PlatForm_PC;
            #if UNITY_IPHONE
                plat = E_PlatForm.E_PlatForm_UNITY_IPHONE;
            #elif UNITY_ANDROID
                plat = E_PlatForm.E_PlatForm_UNITY_ANDROID;
            #endif
            return plat;
        }
        static public void Init_OS()
        {
            E_PlatForm plat = GetPlatform();
            if (plat == E_PlatForm.E_PlatForm_UNITY_IPHONE)
            {
                s_Os = new OS_Iphone();
            }else if (plat == E_PlatForm.E_PlatForm_UNITY_ANDROID)
            {
                s_Os = new OS_Android();
            }
            else
            {
                s_Os = new OS_PC();
            }
        }

        public static String GetUserDocumentPath()
        {
            if (null == s_Os)
            {
                Init_OS();
            }
            return s_Os.GetUserDocumentPath();
        }

        public static string GetAssetbundlesPath()
        {
            if (null == s_Os)
            {
                Init_OS();
            }
            return s_Os.GetAssetbundlesPath();
        }
        public static string GetAssetbundlesURL()
        {
            if (null == s_Os)
            {
                Init_OS();
            }
            return s_Os.GetAssetbundlesURL();
        }
        public static string GetPrefixForWWW()
        {
            if (null == s_Os)
            {
                Init_OS();
            }
            return s_Os.GetPrefixForWWW();
        }

        public static bool InEditor()
        {
            if (null == s_Os)
            {
                Init_OS();
            }
            return s_Os.InEditor;
        }
    }
}
