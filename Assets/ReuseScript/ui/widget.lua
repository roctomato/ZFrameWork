
return class {
    typename = "WrapWidget", 

    -- native 类型是transfrom
    ctor = function (self, native)
        self.native = native
    end, 

    visible = {
        getter = function (self)
            return self.native.gameObject:GetActive()
        end,
        setter = function (self, value)
            self.native.gameObject:SetActive(value)
        end,
    },

    -- interactable = {
    --     getter = function (self)
    --         return self.__native.interactable
    --     end, 
    --     setter = function (self, value)
    --         self.__native.interactable = value
    --     end, 
    -- }, 
}