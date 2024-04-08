using System.CodeDom.Compiler;
using ConnectionApi;
using ClientData;
using Newtonsoft.Json;

namespace ClientDataTest
{
    internal class ConnectionServiceMock : IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectionStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;
        
        public Task Connect(Uri peerUri)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            return true;
        }

        public async Task SendAsync(string message)
        {
            if (serializer.GetResponseHeader(message) == ServerStaticsMock.GetItemsCommandHeader)
            {
                UpdateAllResponseMock response = new UpdateAllResponseMock();
                response.Header = ServerStaticsMock.UpdateAllResponseHeader;
                response.Items = [new ItemDTOMock { Id = new Guid("testId"), Name = "TestItem", Description = "TestDesc", Type = "Potion", Price = 10.0f, IsSold = false }];
                OnMessage?.Invoke(serializer.Serialize(response));
            }
            else if (serializer.GetResponseHeader(message) == ServerStaticsMock.SellItemCommandHeader)
            {
                SellItemCommandMock sellItemCommand = serializer.Deserialize<SellItemCommandMock>(message);
                lastSoldGuid = sellItemCommand.ItemID;

                TransactionResponseMock response = new TransactionResponseMock();
                response.Header = ServerStaticsMock.TransactionResponseHeader;
                response.Succeeded = true;
                OnMessage?.Invoke(serializer.Serialize(response));
            }

            await Task.Delay(0);
        }

        // Fields and methods for test purposes

        private Serializer serializer = Serializer.Create();
        public Guid lastSoldGuid;

        public void MockInflationChanged(List<IItem> items, float newInflation)
        {
            InflationChangedResponseMock response = new InflationChangedResponseMock();
            response.Header = ServerStaticsMock.InflationChangedResponseHeader;
            response.NewInflation = newInflation;

            NewPriceDTOMock[] newPriceDTOs = new NewPriceDTOMock[items.Count];
            int i = 0;
            foreach (IItem item in items)
            {
                newPriceDTOs[i] = new NewPriceDTOMock {ItemID = item.Id, NewPrice = item.Price * newInflation};
                i++;
            }
            response.NewPrices = newPriceDTOs;

            OnMessage?.Invoke(serializer.Serialize(response));
        }

        public void MockUpdateAll(ItemDTOMock[] items)
        {
            UpdateAllResponseMock response = new UpdateAllResponseMock();
            response.Header = ServerStaticsMock.UpdateAllResponseHeader;
            response.Items = items;
            OnMessage?.Invoke(serializer.Serialize(response));
        }
    }
    
	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class ItemDTOMock
	{
		[JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid Id { get; set; }

		[JsonProperty("Name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty("Description", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Description { get; set; }

		[JsonProperty("Type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Type { get; set; }

		[JsonProperty("Price", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public float Price { get; set; }

		[JsonProperty("IsSold", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public bool IsSold { get; set; }
	}
	
	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal abstract class ServerResponseMock
	{
		[JsonProperty("Header", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Header { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class UpdateAllResponseMock : ServerResponseMock
	{
		[JsonProperty("Items", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
		public ICollection<ItemDTOMock> Items { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class InflationChangedResponseMock : ServerResponseMock
	{
		[JsonProperty("NewInflation", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public float NewInflation { get; set; }

		[JsonProperty("NewPrices", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
		public ICollection<NewPriceDTOMock> NewPrices { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class TransactionResponseMock : ServerResponseMock
	{
		[JsonProperty("TransactionId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid TransactionId { get; set; }

		[JsonProperty("Succeeded", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public bool Succeeded { get; set; }
	}
	
	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal abstract class ServerCommandMock
	{
		[JsonProperty("Header", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Header { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class SellItemCommandMock : ServerCommandMock
	{
		[JsonProperty("TransactionID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid TransactionID { get; set; }

		[JsonProperty("ItemID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid ItemID { get; set; }
	}
	
	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class NewPriceDTOMock
	{
		[JsonProperty("ItemID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid ItemID { get; set; }

		[JsonProperty("NewPrice", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public float NewPrice { get; set; }
	}
	
	internal static class ServerStaticsMock
	{
		public static readonly string GetItemsCommandHeader = "GetItems";
		public static readonly string SellItemCommandHeader = "SellItem";
		
		public static readonly string UpdateAllResponseHeader = "UpdateAllItems";
		public static readonly string InflationChangedResponseHeader = "InflationChanged";
		public static readonly string TransactionResponseHeader = "TransactionResponse";
	}
}
