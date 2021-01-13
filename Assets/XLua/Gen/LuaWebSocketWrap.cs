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
    public class LuaWebSocketWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LuaWebSocket);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenUrl", _m_OpenUrl);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnOpen", _m_OnOpen);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTxtMsg", _m_OnTxtMsg);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRawMsg", _m_OnRawMsg);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnErr", _m_OnErr);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnDisconnect", _m_OnDisconnect);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "scriptEnv", _g_get_scriptEnv);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaOnOpen", _g_get_luaOnOpen);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaOnTxtMsg", _g_get_luaOnTxtMsg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaOnRawMsg", _g_get_luaOnRawMsg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaOnErr", _g_get_luaOnErr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "luaOnDisconnect", _g_get_luaOnDisconnect);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "scriptEnv", _s_set_scriptEnv);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "luaOnOpen", _s_set_luaOnOpen);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "luaOnTxtMsg", _s_set_luaOnTxtMsg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "luaOnRawMsg", _s_set_luaOnRawMsg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "luaOnErr", _s_set_luaOnErr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "luaOnDisconnect", _s_set_luaOnDisconnect);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TTABLE))
				{
					XLua.LuaTable _sp = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
					
					var gen_ret = new LuaWebSocket(_sp);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LuaWebSocket constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUrl(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OpenUrl( _url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnOpen(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _url = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OnOpen( _url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnTxtMsg(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 2);
                    int _handle_count = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.OnTxtMsg( _text, _handle_count );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRawMsg(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] _msg = LuaAPI.lua_tobytes(L, 2);
                    int _handle_count = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.OnRawMsg( _msg, _handle_count );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnErr(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    WebSocketSharp.ErrorEventArgs _e = (WebSocketSharp.ErrorEventArgs)translator.GetObject(L, 2, typeof(WebSocketSharp.ErrorEventArgs));
                    
                    gen_to_be_invoked.OnErr( _e );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnDisconnect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _reason = LuaAPI.xlua_tointeger(L, 2);
                    string _str = LuaAPI.lua_tostring(L, 3);
                    
                    gen_to_be_invoked.OnDisconnect( _reason, _str );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scriptEnv(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.scriptEnv);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaOnOpen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaOnOpen);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaOnTxtMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaOnTxtMsg);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaOnRawMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaOnRawMsg);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaOnErr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaOnErr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_luaOnDisconnect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.luaOnDisconnect);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scriptEnv(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.scriptEnv = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_luaOnOpen(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.luaOnOpen = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_luaOnTxtMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.luaOnTxtMsg = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_luaOnRawMsg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.luaOnRawMsg = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_luaOnErr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.luaOnErr = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_luaOnDisconnect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                LuaWebSocket gen_to_be_invoked = (LuaWebSocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.luaOnDisconnect = (XLua.LuaFunction)translator.GetObject(L, 2, typeof(XLua.LuaFunction));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
