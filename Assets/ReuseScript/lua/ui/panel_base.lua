
local container = require("ui.container")

local log = logger.get("ui", "ui_panel")

return class {
    super = container,

    ctor = function (self)
        log.trace("in base panel ctor")
    end,

    oncreate =  function (self)
        self.native=  self.mono.UIObj.transform
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
    unload = function(self)
        self.mono:DoUnload()
    end,


    regClickEvents = function(self,  clickEvents)
        self:regClickEventsToHost(clickEvents, self)
    end,

}