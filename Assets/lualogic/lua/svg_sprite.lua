local super = require("ui.panel_base")
--local effect = require('effect')

return class {
    super = super,

    awake = function (self, path)
        print('lua awake')
        --local cmpt = self.native:AddComponent(typeof(CS.WrapperSvg))
        --print("find", cmpt)
        --cmpt:Init()
      
        --local path = CS.UnityEngine.Application.dataPath..'/Art/SVG/40821.svg'
        --[[
        cmpt:SetSvgPath(path,function_ex.make(self.on_complete, self))

        effect.retation.init(self, self.native
            , 100 --旋转速度
        )

        effect.scale.init(self, self.native
            , 0.3  --初始值
            , 0.5  -- 最大值
            , 0.1 -- 最小值
            , 0.001 -- 速度
        )    
        ]] 

        local cmpt =  self:AddComponent(typeof(CS.SvgAnimation))
        print("find", cmpt)
        cmpt:UseSpriteRender()

        local path = CS.UnityEngine.Application.dataPath..'/Art/svg/20843.svg' --40821

        cmpt:SetPath(path);
        --cmpt:ShowPath(1)
        cmpt:Show();
    
        cmpt:StartDraw(20, function_ex.make(self.on_complete, self))
    end,

    on_complete = function (self)
      
    end,

    update = function (self)
    
    end,
}