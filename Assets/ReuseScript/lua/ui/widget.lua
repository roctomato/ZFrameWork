

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

    GetComponent = function(self, type)
        return self.native:GetComponent(type)
    end,

    AddComponent = function(self, type)
        return self.native.gameObject:AddComponent(type)
    end,

    --类实例挂到monobehaviour中去， 框架会调用类中的 awake start update ondestory 函数
    attach = function (self)
        CS.LuaNormalBehaiour.AttachIns(self.native.gameObject,self, self)
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