
local super = require("ui.panel_base")
local log = logger.get("panel",'webscoket')
return class {
    super = super,

    awake = function (self, args)
        self:register()
        self.ws= CS.LuaWebSocket(self)
        print(self.ws)
    end,

    register = function (self)
        local path_values={
            {'Btn1/Text', 'connect'},
            {'Btn2/Text', 'disconnect'},
            {'Btn3/Text', 'Ping'},
        }
        self:setWidgetTextValues(path_values)

        local button_path={
            'Btn1',
            'Btn2',
            'Btn3',
        }
        self:regClickEvents(button_path)
    end,

    click_Btn1 = function(self)
        log('click btn1')
        self.ws:OpenUrl("ws://127.0.0.1:8000/ws")
    end,

    click_Btn2 = function(self)
        self.ws:Close()
        log('click btn2')
    end,

    click_Btn3 = function(self)
        log('click btn3')
        self.ws:SendText("hello, world")
    end,

    OnOpen= function(self, url)
        log('on open', url)
    end,

    OnErr= function(self, err_msg)
        log( 'on err', err_msg)
    end,

    OnDisconnect= function(self, reson, str)
        log('ondisconnect',reson, str)
    end,

    OnTxtMsg = function(self, msg, count)
        log( 'on text msg', msg, count)
    end,
}