using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zby
{
    class OS_Iphone : I_Os
    {
        override public String GetPlatformPath()
        {
            return Application.persistentDataPath + "//";
        }

        override public String GetPrefixForWWW()
        {
            return InEditor ? "file ://" : "file://";
        }
    }
}
