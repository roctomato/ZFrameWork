from fastapi import FastAPI, WebSocket
from fastapi.responses import HTMLResponse
import json

import websocket_frame

app = FastAPI()


class ChatClient( websocket_frame.WebSocketClient):
    def OnRecv(self, dataType, msg):
        msg = json.loads(msg)
        if msg['method'] in self.handler:
            self.handler[msg['method']](msg['param'])
        else:
            print("unknown method",  msg['method'])
            
    def OnConnect(self, url):
        self.handler ={
            'register':self.handle_register,
            'chat':self.handle_chat,
        }

    def handle_chat(self,param):
        clientMgr.handle_chat(self.name, param, self)
    
    def handle_register(self, param):
        global clientMgr
        print(param)
        self.name = param['name']
        clientMgr.SendHistory(self)
        
    
class CurClientMgr(websocket_frame.WebSocketMgr):
    def __init__( self, cls ):
        websocket_frame.WebSocketMgr.__init__(self, cls)
        self.history =[]
        self.history_msg = None
        self.add_msg("system", "hello world")
        
    def add_msg(self, name, msg):
        self.history.append((name, msg))
        msg = {"method":"history","param":self.history}
        self.history_msg = json.dumps(msg)
        
    def handle_chat(self, name, msg, cl):
        self.add_msg(name, msg)
        send_msg  = {"method":"chat_ntf","param":(name, msg)}
        self.SendAllText(json.dumps(send_msg))
        
    def SendHistory(self, cl):
        cl.SendText(self.history_msg)
    
clientMgr = CurClientMgr(ChatClient)

    
@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    await clientMgr.AddWs(websocket)
    
if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app='chat_websocket:app', host="0.0.0.0", port=8000, reload=True, debug=True)
    