﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace Logic
{
	internal class Shop : IShop
	{
		private readonly IWarehouse warehouse;
		
		public event EventHandler<LogicInflationChangedEventArgs>? InflationChanged;

		public Shop(IWarehouse warehouse)
		{
			this.warehouse = warehouse;

			warehouse.InflationChanged += HandleOnInflationChanged;
		}

		private void HandleOnInflationChanged(object sender, InflationChangedEventArgs args)
		{
			InflationChanged?.Invoke(this, new LogicInflationChangedEventArgs(args));
		}

		public void SellItem(Guid itemId)
		{
			List<IShopItem> items = warehouse.GetItems()
											 .Where(item => !item.IsSold)
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
	}
}