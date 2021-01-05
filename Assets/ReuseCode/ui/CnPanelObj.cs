using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Zby
{
    public abstract class CnUiComponent
    {
        protected Transform Trans;
        protected CnUiComponent()
        {
        }

        protected Transform GetChild(string path)
        {
            return Trans.Find(path);
        }

        protected T GetElement<T>(string path) where T : Component
        {
            var child = GetChild(path);
            if (child == null)
                return default(T);
            return child.GetComponent<T>();
        }

        protected Transform GetChild(Transform parent, string path)
        {
            return parent.Find(path);
        }

        protected T GetElement<T>(Transform parent, string path) where T : Component
        {
            var child = GetChild(parent, path);
            if (child == null)
                return default(T);
            return child.GetComponent<T>();
        }

        public bool InitUiComponent(Transform trans)
        {
            Trans = trans;
            return DoInit();
        }

        public virtual bool DoInit()
        {
            return false;
        } 
    }

    public interface IViewMgr
    {
         bool CanUnload(CnPanelObj view);
         bool DestoryView(CnPanelObj view);
         bool DoShow(CnPanelObj view);
         bool DoHide(CnPanelObj view);
    }

    public abstract class CnPanelObj
    {
        protected GameObject _uiObj;
        public GameObject UIObj { get { return _uiObj; } }

        protected int _zOrder; //在ui栈中的打开的顺序
        public int ZOrder { get { return _zOrder; } }

        protected IViewMgr _viewMgr;
        private HashSet<string> _evtSet;

        public virtual void InitArgs(int zOrder, GameObject go, IViewMgr mgr, params object[] args)
        {
            this._zOrder = zOrder;
            this._uiObj = go;
            this._viewMgr = mgr;
            this._evtSet = new HashSet<string>();
            
            try
            {
                CnUiComponent comp = this.GetCnUiComponent();
                if ( comp != null)
                {
                    bool ret = comp.InitUiComponent(this._uiObj.transform);
                    ZLog.I(this._uiObj, "ui {0}  init compent {1}", this.GetName(), ret ?"ok":"failed");
                }
                else
                {
                    ZLog.I(this._uiObj, "ui {0} no init compent", this.GetName());
                }
                this.OnLoad(args);
            }catch( Exception e)
            {
                ZLog.E(this._uiObj, " ui {0} OnLoad err {1}", this.GetName(), e.ToString());
            }
        }
        public string GetName()
        {
            return _uiObj.name;
        }
        public bool Hide(  )
        {
            return this._viewMgr.DoHide(this);
        }
        public bool Show()
        {
            return this._viewMgr.DoShow(this);
        }

       
        public void AddChild( Transform child, bool active)
        {
            child.SetParent( _uiObj.transform, active);
        }


        public bool DoUnload()
        {
            bool ret = false;
            do
            {
                if (!this._viewMgr.CanUnload(this))
                {
                    ZLog.E(this._uiObj, " ui {0} order {1} can not unload by frame", this.GetName(), this.ZOrder);
                    break;
                }

                if (!this.OnUnload())
                {
                    ZLog.E(this._uiObj, " ui {0} order {1} can not unload by view", this.GetName(), this.ZOrder);
                    break;
                }

                if (this._evtSet.Count > 0 )
                {
                    foreach( var evt_cate in _evtSet)
                    {
                        EventMgr.Instance.AddEvt(ViewEvtHandler, evt_cate, false);
                    }
                }
                this._viewMgr.DestoryView(this);
                ret = true;
            } while (false);
            
            return ret;
        }
        public void RegisertEvt(string evt)
        {
            do
            {
                if ( null == evt || evt.Length == 0)
                {
                    ZLog.E(null, "evt type err");
                    break;
                }

                if (_evtSet.Contains(evt))
                {
                    ZLog.E(null, "evt {0} already register", evt);
                    break;
                }
                EventMgr.Instance.AddEvt(ViewEvtHandler, evt, true);
                _evtSet.Add(evt);
            } while (false);
            
        }
       
        public void DoBring2Top(CnPanelObj view )
        {
            this.OnTop(view );
        }
    

        public void SetClickEventOnce(Button btn, Action<object[]> method, object[] args)
        {
            if (btn == null){
				ZLog.E(this._uiObj, "btn null");
                return;
			}
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                method(args);
            });
			ZLog.D(this._uiObj,"Add btn evt ok");
        }
        protected Transform GetChild(string childName)
        {
            return GetChild(this._uiObj.transform, childName);
        }

        protected Transform GetChild(Transform parent, string childName)
        {
            var res = parent.Find(childName);
            if (res == null)
                ZLog.E(this._uiObj, string.Format("{0}不存在", childName));
            return res;
        }

        protected T GetElement<T>(string childName) where T : Component
        {
            return GetElement<T>(this._uiObj.transform, childName);
        }

        protected T GetElement<T>(Transform parent, string childName) where T : Component
        {
            var child = GetChild(parent, childName);
            if (child)
            {
                var res = child.GetComponent<T>();
                if (res == null)
                {
                    ZLog.E(this._uiObj, string.Format("{1}不存在节点不存在{2}", childName, typeof(T).Name));
                }
                return res;
            }
            return null;
        }
        /// ////////////////////////////////
        public virtual void OnLoad(params object[] args) { } //创建时调用，会在start前调用
        public virtual bool OnUnload() { return true; } //移除前调用
        public virtual void OnBehind(CnPanelObj topview ) { } //从顶层移到后一层
        public virtual void OnTop(CnPanelObj topview) { }  //从后层变到顶层
        public virtual void ViewEvtHandler(object sender, string event_cate, int event_type,object param) { } //事件侦听
        public virtual CnUiComponent GetCnUiComponent() { return null; }
    }
}