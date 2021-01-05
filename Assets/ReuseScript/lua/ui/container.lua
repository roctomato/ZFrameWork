local button = require("ui.button")
local text   = require("ui.text")
local input_field = require("ui.inputfield")
local widget = require("ui.widget")

local log = logger.get("ui", "container")

return class {
    typename = "Container", --容器
    super = widget,

    ctor = function (self, native)
        widget.ctor(self, native)
    end,

    -- 功能函数

    --将类class_type实例化到路径path上去
    init_child = function(self, path, class_type,...)
        return class_type(self:find_transform(path),...)
    end,

    --将类class_name实例化到路径path上去，class_name最好为类似panel_base的类有 awake start update ondestory 函数
    create_behaviour = function(self, path, class_name, ...)
        local go =  self:find_gameobject(path)
        local arg = {...}
        --print(go, arg)
        local ins = CS.LuaNormalBehaiour.CreateIns(go,class_name,arg)
        return ins
    end,

    find_gameobject = function(self,path)
        return self.native:Find(path).gameObject
    end,

    find_transform = function(self,path)
        return self.native:Find(path)
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

    --按照  "_txt"+text_path 生成引用变量
    find_button = function (self, path)
        local btn
        local obj = self:find(path, "Button")
        if obj then
            btn = button(obj)
        end
        return btn
    end,

    init_inner_text = function( self, widget_path)
        local obj =self.native.transform:Find(widget_path)
        if not obj then
            log.error(" not such widget", widget_path)
            return false
        end
        local wd = widget(obj)
        local txt = wd:find_inner_text()
        if not txt then
            log.error(" not find child text", widget_path)
            return false
        end

        local btn_name = widget_path
        local rst = string.find(widget_path, "/")
        if rst then
            btn_name = utility.lastStringOf( widget_path, "/")
        end

        self["_txt" .. btn_name]=txt
        return true
    end,

    initWidgetTextValues = function(self,  text_paths)
        if type(text_paths) == "table" then
            for _, path in pairs(text_paths) do
                self:init_inner_text(path)
            end
        else
            log.error("init button failed: text_paths type err", type(text_paths))
        end
    end,

    setWidgetTextValues = function(self,  text_paths_value)
        if type(text_paths_value) == "table" then
            for _, item in pairs(text_paths_value) do
                local path = item[1]
                local txt = item[2]
                local txt_ctrl =self:find_text(path)
                if txt_ctrl then
                    txt_ctrl.text = txt
                else
                    log.error(" not such widget", path)
                end
                --self:init_inner_text(path)
            end
        else
            log.error("init button failed: text_paths type err", type(text_paths))
        end
    end,

    init_text_value = function( self, text_path)
        local btnwrap = self:find_text(text_path)
        if not btnwrap then
            log.error(" not such text", text_path)
            return false
        end

        local btn_name = text_path
        local rst = string.find(text_path, "/")
        if rst then
            btn_name = utility.lastStringOf( text_path, "/")
        end

        self["_txt" .. btn_name]=btnwrap
        return true
    end,

    initTextValues = function(self,  text_paths)
        if type(text_paths) == "table" then
            for _, path in pairs(text_paths) do
                self:init_text_value(path)
            end
        else
            logger.error("init button failed: text_paths type err", type(text_paths))
        end
    end,

    --按照  "_ipt"+input_path 生成引用变量
    init_input_value = function( self, input_path)
        local btnwrap = self:find_input(input_path)
        if not btnwrap then
            log.error("register event failed: not such input", input_path)
            return false
        end

        local btn_name = input_path
        local rst = string.find(input_path, "/")
        if rst then
            btn_name = utility.lastStringOf( input_path, "/")
        end

        self["_ipt" .. btn_name]=btnwrap
        return true
    end,

    initInputValues = function(self,  input_paths)
        if type(input_paths) == "table" then
            for _, path in pairs(input_paths) do
                self:init_input_value(path)
            end
        else
            log.error("init button failed: btn_paths type err", type(input_paths))
        end
    end,

    --按照  "_btn"+button名称 生成引用变量
    init_button_value = function( self, btn_path)
        local btnwrap = self:find_button(btn_path)
        if not btnwrap then
            log.error("register event failed: not such button", btn_path)
            return false
        end

        local btn_name = btn_path
        local rst = string.find(btn_path, "/")
        if rst then
            btn_name = utility.lastStringOf( btn_path, "/")
        end

        self["_btn" .. btn_name]=btnwrap
        return true
    end,

    initButtonValues = function(self,  btn_paths)
        if type(btn_paths) == "table" then
            for _, path in pairs(btn_paths) do
                self:init_button_value(path)
            end
        else
            log.error("init button failed: btn_paths type err", type(btn_paths))
        end
    end,

    --按照  "click_"+button名称 注册回调
    reg_btn_click = function( self, btn_path, host)
        local btnwrap = self:find_button(btn_path)
        if not btnwrap then
            log.error("register event failed: not such button", btn_path)
            return false
        end

        local btn_name = btn_path
        local rst = string.find(btn_path, "/")
        if rst then
            btn_name = utility.lastStringOf( btn_path, "/")
        end

        local cb = host["click_" .. btn_name]
        if not cb then
            log.error("register event failed: not such function", cb)
            return false
        end

        btnwrap:register_click( cb, host)
        return true
    end,

    regClickEventsToHost = function(self,  clickEvents, host)
        if type(clickEvents) == "table" then
            for _, path in pairs(clickEvents) do
                self:reg_btn_click(path, host)
            end
        else
            log.error("register event failed: clickEvents type err", type(clickEvents))
        end
    end,
}