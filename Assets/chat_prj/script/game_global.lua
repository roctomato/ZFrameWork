

local ui_mgr = CS.ChatLuaUIMgr.InitByFind('Canvas')

game_global = { --所有逻辑相关的管理类由 game_global索引
   debug_level = 'DEBUG',

   ui_mgr = ui_mgr, --ui管理类

   chat_server ="ws://127.0.0.1:8000/ws",

   name = '聊天王',
}

return game_global