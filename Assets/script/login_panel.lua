
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
        local btn =self.mono.UIObj.transform:Find("ButtonLogin")
        local cmpt = btn:GetComponent("Button")
        cmpt.onClick:AddListener(function_ex.make(self.onLoginBtnClicked, self))
    end,

    onLoginBtnClicked = function(self)
        print("click", self.a)
        self.mono:DoUnload()
        ui_mgr:LoadPanel("panel", "test_panel", false, {'has monobahavor'})
	end,
}