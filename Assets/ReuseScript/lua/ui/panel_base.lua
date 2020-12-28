

local button = require("ui.button")
local text   = require("ui.text")
local input_field = require("ui.InputField")

local log = logger.get("ui", "ui_panel")

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
    
    find_input = function( self, path)
        local input 
        local obj = self:find(path,"InputField")
        if obj then
            input = input_field(obj)
        end
        return input
    end,

    find_text = function( self, path)
        local txt 
        local obj = self:find(path,"Text")
        if obj then
            txt = text(obj)
        end
        return txt
    end,

    find_button = function (self, path)
        local btn
        local obj = self:find(path, "Button")
        if obj then
            btn = button(obj)
        end
        return btn
    end,

    --按照  "click_"+button名称 注册回调
    reg_btn_click = function( self, btn_path)
        local btnwrap = self:find_button(btn_path)
        if not btnwrap then
            logger.error("register event failed: not such button", btn_path)
            return false
        end

        local btn_name = btn_path
        local rst = string.find(btn_path, "/")
        if rst then
            btn_name = utility.lastStringOf( btn_path, "/")
        end

        local cb = self["click_" .. btn_name]
        if not cb then
            logger.error("register event failed: not such function", cb)
            return false
        end

        btnwrap:register_click( cb, self)
        return true
    end,

    regClickEvents = function(self,  clickEvents)
        if type(clickEvents) == "table" then
            for _, path in pairs(clickEvents) do
                self:reg_btn_click(path)
            end
        else
            logger.error("register event failed: clickEvents type err", type(clickEvents))
        end
    end,

}