﻿using System;
using System.Collections.Generic;
using Data;

namespace LogicTest
{
	public class DataApiMock : DataAbstractApi
	{
		private readonly WarehouseMock warehouseMock = new WarehouseMock();

		public override IWarehouse GetWarehouse()
		{
			return warehouseMock;
		}
	}

	public class ItemMock : IItem
	{
		public ItemMock(string name, string description, ItemType type, float price, bool isSold)
		{
			Id = Guid.NewGuid();
			Name = name;
			Description = description;
			Type = type;
			Price = price;
			IsSold = isSold;
		}

		public Guid Id { get; }
		public string Name { get; }
		public string Description { get; }
		public ItemType Type { get; }
		public float Price { get; set; }
		public bool IsSold { get; set; }
		public object Clone()
		{
			throw new NotImplementedException();
		}
	}

	public class WarehouseMock : IWarehouse
	{
		private readonly List<IItem> allItems;
		private readonly List<IItem> availableItems;

		public WarehouseMock()
		{
			availableItems = new List<IItem>
			{
				new ItemMock("Test", "TestDesc", ItemType.Armor, 10f, false),
				new ItemMock("Test2", "TestDesc2", ItemType.Armor, 11f, false)
			};
			allItems = new List<IItem>
			{
				new ItemMock("Sold", "SoldDesc2", ItemType.Helmet, 11f, true)
			};
			allItems.AddRange(availableItems);
		}

		public void SellItem(Guid itemId)
		{
			IItem? item = availableItems.Find((item) => item.Id == itemId);
			if (item != null)
			{
				availableItems.Remove(item);
			}
		}

		public List<IItem> GetItems()
		{
			return allItems;
		}

		public List<IItem> GetAvailableItems()
		{
			return availableItems;
		}

		public float GetCurrentInflation()
		{
			return 1f;
		}

		public event EventHandler<InflationChangedEventArgs>? InflationChanged;

		public IItem CreateItem(string name, string description, ItemType type, float price)
		{
			throw new NotImplementedException();
		}

		public void AddItem(IItem itemToAdd)
		{
			throw new NotImplementedException();
		}

		public void RemoveItem(Guid itemIdToRemove)
		{
			throw new NotImplementedException();
		}

		public IItem GetItemByID(Guid guid)
		{
			throw new NotImplementedException();
		}

		public List<IItem> GetItemsByType(ItemType type)
		{
			if (type == ItemType.Armor)
			{
				return availableItems;
			}

			return new List<IItem>();
		}
	}
}
