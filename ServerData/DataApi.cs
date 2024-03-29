namespace Data
{
	internal class DataApi : DataAbstractApi
	{
		private readonly Warehouse warehouse;

		public DataApi()
		{
			warehouse = new Warehouse();
		}

		public override IWarehouse GetWarehouse()
		{
			return warehouse;
		}

	}
}