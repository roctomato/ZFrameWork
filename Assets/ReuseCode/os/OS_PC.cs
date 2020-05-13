using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zby
{
    class OS_PC : I_Os
    {
        override public String GetPlatformPath()
        {
            string path = Application.dataPath + "/../";
            return path;
        }

        override public String GetAssetbundlesPath()
        {
            return Application.dataPath + "/../" + "AssetsPC/";
        }
        override public String GetPrefixForWWW()
        {
            return "file :///_";
        }
    }
}
