local myblob = require('myblob')
local class  = require('class')

local DL  = {}


DL.VecInt32 = class { --  cmment
    super = myblob.VecNormal,
    ctor = function (self)
        myblob.VecNormal.ctor(self,"LsInt32",0)
    end
}


DL.VecString = class { --  cmment
    super = myblob.VecNormal,
    ctor = function (self)
        myblob.VecNormal.ctor(self,"string","")
    end
}


DL.SetUInt16 = class { --  cmment
    super = myblob.VecNormal,
    ctor = function (self)
        myblob.VecNormal.ctor(self,"LsUInt16",0)
    end
}


DL.SetUInt32 = class { --  cmment
    super = myblob.VecNormal,
    ctor = function (self)
        myblob.VecNormal.ctor(self,"LsUInt32",0)
    end
}

DL.MsgHeader = class { --消息头
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.msg_id = 0  --  消息id
        self.index = 0  --  消息序列号
        self.param = 0  --  其他参数
    end,

    read_from = function(self, rb )
        self.msg_id = rb:ReadUInt16()
        self.index = rb:ReadUInt16()
        self.param = rb:ReadInt32()
    end,

    write_to = function( self, buff)
        buff:WriteUInt16( self.msg_id )
        buff:WriteUInt16( self.index )
        buff:WriteInt32( self.param )
    end,

    print_data = function (self)
        print('msg_id', self.msg_id )
        print('index', self.index )
        print('param', self.param )
    end,
}

DL.SFiveLevel = class { --名称等级
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.five = ""  --  汉字或五行字符
        self.level = 0  --  等级
    end,

    read_from = function(self, rb )
        self.five = rb:ReadString()
        self.level = rb:ReadInt16()
    end,

    write_to = function( self, buff)
        buff:WriteString( self.five )
        buff:WriteInt16( self.level )
    end,

    print_data = function (self)
        print('five', self.five )
        print('level', self.level )
    end,
}


DL.FiveLevel = class{ -- 
    super = myblob.VecUserData,
    ctor = function(self)
        myblob.VecUserData.ctor(self,DL.SFiveLevel )
    end,
}

DL.role_base_info = class { --角色基本信息
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.roleid = 0  --  角色ID
        self.role_name = ""  --  角色名
        self.level = 0  --  墨客等级
        self.fivelevel = DL.FiveLevel() -- 五行等级
    end,

    read_from = function(self, rb )
        self.roleid = rb:ReadUInt32()
        self.role_name = rb:ReadString()
        self.level = rb:ReadInt16()
        self.fivelevel:read_from(rb)
    end,

    write_to = function( self, buff)
        buff:WriteUInt32( self.roleid )
        buff:WriteString( self.role_name )
        buff:WriteInt16( self.level )
        self.fivelevel:write_to(buff)
    end,

    print_data = function (self)
        print('roleid', self.roleid )
        print('role_name', self.role_name )
        print('level', self.level )
        print('fivelevel:')
        self.fivelevel:print_data()
    end,
}

DL.role_login_info = class { --角色登录逻辑服信息
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.roleid = 0  --  角色ID
        self.account = ""  --  账号名
        self.platform = ""  --  平台
        self.token = ""  --  认证串
    end,

    read_from = function(self, rb )
        self.roleid = rb:ReadUInt32()
        self.account = rb:ReadString()
        self.platform = rb:ReadString()
        self.token = rb:ReadString()
    end,

    write_to = function( self, buff)
        buff:WriteUInt32( self.roleid )
        buff:WriteString( self.account )
        buff:WriteString( self.platform )
        buff:WriteString( self.token )
    end,

    print_data = function (self)
        print('roleid', self.roleid )
        print('account', self.account )
        print('platform', self.platform )
        print('token', self.token )
    end,
}

DL.login_version_req = class { --版本号检测请求
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.version  = 0  --  版本号
        self.platform  = ""  --  客户端平台
        self.client_info  = ""  --  客户端信息
    end,

    read_from = function(self, rb )
        self.version  = rb:ReadInt32()
        self.platform  = rb:ReadString()
        self.client_info  = rb:ReadString()
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.version  )
        buff:WriteString( self.platform  )
        buff:WriteString( self.client_info  )
    end,

    print_data = function (self)
        print('version ', self.version  )
        print('platform ', self.platform  )
        print('client_info ', self.client_info  )
    end,
}

DL.login_version_resp = class { --版本号检测应答
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.error_code = 0  --  错误码
        self.challenge = ""  --  加密串
        self.upgrade_url = ""  --  热更地址
    end,

    read_from = function(self, rb )
        self.error_code = rb:ReadInt32()
        self.challenge = rb:ReadString()
        self.upgrade_url = rb:ReadString()
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.error_code )
        buff:WriteString( self.challenge )
        buff:WriteString( self.upgrade_url )
    end,

    print_data = function (self)
        print('error_code', self.error_code )
        print('challenge', self.challenge )
        print('upgrade_url', self.upgrade_url )
    end,
}

DL.login_auth_req = class { --登陆请求请求
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.type = 0  --  认证方式
        self.name = ""  --  用户名
        self.password = ""  --  密码
        self.reserve = ""  --  保留
    end,

    read_from = function(self, rb )
        self.type = rb:ReadInt32()
        self.name = rb:ReadString()
        self.password = rb:ReadString()
        self.reserve = rb:ReadString()
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.type )
        buff:WriteString( self.name )
        buff:WriteString( self.password )
        buff:WriteString( self.reserve )
    end,

    print_data = function (self)
        print('type', self.type )
        print('name', self.name )
        print('password', self.password )
        print('reserve', self.reserve )
    end,
}

DL.login_auth_resp = class { --登陆请求应答
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.error_code = 0  --  错误码
        self.role_info = DL.role_base_info() -- 角色信息
    end,

    read_from = function(self, rb )
        self.error_code = rb:ReadInt32()
        self.role_info:read_from(rb)
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.error_code )
        self.role_info:write_to(buff)
    end,

    print_data = function (self)
        print('error_code', self.error_code )
        print('role_info:')
        self.role_info:print_data()
    end,
}

DL.test_login_auth_req = class { --测试用登陆请求请求
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.name = ""  --  用户名
        self.reserver = ""  --  保留
    end,

    read_from = function(self, rb )
        self.name = rb:ReadString()
        self.reserver = rb:ReadString()
    end,

    write_to = function( self, buff)
        buff:WriteString( self.name )
        buff:WriteString( self.reserver )
    end,

    print_data = function (self)
        print('name', self.name )
        print('reserver', self.reserver )
    end,
}

DL.test_login_auth_resp = class { --测试用登陆请求应答
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.error_code = 0  --  错误码
        self.role_info = DL.role_base_info() -- 角色信息
    end,

    read_from = function(self, rb )
        self.error_code = rb:ReadInt32()
        self.role_info:read_from(rb)
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.error_code )
        self.role_info:write_to(buff)
    end,

    print_data = function (self)
        print('error_code', self.error_code )
        print('role_info:')
        self.role_info:print_data()
    end,
}

return DL
