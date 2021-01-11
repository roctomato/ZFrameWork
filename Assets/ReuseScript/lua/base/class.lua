--[[
    用lua实现的模拟类
    实现了
    1.类类型的定义，以及类类型的实例化
    2.单继承以及多继承
    3.实现多态，即子类的同名函数覆盖父类的同名函数

    super 单继承基类
    supers 多个父类写到一个表中

    ctor 构造函数, 创建类实例后会调用此函数，如果子类没实现此函数，则会依次查找所有父类们的构造函数，如果找到只调用找到的第一个构造函数 
         没找到就不调用。建议所以类成员变量在此函数中定义并初始化

代码范例
    定义类类型
    local class_type = class{
        ctor = function ( self, a, name )
            self.a = a
            self.name = name
            print('in class_type ctor ', self.a, self.name )
        end,

        log = function ( self )
             print('in class_type ctor ', self.a, self.name )
        end,

    }

    local ins = class_type(11,'test') --实例化

    local class_b = class {

    }
]]

--获取父类列表
local get_super_class = function( cls )
    if cls.supers ~= nil then
        return cls.supers
    elseif cls.super ~= nil then
        return { cls.super }
    end
    return nil
end

--获取类模板中名为key的值(主要用来获得函数)
local get_func = function ( cls, key )
    local ret = cls[key]
    if  not ret then         
        local supers = get_super_class(cls)
        if  supers then
            for _,super in ipairs(supers)do
                ret = super[key]
                if ret then
                    break
                end
            end 
        end   
    end
    return ret
end

--判断cls是否是cls_parent的子类
local cls_is_subclassof 
cls_is_subclassof = function ( cls, cls_parent)
    local supers = get_super_class(cls)
    if not supers then
        return false
    end
  
    for _,super in ipairs(supers) do
        if cls_parent == super then
            ret = true
            break
        end
        ret = cls_is_subclassof(super, cls_parent)
        if ret then
            break
        end
    end 
    
    return ret 
end

--创建类实例 cls类定义 creator为cls类的创建类
local new_ins = function ( cls, creator, ... )
    local ins = setmetatable({}, 
    {
        __index=function ( self, key )
            local ret = get_func(cls,key)
            if ret and type(ret) == "table" then
                local getter = ret.getter
                if getter then 
                    ret = getter(self)
                end
            end
            return ret
        end,

        __newindex = function (self, key, value)
            local func = get_func(cls,key)
            if type(func) == "table" then
                local setter = func.setter
                if type(setter) == "function" then  
                    setter(self, value) 
                    return
                end
            end
            rawset(self, key, value)
        end,
    })

    ins.isSubClassOf = function (self, cls_type )
        return cls_is_subclassof(cls, cls_type)
    end

    ins.creator = creator
    if ins.ctor then
        ins:ctor(...)
    end
    return ins
end

-- 生成cls类的创建类
local class = function ( cls )
    
    local cls_creator =setmetatable({},
    {
        __call=function (self, ... )
            return new_ins(cls, self, ...)
        end,

        __index =function (self, key )
            return get_func(cls,key)
        end,
    })

    cls_creator.class = cls

    cls_creator.isSubClassOf = function (self, cls_type )
        return cls_is_subclassof(cls, cls_type)
    end

    cls_creator.new = function ( ... )
        return new_ins(cls,cls_creator, ...)
    end
    return cls_creator
end

return class