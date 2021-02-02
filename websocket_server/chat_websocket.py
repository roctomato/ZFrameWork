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
            'register':self.handle_register
        }

    def handle_register(self, param):
        print(param)
        self.name = param['name']
    
clientMgr = websocket_frame.WebSocketMgr(ChatClient)

    
@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    await clientMgr.AddWs(websocket)
    
if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app='chat_websocket:app', host="0.0.0.0", port=8000, reload=True, debug=True)
    