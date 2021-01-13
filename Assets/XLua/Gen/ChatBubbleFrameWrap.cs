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
    public class ChatBubbleFrameWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ChatBubbleFrame);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 5, 5);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Show", _m_Show);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHeight", _m_GetHeight);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "_imageChatBubble", _g_get__imageChatBubble);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_frameChatBubble", _g_get__frameChatBubble);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_imageEmotionBubble", _g_get__imageEmotionBubble);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_textChat", _g_get__textChat);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "_imageHead", _g_get__imageHead);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "_imageChatBubble", _s_set__imageChatBubble);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_frameChatBubble", _s_set__frameChatBubble);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_imageEmotionBubble", _s_set__imageEmotionBubble);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_textChat", _s_set__textChat);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "_imageHead", _s_set__imageHead);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IMAGE_EMOTION_HEIGHT", ChatBubbleFrame.IMAGE_EMOTION_HEIGHT);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FRAME_BUBBLE_HEIGHT_BASE", ChatBubbleFrame.FRAME_BUBBLE_HEIGHT_BASE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DEFAULT_SCREEN_WIDTH", ChatBubbleFrame.DEFAULT_SCREEN_WIDTH);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new ChatBubbleFrame();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ChatBubbleFrame constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Show(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _text = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.Show( _text );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHeight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetHeight(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__imageChatBubble(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._imageChatBubble);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__frameChatBubble(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._frameChatBubble);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__imageEmotionBubble(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._imageEmotionBubble);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__textChat(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._textChat);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get__imageHead(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked._imageHead);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__imageChatBubble(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._imageChatBubble = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__frameChatBubble(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._frameChatBubble = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__imageEmotionBubble(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._imageEmotionBubble = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__textChat(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._textChat = (UnityEngine.UI.Text)translator.GetObject(L, 2, typeof(UnityEngine.UI.Text));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set__imageHead(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ChatBubbleFrame gen_to_be_invoked = (ChatBubbleFrame)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked._imageHead = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
