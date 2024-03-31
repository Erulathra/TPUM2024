using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientApi;
using Logic;

namespace ServerPresentation
{
	internal class Program
	{
		private readonly LogicAbstractApi logicAbstractApi;

		private WebSocketConnection? webSocketConnection;

		private Program(LogicAbstractApi logicAbstractApi)
		{
			this.logicAbstractApi = logicAbstractApi;
			logicAbstractApi.GetShop().InflationChanged += HandleInflationChanged;
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

			webSocketConnection = connection;
		}

		private async void OnMessage(string message)
		{
			Console.WriteLine($"New message: {message}");

			Serializer serializer = Serializer.Create();
			ServerCommand serverCommand = serializer.Deserialize<ServerCommand>(message);
			if (serverCommand.Command == ServerCommand.GetItemsRequest().Command)
			{
				await SendItems();
			}
		}

		private async Task SendItems()
		{
			if (webSocketConnection == null)
				return;

			Console.WriteLine($"Sending items...");

			UpdateAllResponse serverResponse = new UpdateAllResponse();
			List<IShopItem> items = logicAbstractApi.GetShop().GetItems();
			serverResponse.Items = items.Select(x => x.ToDTO()).ToArray();

			Serializer serializer = Serializer.Create();
			string responseJson = serializer.Serialize(serverResponse);
			Console.WriteLine(responseJson);

			await webSocketConnection.SendAsync(responseJson);
		}
		
		private async void HandleInflationChanged(object? sender, LogicInflationChangedEventArgs args)
		{
			if (webSocketConnection == null)
				return;
			
			Console.WriteLine($"New inflation: {args.NewInflation}");

			List<IShopItem> items = logicAbstractApi.GetShop().GetItems();
			InflationChangedResponse inflationChangedResponse = new InflationChangedResponse();
			inflationChangedResponse.NewInflation = args.NewInflation;
			inflationChangedResponse.NewPrices = items.Select(x => new NewPriceDTO(x.Id, x.Price)).ToArray();

			Serializer serializer = Serializer.Create();
			string responseJson = serializer.Serialize(inflationChangedResponse);
			Console.WriteLine(responseJson);

			await webSocketConnection.SendAsync(responseJson);
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