using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zby
{
    public enum E_PlatForm
    {
        E_PlatForm_PC,
        E_PlatForm_UNITY_IPHONE,
        E_PlatForm_UNITY_ANDROID,
    }

    public class I_Os
    {
        private bool _inEditor;

        public  bool InEditor
        {
            get { return _inEditor; }
        }
        public I_Os()
        {
#if UNITY_EDITOR
            _inEditor = true;
#else
            _inEditor =false;
#endif
        }

        public String GetUserDocumentPath()
        {
            if (_inEditor)
            {
                return Application.dataPath + "/../Res/";
            }
            return GetPlatformPath();
        }

        virtual public String GetPlatformPath() { return null; }
        virtual public String GetAssetbundlesPath() { return null; }
        virtual public String GetAssetbundlesURL() // pc iphone
        {
            string url = GetAssetbundlesPath();
            url = "file://" + url;
            return url;
        }

        virtual public String GetPrefixForWWW()
        {
            return null;
        }
    }

}
