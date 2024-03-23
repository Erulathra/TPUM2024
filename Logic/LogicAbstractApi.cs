using System;
using System.Collections.Generic;
using Data;

namespace Logic
{
	public class LogicInflationChangedEventArgs : EventArgs
	{
	    public float NewInflation { get; }

	    public LogicInflationChangedEventArgs(float newInflation)
	    {
		    this.NewInflation = newInflation;
	    }

	    internal LogicInflationChangedEventArgs(InflationChangedEventArgs args)
	    {
		    this.NewInflation = args.NewInflation;
	    }
	}
	
	public enum LogicItemType 
	{
		Potion = 0,
		Sword = 1,
		Armor = 2,
		Helmet = 3
	}
	
	public interface IShopItem
	{
        Guid Id { get; }
        string Name { get; }
        string Description { get; }

        LogicItemType Type { get; }
        float Price { get; }

        bool IsSold { get; }
	}

	public interface IShop
	{
		public event EventHandler<LogicInflationChangedEventArgs> InflationChanged;
		
	    public void SellItem(Guid itemId);
	    
	    public List<IShopItem> GetItems();
	    public List<IShopItem> GetAvailableItems();
	    public List<IShopItem> GetItemsByType(LogicItemType logicItemType);
	}

	public abstract class LogicAbstractApi
	{
		public DataAbstractApi DataApi { get; private set; }
		
		public LogicAbstractApi(DataAbstractApi dataApi)
		{
			DataApi = dataApi;
		}

		public static LogicAbstractApi Create(DataAbstractApi? dataAbstractApi = null)
		{
			DataAbstractApi dataApi = dataAbstractApi ?? DataAbstractApi.Create();
			return new Logic(dataApi);
		}

		public abstract IShop GetShop();
	}
}