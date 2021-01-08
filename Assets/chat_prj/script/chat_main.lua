--c#调用lua的入口模块

require "global" -- 导入全局通用的一些类
require("game_global") --导入逻辑相关

local log = logger.get("logic", "main")

--主入口函数。从这里开始lua逻辑
function Main()
    logger.set_level(game_global.debug_level)

    log("from lua logic start.")
    game_global.ui_mgr:LoadPanel("Chat/ChatPanel", "chat_panel", true, {'has monobahavor'})
end

-- 销毁前调用
function Quit()
    log("lua quit.")
    game_global.ui_mgr:UnloadAll()
end