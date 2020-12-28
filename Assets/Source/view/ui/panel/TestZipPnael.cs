using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Zby;
using ICSharpCode.SharpZipLib.Zip;

public class TestZipPnael :  CnViewBase
{
    Panel_component  _component;
    // Start is called before the first frame update
    void Start()
    {
        _component._btn1.transform.Find("Text").GetComponent<Text>().text = "LOADZIP";
        SetClickEventOnce(_component._btn1, ClickBtnConnet, new object[] { "cnt" });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClickBtnConnet(object[] args)
    {
        //ZLog.D(this, "click connect args {0}", args[0]);
        string luazip = Application.streamingAssetsPath + "/core.zip";
        using (ZipFile zip = new ZipFile( luazip))
        {
        

        foreach (ZipEntry entry in zip)
        {
            //ZipFile zipFile = new ZipFile(zipFile.GetInputStream(entry));
            ZLog.D(this, entry.Name);
              using (var stream =  zip.GetInputStream(entry))
                    {
                        byte[] buffer = new byte[entry.Size];
                        stream.Read(buffer, 0, buffer.Length);
                        ZLog.D(this, "{0}",Encoding.UTF8.GetString(buffer));
                        break;
                       // return true;
                  }
        }

        }
    }
    
    public override CnUiComponent GetCnUiComponent()
    {
        if (null == _component)
        {
            _component = new Panel_component();
        }
        return _component;
    }
}
