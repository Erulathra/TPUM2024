using System;
using System.Text;

namespace ClientApi
{
	public static class Utils
	{
		public static ArraySegment<byte> GetArraySegment(this string message)
		{
			byte[] buffer = Encoding.UTF8.GetBytes(message);
			return new ArraySegment<byte>(buffer);
		}
	}
}