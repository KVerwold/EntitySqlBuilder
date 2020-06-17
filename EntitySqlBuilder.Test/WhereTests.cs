using EntitySqlBuilder.Test.Entities;
using NUnit.Framework;

namespace EntitySqlBuilder.Test
{
	public class WhereTests
	{

		[Test]
		public void Where_1_Equals_1()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT * FROM Customer WHERE 1=1";
			var sql = builder.Select.All().From()
									.Where.Expr(c=>1==1)
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


		[Test]
		public void Where_Lastname_Equals_Doe()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT * FROM Customer WHERE (Lastname = 'Doe')";
			var sql = builder.Select.All().From()
									.Where.Expr(c => c.Lastname == "Doe")
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Where_Lastname_Equals_Doe_WithAlias()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("cust");
			const string compareSql = "SELECT * FROM Customer [cust] WHERE ([cust].Lastname = 'Doe')";
			var sql = builder.Select.All().From()
									.Where.Expr(c => c.Lastname == "Doe")
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Where_Lastname_Equals_Doe_And_Firstname_Equals_John()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT * FROM Customer WHERE ((Lastname = 'Doe') AND (Firstname = 'John'))";
			var sql = builder.Select.All().From()
									.Where.Expr(c => c.Lastname == "Doe" && c.Firstname == "John")
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Where_Firstname_Equals_John_Or_Firstname_Equals_Jill()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT * FROM Customer WHERE ((Firstname = 'John') OR (Firstname = 'Jill'))";
			var sql = builder.Select.All().From()
									.Where.Expr(c => c.Firstname == "John" || c.Firstname == "Jill")
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Where_Age_Equals_LocalVariable()
		{
			var builder = new SqlBuilder<Customer>();
			var age = 40;
			const string compareSql = "SELECT * FROM Customer WHERE (Age = 40)";
			var sql = builder.Select.All().From()
									.Where.Expr(c => c.Age == age)
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Where_Firstname_Equals_LocalVariable()
		{
			var builder = new SqlBuilder<Customer>();
			var name = "John";
			const string compareSql = "SELECT * FROM Customer WHERE (Firstname = 'John')";
			var sql = builder.Select.All().From()
									.Where.Expr(c => c.Firstname == name)
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Where_Lastname_Equals_Doe_And_NestedCondition_Firstname_Equals_John_Or_Firstname_Equals_Jill()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT * FROM Customer WHERE ((Lastname = 'Doe') AND ((Firstname = 'John') OR (Firstname = 'Jill')))";
			var sql =
				builder.Select.All().From()
					.Where.Expr(c => c.Lastname == "Doe" && (c.Firstname == "John" || c.Firstname == "Jill"))
					.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Where_Customer_Age_Greater_20_And_CustomerGroup_Name_Equals_Gold()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("c");
			builder.AddTableAlias<CustomerGroup>("cg");

			const string compareSql = 
				"select * from Customer [c] " +
				"join CustomerGroup [cg] on ([c].CustomerGroupId = [cg].Id) " +
				"where (([c].Age > 20) and ([cg].Name = 'Gold'))";
			var sql = builder.Select.All().From()
				.Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id)
				.Where.Expr<Customer,CustomerGroup>((c,cg)=> c.Age>20 && cg.Name=="Gold")
				.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}



	}
}
