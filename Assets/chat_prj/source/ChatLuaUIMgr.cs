
using UnityEngine;
using Zby;
using XLua;

class ChatUILoader : TmplLoaderBase
{
    string root_path;
    public ChatUILoader() : base("ui")
    {
        this.LoadFunc = LoadRes;
        root_path = "Assets/Resources/View/";
    }

    UnityEngine.Object LoadRes(string name)
    {
        string full_path = root_path + name + ".prefab";
        ZLog.I(null, "{0}", full_path);
        return LoadResource.LoadAssetAtPath(full_path, typeof(GameObject));
    }
}
[LuaCallCSharp]
public class ChatLuaUIMgr:  LuaObjMgr
    {
        private static ChatLuaUIMgr m_Instance;
        public static ChatLuaUIMgr Instance
        {
            get
            {
                return m_Instance;
            }
        }

        ChatUILoader _resLoader;

     

        public static ChatLuaUIMgr InitByFind(string name)
        {
            if (m_Instance == null)
            {
                GameObject go = GameObject.Find(name);
                m_Instance = new ChatLuaUIMgr(go);
            }
            return m_Instance;
        }

       
        private ChatLuaUIMgr( GameObject root):base(root)
        {
            _resLoader = new ChatUILoader();
        }


        public override GameObject FindUIRes(string name)
        {
            return _resLoader.FindTmpl(name);
        }
        ///////////////
    } 