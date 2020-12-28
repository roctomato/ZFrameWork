using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using Zby;
public class test_zip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CnUIMgr.Instance.Load<TestZipPnael>("Panel");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
