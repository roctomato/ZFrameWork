#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class LuaUIMgrWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LuaUIMgr);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadCanvas", _m_LoadCanvas);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadSimplePanel", _m_LoadSimplePanel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AttachSimplePanel", _m_AttachSimplePanel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AttachPanel", _m_AttachPanel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadMonoPanel", _m_LoadMonoPanel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadPanel", _m_LoadPanel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FindUIRes", _m_FindUIRes);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 1, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "InitByCreate", _m_InitByCreate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitByFind", _m_InitByFind_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "InitByLoad", _m_InitByLoad_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "LuaUIMgr does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitByCreate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaUIMgr.InitByCreate( _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitByFind_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LuaUIMgr.InitByFind( _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitByLoad_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = LuaUIMgr.InitByLoad(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadCanvas(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaUIMgr gen_to_be_invoked = (LuaUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.LoadCanvas(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSimplePanel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaUIMgr gen_to_be_invoked = (LuaUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _ui_res = LuaAPI.lua_tostring(L, 2);
                    string _lua_cls = LuaAPI.lua_tostring(L, 3);
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.LoadSimplePanel( _ui_res, _lua_cls, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AttachSimplePanel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaUIMgr gen_to_be_invoked = (LuaUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _ui_res = LuaAPI.lua_tostring(L, 2);
                    string _lua_cls = LuaAPI.lua_tostring(L, 3);
                    bool _show = LuaAPI.lua_toboolean(L, 4);
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.AttachSimplePanel( _ui_res, _lua_cls, _show, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AttachPanel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaUIMgr gen_to_be_invoked = (LuaUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _ui_res = LuaAPI.lua_tostring(L, 2);
                    string _lua_cls = LuaAPI.lua_tostring(L, 3);
                    bool _show = LuaAPI.lua_toboolean(L, 4);
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.AttachPanel( _ui_res, _lua_cls, _show, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadMonoPanel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaUIMgr gen_to_be_invoked = (LuaUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _ui_res = LuaAPI.lua_tostring(L, 2);
                    string _lua_cls = LuaAPI.lua_tostring(L, 3);
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.LoadMonoPanel( _ui_res, _lua_cls, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadPanel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaUIMgr gen_to_be_invoked = (LuaUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _ui_res = LuaAPI.lua_tostring(L, 2);
                    string _lua_cls = LuaAPI.lua_tostring(L, 3);
                    bool _asSimple = LuaAPI.lua_toboolean(L, 4);
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.LoadPanel( _ui_res, _lua_cls, _asSimple, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindUIRes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaUIMgr gen_to_be_invoked = (LuaUIMgr)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.FindUIRes( _name );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, LuaUIMgr.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
