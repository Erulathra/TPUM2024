using System;
using System.Threading.Tasks;
using Logic;

namespace Model
{
	public class ModelConnectionService
	{
		public event Action<string>? Logger;
		public event Action? OnConnectionStateChanged;
		
	    public event Action<string>? OnMessage;
	    public event Action? OnError;

	    private readonly ILogicConnectionService connectionService;

	    public ModelConnectionService(ILogicConnectionService connectionService)
	    {
		    this.connectionService = connectionService;
		    this.connectionService.Logger += (message) => Logger?.Invoke(message);
		    this.connectionService.OnConnectionStateChanged += () => OnConnectionStateChanged?.Invoke();
		    this.connectionService.OnMessage += (message) => OnMessage?.Invoke(message);
		    this.connectionService.OnError += () => OnError?.Invoke();
	    }


	    public async Task Connect(Uri peerUri)
	    {
		    await connectionService.Connect(peerUri);
	    }

	    public async Task Disconnect()
	    {
		    await connectionService.Disconnect();
	    }

	    public bool IsConnected()
	    {
		    return connectionService.IsConnected();
	    }
	}
}