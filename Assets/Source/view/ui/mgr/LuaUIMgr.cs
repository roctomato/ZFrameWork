
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

        public static LuaUIMgr Create(string name, bool asPath)
        {
            if (m_Instance == null)
            {
                m_Instance = new LuaUIMgr(name, asPath);
            }
            return m_Instance;
        }

        public bool LoadPanel(string ui_res, string lua_cls, bool asSimple, LuaTable param)
        {
            bool ret = false;
            do{
                if (asSimple){
                    LoadPanelShow<LuaPanelBase>("panel", "test_panel", param);
                }else{
                    Load<LuaViewBase>("panel", "test_panel", param);
                }
                ret = true;
            }while(false);
            return ret;
        }
        private LuaUIMgr( string name, bool asPath):base(name, asPath)
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