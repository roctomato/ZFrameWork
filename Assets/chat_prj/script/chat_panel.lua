local super = require("ui.panel_base")
local container = require("ui.container")
local ui_mgr = game_global.ui_mgr

local log = logger.get('panel','chat_panel')
local json = require('base.json')

local ChatItem = class {
    super = container,

    ctor = function (self, native, chat_text)
        container.ctor(self, native)
        self.cmnt = native:GetComponent(typeof(CS.ChatBubbleFrame))
        self.cmnt:Show(chat_text)
    end,

    GetHeight = function (self)
        return self.cmnt:GetHeight()
    end
}

local SelfChatItem = class {
    super = ChatItem,

    ctor = function (self, native, chat_text)
        ChatItem.ctor(self, native, chat_text)
        --self.cmnt = native:GetComponent(typeof(CS.ChatBubbleFrame))
        --self.cmnt:Show(chat_text)
        self.send = self:find_widget('ChatBubble/SendingLabel')
    end,

    HideSend = function (self)
        self.send.visible = false
    end
}

return class {
    super = super,

    awake = function (self, args)
        -- body
        self:register()
        self.chat_list ={}
        self.ws= CS.LuaWebSocket(self)
        self.ws:OpenUrl( game_global.chat_server)
    end,

    ondestroy =function (self)
        print("ondestory")
        self.ws:Disconnect()
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

    show_chat_content = function (self, name, txt)
        local trans =ui_mgr:InitResEx('Chat/FriendChatBubbleFrame', self.ChatBubbleGrid.native).transform
        --local trans =ui_mgr:InitResEx('Chat/SelfChatBubbleFrame', self.ChatBubbleGrid.native).transform
        local txt = name .. '\n' .. txt
        local chat_item = ChatItem(trans, txt)
        table.insert (self.chat_list, chat_item)
        self:UpdateChatBubbleGrid()
    end,
    show_self_content = function (self, txt, show_send)
        local trans =ui_mgr:InitResEx('Chat/SelfChatBubbleFrame', self.ChatBubbleGrid.native).transform
        --local txt =  txt
        local chat_item = SelfChatItem(trans, txt)
        if not show_send then
            chat_item:HideSend()
        end
        table.insert (self.chat_list, chat_item)
        self:UpdateChatBubbleGrid()
    end,
    
    click_SendButton = function(self)
        log.info("click", self._iptChatInputField.text)
        --local trans =ui_mgr:InitResEx('Chat/FriendChatBubbleFrame', self.ChatBubbleGrid.native).transform
        --local chat_item = ChatItem(trans, self._iptChatInputField.text)
        --local txt = game_global.name .. '\n' .. self._iptChatInputField.text
        self:show_self_content( self._iptChatInputField.text, false)
        self._iptChatInputField.text=''
        --table.insert (self.chat_list, chat_item)
        --self:UpdateChatBubbleGrid()
    end,

    UpdateChatBubbleGrid = function(self)
        local sumHeight = 0
        for _, cnmt in ipairs(self.chat_list) do
            sumHeight = sumHeight + cnmt:GetHeight()
        end

        self.gridChatBubble:GetComponent('RectTransform').sizeDelta = CS.UnityEngine.Vector2(960, sumHeight);
        self.scrollChatLog.verticalNormalizedPosition = 0;
    end,

    --websocket event
    send_register=function (self)
        local msg ={method='register',param ={name=game_global.name }}
        self.ws:SendText(json.encode(msg))
    end,
    OnOpen= function(self, url)
        log('on open', url)
        self:send_register()
    end,

    OnErr= function(self, err_msg)
        log( 'on err', err_msg)
    end,

    OnDisconnect= function(self, reson, str)
        log('ondisconnect',reson, str)
    end,

    OnTxtMsg = function(self, msg, count)
        log( 'on text msg', msg, count)
    end,
}