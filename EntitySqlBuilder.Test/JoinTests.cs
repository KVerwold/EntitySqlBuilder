using EntitySqlBuilder.Test.Entities;
using NUnit.Framework;

namespace EntitySqlBuilder.Test
{
	public class JoinTests
	{

		[Test]
		public void Join_Customer_And_Customer()
		{
			var builder = new SqlBuilder<Customer>();

			const string compareSql = "select [c1].* from Customer [c1] " +
				"join Customer [c2] on (([c1].Id = [c2].Id) and ([c2].Age > 20))";
			var sql = builder.Select.All("c1").From(null,"c1")
				.Join<Customer>(alias:"c2").Expr<Customer, Customer>((c1, c2) => c1.Id == c2.Id && c2.Age > 20).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


		[Test]
		public void Join_Customer_And_CustomerGroup()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");

			const string compareSql = "select * from Customer [c] " +
				"join CustomerGroup [cg] on ([c].CustomerGroupId = [cg].Id)";
			var sql = builder.Select.All().From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Join_Customer_And_CustomerGroup_Select_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");

			const string compareSql = "select [c].* from Customer [c] " +
				"join CustomerGroup [cg] on ([c].CustomerGroupId = [cg].Id)";
			var sql = builder.Select.All<Customer>().From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Join_Customer_And_CustomerGroup_Select_Customer_LastName_CustomerGroup_Name()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");

			const string compareSql = "select [c].Lastname,[cg].Name from Customer [c] " +
				"join CustomerGroup [cg] on ([c].CustomerGroupId = [cg].Id)";
			var sql = builder.Select.Column(c=>c.Lastname).Column<CustomerGroup>(cg=>cg.Name).From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Join_Customer_And_CustomerGroup_Select_Customer_FirstName_LastName_CustomerGroup_Id_Name()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");

			const string compareSql = "select [c].Firstname,[c].Lastname,[cg].Id,[cg].Name from Customer [c] " +
				"join CustomerGroup [cg] on ([c].CustomerGroupId = [cg].Id)";
			var sql = builder.Select.Columns(c => c.Firstname, c=>c.Lastname).Columns<CustomerGroup>(cg => cg.Id, cg=>cg.Name).From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


		[Test]
		public void Join_Customer_And_CustomerGroup_On_Age_Larger_20()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");

			const string compareSql = "select * from Customer [c] " +
				"join CustomerGroup [cg] on (([c].CustomerGroupId = [cg].Id) and ([c].Age > 20))";
			var sql = builder.Select.All().From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id && c.Age>20).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Join_Customer_And_CustomerGroup_And_Order()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");
			builder.AddTableAlias<Order>("o");

			const string compareSql = "select * from Customer [c] " +
				"join CustomerGroup [cg] on ([c].CustomerGroupId = [cg].Id) " +
				"join Order [o] on ([c].Id = [o].CustomerId)";
			var sql = builder.Select.All().From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id)
				.Join<Order>().Expr<Customer, Order>((c, o) => c.Id == o.CustomerId)
				.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


		[Test]
		public void Join_Customer_And_CustomerGroup_And_Order_And_OrderLine()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");
			builder.AddTableAlias<Order>("o");
			builder.AddTableAlias<OrderLine>("ol");

			const string compareSql = "select * from Customer [c] " +
				"join CustomerGroup [cg] on ([c].CustomerGroupId = [cg].Id) " +
				"join Order [o] on ([c].Id = [o].CustomerId) " +
				"join OrderLine [ol] on ([o].Id = [ol].OrderId)";
			var sql = builder.Select.All().From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id)
				.Join<Order>().Expr<Customer, Order>((c, o) => c.Id == o.CustomerId)
				.Join<OrderLine>().Expr<Order, OrderLine>((o, ol) => o.Id == ol.OrderId)
				.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}



	}
}
