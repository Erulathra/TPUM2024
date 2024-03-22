using System;
using System.Collections.Generic;

namespace Data
{
	public enum ItemType 
	{
		Potion = 0,
		Sword = 1,
		Armor = 2,
		Helmet = 3
	}
	
    public interface IItem
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }

        ItemType Type { get; }
        float Price { get; set; }

        bool IsSold { get; set;  }
    }
    
    public interface IWarehouse
    {
	    public IItem CreateItem(string name, string description, ItemType type, float price);

	    public void SellItem(Guid itemId);
	    
	    public List<IItem> GetItems();
	    public List<IItem> GetAvailableItems();
	    
	    public void AddItem(IItem itemToAdd);
	    public void RemoveItem(Guid itemIdToRemove);

	    public IItem GetItemByID(Guid guid);
	    public List<IItem> GetItemsByType(ItemType type);

    }

    public abstract class DataAbstractApi
    {
	    public static DataAbstractApi Create()
	    {
		    return new DataApi();
	    }

	    public abstract IWarehouse GetWarehouse();
    }
}
