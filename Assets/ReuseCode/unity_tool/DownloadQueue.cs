using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public delegate void OnDownloadOk(WWW www, DownloadData urlData);
public class DownloadData
{
    public enum DownloadType
    {
        DownloadType_Get_URL   = 0,
        DownloadType_Post_Data = 1,
        DownloadType_Post_Form = 2,
        DownloadType_and_Cache = 3,
    }
    public DownloadData(DownloadType dtype)
    {
        _type = dtype;
    }

    public DownloadType _type;
    public string _url;
    public OnDownloadOk _cb;
    public int _timeout; //超时时间 -1，一直等待
    public int _version;
    public object _data;
    public object _userData;

    public bool _isStart;
    public WWW _curWww;
    public DateTime _startTime;  //开始下载时间
    

    public int PassTime(DateTime now)
    {
        return (int)(now.Ticks - _startTime.Ticks);
    }
    public bool IsTimeout(DateTime now)
    {
        if (  _timeout <= 0 )
            return false;
        int pass = PassTime (now) / 10000000;
        //Debug.Log( string.Format("{0} {1}", pass, _timeout));
        return pass >= _timeout;
    }

    public WWW CreateWWW()
    {
        switch (_type)
        {
            case DownloadType.DownloadType_Get_URL:
                _curWww = new WWW(_url);
                break;
            case DownloadType.DownloadType_Post_Data:
                byte[] postData = this._data as byte[];
                _curWww = new WWW(_url, postData);
                break;
            case DownloadType.DownloadType_Post_Form:
                WWWForm form = this._data as WWWForm;
                _curWww = new WWW(_url, form);
                break;
            case DownloadType.DownloadType_and_Cache:
                _curWww = WWW.LoadFromCacheOrDownload(_url, _version);
                break;          
        }

        return _curWww;
    }
}

public class DownloadQueue : MonoBehaviour {


    private Queue<DownloadData> _taskQueue;
    private HashSet<DownloadData> _doingSet;

    private int  _curStart = 0;
    private int _maxQueue = 5;
    public int MaxQueue
    {
        get { return _maxQueue; }
        set { 
            if (value < 1) 
                return;
            _maxQueue = value;
        }
    }

    bool HasEmptyQueue()
    {
        return _curStart < _maxQueue;
    }
	// Use this for initialization
	void Start () {
        _taskQueue = new Queue<DownloadData>();
        _doingSet = new HashSet<DownloadData>();
        _curStart = 0;
        InvokeRepeating("CheckTimeout", 1, 2);
        Debug.Log("download queue start");
	}

    void CheckTimeout()
    {
        DateTime now = System.DateTime.Now;
        foreach (DownloadData item in _doingSet)
        {
            if (item.IsTimeout(now))
            {
                item._curWww.Dispose();
            }
        }
    }
	
    ///////////////////////////////////////////////////////////////////////////
    IEnumerator DownloadCortine()
    {
        _curStart  += 1;
        while (this._taskQueue.Count > 0)
        {
            DownloadData task = this._taskQueue.Dequeue();
            if (task != null && !task._isStart)
            {
                task._isStart = true;
                task.CreateWWW();
                task._startTime = System.DateTime.Now;
                _doingSet.Add(task);
                yield return task._curWww;
                task._cb(task._curWww, task);
                _doingSet.Remove(task);
            }
        }
        _curStart -= 1;
        //return null;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="version"></param>
    /// <param name="cb"></param>
    public DownloadData AddDownLoadCache(string url, int version, OnDownloadOk cb, int timeout = -1, object userData =null)
    {
        DownloadData data = new DownloadData(DownloadData.DownloadType.DownloadType_and_Cache);
        data._url = url;
        data._version = version;
        data._cb = cb;
        data._timeout = timeout;
        data._userData = userData;
        AddTask(data);
        return data;
    }
    public DownloadData AddDownloadUrl(string url,  OnDownloadOk cb, int timeout = -1, object userData =null)
    {
        DownloadData data = new DownloadData(DownloadData.DownloadType.DownloadType_Get_URL);
        data._url = url;
        data._cb = cb;
        data._timeout = timeout;
        data._userData = userData;
        AddTask(data);
        return data;
    }
    public DownloadData AddPostData(string url, byte[] postData, OnDownloadOk cb, int timeout = -1, object userData = null)
    {
        DownloadData data = new DownloadData(DownloadData.DownloadType.DownloadType_Post_Data);
        data._url = url;
        data._cb = cb;
        data._data = postData;
        data._timeout = timeout;
        data._userData = userData;
        AddTask(data);
        return data;
    }

    public DownloadData AddPostForm(string url, WWWForm postData, OnDownloadOk cb, int timeout = -1, object userData = null)
    {
        DownloadData data = new DownloadData(DownloadData.DownloadType.DownloadType_Post_Form);
        data._url = url;
        data._cb = cb;
        data._data = postData;
        data._timeout = timeout;
        data._userData = userData;
        AddTask(data);
        return data;
    }
    void AddTask(DownloadData data)
    {
        data._isStart = false;
        _taskQueue.Enqueue(data);
        if (this.HasEmptyQueue())
        {
            StartCoroutine("DownloadCortine");
        }
    }
}
