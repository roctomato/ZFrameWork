class = require('base.class')
function_ex = require('base.functor')
--enum = require("base.enum")
--utility = require("base.utility")

logger = require('base.logger')

local log = logger.get("logic", "pos1")
logger.set_level("DEBUG")

ui_mgr = CS.LuaUIMgr.InitByCreate('myui')

--主入口函数。从这里开始lua逻辑
function Main()
    log("from lua logic start.")

    ui_mgr:LoadSimplePanel('Panel', "test_websocket_panel" )

    --ui_mgr:LoadSimplePanel('InputPanel', "test_input" )
    --ui_mgr:LoadSimplePanel('InputPanel', "ui.panel_base" )
end

-- 销毁前调用
function Quit()
    log("lua quit.")
    ui_mgr:UnloadAll()
end
