using System;
using ClientApi;

namespace ClientData
{
	internal class Item : IItem, ICloneable
	{
		public Guid Id { get; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public ItemType Type { get; }
		public float Price { get; set; }
		public bool IsSold { get; set; }


		public Item(Guid id, string name, string description, ItemType type, float price, bool isSold)
		{
			Id = id;
			Name = name;
			Description = description;
			Type = type;
			Price = price;
			IsSold = isSold;
		}

		public object Clone()
		{
			Item clone = (Item)MemberwiseClone();
			clone.Name = string.Copy(Name);
			clone.Description = string.Copy(Description);
			return clone;
		}

		protected bool Equals(Item other)
		{
			return Id.Equals(other.Id) && Name == other.Name && Description == other.Description && Type == other.Type && Price.Equals(other.Price) && IsSold == other.IsSold;
		}

		public override bool Equals(object? obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != this.GetType())
				return false;
			return Equals((Item)obj);
		}

		public override int GetHashCode()
		{
			int hashCode = Id.GetHashCode();
			hashCode = (hashCode * 397) ^ Name.GetHashCode();
			hashCode = (hashCode * 397) ^ Description.GetHashCode();
			hashCode = (hashCode * 397) ^ (int)Type;
			hashCode = (hashCode * 397) ^ Price.GetHashCode();
			hashCode = (hashCode * 397) ^ IsSold.GetHashCode();
			return hashCode;
		}
	}
}