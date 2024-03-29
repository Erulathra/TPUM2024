using ClientData;
using Logic;

namespace ClientLogic
{
	
	internal class Logic : LogicAbstractApi
	{
		private readonly IShop shop;
		private readonly ILogicConnectionService connectionService;

		public Logic(DataAbstractApi dataApi)
			: base(dataApi)
		{
			shop = new Shop(dataApi.GetWarehouse());
			connectionService = new LogicConnectionService(dataApi.GetConnectionService());
		}

		public override IShop GetShop()
		{
			return shop;
		}

		public override ILogicConnectionService GetConnectionService()
		{
			return connectionService;
		}
	}
}