
class = require('base.class')
function_ex = require('base.functor')
--

local enum = require("base.enum")
local logger = require('base.logger')

--logger.mod("模块名") -- 定义新模块
--logger.mod("模块名").cat("新类别") -- 定义新类别


local log = logger.get("logic", "pos1")

logger.set_level("DEBUG")
 ui_mgr = CS.LuaUIMgr.InitByLoad()

function test_class()
    -- body
    local panel = require('test_panel')
    local pl = panel:new()
    pl:update()

    print(pl.a)

    local p2 = panel()
    print(p2.a)
end

function test_log()
    -- body
    print( logger.get_level())
    logger.get("logic", "pos1").debug("D from lua logic start.")
    logger.get("logic", "pos2").info("I from lua logic start.")
    logger.get("logic", "pos3").warn("W from lua logic start.")
    logger.get("logic", "pos4").error("E from lua logic start.")
end


--主入口函数。从这里开始lua逻辑
function Main()
    log("from lua logic start.")

    --local ui_mgr = CS.LuaUIMgr.InitByLoad("myui")

    --ui_mgr:LoadPanel("LoginGame", "login_panel", true, {})
    ui_mgr:LoadPanel("panel", "test_panel", false, {'has monobahavor'})
    --ui_mgr:LoadPanel("CardFightFrame", "ui.panel_base", false, {'has monobahavor'})
    --test_class()
end

-- 销毁前调用
function Quit()
    log("lua quit.")
    ui_mgr:UnloadAll()
end