local super = require("ui.panel_base")
local log = logger.get('panel', 'input_panel')

return  class {
    super = super,

    awake = function (self, args)
        local button_path={
            'OK',
            'Canel',
        }
        self:regClickEvents(button_path)

        self.input_path = self:find_input('InputField')
    end,

    click_OK = function (self)
        log( self.input_path.text)
        ui_mgr:LoadMonoPanel(self.input_path.text, "ui.escape_panel",{'param'})
    end,

    click_Canel = function (self)
        self:unload()
    end,

}