using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using ServerPresentation;

namespace ClientData
{
	internal class ConnectionService : IConnectionService
	{
		public event Action<string>? Logger;
		public event Action? OnConnectionStateChanged;
		public event Action<string>? OnMessage;
		public event Action? OnError;
		public event Action? OnDisconnect;
		
		internal WebSocketConnection? WebSocketConnection { get; private set; }
		public async Task Connect(Uri peerUri)
		{
			try
			{
				Logger?.Invoke($"Connecting to {peerUri}");
				WebSocketConnection = await WebSocketClient.Connect(peerUri, Logger);
				OnConnectionStateChanged?.Invoke();
				WebSocketConnection.OnMessage = (message) => OnMessage?.Invoke(message);
				WebSocketConnection.OnError = () => OnError?.Invoke();
				WebSocketConnection.OnClose = () => OnDisconnect?.Invoke();
			}
			catch (WebSocketException exception)
			{
				Logger?.Invoke($"WebSocked exception: {exception.Message}");
				OnError?.Invoke();
			}
		}
		
		

		public async Task Disconnect()
		{
			if (WebSocketConnection != null)
			{
				await WebSocketConnection.DisconnectAsync();
			}
		}

		public bool IsConnected()
		{
			return WebSocketConnection != null;
		}

		public async Task SendAsync(string message)
		{
			if (WebSocketConnection != null)
			{
				await WebSocketConnection.SendAsync(message);
			}
		}
	}
}