
--print("in test panel lua")
return class {

    ctor = function (self)
        print("构造")
        self.a= 0
    end,

    awake = function (self, args)
        -- body
        print("awake", self.a) --
        print('args', args)
        if args  then 
            for k, v in ipairs(args) do
                print(k,v)
            end
        end

        self:register()
        --self.self.transform:RegisterBtnClickEvent("Btn1",  function_ex.make(self.onLoginBtnClicked, self))
    end,

    start = function (self)
        print("start", self.a)
        --print(self.self:FindButton("Btn1"))
        --self:register()
    end,

    register = function (self)
        -- body 
        print(self.mono.UIObj.transform) --CS.UnityEngine.GameObject.Find("Button"):GetComponent("Button").onClick:AddListener(util.coroutine_call(buy))

        local btn =self.mono.UIObj.transform:Find("Btn1") --.GetComponent("Button").onClick:AddListener(self:onLoginBtnClicked())
        print("btn", btn)

        local cmpt = btn:GetComponent("Button")
        print(cmpt)
        cmpt.onClick:AddListener(function_ex.make(self.onLoginBtnClicked, self))
    end,

    update = function (self)
        -- body
        self.a = self.a + 1
    end,

    onLoginBtnClicked = function(self)
        print("click", self.a)
        self.mono:DoUnload()
	end,
}