using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Zby
{
    public class WrapperCnViewBase: CnPanelObj
    {
        CnViewBase  view;

        public WrapperCnViewBase(CnViewBase  view)
        {
            this.view = view;
        }
       
        public override void InitArgs(int zOrder, GameObject go, IViewMgr mgr, params object[] args)
        {
            this.view.Host = this;
            base.InitArgs(zOrder,go, mgr, args );
        }
        public override void OnLoad(params object[] args) { //创建时调用，会在start前调用
            this.view.OnLoad( args);
        } 
        public override bool OnUnload() { return  this.view.OnUnload(); } //移除前调用
        public override void OnBehind(CnPanelObj topview ) {  this.view.OnBehind(topview);} //从顶层移到后一层
        public override void OnTop(CnPanelObj topview) { this.view.OnTop(topview); }  //从后层变到顶层
        public override void ViewEvtHandler(object sender, string event_cate, int event_type,object param) { //事件侦听
            this.view.ViewEvtHandler( sender, event_cate, event_type, param );
        }
        public override CnUiComponent GetCnUiComponent() { return this.view.GetCnUiComponent(); }
    }
    public abstract class CnViewBase : MonoBehaviour
    {
        WrapperCnViewBase _hostObj;
        public WrapperCnViewBase Host{
            get{ return _hostObj;}
            set{ _hostObj = value; }
        }
        
        public GameObject UIObj { get { return _hostObj.UIObj; } }

        public int ZOrder { get { return _hostObj.ZOrder; } }

        public string GetName()
        {
            return _hostObj.GetName();
        }
        public void Hide(  )
        {
            this._hostObj.Hide();
        }
        public bool Show()
        {
            return _hostObj.Show();
        }

        public bool DoUnload()
        {
            return _hostObj.DoUnload();
        }
        public void RegisertEvt(string evt)
        {
            _hostObj.RegisertEvt(evt);
        }
       
        protected void SetClickEventOnce(Button btn, Action<object[]> method, object[] args)
        {
            _hostObj.SetClickEventOnce( btn, method, args);
        }
      
        /// ////////////////////////////////
        public virtual void OnLoad(params object[] args) { } //创建时调用，会在start前调用
        public virtual bool OnUnload() { return true; } //移除前调用
        public virtual void OnBehind(CnPanelObj topview ) { } //从顶层移到后一层
        public virtual void OnTop(CnPanelObj topview) { }  //从后层变到顶层
        public virtual void ViewEvtHandler(object sender, string event_cate, int event_type,object param) { } //事件侦听
        public virtual CnUiComponent GetCnUiComponent() { return null; }
    }

    public class UiEventListener : UnityEngine.EventSystems.EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;

        public static UiEventListener Get(GameObject go)
        {
            UiEventListener listener = go.GetComponent<UiEventListener>();
            if (listener == null) listener = go.AddComponent<UiEventListener>();
            return listener;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null) onClick(gameObject);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }
    }
}
