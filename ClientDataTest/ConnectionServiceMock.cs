using ConnectionApi;
using ClientData;

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
            if (serializer.GetResponseHeader(message) == ServerStatics.GetItemsCommandHeader)
            {
                UpdateAllResponse response = new UpdateAllResponse();
                response.Header = ServerStatics.UpdateAllResponseHeader;
                response.Items = [new ItemDTO { Id = new Guid("testId"), Name = "TestItem", Description = "TestDesc", Type = "Potion", Price = 10.0f, IsSold = false }];
                OnMessage?.Invoke(serializer.Serialize(response));
            }
            else if (serializer.GetResponseHeader(message) == ServerStatics.SellItemCommandHeader)
            {
                SellItemCommand sellItemCommand = serializer.Deserialize<SellItemCommand>(message);
                lastSoldGuid = sellItemCommand.ItemID;

                TransactionResponse response = new TransactionResponse();
                response.Header = ServerStatics.TransactionResponseHeader;
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
            InflationChangedResponse response = new InflationChangedResponse();
            response.Header = ServerStatics.InflationChangedResponseHeader;
            response.NewInflation = newInflation;

            NewPriceDTO[] newPriceDTOs = new NewPriceDTO[items.Count];
            int i = 0;
            foreach (IItem item in items)
            {
                newPriceDTOs[i] = new NewPriceDTO {ItemID = item.Id, NewPrice = item.Price * newInflation};
                i++;
            }
            response.NewPrices = newPriceDTOs;

            OnMessage?.Invoke(serializer.Serialize(response));
        }

        public void MockUpdateAll(ItemDTO[] items)
        {
            UpdateAllResponse response = new UpdateAllResponse();
            response.Header = ServerStatics.UpdateAllResponseHeader;
            response.Items = items;
            OnMessage?.Invoke(serializer.Serialize(response));
        }
    }
}
