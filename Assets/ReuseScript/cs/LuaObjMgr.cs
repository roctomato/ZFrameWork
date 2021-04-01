
using UnityEngine;
using UnityEngine.Events;
using Zby;
using XLua;

[LuaCallCSharp]
public class LuaObjMgr:  UIMgrBase
{
    public LuaObjMgr(GameObject go):base(go) {
        
    }
    public LuaTable LoadSimplePanel(string ui_res, string lua_cls, LuaTable param)
    {
        LuaPanelBase ins = LoadPanelShow<LuaPanelBase>(ui_res, lua_cls, param);
        return ins.LuaClass;
    }
        
    public LuaTable AttachSimplePanel(string ui_res, string lua_cls, bool show, LuaTable param)
    {
        LuaPanelBase ins = AttachPanel<LuaPanelBase>(ui_res,show, lua_cls, param);
        return ins.LuaClass;
    }

    public LuaTable AttachPanel(string ui_res, string lua_cls, bool show, LuaTable param)
    {
        LuaViewBase ins = AttachPanelEx<LuaViewBase>(ui_res,show, lua_cls, param);
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

    public  Sprite ResourcesSprite(string path)
    {
        return Resources.Load<Sprite>(path);
    }

   
}
[LuaCallCSharp] 
public class LuaUIEvt
{
    static public  void RegistClick(GameObject go, UnityAction action)
    {
        UiEventListenerEx.Get(go).onClick = action;
    }

    static public void RegistDown(GameObject go, UnityAction action)
    {
        UiEventListenerEx.Get(go).onDown = action;
    }
    static public void RegistUp(GameObject go, UnityAction action)
    {
        UiEventListenerEx.Get(go).onUp = action;
    }
}