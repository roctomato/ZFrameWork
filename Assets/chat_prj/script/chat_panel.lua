local super = require("ui.panel_base")
local container = require("ui.container")
local ui_mgr = game_global.ui_mgr

local log = logger.get('panel','chat_panel')

local ChatItem = class {
    super = container,

    ctor = function (self, native, chat_text)
        container.ctor(self, native)
        self.cmnt = native:GetComponent(typeof(CS.ChatBubbleFrame))
        self.cmnt:Show(chat_text)
        --log(cmnt:GetHeight())
        --log(cmnt)
        --self:set_child_visible('EmotionBubble', false)
        --[[
        local txt_ctrl =self:find_text('ChatBubble/Text')
        if txt_ctrl then
            txt_ctrl.text = chat_text
        end
        ]]
    end,

    GetHeight = function (self)
        return self.cmnt:GetHeight()
    end

}

return class {
    super = super,

    awake = function (self, args)
        -- body
        self:register()
        self.chat_list ={}
    end,

    register = function (self)
        local button_path ={
            "InputBar/SendButton", -- click_SendButton
        }
        self:regClickEvents(button_path)

        local input_path ={
            "InputBar/ChatInputField", -- _iptChatInputField
        }
        self:initInputValues(input_path)

        local toggle = self:find_toggle("InputBar/EmotionToggle")
        toggle:register_onchange(self.on_EmotionToggle, self)

        self.emonPanel = self:find_container('EmotionGrid')
        self.ChatBubbleGrid = self:find_container('ChatLog/ChatBubbleGrid')
        self.gridChatBubble = self.ChatBubbleGrid:GetComponent('VerticalLayoutGroup')

        self.scrollChatLog = self:find('ChatLog','ScrollRect')

        --print(self.gridChatBubble)
    end,

    on_EmotionToggle = function(self, checked)
        log.info("change", self, checked)
        self.emonPanel.visible = checked
        --self.ChatBubbleGrid.visible = checked
    end,

    click_SendButton = function(self)
        log.info("click", self._iptChatInputField.text)
        local trans =ui_mgr:InitResEx('Chat/FriendChatBubbleFrame', self.ChatBubbleGrid.native).transform
        local chat_item = ChatItem(trans, self._iptChatInputField.text)
        self._iptChatInputField.text=''
        table.insert (self.chat_list, chat_item)
        self:UpdateChatBubbleGrid()
    end,

    UpdateChatBubbleGrid = function(self)
        local sumHeight = 0
        for _, cnmt in ipairs(self.chat_list) do
            sumHeight = sumHeight + cnmt:GetHeight()
        end

        self.gridChatBubble:GetComponent('RectTransform').sizeDelta = CS.UnityEngine.Vector2(960, sumHeight);
        self.scrollChatLog.verticalNormalizedPosition = 0;
    end,

}