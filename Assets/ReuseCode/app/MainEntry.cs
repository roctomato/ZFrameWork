using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Zby
{
    public class MainEntry : MonoBehaviour
    {
        //config
        public bool     _hasLogFile ;
        public string   _startClass ;

        MainEntryEvent  _mainEntryEvent;
        LogFile         _logFile;

        void InvokeAwake()
        {
            if ( _startClass == null || "" == _startClass)
            {
                _startClass = "Main";
            }

            try
            {
                Assembly currentAssem = Assembly.GetExecutingAssembly();
                Type type = currentAssem.GetType(_startClass);
                type.InvokeMember("Awake",
                    System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public
                    , null, null, null);
                ZLog.I(this, "Invoke {0}.Awake ok", _startClass);
            }
            catch (Exception e)
            {
                ZLog.E(this, "Invoke {0}.Awake failed, reason {1}", _startClass, e.ToString());
            }
        }

        void Awake()
        {
            _mainEntryEvent = MainEntryEvent.Instance;
            _mainEntryEvent.SetMb(this);

            if (_hasLogFile)
            {
                _logFile = new LogFile();
            }

            ZLog.I(this, "OnAwake");

            InvokeAwake();

            _mainEntryEvent.TriggerAwake();
        }

        // Use this for initialization
        void Start()
        {
            ZLog.I(this, "OnStart");
            _mainEntryEvent.TriggerStart();
        }

        void OnApplicationQuit()
        {
            ZLog.I(this, "OnQuit");
            _mainEntryEvent.TriggerQuit();
            if ( _hasLogFile && _logFile != null)
            {
                _logFile.Close();
            }
        }
    }

}