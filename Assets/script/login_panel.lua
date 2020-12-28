
local super = require("ui.panel_base")

return class {
    super = super, 
    ctor = function (self)
        print("构造")
        self.a= 0
    end,

    awake = function (self, args)
        -- body
        self:register()
    end,

    register = function (self)
        local btnwrap = self:find_button("ButtonLogin")
        btnwrap:register_click(self.onLoginBtnClicked, self)
    end,

    onLoginBtnClicked = function(self)
        print("click", self.a)
        self:unload()
        ui_mgr:LoadPanel("panel", "test_panel", true, {'has monobahavor'})
	end,
}