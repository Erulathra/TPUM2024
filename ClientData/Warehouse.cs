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

		private HashSet<IObserver<InflationChangedEventArgs>> observers;

		public event Action? ItemsUpdated;
		public event Action<bool>? TransactionFinish;


		private readonly IConnectionService connectionService;

		public Warehouse(IConnectionService connectionService)
		{
			observers = new HashSet<IObserver<InflationChangedEventArgs>>();
			
			this.connectionService = connectionService;
			this.connectionService.OnMessage += OnMessage;
		}

		~Warehouse()
		{
			List<IObserver<InflationChangedEventArgs>> cachedObservers = observers.ToList();
			foreach (IObserver<InflationChangedEventArgs>? observer in cachedObservers)
			{
				observer?.OnCompleted();
			}
		}

		private void OnMessage(string message)
		{
			Serializer serializer = Serializer.Create();
			
			if (serializer.GetResponseHeader(message) == UpdateAllResponse.StaticHeader)
			{
				UpdateAllResponse response = serializer.Deserialize<UpdateAllResponse>(message);
				UpdateAllProducts(response);
			}
			else if (serializer.GetResponseHeader(message) == InflationChangedResponse.StaticHeader)
			{
				InflationChangedResponse response = serializer.Deserialize<InflationChangedResponse>(message);
				UpdateAllPrices(response);
			}
			else if (serializer.GetResponseHeader(message) == TransactionResponse.StaticHeader)
			{
				TransactionResponse response = serializer.Deserialize<TransactionResponse>(message);
				if (response.Succeeded)
				{
					RequestItems();
					TransactionFinish?.Invoke(true);
				}
				else
				{
					TransactionFinish?.Invoke(false);
				}
			}
		}

		private void UpdateAllProducts(UpdateAllResponse response)
		{
			if (response.Items == null)
				return;

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
		
		private void UpdateAllPrices(InflationChangedResponse response)
		{
			if (response.NewPrices == null)
				return;

			lock (itemsLock)
			{
				foreach (var newPrice in response.NewPrices)
				{
					if (items.ContainsKey(newPrice.ItemID))
					{
						items[newPrice.ItemID].Price = newPrice.NewPrice;
					}
				}
			}

			foreach (IObserver<InflationChangedEventArgs>? observer in observers)
			{
				observer.OnNext(new InflationChangedEventArgs(response.NewInflation));
			}
		}

		public async Task RequestItems()
		{
			Serializer serializer = Serializer.Create();
			await connectionService.SendAsync(serializer.Serialize(new GetItemsCommand()));
		}

		public async void RequestUpdate()
		{
			if (connectionService.IsConnected())
				await RequestItems();
		}

		public async Task SellItem(Guid itemId)
		{
			if (connectionService.IsConnected())
			{
				Serializer serializer = Serializer.Create();
				SellItemCommand sellItemCommand = new SellItemCommand(itemId);
				await connectionService.SendAsync(serializer.Serialize(sellItemCommand));
			}
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

		public IDisposable Subscribe(IObserver<InflationChangedEventArgs> observer)
		{
			observers.Add(observer);
			return new WarehouseDisposable(this, observer);
		}

		private void UnSubscribe(IObserver<InflationChangedEventArgs> observer)
		{
			observers.Remove(observer);
		}
		
		private class WarehouseDisposable : IDisposable
		{
			private readonly Warehouse warehouse;
			private readonly IObserver<InflationChangedEventArgs> observer;

			public WarehouseDisposable(Warehouse warehouse, IObserver<InflationChangedEventArgs> observer)
			{
				this.warehouse = warehouse;
				this.observer = observer;
			}

			public void Dispose()
			{
				warehouse.UnSubscribe(observer);
			}
		}
	}

}