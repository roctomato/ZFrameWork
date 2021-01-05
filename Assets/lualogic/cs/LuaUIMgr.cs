
using UnityEngine;
using Zby;
using XLua;

[LuaCallCSharp]
public class LuaUIMgr:  LuaObjMgr
    {
        private static LuaUIMgr m_Instance;
        public static LuaUIMgr Instance
        {
            get
            {
                return m_Instance;
            }
        }

        TmplLoaderBase _resLoader;

        public static LuaUIMgr InitByCreate(string name)
        {
            if (m_Instance == null)
            {
                BaseCanvas cvs = new BaseCanvas(name);
                m_Instance = new LuaUIMgr(cvs.Main.gameObject);
            }
            return m_Instance;
        }

        public static LuaUIMgr InitByFind(string name)
        {
            if (m_Instance == null)
            {
                GameObject go = GameObject.Find(name);
                m_Instance = new LuaUIMgr(go);
            }
            return m_Instance;
        }

        public static LuaUIMgr InitByLoad()
        {
            if (m_Instance == null)
            {
                
                m_Instance = new LuaUIMgr( null);
                m_Instance._root  = m_Instance.LoadCanvas().gameObject;
            }
            return m_Instance;
        }
        public  Canvas LoadCanvas(){
            Canvas ret = null;
            do{
                string path ="GUIRoot"; 
                GameObject go = this.FindUIRes(path);
                if (null == go)
                {
                    ZLog.E(null, "no canvas {0}", path);
                    break;
                }

                GameObject ins =  GameObject.Instantiate(go) as GameObject;
                if (ins == null)
                {
                    ZLog.E(null, "{0} Instantiate canvas fail", path);
                    break;
                }
                ins.name = path;
                var cs = ins.transform.Find("Canvas");
                ZLog.I(null, "Load{0} ok", path);
                ret = cs.GetComponent<Canvas>();
            }while(false);
            return ret;
        }
        
        private LuaUIMgr( GameObject root):base(root)
        {
            _resLoader = new TmplLoaderBase("ui");
            _resLoader.LoadFunc = LoadRes;
        }

        UnityEngine.Object LoadRes(string name)
        {
            return Resources.Load("ui/" + name);
        }

        public override GameObject FindUIRes(string name)
        {
            return _resLoader.FindTmpl(name);
        }
        ///////////////
    } 