
local super = require("ui.panel_base")

return class {
    super = super, 

    ctor = function (self)
        super.ctor(self) -- 调用父类构造函数
    end,

    awake = function (self, args)
        -- 注册按钮点击事件
        clickEvents = {
            "btnCall",
        }

        self:regClickEvents(clickEvents)

        self.txt = self:find_text("txtMethod")
        self.iptMethod = self:find_input("iptMethod")
        self.iptMethod.tip = "随便输入些啥的"
    end,

    click_btnCall = function(self)
        print( self.txt.text)
        self.txt.text = "hello"
        print("input:", self.iptMethod.text)
        --self:unload()
	end,
}