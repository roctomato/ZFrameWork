
local super = require("ui.panel_base")

return class {
    super = super, 


    awake = function (self, args)
        -- body
        print("awake", self.a) --
        self:register()
    end,

    start = function (self)
        print("start", self.a)
    end,

    register = function (self)
        -- body
        local btns ={
            "Btn1",
            "Btn2",
            "Btn3",
            "Btn4"
        }

        self:initWidgetTextValues(btns)
        self:initButtonValues(btns)
        self:regClickEvents(btns)

        self._txtBtn1.text = "关闭"
        self._txtBtn2.text = "隐藏按钮"
        self._txtBtn3.text = "显示按钮"
        self._txtBtn4.text = "战报面板"

    end,

    update = function (self)
        self.a = self.a + 1
    end,

    click_Btn1 = function(self)
        self:unload()
        ui_mgr:LoadPanel("LoginGame", "login_panel", true, {})
    end,

    click_Btn2 = function(self)
        print("click btn2")
        self._btnBtn2.visible = false
    end,

    click_Btn3 = function(self)
        self._btnBtn2.visible = true
    end,

    click_Btn4 = function(self)
        print("click btn3")
        self:unload()
        ui_mgr:LoadPanel("CardFightFrame", "report_panel", false, {'has monobahavor'})
    end

}