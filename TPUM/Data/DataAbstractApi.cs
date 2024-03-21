using System;
using System.Collections.Generic;

namespace Data
{
	public enum ItemType 
	{
		Potion,
		Sword,
		Wand,
		Armor
	}
	
    public interface IItem
    {
        Guid Id { get; }
        string Name { get; }
        string Description { get; }

        ItemType Type { get; }
        float Price { get; }
        
        bool IsSold { get; }
    }
    
    public interface IShop
    {
	    public List<IItem> GetItems();
	    public List<IItem> GetAvailableItems();
	    
	    public void AddItem(IItem itemToAdd);
	    public void RemoveItem(IItem itemToRemove);

	    public IItem GetItemByID(Guid guid);
	    public List<IItem> GetItemsByType(ItemType type);
    }

    public abstract class DataAbstractApi
    {
	    public virtual DataAbstractApi Create()
	    {
		    return new DataApi();
	    }

	    public abstract IShop GetShop();
    }
}
