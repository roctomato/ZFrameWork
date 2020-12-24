using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zby;
using duoli;

public class CheckPathPanel : CnViewBase
{
    FightComponent _component;

    void Awake()
    {
         ZLog.D(this,"awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        ZLog.D(this,"start");
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("absoluteURL:{0}\n", Application.absoluteURL);
        sb.AppendFormat("dataPath:{0}\n", Application.dataPath);
        sb.AppendFormat("consoleLogPath:{0}\n", Application.consoleLogPath);
        sb.AppendFormat("persistentDataPath:{0}\n", Application.persistentDataPath);
        sb.AppendFormat("streamingAssetsPath:{0}\n", Application.streamingAssetsPath);
        sb.AppendFormat("temporaryCachePath:{0}\n", Application.temporaryCachePath);
     

        _component.iptFight.text = sb.ToString();
        Text txt = _component.btnSend.GetComponentInChildren<Text>();
        txt.text = "加载配置";
         SetClickEventOnce(_component.btnSend, ClickCall, null);
        //_component.btnSend.Text = "hello";
    }

    // Update is called once per frame
    void Update()
    {
        
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

        ZLog.D(this, "onload order {0}", this._zOrder);
		
    }
    void ClickCall(object[] args)
    {
		BinDataMgr mgr = new BinDataMgr();
        bool ret = mgr.LoadData( Application.dataPath+"/config_data/");
        Debug.Log("load ok");
        Debug.Log(mgr._languageMgr.FindData("UI_ReportTool_Calendar").Content);

        ret = mgr.LoadCsvData(Application.dataPath+"/config_data/");

        languageMgr dmgr = mgr._languageMgr;
        //ret = dmgr.LoadDefaultCsv( Application.dataPath+"/config_data/");
        Debug.Log(ret);
        Debug.Log(dmgr.FindData("UI_ReportTool_Calendar").Content);
    }
}
