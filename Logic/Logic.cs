using System;
using Data;

namespace Logic
{
	
	public class Logic : LogicAbstractApi
	{
		private readonly IShop shop;

		public Logic(DataAbstractApi dataApi)
			: base(dataApi)
		{
			shop = new Shop(dataApi.GetWarehouse());
		}

		public override IShop GetShop()
		{
			return shop;
		}
	}
}