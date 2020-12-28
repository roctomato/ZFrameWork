local super = require "ui.widget"

return class {
    typename = "InputField", 
    super = super,
    
    ctor = function (self, native)
        super.ctor(self, native)
        self._text = self:find_inner_text() 
    end, 

    text = {
         getter = function (self)
             return self.native.text
         end, 
         setter = function (self, value)
             self.native.text = value
         end, 
    },

    tip = {
        getter = function (self)
            return self._text.text
        end, 
        setter = function (self, value)
            self._text.text = value
        end, 
   }
}