using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Zby;

public class TestFightPanel : CnViewBase
{
	public string _url;
	FightComponent _component;
	DownloadQueue _download;
	
    // Start is called before the first frame update
    void Start()
    {
		ZLog.D(this, "in start url {0}", _url);
       SetClickEventOnce(_component.btnSend, ClickCall, new object[] { "Call java" });
	   _download = this.gameObject.AddComponent<DownloadQueue>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnDownloadOk(WWW www, DownloadData urlData)
	{
		ZLog.D(this, "recv {0}", Encoding.UTF8.GetString(www.bytes));
	}
	void ClickCall(object[] args)
    {
		string req = _component.iptFight.text;
		_download.AddPostData(_url, Encoding.UTF8.GetBytes(req), OnDownloadOk);
        ZLog.D(this, "click button req {0}",req);
        
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
}
