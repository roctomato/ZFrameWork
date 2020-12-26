local button   = require("ui.button")

return class {

    ctor = function (self)
        print("in base panel ctor")
    end,

    -- 是否带MonoBehaviour都会调用
    awake = function (self, args)
        print("in base awake")
        if args then
            for k,v in ipairs(args) do
                print(k,v)
            end
        end
    end,

    -- 是否带MonoBehaviour都会调用
    ondestroy = function (self)
        print("in base ondestroy")
    end,

    -- 仅带MonoBehaviour会调用
    start = function (self)
        print("in base start")
    end,

    -- 仅带MonoBehaviour会调用
    update = function (self)
        -- body
    end,

    -- 功能函数
    find = function (self, path, type)
        local obj =self.mono.UIObj.transform:Find(path)
        local cmpt
        if obj then
            cmpt = obj:GetComponent(type)
        end
        return cmpt
    end,

    unload = function(self)
        self.mono:DoUnload()
    end,
    
    find_button = function (self, path)
        local btn
        local obj = self:find(path, "Button")
        if obj then
            btn = button(obj)
        end
        return btn
    end,
}