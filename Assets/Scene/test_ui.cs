using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class test_ui : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CheckSdkPanel();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CheckSdkPanel()
    {
        Transform trans = this.transform.Find("SdkPanel");
        TestSdk_component tc = new TestSdk_component();
        bool  ret = tc.InitUiComponent(trans);
        ZLog.I(this, "check SdkPanel result {0}", ret);
    }
}
