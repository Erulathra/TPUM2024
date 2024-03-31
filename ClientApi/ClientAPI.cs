using System;

namespace ClientApi
{
	[Serializable]
	public abstract class ServerCommand
	{
		public string Header;

		protected ServerCommand(string header)
		{
			Header = header;
		}
	}

	[Serializable]
	public class GetItemsCommand : ServerCommand
	{
		public static string StaticHeader = "GetItems";
		
		public GetItemsCommand()
		:base(StaticHeader)
		{
			
		}
	}
	
	[Serializable]
	public class SellItemCommand : ServerCommand
	{
		public static string StaticHeader = "SellItem";

		public Guid TransactionID;
		public Guid ItemID;
		
		public SellItemCommand(Guid id)
		:base(StaticHeader)
		{
			TransactionID = Guid.NewGuid();
			ItemID = id;
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
	
	[Serializable]
	public class TransactionResponse : ServerResponse
	{
		public static readonly string StaticHeader = "TransactionResponse";

		public Guid TransactionId;
		public bool Succeeded;

		public TransactionResponse()
			: base(StaticHeader)
		{
		}
		
	}
}