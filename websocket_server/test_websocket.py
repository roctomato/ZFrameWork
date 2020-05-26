from fastapi import FastAPI, WebSocket
from fastapi.responses import HTMLResponse

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
client_id = 1
all_client = {}

async def SendAll( cid, data ):
    msg = f"[{cid}]: {data}"
    for ws in all_client.values():
        await ws.send_text(msg)
    
@app.get("/")
async def get():
    return HTMLResponse(html)

async def recv_msg(websocket: WebSocket):
    global client_id
    cid = client_id
    client_id += 1
    
    all_client[ cid] = websocket
    try:
        while True:      
            data = await websocket.receive_text()
            await SendAll(cid, data)
    except  Exception as e:
        print( e )
    del all_client[cid]
    
@app.websocket("/ws")
async def websocket_endpoint(websocket: WebSocket):
    await websocket.accept()
    await recv_msg(websocket)
    