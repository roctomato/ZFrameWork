local myblob = require('myblob')
local class  = require('class')

local DL  = {}


DL.VecUInt32 = class { --  cmment
    super = myblob.VecNormal,
    ctor = function (self)
        myblob.VecNormal.ctor(self,"LsUInt32",0)
    end
}

DL.NameLevel = class { --名称等级
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.name = ""  --  汉字或五行字符
        self.level = 0  --  等级
    end,

    read_from = function(self, rb )
        self.name = rb:ReadString()
        self.level = rb:ReadInt32()
    end,

    write_to = function( self, buff)
        buff:WriteString( self.name )
        buff:WriteInt32( self.level )
    end,

    print_data = function (self)
        print('name', self.name )
        print('level', self.level )
    end,
}


DL.VecNameLevel = class{ -- 
    super = myblob.VecUserData,
    ctor = function(self)
        myblob.VecUserData.ctor(self,DL.NameLevel )
    end,
}

DL.NameIndexLevel = class { --名称等级
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.name = ""  --  汉字或五行字符
        self.level = 0  --  等级
        self.index = 0  --  表中索引
    end,

    read_from = function(self, rb )
        self.name = rb:ReadString()
        self.level = rb:ReadInt32()
        self.index = rb:ReadInt32()
    end,

    write_to = function( self, buff)
        buff:WriteString( self.name )
        buff:WriteInt32( self.level )
        buff:WriteInt32( self.index )
    end,

    print_data = function (self)
        print('name', self.name )
        print('level', self.level )
        print('index', self.index )
    end,
}


DL.VecNameIndexLevel = class{ -- 
    super = myblob.VecUserData,
    ctor = function(self)
        myblob.VecUserData.ctor(self,DL.NameIndexLevel )
    end,
}

DL.Hanz = class { --上阵汉字
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.name = ""  --  汉字或五行字符
        self.level = 0  --  等级
        self.index = 0  --  表中索引
        self.ci = DL.VecNameIndexLevel() -- 成词伏兵
    end,

    read_from = function(self, rb )
        self.name = rb:ReadString()
        self.level = rb:ReadInt32()
        self.index = rb:ReadInt32()
        self.ci:read_from(rb)
    end,

    write_to = function( self, buff)
        buff:WriteString( self.name )
        buff:WriteInt32( self.level )
        buff:WriteInt32( self.index )
        self.ci:write_to(buff)
    end,

    print_data = function (self)
        print('name', self.name )
        print('level', self.level )
        print('index', self.index )
        print('ci:')
        self.ci:print_data()
    end,
}


DL.VecHanz = class{ -- 
    super = myblob.VecUserData,
    ctor = function(self)
        myblob.VecUserData.ctor(self,DL.Hanz )
    end,
}

DL.FiveSkill = class { --五行技能详细信息
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.index = 0  --  表中索引
        self.five = ""  --  五行
        self.level = 0  --  等级
        self.strct = 0  --  结构追击
        self.bushou = 0  --  部首连携
        self.ci = 0  --  成词伏兵
        self.t_strct = 0  --  结构追击加伤害
        self.m_bushou = 0  --  部首联协加伤害
        self.h_lian = 0  --  连段加伤害
        self.s_ci = 0  --  成词加伤害
        self.s_zhong = 0  --  重段加伤害
    end,

    read_from = function(self, rb )
        self.index = rb:ReadInt32()
        self.five = rb:ReadString()
        self.level = rb:ReadInt32()
        self.strct = rb:ReadInt32()
        self.bushou = rb:ReadInt32()
        self.ci = rb:ReadInt32()
        self.t_strct = rb:ReadInt32()
        self.m_bushou = rb:ReadInt32()
        self.h_lian = rb:ReadInt32()
        self.s_ci = rb:ReadInt32()
        self.s_zhong = rb:ReadInt32()
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.index )
        buff:WriteString( self.five )
        buff:WriteInt32( self.level )
        buff:WriteInt32( self.strct )
        buff:WriteInt32( self.bushou )
        buff:WriteInt32( self.ci )
        buff:WriteInt32( self.t_strct )
        buff:WriteInt32( self.m_bushou )
        buff:WriteInt32( self.h_lian )
        buff:WriteInt32( self.s_ci )
        buff:WriteInt32( self.s_zhong )
    end,

    print_data = function (self)
        print('index', self.index )
        print('five', self.five )
        print('level', self.level )
        print('strct', self.strct )
        print('bushou', self.bushou )
        print('ci', self.ci )
        print('t_strct', self.t_strct )
        print('m_bushou', self.m_bushou )
        print('h_lian', self.h_lian )
        print('s_ci', self.s_ci )
        print('s_zhong', self.s_zhong )
    end,
}


