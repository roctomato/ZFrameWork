using System.Diagnostics;
using UnityEngine;

namespace Zby
{
    class SimpleLoader : TmplLoaderBase{
        public SimpleLoader(): base("ui")
        {
            this.LoadFunc = LoadRes;
        }

        UnityEngine.Object LoadRes(string name)
        {
            return Resources.Load("ui/" + name);
        }
    }

    class LoaderInEditor : TmplLoaderBase{
        string root_path;
        public LoaderInEditor(): base("ui")
        {
            this.LoadFunc = LoadRes;
            root_path = "Assets/Resources/ui/";
        }

        UnityEngine.Object LoadRes(string name)
        {
            string full_path = root_path+ name+".prefab";
            ZLog.I(null,"{0}", full_path);
            return LoadResource.LoadAssetAtPath( full_path, typeof(GameObject));
        }
    }

    public class CnUIMgr : UIMgrBase
    {
        private static CnUIMgr m_Instance;
        public static CnUIMgr Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    BaseCanvas cvs = new BaseCanvas("myui");
                    m_Instance = new CnUIMgr(cvs.Main.gameObject);
                }
                return m_Instance;
            }
        }

        LoaderInEditor _resLoader;
        

        public CnUIMgr( GameObject root):base(root)
        {
           _resLoader = new LoaderInEditor();
        }


        public override GameObject FindUIRes(string name)
        {
            return _resLoader.FindTmpl(name);
        }
        ///////////////
    }


    public class CnUIPathMgr : UIMgrBase
    {
        private static CnUIPathMgr m_Instance;
        public static CnUIPathMgr Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject go = GameObject.Find("GUIRoot/Canvas");
                    m_Instance = new CnUIPathMgr(go);
                }
                return m_Instance;
            }
        }

        SimpleLoader _resLoader;

        public CnUIPathMgr(GameObject root ):base(root)
        {
            _resLoader = new SimpleLoader();
        }

        public override GameObject FindUIRes(string name)
        {
            return _resLoader.FindTmpl(name);
        }
        ///////////////
    }

}
