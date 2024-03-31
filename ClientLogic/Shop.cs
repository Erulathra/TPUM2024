﻿using System;
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

		public Shop(IWarehouse warehouse)
		{
			this.warehouse = warehouse;

			warehouse.InflationChanged += HandleOnInflationChanged;
			warehouse.ItemsUpdated += () => ItemsUpdated?.Invoke();
		}

		private void HandleOnInflationChanged(object sender, InflationChangedEventArgs args)
		{
			InflationChanged?.Invoke(this, new LogicInflationChangedEventArgs(args));
		}

		public void RequestUpdate()
		{
			warehouse.RequestUpdate();
		}

		public void SellItem(Guid itemId)
		{
			List<IShopItem> items = warehouse.GetAvailableItems()
											 .Select(item => new ShopItem(item))
											 .Cast<IShopItem>()
											 .ToList();
			IShopItem? foundItem = items.Find((item) => item.Id == itemId);
			if (foundItem != null && !foundItem.IsSold)
			{
				warehouse.SellItem(foundItem.Id);
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