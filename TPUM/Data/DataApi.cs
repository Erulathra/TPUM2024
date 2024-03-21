namespace Data
{
	public class DataApi : DataAbstractApi
	{
		private readonly Shop shop;

		public DataApi()
		{
			shop = new Shop();
		}

		public override IShop GetShop()
		{
			return shop;
		}
	}
}