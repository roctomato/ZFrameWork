local widget = require("ui.widget")
local panel_base = require("ui.panel_base")
local container = require("ui.container")

local log = logger.get("ui", "scroll_panel")

local cell_cmpt = class {
    super = container,
    ctor = function (self, native)
        container.ctor(self, native)
    end,
    set_value = function(self, index, data)
        local path_value={
            {"Tip", data},
            {"Click/Text", "选择"..index},
        }
        self:setWidgetTextValues(path_value)
        self.data = data

        self:reg_btn_click("Click", self)
    end,

    click_Click = function (self)
        log.trace("data", self.data)
    end
}
local test_scrollview = class {
    super = widget,
    ctor = function (self, native)
        widget.ctor(self, native)
        self._scrollView = native:GetComponent(typeof(CS.ScrollView))
       --log.trace("here")
    end,

    set_value = function(self, data)
        log.trace("here")
        self.data = data
        self._scrollView:SetCellUpdate(function(cell, index) self:onSelfCellUpdate(cell, index) end)
        self._scrollView:Init(#data)
        log.trace("here")
    end,

    onSelfCellUpdate = function(self, cell, index)
        --log.info(cell,index)
        local cell_obj = cell_cmpt(cell)
        cell_obj:set_value(index, self.data[index+1])
    end,
}

return class {
    super = panel_base,

    ctor = function (self)
        panel_base.ctor(self) -- 调用父类构造函数
    end,

    awake = function (self, args)
        self.scrollview = test_scrollview(self:find_gameobject("ScrollView").transform)
        self.scrollview:set_value({1,2,3,4,5})
        self:reg_btn_click("Close", self)
    end,

    click_Close = function (self)
        self:unload()
    end
}