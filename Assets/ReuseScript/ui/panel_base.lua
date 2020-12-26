

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
    find_button = function (self, path)
        -- body
        local btn =self.mono.UIObj.transform:Find(path)
        local cmpt = btn:GetComponent("Button")
        return cmpt
    end,
}