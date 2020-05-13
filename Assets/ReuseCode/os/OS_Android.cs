using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zby
{
    class OS_Android : I_Os
    {
        override public String GetPlatformPath()
        {
            return Application.persistentDataPath + "//";
        }
        override public String GetAssetbundlesPath()
        {
            return InEditor ? Application.dataPath + "/../" + "AssetsAndroid/" : Application.dataPath + "!/assets/";
        }

        override public string GetAssetbundlesURL()
        {
            string url = GetAssetbundlesPath();
            return InEditor ? "file ://" + url : "jar:file://" + url;
        }

        override public String GetPrefixForWWW()
        {
            return InEditor ? "file ://"  : "jar:file://" ;
        }
    }
}
