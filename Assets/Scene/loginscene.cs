﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;
public class loginscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CnUIPathMgr.Instance.Load<TestPanel>("LoginGame", (float)10.0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