DL.VecFiveSkill = class{ -- 
    super = myblob.VecUserData,
    ctor = function(self)
        myblob.VecUserData.ctor(self,DL.FiveSkill )
    end,
}

DL.Moke = class { --上阵墨客
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.info = DL.NameLevel() -- 简单信息
        self.skin = 0.0  --  皮肤加成
        self.five = DL.VecNameLevel() -- 五行技能等级
        self.formation = DL.VecHanz() -- 上阵汉字
    end,

    read_from = function(self, rb )
        self.info:read_from(rb)
        self.skin = rb:ReadFloat()
        self.five:read_from(rb)
        self.formation:read_from(rb)
    end,

    write_to = function( self, buff)
        self.info:write_to(buff)
        buff:WriteFloat( self.skin )
        self.five:write_to(buff)
        self.formation:write_to(buff)
    end,

    print_data = function (self)
        print('info:')
        self.info:print_data()
        print('skin', self.skin )
        print('five:')
        self.five:print_data()
        print('formation:')
        self.formation:print_data()
    end,
}

DL.Fight = class { --战斗请求
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.day = 0  --  星期几
        self.jia = DL.Moke() -- 墨客甲
        self.yi = DL.Moke() -- 墨客乙
    end,

    read_from = function(self, rb )
        self.day = rb:ReadInt32()
        self.jia:read_from(rb)
        self.yi:read_from(rb)
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.day )
        self.jia:write_to(buff)
        self.yi:write_to(buff)
    end,

    print_data = function (self)
        print('day', self.day )
        print('jia:')
        self.jia:print_data()
        print('yi:')
        self.yi:print_data()
    end,
}

DL.MokeInfo = class { --战报中的墨客信息
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.name = ""  --  墨客姓名
        self.level = 0  --  墨客等级
        self.blood = 0.0  --  初始血量
        self.skin = 0.0  --  皮肤加成
        self.skill = DL.VecUInt32() -- 上阵主动技能
        self.fiveskill = DL.VecFiveSkill() -- 五行技能等级
        self.formation = DL.VecHanz() -- 上阵汉字
        self.leftblood = 0.0  --  战斗后剩余血量
    end,

    read_from = function(self, rb )
        self.name = rb:ReadString()
        self.level = rb:ReadInt32()
        self.blood = rb:ReadFloat()
        self.skin = rb:ReadFloat()
        self.skill:read_from(rb)
        self.fiveskill:read_from(rb)
        self.formation:read_from(rb)
        self.leftblood = rb:ReadFloat()
    end,

    write_to = function( self, buff)
        buff:WriteString( self.name )
        buff:WriteInt32( self.level )
        buff:WriteFloat( self.blood )
        buff:WriteFloat( self.skin )
        self.skill:write_to(buff)
        self.fiveskill:write_to(buff)
        self.formation:write_to(buff)
        buff:WriteFloat( self.leftblood )
    end,

    print_data = function (self)
        print('name', self.name )
        print('level', self.level )
        print('blood', self.blood )
        print('skin', self.skin )
        print('skill:')
        self.skill:print_data()
        print('fiveskill:')
        self.fiveskill:print_data()
        print('formation:')
        self.formation:print_data()
        print('leftblood', self.leftblood )
    end,
}

