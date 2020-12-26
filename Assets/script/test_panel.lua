
local super = require("ui.panel_base")

return class {
    super = super, 

    ctor = function (self)
        super.ctor(self) -- 调用父类构造函数
        print("构造")
        self.a= 0
    end,

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
        local btnwrap = self:find_button("Btn1")
        btnwrap.text = "关闭"
        btnwrap:register_click(self.onLoginBtnClicked, self)

        local btn2 = self:find_button("Btn2")
        btn2.text = "隐藏按钮"
        btn2:register_click(function (btn2)
            btn2.visible = false
        end
        , btn2)

        local btn3 = self:find_button("Btn3")
        btn3.text = "显示按钮"
        btn3:register_click(function (btn2)
            btn2.visible = true
        end
        , btn2)

        local btn4 = self:find_button("Btn4")
        btn4.text = "战报面板"
        btn4:register_click(self.onOpenReport, self)
       
    end,

    update = function (self)
        -- body
        self.a = self.a + 1
    end,

    onOpenReport = function(self)
        print("click", self.a)
        ui_mgr:LoadPanel("CardFightFrame", "report_panel", false, {'has monobahavor'})
    end,
    
    onLoginBtnClicked = function(self)
        print("click", self.a)
        self:unload()
        ui_mgr:LoadPanel("LoginGame", "login_panel", true, {})
	end,
}