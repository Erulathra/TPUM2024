namespace ClientData
{
	internal class DataApi : DataAbstractApi
	{
		private readonly Warehouse warehouse;
		private readonly ConnectionService connectionService;

		public DataApi()
		{
			connectionService = new ConnectionService();
			warehouse = new Warehouse(connectionService);
		}

		public override IWarehouse GetWarehouse()
		{
			return warehouse;
		}

		public override IConnectionService GetConnectionService()
		{
			return connectionService;
		}
	}
}