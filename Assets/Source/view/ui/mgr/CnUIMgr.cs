using UnityEngine;

namespace Zby
{
    public class CnUIMgr : UIMgrBase
    {
        private static CnUIMgr m_Instance;
        public static CnUIMgr Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CnUIMgr();
                }
                return m_Instance;
            }
        }

        TmplLoaderBase _resLoader;

        public CnUIMgr( ):base("myui")
        {
            _resLoader = new TmplLoaderBase("ui");
            _resLoader.LoadFunc = LoadRes;
        }

        UnityEngine.Object LoadRes(string name)
        {
            return Resources.Load("ui/" + name);
        }

        public override GameObject FindUIRes(string name)
        {
            return _resLoader.FindTmpl(name);
        }
        ///////////////
    }
}
