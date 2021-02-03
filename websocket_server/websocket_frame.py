import asyncio
from enum import Enum

from fastapi import  WebSocket

class WS_DATA_TYPE(Enum):
    TEXT  = 1
    BYTES = 2
    JSON  = 3
    
class SendData:
    def __init__(self, dataType: WS_DATA_TYPE , data):
        self.dataType = dataType
        self.data = data
        
class WebSocketClient:
    def __init__(self, cid, websocket: WebSocket, mgr, dataType: WS_DATA_TYPE = WS_DATA_TYPE.TEXT ):
        self._cid = cid
        self._ws  = websocket
        self._dataType = dataType
        self.send = []
        self.inSend = False
        self.mgr = mgr
        
    def OnRecv(self, dataType, msg):
        pass
    def OnConnect(self, url):
        pass
    def OnDisconnect( self, reason):
        pass
    
    
    async def SendMsg( self ):
        self.inSend = True
        while len(self.send) > 0 :
            msg = self.send.pop()
            if msg.dataType == WS_DATA_TYPE.TEXT:
                await self._ws.send_text(msg.data)
            elif msg.dataType == WS_DATA_TYPE.BYTES:
                await self._ws.send_bytes(msg.data)
            elif self._dataType == WS_DATA_TYPE.JSON:
                await self._ws.send_json(msg.data)
            else:
                print ( "unknown data type %d" %(self._dataType))
                continue
        self.inSend = False
        
    def _AddSendMsg( self, data:SendData):
        self.send.append(data)
        if self.inSend:
            return
        loop = asyncio.get_running_loop()
        loop.create_task(self.SendMsg())

    
    def SendText( self, data ):
        self._AddSendMsg( SendData(WS_DATA_TYPE.TEXT, data))
        
    def SendBytes( self, data ):
        self._AddSendMsg( SendData(WS_DATA_TYPE.BYTES, data))
        
    def SendJson( self, data ):
        self._AddSendMsg( SendData(WS_DATA_TYPE.JSON, data))

    async def  StartRecv( self ):
        self.send.clear()
        self.inSend = False
        err_msg = ""
        
        self.OnConnect( self._ws.url)
    
        try:
            while True:
                if self._dataType == WS_DATA_TYPE.TEXT:
                    data = await self._ws.receive_text()
                elif self._dataType == WS_DATA_TYPE.BYTES:
                    data = await self._ws.receive_bytes()
                elif self._dataType == WS_DATA_TYPE.JSON:
                    data = await self._ws.receive_json()
                else:
                    err_msg = "unknown data type %d" %(self._dataType)
                    break
                self.OnRecv(self._dataType, data)
        except  Exception as e:
            print( "err:", e )
            err_msg = str(e)
        self.OnDisconnect(err_msg)
        self.mgr.Remove(self)
        
class WebSocketMgr:
    def __init__( self, cls ):
        self.curId      = 1
        self.all_client = {}
        self.cls = cls
        
    async def AddWs( self, ws:WebSocket, dataType:WS_DATA_TYPE =  WS_DATA_TYPE.TEXT ):
        cid = self.curId
        wsc = self.cls( cid, ws, self, dataType)
        self.all_client[cid] = wsc
        self.curId += 1
        await wsc.StartRecv()
       
        
    def Remove( self, wsc ):
        del self.all_client[wsc._cid]
        
    def SendAllText( self, txt, except_cl=None):
        for wsc in self.all_client.values():
            if except_cl != wsc:
                wsc.SendText(txt)

    def FindClient( self, cid):
        try:
            wsc = self.all_client[cid]
            return wsc
        except Exception as e:
            return None
        
