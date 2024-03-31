using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientData;

namespace Logic
{
	internal class Shop : IShop
	{
		private readonly IWarehouse warehouse;
		
		public event EventHandler<LogicInflationChangedEventArgs>? InflationChanged;
		public event Action? ItemsUpdated;
		public event Action<bool>? TransactionFinish;

		public Shop(IWarehouse warehouse)
		{
			this.warehouse = warehouse;

			warehouse.InflationChanged += HandleOnInflationChanged;
			warehouse.ItemsUpdated += () => ItemsUpdated?.Invoke();
			warehouse.TransactionFinish += (bool succeeded) => TransactionFinish?.Invoke(succeeded);
		}

		private void HandleOnInflationChanged(object sender, InflationChangedEventArgs args)
		{
			InflationChanged?.Invoke(this, new LogicInflationChangedEventArgs(args));
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
	}
}