using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace Zby
{
    /// <summary>
    /// network state enum
    /// </summary>
    public enum NetWorkState
    {
        [Description("disconnected")]
        CLOSING,

        [Description("initial state")]
        CLOSED,

        [Description("connecting server")]
        CONNECTING,

        [Description("server connected")]
        CONNECTED,

       
        [Description("can transfer")]
        Running
    }
    public class MessagePacket
    {
        public byte[] _buffer; //缓存buff
        public int _size;     //当前缓存实际使用的大小
        public int _capSize;  //当前缓存实际分配的大小
        public int _curBytes; //当前写入位置

        //private int _curHandlePos; //当前读取位置

        public MessagePacket(int sz)
        {
            _size = 0;
            _capSize = 0;
            _curBytes = 0;
           // _curHandlePos = 0;
            if (sz > 0)
            {
                Reset(sz);
            }
        }
        //重设置缓存区
        public void Reset(int sz)
        {
            if (sz > _capSize)
            {
                _buffer = new byte[sz];
                _capSize = sz;
            }
            _curBytes = 0;
            _size = sz;
            //_curHandlePos = 0;
        }
        //是否全部收完了
        public bool IsAllRecieve()
        {
            return _curBytes == _size;
        }
        //剩余需要收的字节数
        public int NeedRecieveBytes()
        {
            return _size - _curBytes;
        }
        public ByteBuffer GetByteBuffer()
        {
            ByteBuffer bb = new ByteBuffer(_buffer);
            return bb;
        }
      
    }

    /*
    协议包收取接口，假设网络协议由包头和包体组成
    框架调用 GetHeadSize(),得到包头长度，然后先接受包头长度的数据，接着调用GetBodySize(),把包头数据传入，得到包体长度
    框架判断包体长度是否落在 GetMaxSize()范围内，是则收数据，否则断开连接
    */
    public interface NetProtocolHandler
    {
         int GetHeadSize() ;                             //包头长度
         int GetMaxSize() ;                              //最大消息长度
         int GetBodySize(MessagePacket header) ; // 包体长度，注意此长度不包含消息头长度
         bool HandleBody(MessagePacket header, MessagePacket body, int handleCount); //return false框架停止处理下条消息
    }

    //网络消息回调接口
    public interface NetHandler
    {
        void OnConnect(); //连接成功回调
        void OnDisconnect(int reason, string str); //断开连接回调
    }

   
}
