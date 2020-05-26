from fastapi import FastAPI, WebSocket
from fastapi.responses import HTMLResponse

from websocket_frame import WebSocketClient, WebSocketMgr, WS_DATA_TYPE

app = FastAPI()

html = """
<!DOCTYPE html>
<html>
    <head>
        <title>Chat</title>
    </head>
    <body>
        <h1>WebSocket Chat</h1>
        <form action="" onsubmit="sendMessage(event)">
            <input type="text" id="messageText" autocomplete="off"/>
            <button>Send</button>
        </form>
        <ul id='messages'>
        </ul>
        <script>
            var ws = new WebSocket("ws://localhost:8000/ws");
            ws.onmessage = function(event) {
                var messages = document.getElementById('messages')
                var message = document.createElement('li')
                var content = document.createTextNode(event.data)
                message.appendChild(content)
                messages.appendChild(message)
            };
            function sendMessage(event) {
                var input = document.getElementById("messageText")
                ws.send(input.value)
                input.value = ''
                event.preventDefault()
            }
        </script>
    </body>
</html>
"""

@app.get("/")
async def get():
    return HTMLResponse(html)


class CurClient(WebSocketClient):
    def OnRecv(self, dataType, data):
        cid = self._cid
        msg = f"[{cid}]: {data}"
        ws_mgr.SendAllText(msg)
        
    def OnConnect(self, url):
        print (self._cid, " connect")
        
    def OnDisconnect( self, reason):
        print ( self._cid, " Disconnect")
        
ws_mgr = WebSocketMgr(CurClient)

@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    await ws_mgr.AddWs(websocket)
    