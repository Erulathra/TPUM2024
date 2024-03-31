using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientApi;

namespace ClientData
{
	internal class Warehouse : IWarehouse
	{
		private readonly Dictionary<Guid, IItem> items = new Dictionary<Guid, IItem>();
		private readonly object itemsLock = new object();

		public event EventHandler<InflationChangedEventArgs>? InflationChanged;
		public event Action? ItemsUpdated;


		private readonly IConnectionService connectionService;

		public Warehouse(IConnectionService connectionService)
		{
			this.connectionService = connectionService;
			this.connectionService.OnMessage += OnMessage;
			
		}

		private void OnMessage(string message)
		{
			Serializer serializer = Serializer.Create();

			if (serializer.GetResponseHeader(message) == UpdateAllResponse.UpdateAllItemsHeader)
			{
				UpdateAllResponse response = serializer.Deserialize<UpdateAllResponse>(message);

				lock (itemsLock)
				{
					items.Clear();
					foreach (ItemDTO item in response.Items)
					{
						items.Add(item.Id, item.ToItem());
					}
				}

				ItemsUpdated?.Invoke();
			}
		}

		public async Task RequestItems()
		{
			Serializer serializer = Serializer.Create();
			await connectionService.SendAsync(serializer.Serialize(ServerCommand.GetItemsRequest()));
		}

		public async void RequestUpdate()
		{
			if (connectionService.IsConnected())
				await RequestItems();
		}

		public void SellItem(Guid itemId)
		{
			throw new NotImplementedException();
		}
		
		public List<IItem> GetItems()
		{
			List<IItem> result = new List<IItem>();
			lock (itemsLock)
			{
				result.AddRange(items.Values.Select(item => (IItem)item.Clone()));
			}
			
			return result;
		}

		public List<IItem> GetAvailableItems()
		{
			List<IItem> result = new List<IItem>();
			lock (itemsLock)
			{
				result.AddRange(items.Values
					.Where(item => !item.IsSold)
					.Select(item => (IItem)item.Clone()));
			}
			
			return result;
		}

		public void AddItem(IItem itemToAdd)
		{
			lock (itemsLock)
			{
				items.Add(itemToAdd.Id, itemToAdd);
			}
		}

		public void RemoveItem(Guid itemIdToRemove)
		{
			lock (itemsLock)
			{
				items.Remove(itemIdToRemove);
			}
		}

		public IItem GetItemByID(Guid guid)
		{
			IItem result;
			lock (itemsLock)
			{
				if (items.ContainsKey(guid))
				{
					result = items[guid];
				}
				else
				{
					throw new KeyNotFoundException();
				}
			}
			
			return result;
		}

		public List<IItem> GetItemsByType(ItemType type)
		{
			List<IItem> result = new List<IItem>();
			lock (itemsLock)
			{
				result.AddRange(items.Values
					.Where(item => item.Type == type)
					.Select(item => (IItem)item.Clone()));
			}
			
			return result;
		}
	}
}