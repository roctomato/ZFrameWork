
local super = require("ui.panel_base")

return class {
    super = super, 

    ctor = function (self)
        super.ctor(self) -- 调用父类构造函数
    end,

    awake = function (self, args)
        -- 注册按钮点击事件
        clickEvents = {
            "static/Close",
        }

        self:regClickEvents(clickEvents)
    end,

    click_Close = function(self)
        self:unload()
        ui_mgr:LoadPanel("SDKPanel", "sdk_panel", true, {})
	end,
}