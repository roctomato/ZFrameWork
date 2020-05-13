using System;
using System.Collections.Generic;

namespace Zby
{
    public delegate void Evthandler(object sender, string event_cate, int event_type, object param);
    public class EventBase
    {
        public EventBase(string name)
        {
            _name = name;
        }
        public string Name
        {
            get { return _name; }
        }

        public void AddEvt(Evthandler evt, bool add)
        {
            if (add)
            {
                _evtHandler += evt;
            }
            else
            {
                _evtHandler -= evt;
            }
        }

        public void TriggerEvt(object sender, int event_type, object param)
        {
            if (_evtHandler != null)
                _evtHandler(sender, _name, event_type, param);
            else
                ZLog.E(null, "event {0} no handler", event_type);
        }

        private event Evthandler _evtHandler;
        private String _name;
    }
    public class EventMgr
    {
 
        private static EventMgr m_Instance;
        public static EventMgr Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new EventMgr();
                }
                return m_Instance;
            }
        }

        Dictionary<string, EventBase> _dictEvent;
        public EventMgr()
        {
            _dictEvent = new Dictionary<string, EventBase>();
        }

        EventBase GetEventBase(string cate)
        {
            EventBase eb = null;
            if (_dictEvent.ContainsKey(cate))
            {
                eb = _dictEvent[cate];
            }
            else
            {
                eb = new EventBase(cate);
                _dictEvent.Add(cate, eb);
            }
            return eb;
        }
        public void AddEvt(Evthandler evt, string cate, bool add)
        {
            GetEventBase(cate).AddEvt(evt, add);
        }

        
        public void TriggerEvt(object sender, string cate, int event_type, object param)
        {
            GetEventBase(cate).TriggerEvt(sender, event_type, param);
        }
    }
}
