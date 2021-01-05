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
    public class LuaNormalBehaiourWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LuaNormalBehaiour);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLoad", _m_OnLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCreate", _m_OnCreate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DoUnload", _m_DoUnload);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "UIObj", _g_get_UIObj);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaClass", _g_get_LuaClass);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Attach", _m_Attach_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AttachIns", _m_AttachIns_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateIns", _m_CreateIns_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new LuaNormalBehaiour();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LuaNormalBehaiour constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Attach_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _stript_name = LuaAPI.lua_tostring(L, 2);
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        var gen_ret = LuaNormalBehaiour.Attach( _go, _stript_name, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AttachIns_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    XLua.LuaTable _luains = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        var gen_ret = LuaNormalBehaiour.AttachIns( _go, _luains, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateIns_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    XLua.LuaTable _lua_class = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        var gen_ret = LuaNormalBehaiour.CreateIns( _go, _lua_class, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaNormalBehaiour gen_to_be_invoked = (LuaNormalBehaiour)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    string _stript_name = LuaAPI.lua_tostring(L, 2);
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.OnLoad( _stript_name, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    XLua.LuaTable _luains = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.OnLoad( _luains, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LuaNormalBehaiour.OnLoad!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnCreate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaNormalBehaiour gen_to_be_invoked = (LuaNormalBehaiour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    XLua.LuaTable _luains = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    XLua.LuaTable _param = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        var gen_ret = gen_to_be_invoked.OnCreate( _luains, _param );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoUnload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaNormalBehaiour gen_to_be_invoked = (LuaNormalBehaiour)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.DoUnload(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UIObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaNormalBehaiour gen_to_be_invoked = (LuaNormalBehaiour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.UIObj);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaClass(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaNormalBehaiour gen_to_be_invoked = (LuaNormalBehaiour)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaClass);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
