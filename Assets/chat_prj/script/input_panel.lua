local super = require("ui.panel_base")
local log = logger.get('panel', 'input_panel')

return  class {
    super = super,

    awake = function (self, args)
        -- body
        local path_values={
            {'DialogFrame/Title', '测试面板'},
            {'DialogFrame/ConfirmButton/Text', '打开'},
            {'DialogFrame/CancelButton/Text', '退出'},
            {'DialogFrame/InputGrid/LabelInputField/LabelHint', '路径'},
            {'DialogFrame/InputGrid/LabelInputField/Placeholder', ''},
        }
        self:setWidgetTextValues(path_values)

        local button_path={
            'DialogFrame/ConfirmButton',
            'DialogFrame/CancelButton',
        }
        self:regClickEvents(button_path)

        self.input_path = self:find_input('DialogFrame/InputGrid/LabelInputField')
    end,

    click_ConfirmButton = function (self)
        log( self.input_path.text)
        game_global.ui_mgr:LoadMonoPanel(self.input_path.text, "escape_panel",{'param'})
        --game_global.ui_mgr:LoadSimplePanel(self.input_path.text, "escape_panel",{'param'})
    end,

    click_CancelButton = function (self)
        self:unload()
    end,

}