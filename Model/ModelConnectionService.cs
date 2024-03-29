using System;
using System.Threading.Tasks;
using Logic;

namespace Model
{
	public class ModelConnectionService
	{
		public event Action<string>? Logger;
		public event Action? OnConnectionStateChanged;

	    private readonly ILogicConnectionService connectionService;

	    public ModelConnectionService(ILogicConnectionService connectionService)
	    {
		    this.connectionService = connectionService;
		    this.connectionService.Logger += (message) => Logger?.Invoke(message);
		    this.connectionService.OnConnectionStateChanged += () => OnConnectionStateChanged?.Invoke();
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