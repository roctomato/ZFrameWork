
using System;
using System.Collections;
using System.Collections.Generic;


namespace Zby
{
    using UnityEngine;

    static public class LoadResource
    {
            public static Object LoadMainAssetAtPath(string assetPath)
            {
#if UNITY_EDITOR
                return UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPath);
#else
                return null;
#endif
            }

            public static Object LoadAssetAtPath(string assetPath, Type type)
            {
#if UNITY_EDITOR
                return UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, type);
#else
                return null;
#endif
            }
        

        static public UnityEngine.Object Load(string path)
        {
            var obj = Resources.Load( path);
            if (obj == null)
            {
                ZLog.E(null, "{0} load fail", path);
                return null;
            }
            return obj;
        }

        static public UnityEngine.GameObject LoadAsGameObj(string path)
        {
            GameObject ins = null;
            var obj = LoadResource.Load(path);
            if (obj != null)
            {
                ins = UnityEngine.Object.Instantiate(obj) as GameObject;
                if (null != ins)
                {
                   ins.name = obj.name; 
                }
                else
                {
                    ZLog.E(null, "{0} Instantiate fail", path);
                }
            }
            
            return ins;
        }

        public static GameObject LoadObject(Transform parent, string path, bool active)
        {
            GameObject ins = LoadResource.LoadAsGameObj(path);
            if (null == ins)
            {
                return null;
            }
            ins.transform.SetParent(parent, active);
            return ins;
        }
    }

 
}
