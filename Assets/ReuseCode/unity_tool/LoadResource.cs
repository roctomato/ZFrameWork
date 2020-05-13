using UnityEngine;

namespace Zby
{
    static public class LoadResource
    {
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
