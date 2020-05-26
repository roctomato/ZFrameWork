using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using WebSocketSharp;

namespace Zby
{
    public interface WebSocketHandler
    {
        void OnOpen(string url);
        bool OnTxtMsg(string text, int handle_count);
        bool OnRawMsg(byte[] msg, int handle_count);

        void OnErr(ErrorEventArgs e);
        void OnDisconnect(int reason, string str); //断开连接回调
    }

    class WsMsg
    {
        public readonly byte[] _rawMsg;
        public readonly string _txtMsg;
        public readonly bool _isRaw;

        public WsMsg(byte[] msg)
        {
            _isRaw = true;
            _rawMsg = msg;
        }

        public WsMsg(string msg)
        {
            _isRaw = false;
            _txtMsg = msg;
        }
    }
    public class ZbyWebSocket
    {
        private MonoBehaviour _mb;
        private WebSocketHandler _handler;

        private WebSocket _webSocket;
        private NetWorkState netWorkState = NetWorkState.CLOSED;   //current network state

        ErrorEventArgs _errEvt = null;

        private int _disReason;
        private string _err;

        private string _url;

        bool _isSending = false;  //正在发送中	
        Queue<WsMsg> _sendQueue; //消息队列
        Queue<WsMsg> _resvMsgQueue; //消息队列

        public ZbyWebSocket(WebSocketHandler handler, MonoBehaviour mb)
        {
            Init(handler, mb);
            InitQueue();
        }

        public ZbyWebSocket()
        {
            InitQueue();
        }

        public void Init(WebSocketHandler handler, MonoBehaviour mb)
        {
            _handler = handler;
            _mb = mb;
        }

        void InitQueue()
        {
            _isSending = false;
            _sendQueue = new Queue<WsMsg>();
            _resvMsgQueue = new Queue<WsMsg>();
        }
        public NetWorkState GetState()
        {
            return netWorkState;
        }
        public void Open(string url)
        {
            _url = url;
            ZLog.D(null, "start connect {0} ", _url);
            Open();
        }
        public void Open()
        {
            if (_handler == null ||  _mb == null)
            {
                netWorkState = NetWorkState.CLOSED;
                throw new Exception("handler null");
            }

            if (netWorkState != NetWorkState.CLOSED)
            {
                throw new Exception("state err" + netWorkState.ToString());
            }

            netWorkState = NetWorkState.CONNECTING;
            _mb.StartCoroutine(_AutoUpdate());
            this._webSocket = new WebSocket(_url);
            RegisterEvt(true);

            _resvMsgQueue.Clear();
            _sendQueue.Clear();
            this._webSocket.ConnectAsync();
        }
        public void Close()
        {
            _webSocket.CloseAsync();
        }
        public bool SendText(string msg)
        {
            return this.AddSendMsg(new WsMsg(msg));
        }
        public bool SendRaw(byte[] msg)
        {
            return this.AddSendMsg(new WsMsg(msg));
        }
        public bool CanSend()
        {
            return this.netWorkState == NetWorkState.Running;
        }
        void OnSendComplete(bool completed)
        {
            if (this.netWorkState != NetWorkState.Running)
            {
                this._isSending = false;
                return;
            }

            if ( !completed )
            {
                return;
            }

            lock (this._sendQueue)
            {
                if (this._sendQueue.Count == 0)
                {
                    this._isSending = false;
                }
                else
                {
                    WsMsg thisSend = this._sendQueue.Dequeue();
                    DoSendMsgAsync(thisSend);
                }
            }
        }
        void DoSendMsgAsync( WsMsg thisSend)
        {
            if (thisSend != null)
            {
                this._isSending = true;
                if (thisSend._isRaw)
                {
                    _webSocket.SendAsync(thisSend._rawMsg, OnSendComplete);
                }
                else
                {
                    _webSocket.SendAsync(thisSend._txtMsg, OnSendComplete);
                }
            }
        }
        bool AddSendMsg( WsMsg msg)
        {
            bool ret = false;
            WsMsg thisSend = null;
            do
            {
                if (!this.CanSend())
                {
                    break;
                }

                lock (_sendQueue)
                {
                    //发送队列为空，且当前不在发送过程中
                    if (this._sendQueue.Count == 0 && !this._isSending)
                    {
                        thisSend = msg;
                    }
                    else
                    {
                        this._sendQueue.Enqueue(msg);
                        if (!this._isSending) // 当前没在发送则开始发送
                        {
                            thisSend = this._sendQueue.Dequeue();
                        }
                    }

                    DoSendMsgAsync(thisSend);
                }
                ret = true;
            } while (false);
            return ret;
        }
        void RegisterEvt( bool enabled)
        {
            if (enabled)
            {
                _webSocket.OnOpen += OnOpen;
                _webSocket.OnMessage += OnMessage;
                _webSocket.OnError += OnError;
                _webSocket.OnClose += OnClose;
            }
            else
            {
                _webSocket.OnOpen -= OnOpen;
                _webSocket.OnMessage -= OnMessage;
                _webSocket.OnError -= OnError;
                _webSocket.OnClose -= OnClose;
            }
        }

