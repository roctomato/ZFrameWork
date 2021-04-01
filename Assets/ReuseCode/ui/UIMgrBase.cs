using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


using UnityEngine;
using UnityEngine.Events;

namespace Zby
{
  
    public class UIMgrBase : IViewMgr
    {
        protected GameObject _root;
        protected List<CnPanelObj> _viewStack;
        protected HashSet<CnPanelObj> _initViewSet;

        private int _viewID;

        public UIMgrBase( GameObject root)
        {
            _root = root;
            _viewStack = new List<CnPanelObj>();
            _initViewSet = new HashSet<CnPanelObj>();
            _viewID = 0;
        }
        public void UnloadAll()
        {
            foreach (CnPanelObj p in _viewStack) 
            { 
            　　 if  (p != null){
                    GameObject.Destroy(p.UIObj);
                }
            } 
            this._viewStack.Clear();

            foreach (CnPanelObj p in _initViewSet) 
            { 
            　　 if  (p != null){
                    GameObject.Destroy(p.UIObj);
                }
            } 
            this._initViewSet.Clear();

        }
       
        public CnPanelObj GetTopView()
        {
            CnPanelObj ret = null;
            if (this._viewStack.Count > 0)
            {
                ret = this._viewStack.Last();
            }
            return ret;
        }
        
        GameObject InitRes(string name )
        {
            return InitResEx(name, _root.transform);
        }
        public GameObject InitResEx(string name, Transform parent)
        {
            GameObject ins = null;
            do
            {
                GameObject go = this.FindUIRes(name);
                if (null == go)
                {
                    ZLog.E(null, "no ui {0}", name);
                    break;
                }
                ins = GameObject.Instantiate(go) as GameObject;
                if (ins == null)
                {
                    ZLog.E(null, "{0} Instantiate fail", name);
                    break;
                }
                string[] strList = name.Split(new Char[] { '/','\\' }, StringSplitOptions.RemoveEmptyEntries);
                string new_name = strList[strList.Length - 1];
                ins.transform.SetParent(parent, false);
                ins.transform.localPosition = Vector3.zero;
                ins.transform.localScale = Vector3.one;
                ins.transform.localRotation = Quaternion.identity;
                ins.name = new_name;
                ZLog.I(null, "Load {0} ok", name);
            } while (false);
            return ins;
        }
        public  T AttachPanelEx<T>(string path, bool show, params object[] args) where T : CnViewBase
        {
            T view = null;
            do
            {
                Transform trans = this._root.transform.Find(path);
                if ( null == trans){
                    break;
                }
                GameObject ins =  trans.gameObject;
                if ( null == ins ){
                    break;
                }
                
                view = ins.AddComponent<T>();
                if (view != null)
                {
                    WrapperCnViewBase panelObj =  new WrapperCnViewBase(view);
                    if (panelObj != null){
                        InitNewPanel(panelObj, ins, show, args );
                    }
                }
            } while (false);
            return view;
        }
        public T  AttachPanel<T>(string path, bool show, params object[] args) where T : CnPanelObj, new()
        {
            T view =null;// 
            do
            {
                Transform trans = this._root.transform.Find(path);
                if ( null == trans){
                    break;
                }
                GameObject ins =  trans.gameObject;
                if ( null == ins ){
                    break;
                }
                
                view =  new T();
                if (view != null)
                {
                    InitNewPanel(view, ins, show, args );
                }
            } while (false);
            return view;
        }
        public void InitNewPanel( CnPanelObj view,  GameObject ins, bool show, params object[] args )
        {
            view.InitArgs(_viewID, ins, this, args);
            _viewID++;
            if (show)
            {
                DoShow(view);
            }
            else
            {
                DoHide(view);
            }
        }
        protected T LoadPanel<T>(string name, bool show, params object[] args) where T : CnPanelObj, new()
        {
            T view =null;// 
            do
            {
                GameObject ins = InitRes(name);
                if ( null == ins ){
                    break;
                }
                
                view =  new T();
                if (view != null)
                {
                    InitNewPanel(view, ins, show, args );
                }
            } while (false);
            return view;
        }
        //初始化后，并不显示
        public T InitPanel<T>(string name, params object[] args) where T : CnPanelObj, new()
        {
            return LoadPanel<T>(name, false, args);
        }
        //初始化后，显示
        public T LoadPanelShow<T>(string name, params object[] args) where T : CnPanelObj, new()
        {
            return LoadPanel<T>(name, true, args);
        }

        protected T Load<T>(string name, bool show, params object[] args) where T : CnViewBase
        {
            T view = null;
            do
            {
                GameObject ins = InitRes(name);
                if ( null == ins ){
                    break;
                }
                
                view = ins.AddComponent<T>();
                if (view != null)
                {
                    WrapperCnViewBase panelObj =  new WrapperCnViewBase(view);
                    if (panelObj != null){
                        InitNewPanel(panelObj, ins, show, args );
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
        public bool IsTop(CnPanelObj view)
        {
            return view == this.GetTopView();
        }

        public virtual GameObject FindUIRes(string name)
        {
            return null;
        }
        //implement IViewMgr
        public bool DestoryView(CnPanelObj view)
        {
            bool isTop = this.IsTop(view);

            this._viewStack.Remove(view);
          
            if (isTop)
            {
                CnPanelObj next = this.GetTopView();
                if (next != null)
                    next.DoBring2Top(view);
            }
            GameObject.Destroy(view.UIObj);
            return true;
        }
        public bool CanUnload(CnPanelObj view)
        {
            if ( this._viewStack.Contains(view) )
            {
                return true;
            }
            else
            {
                ZLog.E(null, "{0} no view {1}_{2}", this._root.name, view.GetName(), view.ZOrder);
            }
            return false;// this.IsTop(view);
        }
        public bool DoShow(CnPanelObj view)
        {
            bool ret = false;
            do{
                CnPanelObj last_view = this.GetTopView();
                this._viewStack.Add(view);
                if (last_view != null)
                {
                    last_view.OnBehind(view);
                }
                 
                if ( this._initViewSet.Contains( view) )
                {
                    this._initViewSet.Remove(view);
                    //ZLog.E(null, "{0} no init view {1}_{2}", this._thisCanvas.name, view.GetName(), view.ZOrder);
                    //break;
                }
                view.UIObj.SetActive(  true );
                ret = true;
            }while(false);
            return ret;
        }

        public bool DoHide(CnPanelObj view)
        {
            bool ret = false;

            if ( this._viewStack.Contains(view) ){
                bool isTop = this.IsTop(view);
                this._viewStack.Remove(view);
                if (isTop)
                {
                    CnPanelObj next = this.GetTopView();
                    if (next != null)
                        next.DoBring2Top(view);
                }
            }
            this._initViewSet.Add(view);
            view.UIObj.SetActive(  false );
            ret = true;
            return ret;
        }

       
        ///////////////////////////////////////////

    }
}
