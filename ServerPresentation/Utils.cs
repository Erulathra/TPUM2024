using System;
using System.Text;
using ConnectionApi;
using Logic;

namespace ServerPresentation
{
	internal static class Utils
	{
		
		public static ItemDTO ToDTO(this IShopItem item)
		{
			return new ItemDTO(
				item.Id,
				item.Name,
				item.Description,
				item.Type.ToString(),
				item.Price,
				item.IsSold
			);
		}
	}
}