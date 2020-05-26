using System;
using WebSocketSharp;

using UnityEngine.UI;
using Zby;


public class TestWebSocket : CnViewBase, WebSocketHandler
{
    Panel_component _component;
    ZbyWebSocket _ws;
    string _url;
    // Use this for initialization
    void Start () {
        ZLog.D(this, "start socket order {0}", this._zOrder);

        _component._btn1.transform.Find("Text").GetComponent<Text>().text = "connect";
        SetClickEventOnce(_component._btn1, ClickBtnConnet, new object[] { "cnt" });


        _component._btn2.transform.Find("Text").GetComponent<Text>().text = "disconnect";
        SetClickEventOnce(_component._btn2, ClickDisconnet, new object[] { "dis" });

        _component._text.text = "init";

        _ws = new ZbyWebSocket(this, this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ClickDisconnet(object[] args)
    {
        ZLog.D(this, "click connect args {0}", args[0]);
        _ws.Close();
    }
    void ClickBtnConnet(object[] args)
    {
        ZLog.D(this, "click connect args {0}", args[0]);
        _ws.Open(_url);//"ws://localhost:8000/ws"
    }
    
    public override CnUiComponent GetCnUiComponent()
    {
        if (null == _component)
        {
            _component = new Panel_component();
        }
        return _component;
    }

    public void OnOpen(string url)
    {
        ZLog.I(this, "connect {0}", url);
        _component._text.text = "connected "+ url;
        _ws.SendText("hello world");
        throw new Exception("handler null");
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

    public void OnErr(ErrorEventArgs e)
    {
        ZLog.E(this, "Err:[{0}] {1}", e.Message, e.Exception);
    }
    public void OnDisconnect(int reason, string str)
    {
        ZLog.E(this, "disconnect [{0}] {1}", reason, str);
    }

    public override void OnLoad(params object[] args)
    {
        ZLog.D(this, "onload order {0} url {1}", this._zOrder, args[0]);
        _url = args[0] as String;
        
    }
}
