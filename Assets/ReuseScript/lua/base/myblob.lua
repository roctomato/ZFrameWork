local class = require('class')

local myblob ={}

--序列化类
myblob.BufferWrite = class {
    ctor = function (self)
        self.out=''
        self.type_map ={
            ["LsInt8"]=  self.WriteInt8,
            ["LsUInt8"]= self.WriteUInt8,
            ["LsInt16"]= self.WriteInt16,
            ["LsUInt16"]=self.WriteUInt16,
            ["LsInt32"]= self.WriteInt32,
            ["LsUInt32"]=self.WriteUInt32,
            ["LsInt64"]= self.WriteInt64,
            ["LsUInt64"]=self.WriteUInt64,
            ["float"]=   self.WriteFloat,
            ["double"]=  self.WriteDouble,
            ["string"]=  self.WriteString,
            ["BigString"]= self.WriteBigString,
        }
    end,

    WriteInt8 = function(self,v)
        self.out= self.out..string.pack("b",v)
    end,

    WriteUInt8 = function(self,v)
        self.out= self.out..string.pack("B",v)
    end,

    WriteInt16 = function(self,v)
        self.out= self.out..string.pack("h",v)
    end,

    WriteUInt16 = function(self,v)
        self.out= self.out..string.pack("H",v)
    end,

    WriteInt32 = function(self,v)
        self.out= self.out..string.pack("i4",v)
    end,

    WriteUInt32 = function(self,v)
        self.out= self.out..string.pack("I4",v)
    end,

    WriteInt64 = function(self,v)
        self.out= self.out..string.pack("l",v)
    end,

    WriteUInt64 = function(self,v)
        self.out= self.out..string.pack("L",v)
    end,

    WriteFloat = function(self,v)
        self.out= self.out..string.pack("f",v)
    end,

    WriteDouble = function(self,v)
        self.out= self.out..string.pack("d",v)
    end,

    WriteString = function(self,v)
        self.out= self.out..string.pack("s2",v)
    end,

    WriteBigString = function(self,v)
        self.out= self.out..string.pack("s4",v)
    end,

    WriteList = function(self, li, vtype)
        local n = #li
        self:WriteUInt16( n )
        local func = self.type_map[vtype]
        for _,v in ipairs(li) do
            func( self,v)
        end
    end,

    WriteUserDatalist = function ( self, v )
        --'object must has method write_to'
        local n = #v
        self:WriteUInt16( n )
        for _,data in ipairs(v) do
            data:write_to( self )
        end
    end,

    AppendBuff = function (self, v)
        self.out = self.out..v
    end,

    GetBuff= function (self)
        return self.out
    end,
}

--反序列化类
myblob.BufferRead = class {
    ctor = function (self, buff)
        self.buff  = buff
        self.index = 1
        self.type_map ={
            ["LsInt8"]  = self.ReadInt8,
            ["LsUInt8"] = self.ReadUInt8,
            ["LsInt16"] = self.ReadInt16,
            ["LsUInt16"]=self.ReadUInt16,
            ["LsInt32"] =self.ReadInt32,
            ["LsUInt32"]=self.ReadUInt32,
            ["LsInt64"] =self.ReadInt64,
            ["LsUInt64"]=self.ReadUInt64,
            ["float"]=self.ReadFloat,
            ["double"]=self.ReadDouble,
            ["string"]=self.ReadString,
            ["BigString"]=self.ReadBigString,
        }
    end,

    ReadInt8 = function (self)
        local val
        val, self.index = string.unpack('b',self.buff, self.index)
        return val
    end,

    ReadUInt8 = function (self)
        local val
        val, self.index = string.unpack('B',self.buff, self.index)
        return val
    end,

    ReadInt16 = function (self)
        local val
        val, self.index = string.unpack('h',self.buff, self.index)
        return val
    end,

    ReadUInt16 = function (self)
        local val
        val, self.index = string.unpack('H',self.buff, self.index)
        return val
    end,

    ReadInt32 = function (self)
        local val
        val, self.index = string.unpack('i4',self.buff, self.index)
        return val
    end,

    ReadUInt32 = function (self)
        local val
        val, self.index = string.unpack('I4',self.buff, self.index)
        return val
    end,

    ReadInt64 = function (self)
        local val
        val, self.index = string.unpack('l',self.buff, self.index)
        return val
    end,

    ReadUInt64 = function (self)
        local val
        val, self.index = string.unpack('L',self.buff, self.index)
        return val
    end,

    ReadFloat = function (self)
        local val
        val, self.index = string.unpack('f',self.buff, self.index)
        return val
    end,

    ReadDouble = function (self)
        local val
        val, self.index = string.unpack('d',self.buff, self.index)
        return val
    end,

    ReadString = function (self)
        local val
        val, self.index = string.unpack('s2',self.buff, self.index)
        return val
    end,

    ReadBigString = function (self)
        local val
        val, self.index = string.unpack('s4',self.buff, self.index)
        return val
    end,

    ReadList = function (self, li, vtype)
        local n  = self:ReadUInt16()
        local func =  self.type_map[vtype]
        local i = 0
        while i < n do
            local v = func(self)
            table.insert( li, v)
            i = i + 1
        end
        return li
    end,

    ReadUserDatalist = function ( self, li, class_fac )
        --'class_fac create object and it must has method read_from'
        local n = self:ReadUInt16()
        local i = 0
        while i < n do
            local v = class_fac()
            v:read_from(self)
            table.insert( li, v)
            i = i + 1
        end
        return li
    end,
}

--结构、数组基类
myblob.SerialseBytesBase = class {
    write_to = function ( self, buff)
        error('NotImplementedError write_to')
    end,

    to_bytes = function (self)
       local buff = myblob.BufferWrite()
        self:write_to(buff)
        return buff:GetBuff()
    end,

    read_from = function(self, rb )
        --'rb -  BufferRead' 
        error('NotImplementedError read_from')
    end,

    read_bytes = function (self, buff)
        --'buff bytes'
        local rb = myblob.BufferRead(buff)
        self:read_from(rb)
    end,

    print_data = function (self)
        error('NotImplementedError print_data')
    end,
}

--结构的数组基类
myblob.VecUserData = class {
    super =  myblob.SerialseBytesBase,

    ctor = function (self, class_fac)
        self.class_fac = class_fac
        self.li        = {}
    end,

    write_to = function ( self, buff)
        buff:WriteUserDatalist(self.li)
    end,

    read_from = function(self, rb )
        rb:ReadUserDatalist( self.li, self.class_fac)
    end,

    print_data = function (self)
        print("total:", #self.li)
        for _, data in ipairs(self.li) do
            data:print_data()
        end
    end,

    add = function (self)
        local ins = self.class_fac()
        table.insert( self.li, ins)
        return ins
    end,

    get_item = function (self, idx)
        return self.li[idx]
    end,
}

myblob.VecNormal = class {
    super =  myblob.SerialseBytesBase,
  
    ctor = function(self, normal_type, check_v)
        self.normal_type = normal_type
        self.check_v = check_v
        self.li = {}
    end,

    write_to=function( self, buff)
        buff:WriteList(self.li, self.normal_type)
    end,

    read_from = function(self, rb )
        rb:ReadList( self.li, self.normal_type)
    end,

    print_data = function(self)
        print("total:", #self.li)
        for _, data in ipairs(self.li) do
            print(data)
        end
    end,

    add = function (self,v)
        if type(v) ~= type(self.check_v) then
            error ('TypeError')
        end
        table.insert( self.li, v)
    end,

    get_item = function (self, idx)
        return self.li[idx]
    end,
}
return myblob