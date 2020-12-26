
local super = require("ui.panel_base")

return class {
    super = super, 

    ctor = function (self)
        super.ctor(self) -- 调用父类构造函数
    end,

    awake = function (self, args)
        local btnwrap = self:find_button("static/Close")
        btnwrap:register_click(self.onclose, self)
    end,

    onclose = function(self)
        self:unload()
	end,
}