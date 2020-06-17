using System;
using System.Collections.Generic;
using System.Text;

namespace EntitySqlBuilder.Test.Entities
{
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int ProductNumber { get; set; }
		public decimal ListPrice { get; set; }
	}
}
