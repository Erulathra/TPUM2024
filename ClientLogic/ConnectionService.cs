using System;
using System.Threading.Tasks;

namespace Logic
{
	internal class LogicConnectionService : ILogicConnectionService
	{
		public event Action<string>? Logger;
		public event Action? OnConnectionStateChanged;
		
	    public event Action<string>? OnMessage;
	    public event Action? OnError;

		private readonly ClientData.IConnectionService dataConnectionService;

		public LogicConnectionService(ClientData.IConnectionService connectionService)
		{
			dataConnectionService = connectionService;
			
		    connectionService.Logger += (message) => Logger?.Invoke(message);
		    connectionService.OnConnectionStateChanged += () => OnConnectionStateChanged?.Invoke();
		    
		    connectionService.OnMessage += (message) => OnMessage?.Invoke(message);
		    connectionService.OnError += () => OnError?.Invoke();
		}

		public async Task Connect(Uri peerUri)
		{
			await dataConnectionService.Connect(peerUri);
		}

		public async Task Disconnect()
		{
			await dataConnectionService.Disconnect();
		}

		public bool IsConnected()
		{
			return dataConnectionService.IsConnected();
		}
	}
}