using Zby;

using UnityEngine;
using UnityEngine.UI;

public  class Main
{
   
    public static void Awake()
    {
        ZLog.I(null, "in Main Awake");
        CnUIMgr.Instance.Load<TestSocketPanel>("Panel");

        //CnUIMgr.Instance.Load<TestPanel>("Panel", 5);

       

    }
    
}