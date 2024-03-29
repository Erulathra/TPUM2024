using System;
using System.Threading.Tasks;
using Logic;

namespace ServerPresentation
{
	internal class Program
	{
		private readonly LogicAbstractApi logicAbstractApi;

		private Program(LogicAbstractApi logicAbstractApi)
		{
			this.logicAbstractApi = logicAbstractApi;
		}

		private async Task StartConnection()
		{
			while (true)
			{
                Console.WriteLine("Waiting for connect...");
                await WebSocketServer.StartServer(21370, OnConnect);
			}
		}

		private void OnConnect(WebSocketConnection connection)
		{
			Console.WriteLine($"Connected to {connection}");

			connection.OnMessage = OnMessage;
			connection.OnError = OnError;
			connection.OnClose = OnClose;
		}

		private void OnMessage(string message)
		{
			Console.WriteLine($"New message: {message}");
		}

		private void OnError()
		{
			Console.WriteLine($"Connection error");
		}

		private void OnClose()
		{
			Console.WriteLine($"Connection closed");
		}

		private static async Task Main(string[] args)
		{
			Program program = new Program(LogicAbstractApi.Create());
			await program.StartConnection();
		}
	}
}