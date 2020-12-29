

return class {
    typename = "WrapWidget",

    -- native 类型是transfrom
    ctor = function (self, native)
        self.native = native
    end,

    -- 功能函数
    find_inner_text = function(self)
        if self.native then
            return self.native:GetComponentInChildren(typeof(CS.UnityEngine.UI.Text))
        end
        return nil
    end,


    find = function (self, path, type)
        local obj =self.native:Find(path)
        local cmpt
        if obj then
            cmpt = obj:GetComponent(type)
        end
        return cmpt
    end,

    find_input = function( self, path)
       return self:find(path,"InputField")
    end,

    find_text = function( self, path)
        return self:find(path,"Text")
    end,

    find_button = function (self, path)
        return  self:find(path, "Button")
    end,

    -- 通用属性
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