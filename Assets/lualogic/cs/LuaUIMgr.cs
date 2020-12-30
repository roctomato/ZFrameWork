
using UnityEngine;
using Zby;
using XLua;

[LuaCallCSharp]
public class LuaUIMgr:  UIMgrBase
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
                m_Instance = new LuaUIMgr(name, InitUIRootType.ByCreate);
            }
            return m_Instance;
        }

        public static LuaUIMgr InitByFind(string name)
        {
            if (m_Instance == null)
            {
                m_Instance = new LuaUIMgr(name, InitUIRootType.ByFind);
            }
            return m_Instance;
        }

        public static LuaUIMgr InitByLoad()
        {
            if (m_Instance == null)
            {
                string name="";
                m_Instance = new LuaUIMgr(name, InitUIRootType.LateInit);
                m_Instance.LoadCanvas(m_Instance.LoadCanvas);
            }
            return m_Instance;
        }
         public Canvas LoadCanvas(){
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
        public LuaTable LoadSimplePanel(string ui_res, string lua_cls, LuaTable param)
        {
            LuaPanelBase ins = LoadPanelShow<LuaPanelBase>(ui_res, lua_cls, param);
            return ins.LuaClass;
        }
        public LuaTable LoadMonoPanel(string ui_res, string lua_cls, LuaTable param)
        {
            LuaViewBase ins = Load<LuaViewBase>(ui_res, lua_cls, param);
            return ins.LuaClass;
        }
        public LuaTable LoadPanel(string ui_res, string lua_cls, bool asSimple, LuaTable param)
        {
            LuaTable ret = null;
           
            if (asSimple){
                ret = LoadSimplePanel(ui_res, lua_cls, param);
            }else{
                ret = LoadMonoPanel(ui_res, lua_cls, param);
            }
               
            return ret;
        }
        private LuaUIMgr( string name, InitUIRootType asPath):base(name, asPath)
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