using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientData
{
	internal class WebSocketClient
	{
		public static async Task<WebSocketConnection> Connect(Uri peer, Action<string>? logger)
		{
			ClientWebSocket clientWebSocket = new ClientWebSocket();
			await clientWebSocket.ConnectAsync(peer, CancellationToken.None);
			switch (clientWebSocket.State)
			{
				case WebSocketState.Open:
					logger?.Invoke($"Opening WebSocket connection to remote server {peer}");
					WebSocketConnection socket = new ClientWebSocketConnection(clientWebSocket, peer, logger);
					return socket;

				default:
					logger?.Invoke($"Cannot connect to remote node status {clientWebSocket.State}");
					throw new WebSocketException($"Cannot connect to remote node status {clientWebSocket.State}");
			}
		}

		private class ClientWebSocketConnection : WebSocketConnection
		{
			private readonly ClientWebSocket clientWebSocket;
			private readonly Action<string> log;
			private readonly Uri peer;

			public ClientWebSocketConnection(ClientWebSocket clientWebSocket, Uri peer, Action<string> log)
			{
				this.clientWebSocket = clientWebSocket;
				this.peer = peer;
				this.log = log;
				Task.Factory.StartNew(ClientMessageLoop);
			}

			protected override Task SendTask(string message)
			{
				return clientWebSocket.SendAsync(message.GetArraySegment(), WebSocketMessageType.Text, true, CancellationToken.None);
			}

			public override Task DisconnectAsync()
			{
				return clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Shutdown procedure started", CancellationToken.None);
			}

			public override string ToString()
			{
				return peer.ToString();
			}

			private void ClientMessageLoop()
			{
				try
				{
					byte[] buffer = new byte[1024];
					while (true)
					{
						ArraySegment<byte> segment = new ArraySegment<byte>(buffer);
						WebSocketReceiveResult result = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
						if (result.MessageType == WebSocketMessageType.Close)
						{
							OnClose?.Invoke();
							clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "I am closing", CancellationToken.None).Wait();
							return;
						}

						int count = result.Count;
						while (!result.EndOfMessage)
						{
							if (count >= buffer.Length)
							{
								OnClose?.Invoke();
								clientWebSocket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "That's too long",
										CancellationToken.None)
									.Wait();
								return;
							}

							segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
							result = clientWebSocket.ReceiveAsync(segment, CancellationToken.None).Result;
							count += result.Count;
						}

						string message = Encoding.UTF8.GetString(buffer, 0, count);
						OnMessage?.Invoke(message);
					}
				}
				catch (Exception ex)
				{
					log($"Connection has been broken because of an exception {ex}");
					clientWebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError,
						"Connection has been broken because of an exception",
						CancellationToken.None).Wait();
				}
			}
		}
	}
}