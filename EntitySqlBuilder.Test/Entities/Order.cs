using System;


namespace EntitySqlBuilder.Test.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int CustomerId { get; set; }
	}
}
