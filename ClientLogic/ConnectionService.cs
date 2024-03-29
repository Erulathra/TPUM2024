using System;
using System.Threading.Tasks;

namespace Logic
{
	internal class LogicConnectionService : ILogicConnectionService
	{
		public event Action<string>? Logger;
		public event Action? OnConnectionStateChanged;

		private ClientData.IConnectionService dataConnectionService;

		public LogicConnectionService(ClientData.IConnectionService connectionService)
		{
			dataConnectionService = connectionService;
			
		    connectionService.Logger += (message) => Logger?.Invoke(message);
		    connectionService.OnConnectionStateChanged += () => OnConnectionStateChanged?.Invoke();
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