DL.DayBuff = class { --日期加成
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.index = 0  --  星期几
        self.five = ""  --  五行
        self.percent = 0.0  --  日期加成
    end,

    read_from = function(self, rb )
        self.index = rb:ReadInt32()
        self.five = rb:ReadString()
        self.percent = rb:ReadFloat()
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.index )
        buff:WriteString( self.five )
        buff:WriteFloat( self.percent )
    end,

    print_data = function (self)
        print('index', self.index )
        print('five', self.five )
        print('percent', self.percent )
    end,
}

DL.FightInfo = class { --战斗信息
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.day = DL.DayBuff() -- 星期几加成
        self.jia = DL.MokeInfo() -- 墨客甲
        self.yi = DL.MokeInfo() -- 墨客乙
    end,

    read_from = function(self, rb )
        self.day:read_from(rb)
        self.jia:read_from(rb)
        self.yi:read_from(rb)
    end,

    write_to = function( self, buff)
        self.day:write_to(buff)
        self.jia:write_to(buff)
        self.yi:write_to(buff)
    end,

    print_data = function (self)
        print('day:')
        self.day:print_data()
        print('jia:')
        self.jia:print_data()
        print('yi:')
        self.yi:print_data()
    end,
}

DL.FightRecord = class { --战斗伤害数值
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.bihua = ""  --  参与战斗的笔画
        self.normal = 0.0  --  普攻
        self.zhong = 0.0  --  重段
        self.lian = 0.0  --  连段
        self.total = 0.0  --  总攻击力
        self.jiegou_attack = 0.0  --  结构技伤害
        self.bushou_attack = 0.0  --  部首连携
        self.ci_attack = 0.0  --  成词伏兵
        self.hurt = 0.0  --  收到的伤害
        self.left_blood = 0.0  --  剩余血量
    end,

    read_from = function(self, rb )
        self.bihua = rb:ReadString()
        self.normal = rb:ReadFloat()
        self.zhong = rb:ReadFloat()
        self.lian = rb:ReadFloat()
        self.total = rb:ReadFloat()
        self.jiegou_attack = rb:ReadFloat()
        self.bushou_attack = rb:ReadFloat()
        self.ci_attack = rb:ReadFloat()
        self.hurt = rb:ReadFloat()
        self.left_blood = rb:ReadFloat()
    end,

    write_to = function( self, buff)
        buff:WriteString( self.bihua )
        buff:WriteFloat( self.normal )
        buff:WriteFloat( self.zhong )
        buff:WriteFloat( self.lian )
        buff:WriteFloat( self.total )
        buff:WriteFloat( self.jiegou_attack )
        buff:WriteFloat( self.bushou_attack )
        buff:WriteFloat( self.ci_attack )
        buff:WriteFloat( self.hurt )
        buff:WriteFloat( self.left_blood )
    end,

    print_data = function (self)
        print('bihua', self.bihua )
        print('normal', self.normal )
        print('zhong', self.zhong )
        print('lian', self.lian )
        print('total', self.total )
        print('jiegou_attack', self.jiegou_attack )
        print('bushou_attack', self.bushou_attack )
        print('ci_attack', self.ci_attack )
        print('hurt', self.hurt )
        print('left_blood', self.left_blood )
    end,
}

DL.TurnRecord = class { --一回合战斗数据
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.index = 0  --  第几回合
        self.type = 0  --  战斗类型 0-普攻 1-结构攻击 2-部首连携 3-成词伏兵
        self.bk_five = ""  --  背景五行
        self.bk_value = 0.0  --  背景加成
        self.jia = DL.FightRecord() -- 甲的攻击数据
        self.yi = DL.FightRecord() -- 乙的攻击数据
    end,

    read_from = function(self, rb )
        self.index = rb:ReadInt32()
        self.type = rb:ReadInt32()
        self.bk_five = rb:ReadString()
        self.bk_value = rb:ReadFloat()
        self.jia:read_from(rb)
        self.yi:read_from(rb)
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.index )
        buff:WriteInt32( self.type )
        buff:WriteString( self.bk_five )
        buff:WriteFloat( self.bk_value )
        self.jia:write_to(buff)
        self.yi:write_to(buff)
    end,

    print_data = function (self)
        print('index', self.index )
        print('type', self.type )
        print('bk_five', self.bk_five )
        print('bk_value', self.bk_value )
        print('jia:')
        self.jia:print_data()
        print('yi:')
        self.yi:print_data()
    end,
}


