
local super = require("ui.panel_base")

return class {
    super = super,

    awake = function (self, args)
        -- 注册按钮点击事件
        local clickEvents = {
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