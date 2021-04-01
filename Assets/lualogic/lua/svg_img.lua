local super = require("ui.panel_base")
return class {
    super = super,

    awake = function (self, args)
        print('lua awake', typeof(CS.SvgAnimation), self.native, self.native.gameObject.AddComponent )

        self.native.transform.localScale = CS.UnityEngine.Vector3( 1.5 , 1.5 ,0)
        
        local cmpt =  self:AddComponent(typeof(CS.SvgAnimation))
        print("find", cmpt)

        cmpt:UseNormalImg()

        local path = CS.UnityEngine.Application.dataPath..'/Art/svg/32511.svg' --40821

        cmpt:SetPath(path);
        cmpt:Show();
    
        cmpt:StartDraw(20, function_ex.make(self.on_complete, self))
        self.cmpt = cmpt
    end,

    on_complete = function (self)
        print("draw complete")
        --self.cmpt:Redraw()

    end,

    update = function (self)
    end,
}