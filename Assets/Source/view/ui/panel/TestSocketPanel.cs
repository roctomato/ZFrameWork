﻿
using UnityEngine.UI;
using Zby;

public class TestSocketPanel : CnViewBase
{
    Chat_Socket     _socket;

    Panel_component _component;

    // Use this for initialization
    void Start () {
        ZLog.D(this, "start socket order {0}", this._zOrder);

        _component._btn1.transform.Find("Text").GetComponent<Text>().text = "connect";
        SetClickEventOnce(_component._btn1, ClickBtnConnet, new object[] { "cnt" });


        _component._btn2.transform.Find("Text").GetComponent<Text>().text = "disconnect";
        SetClickEventOnce(_component._btn2, ClickDisconnet, new object[] { "dis" });

        _component._text.text = "init";

        _socket = new Chat_Socket(this);

        _socket.ChatEvent += OnRecvChat;
        _socket.ConnectEvent += OnConnetEvent;
        _socket.DisEvent += OnDisconnectEvent;
    }
    void OnConnetEvent()
    {
        _component._text.text = "connect ok";
    }
    void OnDisconnectEvent(int reason, string str)
    {
        _component._text.text = string.Format("failed[{0}]:{1}", reason, str);
    }
    void OnRecvChat(string content)
    {
        _component._text.text = content;
    }
    // Update is called once per frame
    void Update () {
		
	}
    void ClickDisconnet(object[] args)
    {
        ZLog.D(this, "click connect args {0}", args[0]);
        _socket.Close();
    }
    void ClickBtnConnet(object[] args)
    {
        ZLog.D(this, "click connect args {0}", args[0]);   
        _socket.Connect("192.168.11.50", 50008);
    }

    //
    public override CnUiComponent GetCnUiComponent()
    {
        if ( null == _component)
        {
            _component = new Panel_component();
        }
        return _component;
    }
    public override void OnLoad(params object[] args) 
    {
        ZLog.D(this, "onload order {0}", this._zOrder);
    }
    public override bool OnUnload() {
        ZLog.D(this, "onunload order {0}", this._zOrder);
        return true;
    }

    public override void OnBehind(CnViewBase topview) //从顶层移到后一层
    {
        ZLog.D(this, "OnBehind order {0}", this._zOrder);
    }
    public override void OnTop(CnViewBase topview) //从后层变到顶层
    {
        ZLog.D(this, "OnTop order {0}", this._zOrder);
    }  
}
