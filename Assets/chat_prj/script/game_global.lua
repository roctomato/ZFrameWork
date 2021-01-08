

local ui_mgr = CS.ChatLuaUIMgr.InitByFind('Canvas')

game_global = { --所有逻辑相关的管理类由 game_global索引
   debug_level = 'DEBUG',

   ui_mgr = ui_mgr, --ui管理类
}

return game_global