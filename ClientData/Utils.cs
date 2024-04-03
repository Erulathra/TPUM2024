using System;
using System.Text;
using ClientApi;

namespace ClientData
{
	internal static class Utils
	{
		public static ItemType ItemTypeFromString(string typeAsString)
		{
			return (ItemType)Enum.Parse(typeof(ItemType), typeAsString);
		}
		
		public static string ToString(this ItemType typeAsString)
		{
			return Enum.GetName(typeof(ItemType), typeAsString) ?? throw new InvalidOperationException();
		}
		
		public static IItem ToItem(this ItemDTO itemDTO)
		{
			return new Item(
				itemDTO.Id,
				itemDTO.Name,
				itemDTO.Description,
				ItemTypeFromString(itemDTO.Type),
				itemDTO.Price,
				itemDTO.IsSold
			);
		}
	}
}