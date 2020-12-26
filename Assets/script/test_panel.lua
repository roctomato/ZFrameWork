
local super = require("ui.panel_base")
local button   = require("ui.button")

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
        local btnwrap = button(self:find_button("Btn1"))
        btnwrap.text = "关闭"
        btnwrap:register_click(self.onLoginBtnClicked, self)

        local btn2 = button(self:find_button("Btn2"))
        btn2.text = "隐藏按钮"
        btn2:register_click(function (btn2)
            btn2.visible = false
        end
        , btn2)

        local btn3 = button(self:find_button("Btn3"))
        btn3.text = "显示按钮"
        btn3:register_click(function (btn2)
            btn2.visible = true
        end
        , btn2)
    end,

    update = function (self)
        -- body
        self.a = self.a + 1
    end,

    onLoginBtnClicked = function(self)
        print("click", self.a)
        self.mono:DoUnload()
	end,
}