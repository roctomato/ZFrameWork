
local super = require("ui.panel_base")

return class {
    super = super, 

    ctor = function (self)
        super.ctor(self) -- 调用父类构造函数
        print("构造")
        
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
        btnwrap:find_inner_text().text = "关闭"
        btnwrap:register_click(self.click_Btn1, self)

        local btn2 = self:find_button("Btn2")
        btn2:find_inner_text().text = "隐藏按钮"
        btn2:register_click(function (btn2)
            btn2.visible = false
        end
        , btn2)

        local btn3 = self:find_button("Btn3")
        btn3:find_inner_text().text = "显示按钮"
        btn3:register_click(function (btn2)
            btn2.visible = true
        end
        , btn2)

        local btn4 = self:find_button("Btn4")
        btn4:find_inner_text().text = "战报面板"
        btn4:register_click(self.click_Btn4, self)
       
    end,

    update = function (self)
        self.a = self.a + 1
    end,

    click_Btn1 = function(self)
        self:unload()
        ui_mgr:LoadPanel("LoginGame", "login_panel", true, {})
    end,

    click_Btn4 = function(self) 
        self:unload()
        ui_mgr:LoadPanel("CardFightFrame", "report_panel", false, {'has monobahavor'})
    end

   
}