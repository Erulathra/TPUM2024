using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientData
{
	public enum ItemType 
	{
		Potion = 0,
		Sword = 1,
		Armor = 2,
		Helmet = 3
	}
	
    public interface IItem : ICloneable
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
	    /** Called when inflation changes */
	    public event EventHandler<InflationChangedEventArgs> InflationChanged;
	    
	    public void SellItem(Guid itemId);
	    
	    public List<IItem> GetItems();
	    public List<IItem> GetAvailableItems();
	    
	    public IItem GetItemByID(Guid guid);
	    public List<IItem> GetItemsByType(ItemType type);

    }

    public interface IConnectionService
    {
	    public event Action<string>? Logger;
	    public event Action? OnConnectionStateChanged;

	    public event Action<string>? OnMessage;
	    public event Action? OnError;
	    public event Action? OnDisconnect;
	    
	    
	    public Task Connect(Uri peerUri);
	    public Task Disconnect();
	    
	    public bool IsConnected();
    }

    public abstract class DataAbstractApi
    {
	    public static DataAbstractApi Create(IConnectionService? connectionService)
	    {
		    return new DataApi(connectionService);
	    }

	    public abstract IWarehouse GetWarehouse();
	    public abstract IConnectionService GetConnectionService();
    }

    public class InflationChangedEventArgs : EventArgs
    {
	    public float NewInflation { get; }

	    public InflationChangedEventArgs(float newInflation)
	    {
		    this.NewInflation = newInflation;
	    }
    }
}
