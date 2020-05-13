using System.Text;
using UnityEngine;
using Zby;

public class P32M32Header
{
    public static readonly int HEADER_SIZE = 8;
    public int  size;
    public uint cmd; // 1 rigister 2 chat 3 chat ntf

    public P32M32Header(ByteBuffer bb)
    {
        size = bb.ReadInt();
        cmd = bb.ReadUInt();
    }
    
    public P32M32Header(uint cmd, int bodysz)
    {
        size = HEADER_SIZE + bodysz;
        this.cmd = cmd;
    }
    public int GetBodySz()
    {
        return size - HEADER_SIZE;
    }
    public void WriteTo( ByteBuffer bb)
    {
        bb.WriteInt(size).WriteUInt(cmd);
    }
};

public class P32M32SendMsg: SendMsg
{
    P32M32Header head;
    byte[] body;

    public P32M32SendMsg(uint cmd, byte[] data)
    {
        head = new P32M32Header(cmd, data.Length);
        body = data;
    }

    public byte[] GetSendBuff()
    {
        ByteBuffer bb = new ByteBuffer();
        head.WriteTo(bb);
        bb.AppendBytes(body);
        return bb.ToBytes();
    }
}
public class NetProtocol32Cmd32 :  NetProtocolHandler
{
    public delegate bool HandleMsg(P32M32Header header, byte[] body, int handleCount);
    public HandleMsg _handler;

    int          _maxSize;
    P32M32Header _head;
    

    public NetProtocol32Cmd32(int maxSz = 100*1024)
    {
        _maxSize = maxSz;
    }
    public int GetHeadSize()
    {
        return P32M32Header.HEADER_SIZE;
    }
    public int GetMaxSize()
    {
        return _maxSize;
    }
    public int GetBodySize(MessagePacket header)
    {
        ByteBuffer bb = header.GetByteBuffer();
        _head = new P32M32Header(bb);
        return _head.GetBodySz();
    }

    public bool HandleBody(MessagePacket header, MessagePacket body, int handleCount)
    {
        return _handler(_head, body._buffer, handleCount);
    }
}

public class Chat_Socket: ZbySocketEx, NetHandler
{
    public delegate void OnConnetEvent();
    public delegate void OnDisconnectEvent(int reason, string str);
    public delegate void OnRecvChat(string content);

    public event OnConnetEvent ConnectEvent;
    public event OnDisconnectEvent DisEvent;
    public event OnRecvChat ChatEvent;

    NetProtocol32Cmd32 _protol;
    public Chat_Socket(MonoBehaviour mb )
    {
        _protol = new NetProtocol32Cmd32();
        this.Init(this, _protol, mb);
        _protol._handler = HandleMsg;
    }

    public void OnConnect()
    {
        ZLog.I(null, "connect");
        string data = "hello from unity";
        ByteBuffer bb = new ByteBuffer();
        bb.WriteString(data);
        P32M32SendMsg sm = new P32M32SendMsg(2, bb.ToBytes());
        this.Send(sm);
        if (ConnectEvent != null)
        {
            ConnectEvent();
        }
    }
    public void OnDisconnect(int reason, string str)
    {
        ZLog.I(null, "disconnect:{0} {1}", reason, str);
        if ( null != DisEvent)
        {
            DisEvent(reason, str);
        }
    }

    bool HandleMsg(P32M32Header header, byte[] body, int handleCount)
    {
        ByteBuffer bb = new ByteBuffer(body);
        switch(header.cmd)
        {
            case 3:
            {
                string name = bb.ReadString();
                string  content = bb.ReadString();
                string show = string.Format("recv [{0}]:{1}", name, content);
                ZLog.D(null, show);
                if ( null != ChatEvent)
                    {
                        ChatEvent(show);
                    }
                break;
            }
        }
        return true;
    }
}