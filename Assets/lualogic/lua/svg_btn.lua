local super = require("ui.panel_base")
return class {
    super = super,

    awake = function (self, args)
        self.btn = self:find_button('Button')
        self.btn:register_click( self.on_click, self)
        print(self.btn)
        local cmpt =  self.btn:AddComponent(typeof(CS.SvgAnimation))
        print("find", cmpt)

        cmpt:UseNormalImg()

        local path = CS.UnityEngine.Application.dataPath..'/Art/svg/32511.svg' --40821

        cmpt:SetPath(path);
        cmpt:Show();
    end,

    on_click = function (self)
        print('click')
    end
}