using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class test_sdk : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MainEntryEvent.Instance.Start();
        CnUIMgr.Instance.Load<TestSdkPanel>("SdkPanel");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
