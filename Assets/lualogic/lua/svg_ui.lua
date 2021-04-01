local super = require("ui.panel_base")
return class {
    super = super,

    awake = function (self, args)
        print('lua awake', typeof(CS.SvgAnimation), self.native, self.native.gameObject.AddComponent )

        self.native.transform.localScale = CS.UnityEngine.Vector3( 1.5 , 1.5 ,0)
        
        local cmpt =  self:AddComponent(typeof(CS.SvgAnimation))
        print("find", cmpt)
        cmpt:UseSvgImg()
        

        local path = CS.UnityEngine.Application.dataPath..'/Art/svg/32511.svg' --40821

        cmpt:SetPath(path);
        --cmpt:ShowPath(10)
        cmpt:Show();
    
        --cmpt:StartDraw(20, function_ex.make(self.on_complete, self))
        self.cmpt = cmpt
    end,

    on_complete = function (self)
        print("draw complete")
        self.cmpt:Redraw()
        --self:unload()
        --local strke = 'Prefabs/Effect2D/Strokes/strokexg.prefab'
        --game_mgr:LoadSimplePanel(strke, "effect_node",  {'has monobahavor'})

    end,

    update = function (self)
        --effect.retation.update(self )
        --effect.scale.update(self)
        --print('unpdate')
        --[[
        self.start = Time.deltaTime* self.speed + self.start
        self.native.rotation = CS.UnityEngine.Quaternion.Euler(0,0,self.start );

        self.step  = self.step + 0.001
        self.scale =  math.sin( self.step ) / 4 +  0.75
 
        self.native.localScale = CS.UnityEngine.Vector3( self.scale , self.scale ,0)
        ]]
    end,
}