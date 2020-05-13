using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace Zby
{
    public class BaseCanvas 
    {
        protected Canvas     _curCanvas;
        protected GameObject _canvasHost;
        protected CanvasScaler _curCanvasScaler;
        protected GraphicRaycaster _curGraphicRaycaster;

        protected EventSystem           _eventSystem;
        protected StandaloneInputModule _InputMd;

        public Canvas Main
        {
            get { return _curCanvas; }
        }
     

        void InitEventSystemByCode(Transform parent )
        {
            GameObject EventSystem = new GameObject("EventSystem"); 
            
            _eventSystem = EventSystem.AddComponent<EventSystem>();
            _InputMd = EventSystem.AddComponent<StandaloneInputModule>();

            EventSystem.transform.SetParent(parent, true);
        }

        void InitCanvas(string name, Transform parent)
        {
            _canvasHost = new GameObject(name);
          
            _curCanvas = _canvasHost.AddComponent<Canvas>();
            _curCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _curCanvasScaler=   _canvasHost.AddComponent<CanvasScaler>();
            _curCanvasScaler.scaleFactor = 1;
            _curGraphicRaycaster=  _canvasHost.AddComponent<GraphicRaycaster>();
            _curGraphicRaycaster.ignoreReversedGraphics = true;
            _curCanvas.sortingOrder = 0;
            _canvasHost.transform.SetParent(parent, true);
            _canvasHost.layer = 5;// ui layer
        }

        public BaseCanvas(string name, Transform parent = null)
        {
            this.InitEventSystemByCode(parent);
            this.InitCanvas( name, parent);
        }

        public string GetCanvasName()
        {
            return _canvasHost.name;
        }
    }
}
