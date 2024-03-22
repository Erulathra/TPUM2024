using System;

namespace Data
{
	public class Item : IItem
	{
		public Guid Id { get; }
		public string Name { get; }
		public string Description { get; }
		public ItemType Type { get; }
		public float Price { get; set; }
		public bool IsSold { get; set; }


		public Item(string name, string description, ItemType type, float price)
		{
			Id = Guid.NewGuid();
			Name = name;
			Description = description;
			Type = type;
			Price = price;
			IsSold = false;
		}
	}
}