using System.Collections.Generic;
using System.Linq;


using UnityEngine;

namespace Zby
{
    public class UIMgrBase : BaseCanvas, IViewMgr
    {
        protected List<CnViewBase> _viewStack;
        protected HashSet<CnViewBase> _initViewSet;

        private int _viewID;

        public UIMgrBase( string name):base(name)
        {
            _viewStack = new List<CnViewBase>();
            _initViewSet = new HashSet<CnViewBase>();
            _viewID = 0;
        }

        public CnViewBase GetTopView()
        {
            CnViewBase ret = null;
            if (this._viewStack.Count > 0)
            {
                ret = this._viewStack.Last();
            }
            return ret;
        }
        public T Load<T>(string name, bool show, params object[] args) where T : CnViewBase
        {
            T view = null;
            do
            {
                GameObject go = this.FindUIRes(name);
                if (null == go)
                {
                    ZLog.E(null, "no ui {0}", name);
                    break;
                }
                GameObject ins = GameObject.Instantiate(go) as GameObject;
                if (ins == null)
                {
                    ZLog.E(null, "{0} Instantiate fail", name);
                    break;
                }
                
                ins.transform.SetParent(this._canvasHost.transform, false);
                ins.transform.localPosition = Vector3.zero;
                ins.transform.localScale = Vector3.one;
                ins.transform.localRotation = Quaternion.identity;
                ins.name = name;
                ZLog.I(null, "Load{0} ok", name);
                view = ins.AddComponent<T>();
                if (view)
                {
                    view.InitArgs(_viewID, ins, this, args);
                    _viewID++;
                    if (show)
                    {
                        CnViewBase last_view = this.GetTopView();
                        this._viewStack.Add(view);
                        if (last_view)
                        {
                            last_view.OnBehind(view);
                        }
                    }
                    else
                    {
                        this._initViewSet.Add(view);
                        view.Hide();
                    }
                }
            } while (false);
            return view;

        }
        /// <summary>
        /// 初始化后，并不显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public T InitView<T>(string name, params object[] args) where T : CnViewBase
        {
            return Load<T>(name, false, args);
        }
        public T Load<T>(string name, params object[] args) where T : CnViewBase
        {
            return Load<T>(name, true, args);
          
        }
        public bool IsTop(CnViewBase view)
        {
            return view == this.GetTopView();
        }

        public virtual GameObject FindUIRes(string name)
        {
            return null;
        }
        //implement IViewMgr
        public bool DestoryView(CnViewBase view)
        {
            bool isTop = this.IsTop(view);

            this._viewStack.Remove(view);
          
            if (isTop)
            {
                CnViewBase next = this.GetTopView();
                if (next)
                    next.DoBring2Top(view);
            }
            GameObject.DestroyImmediate(view.UIObj);
            return true;
        }
        public bool CanUnload(CnViewBase view)
        {
            if ( this._viewStack.Contains(view) )
            {
                return true;
            }
            else
            {
                ZLog.E(null, "{0} no view {1}_{2}", this.GetCanvasName(), view.GetName(), view.ZOrder);
            }
            return false;// this.IsTop(view);
        }
        public bool DoShow(CnViewBase view)
        {
            bool ret = false;
            do{
                if ( !this._initViewSet.Contains( view) )
                {
                    ZLog.E(null, "{0} no init view {1}_{2}", this.GetCanvasName(), view.GetName(), view.ZOrder);
                    break;
                }
                CnViewBase last_view = this.GetTopView();
                this._viewStack.Add(view);
                if (last_view)
                {
                    last_view.OnBehind(view);
                }
                this._initViewSet.Remove(view);
                ret = true;
            }while(false);
            return ret;
        }
        ///////////////////////////////////////////

    }
}
