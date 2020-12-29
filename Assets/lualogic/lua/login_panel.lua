
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
        local button_path ={
            "ButtonLogin", -- 自动生成 button 变量 self._btnButtonLogin
        }
        self:initButtonValues(button_path)
        self._btnButtonLogin:register_click(self.onLoginBtnClicked, self)

        local input_path ={
            "ip/InputIP",
            "accountRoot/InputAccount",
            "passwordRoot/InputPassword",
        }
        self:initInputValues(input_path)

        local inner_text = {
            "ip/InputIP",
            "accountRoot",
            "passwordRoot"
        }
        self:initWidgetTextValues(inner_text)
        self._txtaccountRoot.text = "hello"
        self._txtpasswordRoot.text = "world"
    end,

    onLoginBtnClicked = function(self)
        print("click", self.a)
        self:unload()
        ui_mgr:LoadPanel("panel", "test_panel", true, {'has monobahavor'})
        print("ip", self._iptInputIP.text)
        print("account", self._iptInputAccount.text)
        print("password", self._iptInputPassword.text)
	end,
}