using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zby;

public class check_path : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
          CnUIMgr.Instance.Load<CheckPathPanel>("FightPanel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
