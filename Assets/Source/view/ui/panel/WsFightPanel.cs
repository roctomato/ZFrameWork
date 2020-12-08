using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using WebSocketSharp;
using Zby;

public class WsFightPanel  : CnViewBase, WebSocketHandler
{
	
    public string _url;
	ZbyWebSocket _ws;
	
	FightComponent _component;
	
	
    // Start is called before the first frame update
    void Start()
    {
	   ZLog.D(this, "in start url {0}", _url);
       SetClickEventOnce(_component.btnSend, ClickCall, new object[] { "Call java" });
	   _ws = new ZbyWebSocket(this, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void ClickCall(object[] args)
    {
		
        //ZLog.D(this, "click button req {0}",req);
         _ws.Open(_url);
    }
	public override CnUiComponent GetCnUiComponent()
    {
        if (null == _component)
        {
            _component = new FightComponent();
        }
        return _component;
    }
	
	public override void OnLoad(params object[] args) 
    {
        _url = args[0] as string;
        ZLog.D(this, "onload order {0}", this._zOrder);
		
    }
	
	////implement WebSocketHandler
	 public void OnOpen(string url)
    {
        ZLog.I(this, "connect {0}", url);
        //_component._text.text = "connected "+ url;
		string req = _component.iptFight.text;
        _ws.SendText(req);
        //throw new Exception("handler null");
    }
	
	public bool OnTxtMsg(string text, int handle_count)
    {
        ZLog.I(this, "recv:{0}", text);
        return true;
    }
    public bool OnRawMsg(byte[] msg, int handle_count)
    {
        ZLog.I(this, "recv:{0}", msg);
        return true;
    }
           //void OnErr(ErrorEventArgs e)
   
    public void OnDisconnect(int reason, string str)
    {
        ZLog.E(this, "disconnect [{0}] {1}", reason, str);
    }

	public void OnErr(ErrorEventArgs e)
    {
        ZLog.E(this, "Err:[{0}] {1}", e.Message, e.Exception);
    }
	
	////////////////////////////////////////////////////////
}
