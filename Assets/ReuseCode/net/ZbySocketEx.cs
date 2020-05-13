using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Collections;

namespace Zby
{
    public class RecvMsg
    {
        public MessagePacket _headerPack; //消息头缓冲
        public MessagePacket _bodyPack;   //消息体缓冲
    }

    public interface SendMsg
    {
        byte[] GetSendBuff();
    }

    public class SimpleSendMsg:SendMsg
    {
        byte[] _buff;

        public SimpleSendMsg(byte[] buf)
        {
            _buff = buf;
        }

        public  byte[] GetSendBuff()
        {
            return _buff;
        }
    }

    public class ZbySocketEx : IDisposable
	{
        public static readonly int WAIT_DISCONNECT_TIMES = 8;
        public static readonly int WAIT_DISCONNECT_TRIGGER = 1;

        private NetProtocolHandler _protocol;
        private NetHandler         _handler; //网络消息回调接口
        private MonoBehaviour      _mb;

        private Socket socket;
        private NetWorkState netWorkState = NetWorkState.CLOSED;   //current network state
		
		private string        _host;
		private int           _port;

        private int         _disReason;
        private string      _err;

		private bool disposed = false;

        int _tryDisconnect = 0;

        RecvMsg _curRecvMsg;
        Queue<RecvMsg> _resvMsgQueue; //消息队列

        bool _isSending = false;  //正在发送中	
        Queue<SendMsg> _sendQueue; //消息队列

        public ZbySocketEx(NetHandler handler, NetProtocolHandler protocol, MonoBehaviour mb)
        {
            _handler  = handler;
            _protocol = protocol;
            _mb = mb;

            _resvMsgQueue = new Queue<RecvMsg>();
            _sendQueue = new Queue<SendMsg>();
        }

        public ZbySocketEx()
        {
            _resvMsgQueue = new Queue<RecvMsg>();
            _sendQueue = new Queue<SendMsg>();
        }

        public void Init( NetHandler handler, NetProtocolHandler protocol, MonoBehaviour mb)
        {
            _handler = handler;
            _protocol = protocol;
            _mb = mb;
        }

        public NetWorkState GetState() 
        { 
            return netWorkState; 
        }
       
