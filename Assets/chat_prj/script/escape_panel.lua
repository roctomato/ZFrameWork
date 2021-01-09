local super = require("ui.panel_base")
local log = logger.get('panel', 'input_panel')

return  class {
    super = super,

    update = function (self)
        if CS.UnityEngine.Input.GetKeyDown(CS.UnityEngine.KeyCode.Escape) then
             self:unload()
        end
    end,
}