        void OnClose(object sender, EventArgs e)
        {
            CloseEventArgs ce = e as CloseEventArgs;
            netWorkState = NetWorkState.CLOSING;
            _disReason = ce.Code;
            _err = ce.Reason;

        }
        void OnError(object sender, EventArgs e)
        {
            _errEvt = e as ErrorEventArgs;
        }
        void OnOpen(object sender, EventArgs e)
        {
            netWorkState = NetWorkState.CONNECTED;
        }
        void OnMessage(object sender, EventArgs e)
        {
            MessageEventArgs me = e as MessageEventArgs;
            do
            {
                if (me.IsText)
                {
                    lock (_resvMsgQueue)
                    {
                        this._resvMsgQueue.Enqueue(new WsMsg(me.Data));
                    }
                    break;
                }

                if (me.IsBinary)
                {
                    lock (_resvMsgQueue)
                    {
                        this._resvMsgQueue.Enqueue(new WsMsg(me.RawData));
                    }
                    break;
                }

                ZLog.E(_mb,"unkonwn msg {0}", me.ToString());
            } while (false);
        }
        void CheckErr()
        {
            if (_errEvt != null )
            {
                _handler.OnErr(_errEvt);
                _errEvt = null;
            }
        }
        private IEnumerator _AutoUpdate()
        {
            bool running = true;
            while (running)
            {
                try
                {
                    running = Update();
                }catch(Exception e)
                {
                    ZLog.E(_mb,"{0}", e);
                }
                yield return 1;
            }
            ZLog.I(_mb, "{0} exit loop", _url);
        }
        void HandleConnect()
        {
            netWorkState = NetWorkState.Running;
            _handler.OnOpen(_url);
        }
        void PeekMessage()
        {
            lock (_resvMsgQueue)
            {
                int handleCount = 0;
                while (_resvMsgQueue.Count > 0)
                {
                    WsMsg msg = _resvMsgQueue.Dequeue();
                    handleCount++;
                    bool handleNextMsg = false;
                    if (msg._isRaw)
                    {
                        handleNextMsg = _handler.OnRawMsg(msg._rawMsg, handleCount);
                    }
                    else
                    {
                        handleNextMsg = _handler.OnTxtMsg(msg._txtMsg, handleCount);
                    }

                    if (!handleNextMsg)
                    {
                        break;
                    }
                }
            }
        }
        void HandleClosing()
        {
            _handler.OnDisconnect(_disReason, _err);
            netWorkState = NetWorkState.CLOSED;
        }
        public bool Update()
        {
            CheckErr();
            bool goon = true;
            switch (netWorkState)
            {
                case NetWorkState.CLOSING:
                    HandleClosing();
                    break;
                case NetWorkState.CONNECTING:
                    //CheckTimeOut();
                    break;
                case NetWorkState.CONNECTED:
                    HandleConnect();
                    break;
                case NetWorkState.Running:
                    PeekMessage();
                    break;
                case NetWorkState.CLOSED:
                    goon = false;
                    break;
                default:
                    ZLog.I(_mb, "stop socket {0}", netWorkState);
                    goon = false;
                    break;
            }
            return goon;
        }
    }
}