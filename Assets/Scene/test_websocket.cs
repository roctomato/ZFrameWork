using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class test_websocket : MonoBehaviour {

    public string URL; 
	// Use this for initialization
	void Start () {
        CnUIMgr.Instance.Load<TestWebSocket>("Panel", URL);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
