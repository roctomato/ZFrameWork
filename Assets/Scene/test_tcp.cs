using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class test_tcp : MonoBehaviour {

    public string Host;
    public int Port;
	// Use this for initialization
	void Start () {
        MainEntryEvent.Instance.Start();
        CnUIMgr.Instance.Load<TestSocketPanel>("Panel", Host, Port);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
