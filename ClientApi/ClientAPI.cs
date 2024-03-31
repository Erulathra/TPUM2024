using System;

namespace ClientApi
{
	[Serializable]
	public struct ServerCommand
	{
		public string Command;

		private ServerCommand(string command)
		{
			Command = command;
		}

		public static ServerCommand GetItemsRequest()
		{
			return new ServerCommand("GetItems");
		}
	}

	[Serializable]
	public struct ItemDTO
	{
		public Guid Id;
		public string Name;
		public string Description;
		public string Type;
		public float Price;
		public bool IsSold;

		public ItemDTO(Guid id, string name, string description, string type, float price, bool isSold)
		{
			Id = id;
			Name = name;
			Description = description;
			Type = type;
			Price = price;
			IsSold = isSold;
		}
	}

	[Serializable]
	public struct NewPriceDTO
	{
		public Guid ItemID;
		public float NewPrice;

		public NewPriceDTO(Guid itemId, float newPrice)
		{
			ItemID = itemId;
			NewPrice = newPrice;
		}
	}

	[Serializable]
	public abstract class ServerResponse
	{
		public string Header { get; private set; }

		protected ServerResponse(string header)
		{
			Header = header;
		}
	}
	
	[Serializable]
	public class UpdateAllResponse : ServerResponse
	{
		public static readonly string StaticHeader = "UpdateAllItems";

		public ItemDTO[]? Items;

		public UpdateAllResponse()
			: base(StaticHeader)
		{
		}
	}

	[Serializable]
	public class InflationChangedResponse : ServerResponse
	{
		public static readonly string StaticHeader = "InflationChanged";

		public float NewInflation;
		public NewPriceDTO[]? NewPrices;

		public InflationChangedResponse()
			: base(StaticHeader)
		{
		}
		
	}
}