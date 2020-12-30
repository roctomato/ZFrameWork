
local widget = require("ui.widget")
local panel_base = require("ui.panel_base")
local container = require("ui.container")
local fight_report = require('fight_report')
local json = require('base.json')

local log = logger.get("ui", "report_panel")

local player_info = class{
    super = container,

    ctor = function (self, native)
        container.ctor(self, native)
    end,

    set_value= function (self, data) --一次性赋值
        local path_txts ={
            {"PlayerNameText",  data.name},
            {"PlayerLevelText", data.level},
            {"PlayerHPText",    data.blood},
            {"PlayerSkinText",  data.skin},
            {"PlayerLeftHPText",data.leftblood},
        }
        self:setWidgetTextValues(path_txts)
    end
}

local day_info = class{
    super = container,

    ctor = function (self, native)
        container.ctor(self, native)
        self:init_ctrl()
    end,

    init_ctrl = function (self) --记住text变量，
        local txts ={
            "CalendarText",
            "FiveText",
            "FiveAdditionText",
        }

        self:initTextValues(txts)
    end,

    set_value= function (self, data) --可以使用变量多次赋值
        self._txtCalendarText.text = data.index
        self._txtFiveText.text     = data.five
        self._txtFiveAdditionText.text = data.percent
    end
}


local card_card = class {
    super = container,
    ctor = function (self, native)
        --log.trace("card_card",native)
        container.ctor(self, native)
    end,
    set_value = function(self, index, data, win)
        local path_value={
            {"NoText", index},
            {"CodeText", data.index},
            {"LevelText", data.level},
        }
        self:setWidgetTextValues(path_value)
        --log.trace("value", index, data.index, win)
        --self.visible =false
    end
}

local card_scrollview = class {
    super = widget,
    ctor = function (self, native, fd ,other_fd)
        widget.ctor(self, native)
        self._scrollView = native:GetComponent(typeof(CS.ScrollView))
        self.fd = fd
        self.other_fd = other_fd
    end,

    set_value = function(self, data)
        self.data = data
        self._scrollView:SetCellUpdate(function(cell, index) self:onCardSelfCellUpdate(cell, index) end)
        self._scrollView:Init(#data)
    end,

    onCardSelfCellUpdate = function(self, cell, index)
        print(self)
        local idx = index+1
        local cur_data = self.data[idx][self.fd]
        local other_data = self.data[idx][self.other_fd]
        local win = cur_data.blood > other_data.blood
        local card_cell = card_card.new(cell)
        card_cell:set_value( idx, cur_data, win)
        --cell:SetIniText("NoText", index){}
        log.trace("end")
    end,
}

return class {
    super = panel_base,

    ctor = function (self)
        panel_base.ctor(self) -- 调用父类构造函数
    end,

    awake = function (self, args)
        self:init_ctrl()
        self:set_value()
    end,

    set_value = function (self)
        -- body
        local fight_data = json.decode(fight_report.report)

        self.day:set_value(fight_data.info.day)
        self.jia:set_value(fight_data.info.jia)
        self.yi:set_value(fight_data.info.yi)
        self.jia_scrollview :set_value(fight_data.fight_report)
        self.yi_scrollview :set_value(fight_data.fight_report)
    end,

    init_ctrl = function(self)
        -- 注册按钮点击事件
        local clickEvents = {
            "static/Close",
        }
        self:regClickEvents(clickEvents)

        self.day = day_info(self:find_gameobject("dynamic").transform)
        self.jia = player_info(self:find_gameobject("dynamic/players/self").transform)
        self.yi = player_info(self:find_gameobject("dynamic/players/rival").transform)
        self.jia_scrollview = card_scrollview(self:find_gameobject("dynamic/embattles/self").transform,'jia','yi')
        self.yi_scrollview = card_scrollview(self:find_gameobject("dynamic/embattles/rival").transform,'yi','jia')
    end,

    click_Close = function(self)
        self:unload()
        --ui_mgr:LoadPanel("SDKPanel", "sdk_panel", true, {})
	end,
}