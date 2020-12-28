local super = require "ui.widget"

return class {
    typename = "WrapButton", 
    super = super,

    ctor = function (self, native)
        super.ctor(self, native)
    end, 

    register_click = function(self, func, param)
        self.native.onClick:AddListener(function_ex.make(func, param))
    end,

}