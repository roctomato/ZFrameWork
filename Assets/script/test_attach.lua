class = require('base.class')
function_ex = require('base.functor')
logger = require('base.logger')
enum = require("base.enum")
utility = require("base.utility")


local log = logger.get("logic", "pos1")

logger.set_level("DEBUG")
ui_mgr = CS.LuaUIMgr.InitByLoad()

--主入口函数。从这里开始lua逻辑
function Main()
    log("from lua logic start.")
    local go =  CS.UnityEngine.GameObject.Find("/GUIRoot/Canvas/LoginGame")
    CS.LuaNormalBehaiour.Attach(go,"login_panel")
end

-- 销毁前调用
function Quit()
    log("lua quit.")
    ui_mgr:UnloadAll()
end