DL.VecTurnRecord = class{ -- 
    super = myblob.VecUserData,
    ctor = function(self)
        myblob.VecUserData.ctor(self,DL.TurnRecord )
    end,
}

DL.FightHanz = class { --参与战斗的汉字
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.index = 0  --  墨客汉字配表表中的编码
        self.bishun = ""  --  汉字的笔画
        self.five = ""  --  汉字的五行属性
        self.bushou = 0  --  部首索引
        self.jiegou = 0  --  结构索引
        self.ci = DL.VecNameIndexLevel() -- 成词伏兵
        self.blood = 0.0  --  本轮初始血量
        self.five_win = 0.0  --  五行克制加成
        self.level_buff = 0.0  --  字等级加成
        self.bk = 0.0  --  背景加成
        self.day_buff = 0.0  --  日期加成
    end,

    read_from = function(self, rb )
        self.index = rb:ReadInt32()
        self.bishun = rb:ReadString()
        self.five = rb:ReadString()
        self.bushou = rb:ReadInt32()
        self.jiegou = rb:ReadInt32()
        self.ci:read_from(rb)
        self.blood = rb:ReadFloat()
        self.five_win = rb:ReadFloat()
        self.level_buff = rb:ReadFloat()
        self.bk = rb:ReadFloat()
        self.day_buff = rb:ReadFloat()
    end,

    write_to = function( self, buff)
        buff:WriteInt32( self.index )
        buff:WriteString( self.bishun )
        buff:WriteString( self.five )
        buff:WriteInt32( self.bushou )
        buff:WriteInt32( self.jiegou )
        self.ci:write_to(buff)
        buff:WriteFloat( self.blood )
        buff:WriteFloat( self.five_win )
        buff:WriteFloat( self.level_buff )
        buff:WriteFloat( self.bk )
        buff:WriteFloat( self.day_buff )
    end,

    print_data = function (self)
        print('index', self.index )
        print('bishun', self.bishun )
        print('five', self.five )
        print('bushou', self.bushou )
        print('jiegou', self.jiegou )
        print('ci:')
        self.ci:print_data()
        print('blood', self.blood )
        print('five_win', self.five_win )
        print('level_buff', self.level_buff )
        print('bk', self.bk )
        print('day_buff', self.day_buff )
    end,
}

DL.RoundRecord = class { --一轮战斗数据
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.jia = DL.FightHanz() -- 甲的上阵汉字
        self.yi = DL.FightHanz() -- 乙的上阵汉字
        self.record = DL.VecTurnRecord() -- 每回合战斗数据
    end,

    read_from = function(self, rb )
        self.jia:read_from(rb)
        self.yi:read_from(rb)
        self.record:read_from(rb)
    end,

    write_to = function( self, buff)
        self.jia:write_to(buff)
        self.yi:write_to(buff)
        self.record:write_to(buff)
    end,

    print_data = function (self)
        print('jia:')
        self.jia:print_data()
        print('yi:')
        self.yi:print_data()
        print('record:')
        self.record:print_data()
    end,
}


DL.VecRoundRecord = class{ -- 
    super = myblob.VecUserData,
    ctor = function(self)
        myblob.VecUserData.ctor(self,DL.RoundRecord )
    end,
}

DL.FightReport = class { --战报
    super = myblob.SerialseBytesBase,

    ctor = function (self)
        self.info = DL.FightInfo() -- 参与战斗的相关信息
        self.fight_report = DL.VecRoundRecord() -- 每轮战斗数据
    end,

    read_from = function(self, rb )
        self.info:read_from(rb)
        self.fight_report:read_from(rb)
    end,

    write_to = function( self, buff)
        self.info:write_to(buff)
        self.fight_report:write_to(buff)
    end,

    print_data = function (self)
        print('info:')
        self.info:print_data()
        print('fight_report:')
        self.fight_report:print_data()
    end,
}

return DL
