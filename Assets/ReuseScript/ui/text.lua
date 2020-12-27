local super = require "ui.widget"

return class {
    typename = "WrapText", 
    super = super,

    ctor = function (self, native)
        super.ctor(self, native)
    end, 

    -- 含 text 的组件 先调用 init_text 之后可以用属性 text 赋值或取值

    text = {
        getter = function (self)
            return self.native.text
        end,

        setter = function (self, text)
            self.native.text = text
        end,
    },
   
}
