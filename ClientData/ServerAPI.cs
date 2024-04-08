using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConnectionApi
{
	internal static class ServerStatics
	{
		public static readonly string GetItemsCommandHeader = "GetItems";
		public static readonly string SellItemCommandHeader = "SellItem";
		
		public static readonly string UpdateAllResponseHeader = "UpdateAllItems";
		public static readonly string InflationChangedResponseHeader = "InflationChanged";
		public static readonly string TransactionResponseHeader = "TransactionResponse";
	}
	
	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal abstract class ServerCommand
	{
		[JsonProperty("Header", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Header { get; set; }
	}

    [GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    internal partial class GetItemsCommand : ServerCommand
    {

    }

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class SellItemCommand : ServerCommand
	{
		[JsonProperty("TransactionID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid TransactionID { get; set; }

		[JsonProperty("ItemID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid ItemID { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class ItemDTO
	{
		[JsonProperty("Id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid Id { get; set; }

		[JsonProperty("Name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Name { get; set; }

		[JsonProperty("Description", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Description { get; set; }

		[JsonProperty("Type", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Type { get; set; }

		[JsonProperty("Price", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public float Price { get; set; }

		[JsonProperty("IsSold", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public bool IsSold { get; set; }
	}


	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class NewPriceDTO
	{
		[JsonProperty("ItemID", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid ItemID { get; set; }

		[JsonProperty("NewPrice", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public float NewPrice { get; set; }
	}


	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal abstract class ServerResponse
	{
		[JsonProperty("Header", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public string Header { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class UpdateAllResponse : ServerResponse
	{
		[JsonProperty("Items", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
		public ICollection<ItemDTO> Items { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class InflationChangedResponse : ServerResponse
	{
		[JsonProperty("NewInflation", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public float NewInflation { get; set; }

		[JsonProperty("NewPrices", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
		public ICollection<NewPriceDTO> NewPrices { get; set; }
	}

	[GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
	internal class TransactionResponse : ServerResponse
	{
		[JsonProperty("TransactionId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public Guid TransactionId { get; set; }

		[JsonProperty("Succeeded", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
		public bool Succeeded { get; set; }
	}
}