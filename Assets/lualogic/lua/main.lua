
class = require('base.class')
function_ex = require('base.functor')
logger = require('base.logger')
enum = require("base.enum")
utility = require("base.utility")

widget = require("ui.widget")

local log = logger.get("logic", "pos1")

logger.set_level("DEBUG")
ui_mgr = CS.LuaUIMgr.InitByLoad()
game_mgr = CS.LuaGameMgr.InitByCreate('root')
game_mgr:InitPath('Assets/')

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

function test_name_class()
    -- body
     
end

--主入口函数。从这里开始lua逻辑
function Main()
    log("from lua logic start.")

    --local ui_mgr = CS.LuaUIMgr.InitByLoad("myui")
    --ui_mgr:LoadPanel("SDKPanel", "sdk_panel", true, {})
    --ui_mgr:LoadPanel("LoginGame", "login_panel", true, {})
    --ui_mgr:LoadPanel("ScrollPanel", "scroll_panel", true, {'has monobahavor'})
    --ui_mgr:LoadSimplePanel("FightReportPanel", "report_panel",  {'no monobahavor'})
    --ui_mgr:LoadMonoPanel("FightReportFrame", "report_panel",  {'has monobahavor'})
    --panel.visible = true

    --ui_mgr:LoadPanel("ReportSetFrame", "ui.panel_base", true, {'has monobahavor'})
    --test_class()
    --local svg_file = 'Art/svg/19971.svg'
    --game_mgr:LoadSimplePanel(svg_file, "svg_sprite",{})

    --ui_mgr:LoadSimplePanel("SimplePanel", "svg_img",  {'no monobahavor'})
    --ui_mgr:LoadSimplePanel("SvgImg", "svg_ui",  {'no monobahavor'})
    ui_mgr:LoadSimplePanel("svg_btn", "svg_btn",  {'no monobahavor'})
end

-- 销毁前调用
function Quit()
    log("lua quit.")
    ui_mgr:UnloadAll()
end