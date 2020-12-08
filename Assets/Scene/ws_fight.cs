using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class ws_fight : MonoBehaviour
{
   public string URL; 
    // Start is called before the first frame update
    void Start()
    {
        CnUIMgr.Instance.Load<WsFightPanel>("FightPanel", URL);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
