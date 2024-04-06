using System;
using System.ComponentModel.DataAnnotations;

namespace ConnectionApi
{
	[Serializable]
	public abstract class ServerCommand
	{
		public string Header { get; set; }

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

		public Guid TransactionID { get; set; }
		public Guid ItemID { get; set; }
		
		public SellItemCommand(Guid id)
		:base(StaticHeader)
		{
			TransactionID = Guid.NewGuid();
			ItemID = id;
		}
	}

	[Serializable]
	public class ItemDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Type { get; set; }
		public float Price { get; set; }
		public bool IsSold { get; set; }

		public ItemDTO()
		{
			Id = Guid.Empty;
			Name = "None";
			Description = "None";
			Type = "None";
			Price = 0f;
			IsSold = false;
		}

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
		public Guid ItemID { get; set; }
		public float NewPrice { get; set; }

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

		public ItemDTO[]? Items { get; set; }

		public UpdateAllResponse()
			: base(StaticHeader)
		{
		}
	}

	[Serializable]
	public class InflationChangedResponse : ServerResponse
	{
		public static readonly string StaticHeader = "InflationChanged";

		public float NewInflation { get; set; }
		public NewPriceDTO[]? NewPrices { get; set; }

		public InflationChangedResponse()
			: base(StaticHeader)
		{
		}
		
	}
	
	[Serializable]
	public class TransactionResponse : ServerResponse
	{
		public static readonly string StaticHeader = "TransactionResponse";

		public Guid TransactionId { get; set; }
		public bool Succeeded { get; set; }

		public TransactionResponse()
			: base(StaticHeader)
		{
		}
		
	}
}