namespace ClientData
{
	internal class DataApi : DataAbstractApi
	{
		private readonly Warehouse warehouse;
		private readonly IConnectionService connectionService;

		public DataApi(IConnectionService? connectionService)
		{
			this.connectionService = connectionService ?? new ConnectionService();
			warehouse = new Warehouse(this.connectionService);
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