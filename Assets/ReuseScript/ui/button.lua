local super = require "ui.widget"

return class {
    typename = "WrapButton", 
    super = super,

    ctor = function (self, native)
        super.ctor(self, native)

        if native then
            self._text = self.native:GetComponentInChildren(typeof(CS.UnityEngine.UI.Text))
        end
    end, 

    register_click = function(self, func, param)
        -- body
        self.native.onClick:AddListener(function_ex.make(func, param))
    end,

    text = {
        getter = function (self)
            return self._text.text
        end,

        setter = function (self, text)
            self._text.text = text
        end,
    },
}