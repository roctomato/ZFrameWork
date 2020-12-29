local super = require "ui.widget"

return class {
    typename = "WrapText",
    super = super,

    ctor = function (self, native)
        super.ctor(self, native)
    end,

    text = {
        getter = function (self)
            return self.native.text
        end,

        setter = function (self, text)
            self.native.text = text
        end,
    },

}