		public void Connect()
		{
            if (_handler == null || _protocol == null || _mb == null)
            {
                netWorkState = NetWorkState.CLOSED;
                throw new Exception("handler null");
            }

            if ( netWorkState != NetWorkState.CLOSED)
            {
                throw new Exception("state err" + netWorkState.ToString());
            }

            netWorkState = NetWorkState.CONNECTING;
            _mb.StartCoroutine(_AutoUpdate());

            IPAddress ipAddress = null;
			try
			{
				// this implement quickly
				IPAddress[] ips;
				ips = Dns.GetHostAddresses( _host);
				foreach (IPAddress ipa in ips)
				{
					if (ipa.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
					{
						ipAddress = ipa;
						break;
					}
				}
			}
			catch (Exception e)
			{
                OnDisconnect(1, e.ToString());
                return;
			}
			
			if (ipAddress == null)
			{
                OnDisconnect(2, "can not parse host : " + _host);
                return;
			}
			
			this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint ie = new IPEndPoint(ipAddress, _port);
           
			socket.BeginConnect(ie, new AsyncCallback(Callback_ConnectOk), this.socket);
            _resvMsgQueue.Clear();
            _sendQueue.Clear();
            _tryDisconnect = 0;
        }
        public void Connect(string host, int port)
        {
			this._host = host;
			this._port = port;
            ZLog.D(null, "start connect {0} {1}", host, port);
            Connect();
        }

        public void Callback_ConnectOk(IAsyncResult result)
        {
            try
            {
                this.socket.EndConnect(result);
                netWorkState = NetWorkState.CONNECTED;   
            }
            catch (SocketException e)
            {
                OnDisconnect(3, e.ToString());
                Dispose();
            }
            finally
            {

            }
        }
        private IEnumerator _AutoUpdate()
        {
            bool running = true;
            while (running)
            {
                running =Update();
                yield return 1;
            }
            ZLog.I(_mb, "{0}:{1} exit loop", _host, _port);
        }

        public bool Update()
        {
            bool goon = true;
            switch (netWorkState)
            {
                case NetWorkState.CLOSING:
                    HandleClosing();
                    //goon = false;
                    break;
                case NetWorkState.CONNECTING:
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
        void PeekMessage()
        {
            if (_tryDisconnect> 0 )
            {
                if (WAIT_DISCONNECT_TRIGGER == _tryDisconnect)
                {
                    OnDisconnect(8, "try disconnect timeout");

                }
                 _tryDisconnect--; 
                return;
            }

            lock(_resvMsgQueue)
            {
                int handleCount = 0;
                while( _resvMsgQueue.Count > 0)
                {
                    RecvMsg msg = _resvMsgQueue.Dequeue();
                    handleCount++;
                    if ( !_protocol.HandleBody(msg._headerPack, msg._bodyPack, handleCount))
                    {
                        break;
                    }
                }
            }
        }
        void HandleConnect()
        {
            netWorkState = NetWorkState.Running;
            _handler.OnConnect();
            this.StartRecvHeader();
        }
        void HandleClosing()
        {
            _handler.OnDisconnect(_disReason, _err);
            netWorkState = NetWorkState.CLOSED;
            _tryDisconnect = 0;
            if(!this.disposed)
            {
                Dispose();
            }
        }
        public void StartRecvHeader()
        {
            _curRecvMsg = new RecvMsg();
            int headerSize = _protocol.GetHeadSize();
            _curRecvMsg._headerPack = new MessagePacket(headerSize);
            
            this.socket.BeginReceive(_curRecvMsg._headerPack._buffer, 0, headerSize, 0, new AsyncCallback(Callback_ReceiveHeadOK), _curRecvMsg._headerPack);    
        }
        public void Callback_ReceiveHeadOK(IAsyncResult ar)
        {
            int reason = 0;
            string err="";

            try
            {
                int receiveLen = socket.EndReceive(ar);
                do
                {
                    if (receiveLen <= 0)
                    {
                        reason = 4;
                        err = "recv header sz " + receiveLen.ToString();
                        break;
                    }

                    MessagePacket mp = (MessagePacket)ar.AsyncState;
                    mp._curBytes += receiveLen;//自增接收到的数据
                    if (!mp.IsAllRecieve())
                    {
                        //获取的数据长度小于头长度
                        socket.BeginReceive(mp._buffer, mp._curBytes, mp.NeedRecieveBytes(), 0, new AsyncCallback(Callback_ReceiveHeadOK), mp);
                        break;
                    }

                    int bodyLen = this._protocol.GetBodySize(this._curRecvMsg._headerPack);
                    if (bodyLen > _protocol.GetMaxSize())
                    {
                        reason = 4;
                        err = string.Format("too max sz {0} allow {1}", bodyLen, _protocol.GetMaxSize());
                        ZLog.E(_mb, err);
                        break;
                    }
                    _curRecvMsg._bodyPack = new MessagePacket(bodyLen);
                    socket.BeginReceive(this._curRecvMsg._bodyPack._buffer, 0, bodyLen, 0, new AsyncCallback(Callback_ReceiveBodyOK), this._curRecvMsg._bodyPack);
                } while (false);

            }
            catch (Exception e)
            {
                reason = 5;
                err = e.ToString();
                
            }

            if (reason > 0)
            {
                Dispose();
                OnDisconnect( reason,err);
            }
        }
        public void Callback_ReceiveBodyOK(IAsyncResult ar)
        {
            int reason = 0;
            string err = "";

            try
            {
                int receiveLen = socket.EndReceive(ar);
                do
                {
                    if (receiveLen <= 0)
                    {
                        reason = 6;
                        err = "recv body sz " + receiveLen.ToString();
                        break;
                    }

                    MessagePacket mp = (MessagePacket)ar.AsyncState;
                    mp._curBytes += receiveLen;//自增接收到的数据
                    if (!mp.IsAllRecieve())
                    {
                        //获取的数据长度小于头长度
                        socket.BeginReceive(mp._buffer, mp._curBytes, mp.NeedRecieveBytes(), 0, new AsyncCallback(Callback_ReceiveHeadOK), mp);
                        break;
                    }
                    lock (_resvMsgQueue)
                    {
                        _resvMsgQueue.Enqueue(_curRecvMsg);
                    }
                    this.StartRecvHeader();
                } while (false);

            }
            catch (Exception e)
            {
                reason = 7;
                err = e.ToString();
            }

            if (reason > 0)
            {
                Dispose();
                OnDisconnect(reason, err);
            }

        }
        

        private void sendCallback(IAsyncResult asyncSend)
        {
			SendMsg sendBuff = asyncSend.AsyncState as SendMsg; 
			//sendBuff._SendOkTime = System.DateTime.Now; 
          

			socket.EndSend (asyncSend);
            if (this.netWorkState != NetWorkState.Running) {
				this._isSending = false;
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
                    sendBuff = this._sendQueue.Dequeue();
                    ///sendBuff._dequeTime = System.DateTime.Now;
                    byte[] thisSend = sendBuff.GetSendBuff();
					socket.BeginSend(thisSend, 0, thisSend.Length, SocketFlags.None, new AsyncCallback(sendCallback), sendBuff);
                }
            }
        }

        public bool CanSend()
        {
            return this.netWorkState == NetWorkState.Running;
        }

      
        public bool Send( SendMsg buffer) 
        {
            bool ret = false;
            byte[] thisSend= null; 
            SendMsg sendBuff = buffer;
            do
            {
                if( !this.CanSend() ){
                    break;
                }

                lock(_sendQueue)
                {
                        //发送队列为空，且当前不在发送过程中
                        if (this._sendQueue.Count == 0 && !this._isSending)
                        {
                            thisSend = buffer.GetSendBuff() ;
                        }
                        else
                        {
                            this._sendQueue.Enqueue(buffer);
                            if (!this._isSending) // 当前没在发送则开始发送
                            {
                                sendBuff = this._sendQueue.Dequeue();
                                thisSend = sendBuff.GetSendBuff();
                            }
                        }

                        if (thisSend != null)
                        {
                            socket.BeginSend(thisSend, 0, thisSend.Length, SocketFlags.None, new AsyncCallback(sendCallback), sendBuff);
                            this._isSending = true;
                        }
                
                }
                ret = true;
            } while (false);
            return ret;
        }
       

        private void OnDisconnect(int reason, string str)
        {
            if ( netWorkState != NetWorkState.CLOSING)
            {
                netWorkState = NetWorkState.CLOSING;
                _disReason = reason;
                _err = str;
            }
            else
            {
                ZLog.E(_mb, "OnDisconnect state err {0}", netWorkState);
            }
            	
        }
        /*
        public void Close()
        {
            Dispose();
            OnDisconnect(8, "user close socket");
        }*/
        
        public void Close(bool bSync = true)
        {
            if (socket != null)
            {
                if (socket.Connected && 0 == _tryDisconnect)
                {
                    _tryDisconnect = WAIT_DISCONNECT_TIMES;
                    this.socket.Shutdown(SocketShutdown.Both);
                    if (bSync)
                    {
                        socket.Disconnect(false);
                    }
                    else
                    {
                        try
                        {
                            var args = new SocketAsyncEventArgs();
                            socket.DisconnectAsync(args);
                        }
                        catch (Exception e)
                        {
                            socket.Disconnect(false);
                            ZLog.E(null, "Close err {0}", e.ToString());
                        }
                    }
                }
            }
           
            
            /*
            if (OSTools.InEditor())
            {
                if (socket != null)
                {
                    if (socket.Connected)
                    {
                        if (bSync)
                        {
                            socket.Disconnect(true);
                        }
                        else
                        {
                            try
                            {
                                var args = new SocketAsyncEventArgs();
                                socket.DisconnectAsync(args);
                            }
                            catch (Exception e)
                            {
                                socket.Disconnect(true);
                                ZLog.E(null, "Close err {0}", e.ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                Dispose();
                OnDisconnect(8, "user close socket");

            }*/
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                // free managed resources
                try
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Disconnect(true);
                }
                catch (Exception e)
                {
                    //todo : 有待确定这里是否会出现异常，这里是参考之前官方github上pull request。emptyMsg
                   // ZLog.E(_mb, e.ToString());
                }
                finally
                {
                    this.socket.Close();
                    this.socket = null;
                }

                this.disposed = true;
            }
        }

    }
}
