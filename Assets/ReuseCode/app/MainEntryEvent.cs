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
        }
    }
}