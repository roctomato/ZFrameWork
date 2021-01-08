local super = require "ui.widget"

return class {
    typename = "WrapToggle",
    super = super,

    ctor = function (self, native)
        super.ctor(self, native)
    end,

    register_onchange = function(self, func, param)
        self.native.onValueChanged:AddListener(function_ex.make(func, param))
    end,

}