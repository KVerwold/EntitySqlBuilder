using EntitySqlBuilder.Test.Entities;
using NUnit.Framework;


namespace EntitySqlBuilder.Test
{
	public class OrderByTests
	{

		[Test]
		public void Select_All_From_Customers_OrderBy_LastName()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select * from Customer order by Lastname asc";
			var sql = builder.Select.All().From().OrderBy.Column(c => c.Lastname).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_All_From_Customers_OrderBy_Age_Descending()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select * from Customer order by Age desc";
			var sql = builder.Select.All().From().OrderBy.Column(c => c.Age, false).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_All_From_Customers_OrderBy_Lastname_Ascending_Age_Descending()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select * from Customer order by LastName asc,Age desc";
			var sql = builder.Select.All().From().OrderBy.Column(c=>c.Lastname).Column(c => c.Age, false).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_All_From_Customers_OrderBy_Lastname_Age_Ascending()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select * from Customer order by LastName asc,Age asc";
			var sql = builder.Select.All().From().OrderBy.Columns(c => c.Lastname, c=>c.Age).ToString();
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
			var sql = builder.Select.Column(c => c.Lastname).Column<CustomerGroup>(cg => cg.Name).From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}




	}
}
