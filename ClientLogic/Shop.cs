using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientData;

namespace Logic
{
	internal class Shop : IShop, IObserver<InflationChangedEventArgs>
	{
		private readonly IWarehouse warehouse;
		
		public event EventHandler<LogicInflationChangedEventArgs>? InflationChanged;
		public event Action? ItemsUpdated;
		public event Action<bool>? TransactionFinish;

		private IDisposable WarehouseSubscriptionHandle;

		public Shop(IWarehouse warehouse)
		{
			this.warehouse = warehouse;

			WarehouseSubscriptionHandle = warehouse.Subscribe(this);

			warehouse.ItemsUpdated += () => ItemsUpdated?.Invoke();
			warehouse.TransactionFinish += (bool succeeded) => TransactionFinish?.Invoke(succeeded);
		}

		public void RequestUpdate()
		{
			warehouse.RequestUpdate();
		}

		public async Task SellItem(Guid itemId)
		{
			List<IShopItem> items = warehouse.GetAvailableItems()
											 .Select(item => new ShopItem(item))
											 .Cast<IShopItem>()
											 .ToList();
			IShopItem? foundItem = items.Find((item) => item.Id == itemId);
			if (foundItem != null && !foundItem.IsSold)
			{
				await warehouse.SellItem(foundItem.Id);
			}
			else
			{
				throw new KeyNotFoundException();
			}
		}

		public List<IShopItem> GetItems()
		{
			return warehouse.GetItems()
							.Select(item => new ShopItem(item))
							.Cast<IShopItem>()
							.ToList();
		}

		public List<IShopItem> GetAvailableItems()
		{
			return warehouse.GetAvailableItems()
							.Select(item => new ShopItem(item))
							.Cast<IShopItem>()
							.ToList();
		}

		public List<IShopItem> GetItemsByType(LogicItemType logicItemType)
		{
			return warehouse.GetItemsByType((ItemType)logicItemType)
							.Select(item => new ShopItem(item))
							.Cast<IShopItem>()
							.ToList();
		}

		public void OnCompleted()
		{
			WarehouseSubscriptionHandle.Dispose();
		}

		public void OnError(Exception error)
		{
			
		}

		public void OnNext(InflationChangedEventArgs value)
		{
			InflationChanged?.Invoke(this, new LogicInflationChangedEventArgs(value));
		}
	}
}