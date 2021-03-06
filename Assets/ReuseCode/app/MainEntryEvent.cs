using UnityEngine;

namespace Zby
{
    public delegate void OnMainEntryEvent();
    public class MainEntryEvent
    {
        private static MainEntryEvent m_Instance;
        public static MainEntryEvent Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new MainEntryEvent();
                }
                return m_Instance;
            }
        }

        public event OnMainEntryEvent OnAwake;
        public event OnMainEntryEvent OnStart;
        public event OnMainEntryEvent OnQuit;

        private MonoBehaviour _appMb;
        public MonoBehaviour AppMb
        {
            get { return _appMb; }
        }
        public void SetMb(MonoBehaviour mb)
        {
            _appMb = mb;
        }

        LogFile _logFile = null;

        public void Start( bool hasLogFile=true)
        {
            if (hasLogFile)
            {
                _logFile = new LogFile();
            }
            GameObject go = new GameObject("MainEntry");
            go.AddComponent<MainEntry>();
        }

        public void TriggerAwake()
        {
            if(OnAwake != null )
                OnAwake();
        }

        public void TriggerStart()
        {
            if (OnStart != null)
                OnStart();
        }

        public void TriggerQuit()
        {
            if (OnQuit != null)
                OnQuit();

            if ( _logFile != null)
            {
                _logFile.Close();
            }
        }
    }
}