using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zby
{
    public delegate UnityEngine.Object LoadRes(string name);

    public class TmplLoaderBase
    {
        Dictionary<string, UnityEngine.Object> _dictTmpl;
        LoadRes _loadFunc;
        public LoadRes LoadFunc
        {
            set { _loadFunc = value; }
        }

        public string _loaderName;
        public string LoaderName
        {
            get { return _loaderName; }
        }
        public TmplLoaderBase( string name)
        {
            _dictTmpl = new Dictionary<string, UnityEngine.Object>();
            _loaderName = name;
            _loadFunc = null;
        }

        public TmplLoaderBase(string[] namelist)
        {
            _dictTmpl = new Dictionary<string, UnityEngine.Object>();
            this.LoadTmpl(namelist);
        }

        public int LoadTmpl( string[] namelist)
        {
            for (int i = 0; i < namelist.Length; i++)
            {
                this.LoadTmpl(namelist[i]);
            }
            return _dictTmpl.Count;
        }
        public UnityEngine.Object  LoadTmpl( string name )
        {
            string[] strList = name.Split(new Char[] { '/',  }, StringSplitOptions.RemoveEmptyEntries);
            string index = strList[strList.Length - 1];
            UnityEngine.Object obj =  LoadResource.Load( name);
            _dictTmpl[index] = obj;
            return obj;
        }
        public void AddTmpl( string name, UnityEngine.Object obj )
        {
            string[] strList = name.Split(new Char[] { '/', }, StringSplitOptions.RemoveEmptyEntries);
            string index = strList[strList.Length - 1];
            _dictTmpl[index] = obj;
        }
        public GameObject FindTmpl(string name)
        {
            if (this._dictTmpl.ContainsKey(name))
            {
                return this._dictTmpl[name] as GameObject;
            }
            UnityEngine.Object obj = null;
            if (null != this._loadFunc)
            {
                obj = this._loadFunc(name);
            }
            AddTmpl( name, obj );
            return obj as GameObject;
        }
        public GameObject Instantiate( string res_name, Transform parent, bool active=false)
        {
            GameObject go = null;
            do
            {
                GameObject tmpl = this.FindTmpl(res_name);
                if (null == tmpl)
                {
                    ZLog.E(null, "loader, {0} res {1} not found", _loaderName, res_name);
                    break;
                }

                go = GameObject.Instantiate(tmpl) as GameObject;
                go.name = tmpl.name;
                go.transform.SetParent(parent, active);
            } while (false);
            return go;
        }
        public int GetCount()
        {
            return this._dictTmpl.Count;
        }
    }




    public class LoadResMgrBase
    {
        Dictionary<string, TmplLoaderBase> _dictRes;
     

        public LoadResMgrBase()
        {
            _dictRes = new Dictionary<string, TmplLoaderBase>();
        }
        public int GetCount()
        {
            return this._dictRes.Count;
        }

        public TmplLoaderBase AddLoaderFunc(string name, LoadRes fnc)
        {
            TmplLoaderBase loader = this.ConfirmLoadRes(name);
            loader.LoadFunc = fnc;
            return loader;
        }
        public TmplLoaderBase ConfirmLoadRes(string name)
        {
            TmplLoaderBase loader = null;
            if (this._dictRes.ContainsKey(name))
            {
                loader = this._dictRes[name];
            }
            else
            {
                loader = new TmplLoaderBase(name);
                this._dictRes[name] = loader;
            }
            return loader;
        }
        public TmplLoaderBase LoadRes(string name, string[] resList)
        {
            TmplLoaderBase loader = this.ConfirmLoadRes(name);
            loader.LoadTmpl(resList);
            return loader;
        }
        public TmplLoaderBase FindResLoader( string name )
        {
            TmplLoaderBase ret = null;
            if (this._dictRes.ContainsKey(name))
            {
                ret = this._dictRes[name];
            }
            return ret;
        }
        public GameObject FindTmpl(string loader_name, string res_name)
        {
            GameObject go = null;
            do{
                TmplLoaderBase loader = this.FindResLoader(loader_name);
                if (null == loader)
                {
                    ZLog.E(null, "loader {0} not found", loader_name);
                    break;
                }
                go = loader.FindTmpl(res_name);
            }while(false);
            return go;
        }

        public GameObject Instantiate(string loader_name, string res_name, Transform parent, bool active)
        {
            GameObject go = null;
            do
            {
                TmplLoaderBase loader = this.FindResLoader(loader_name);
                if (null == loader)
                {
                    ZLog.E(null, "loader {0} not found", loader_name);
                    break;
                }
                go = loader.Instantiate(res_name, parent, active);
            } while (false);
            return go;
        }
    }
}
