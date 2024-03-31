using System;
using System.Text;
using ClientApi;
using Logic;

namespace ServerPresentation
{
	internal static class Utils
	{
		public static ArraySegment<byte> GetArraySegment(this string message)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(message);
			return new ArraySegment<byte>(buffer);
		}
		
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