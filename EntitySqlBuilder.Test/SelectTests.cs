using EntitySqlBuilder.Test.Entities;
using NUnit.Framework;

namespace EntitySqlBuilder.Test
{
	/// <summary>
	/// Some SQL samples are taken from http://msdn.microsoft.com/en-us/library/ms187731.aspx for AdventureWorks database
	/// </summary>
	public class SelectTests
	{

		[Test]
		public void Select_All_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select * from Customer";
			var sql = builder.Select.All().From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Lastname_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select Lastname from Customer";
			var sql = builder.Select.Column(c=> c.Lastname).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Lastname_As_Name_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select Lastname as [Name] from Customer";
			var sql = builder.Select.Column(c => c.Lastname, "Name").From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Firstname_Lastname_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select Firstname,Lastname from Customer";
			var sql = builder.Select.Columns(c=>c.Firstname, c=>c.Lastname).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Firstname_Lastname_From_Customer_1()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select Firstname,Lastname from Customer";
			var sql = builder.Select.Column(c => c.Firstname).Column(c=>c.Lastname).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Firstname_Lastname_As_Name_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select Firstname+' '+Lastname as Name from Customer";
			var sql = builder.Select.Column("{0}+' '+{1} as Name", c=>c.Firstname, c=>c.Lastname).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_All_From_Customer_WithAlias()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select * from Customer [cust]";
			builder.AddTableAlias<Customer>("cust");
			var sql = builder.Select.All().From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_All_From_Customer_WithAlias_1()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select [cust].* from Customer [cust]";
			builder.AddTableAlias<Customer>("cust");
			var sql = builder.Select.All<Customer>().From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_All_From_Customer_WithAlias_2()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select [cust].* from Customer [cust]";
			builder.AddTableAlias<Customer>("cust");
			var sql = builder.Select.All("cust").From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


		[Test]
		public void Select_Firstname_Lastname_From_Customer_WithAlias()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("cust");
			const string compareSql = "select [cust].Firstname,[cust].Lastname from Customer [cust]";
			var sql = builder.Select.Columns(c => c.Firstname, c => c.Lastname).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Firstname_Lastname_From_Customer_WithAlias_1()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "select [cust].Firstname,[cust].Lastname from Customer [cust]";
			var sql = builder.Select.Column(true, cust => cust.Firstname).Column(true, cust => cust.Lastname).From(alias:"cust").ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_All_From_Customer_ReUseBuilder_Select_Firstname_LastName_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			var compareSql = "select * from Customer";
			var sql = builder.Select.All().From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);

			builder.Clear();

			compareSql = "select Firstname,Lastname from Customer";
			sql = builder.Select.Columns(c => c.Firstname, c => c.Lastname).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Firstname_Lastname_From_Customer_WithTableNotation()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT Firstname,Lastname FROM Northwind.dbo.Customer";
			var sql = builder.Select.Columns(p => p.Firstname, p => p.Lastname).From("Northwind.dbo.Customer").ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Count_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT COUNT(*) AS [Total] FROM Customer";
			var sql = builder.Select.Column("COUNT(*) AS [Total]").From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Average_Age_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT AVG(Age) AS [AverageAge] FROM Customer";
			var sql = builder.Select.Column("AVG({0}) AS [AverageAge]", c=>c.Age).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Min_Age_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT MIN(age) AS [MinAge] FROM Customer";
			var sql = builder.Select.Column("MIN({0}) AS [MinAge]", c => c.Age).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Max_Age_From_Customer_WithAlias()
		{
			var builder = new SqlBuilder<Customer>();
			builder.AddTableAlias<Customer>("cust");
			const string compareSql = "SELECT MAX([cust].Age) AS [MaxAge] FROM Customer [cust]";
			var sql = builder.Select.Column("MAX({0}) AS [MaxAge]", c => c.Age).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


		[Test]
		public void Select_Distinct_Lastname_From_Customer()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT Distinct Lastname FROM Customer";
			var sql = builder.Select.Distinct.Column(c => c.Lastname).From().ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


		[Test]
		public void Select_Name_ProductNumber_ListPrice_As_Price_From_Product_WithTableNotation()
		{
			var builder = new SqlBuilder<Product>();
			const string compareSql = "SELECT Name,ProductNumber,ListPrice AS [Price] FROM Northwind.dbo.Product";
			var sql = builder.Select.Column("{0},{1},{2} AS [Price]", p => p.Name, p => p.ProductNumber, p => p.ListPrice).From("Northwind.dbo.Product").ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Name_ProductNumber_ListPrice_As_Price_From_Product_WithTableNotation_1()
		{
			var builder = new SqlBuilder<Product>();
			const string compareSql = "SELECT Name,ProductNumber,ListPrice AS [Price] FROM Northwind.dbo.Product";
			var sql = builder.Select.Column("{0}", p => p.Name).Column("{0}", p => p.ProductNumber).Column("{0} AS [Price]", p => p.ListPrice).From("Northwind.dbo.Product").ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void Select_Name_ProductNumber_ListPrice_As_Price_From_Product_WithTableNotation_2()
		{
			var builder = new SqlBuilder<Product>();
			const string compareSql = "SELECT Name,ProductNumber,ListPrice AS [Price] FROM Northwind.dbo.Product";
			var sql = builder.Select.Column("{0}", p => p.Name).Column(c => c.ProductNumber).Column("{0} AS [Price]", p => p.ListPrice).From("Northwind.dbo.Product").ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}



		//		static void Main(string[] args)
		//		{
		//			var age = 40;

		//			_stopWatch = new Stopwatch();
		//			_sqlCustomerBuilder = new SqlBuilder<Customer>();
		//			_sqlCustomerBuilder.AutoClauseFormatting = true;

		//			Start();
		//			Write(_sqlCustomerBuilder
		//									.Select.All().From().Where.Text("Isnull({0},'')=''",c=>c.Firstname)
		//									.ToString());

		//			Start();
		//			Write(_sqlCustomerBuilder
		//									.Select.Column(c => c.Firstname, c => c.Lastname)
		//									.From()
		//									.Where.Expr(c => c.Age == age)
		//									.And.OpenBracket
		//												.Expr(c => c.Firstname == "Lars").Or.Expr(c => c.Lastname == "Hansen")
		//											.CloseBracket
		//									.ToString());

		//			Start();
		//			Write(_sqlCustomerBuilder
		//							.Select.Text("Max({0}) as MaxAge",c => c.Age)
		//							.From()
		//							.Where.Expr(c => c.Lastname == "Hansen")
		//							.ToString());

		//			Start();
		//			Write(_sqlCustomerBuilder
		//								.Select.Column(c => c.Lastname).Max(c => c.Age, "MaxAge")
		//								.From()
		//								.Where.Like(c => c.Lastname, "H%")
		//								.GroupBy.Column(c => c.Lastname)
		//								.ToString());

		//			Start();
		//			Write(_sqlCustomerBuilder
		//							.Select.Column(c => c.Lastname).Max(c => c.Age, "MaxAge")
		//							.From()
		//							.Where.Like(c => c.Lastname, "H%")
		//							.GroupBy.Column(c => c.Lastname)
		//							.Having.Max(c => c.Age, "> 50")
		//							.ToString());

		//			Start();
		//			Write(_sqlCustomerBuilder
		//							.Select.Column(c => c.Age, "(Select TOP 1 Age from OTHER_TABLE)").Column(c => c.Lastname, "MyLastname")
		//							.From()
		//							.Where.Like(c => c.Lastname, "H%")
		//							.GroupBy.Column(c => c.Lastname)
		//							.Having.Max(c => c.Age, "> 50")
		//							.ToString());

		//			Start();
		//			Write(_sqlCustomerBuilder.Select.All
		//				.From()
		//				.Where.Expr(c => 1 == 1)
		//				.OrderBy.Column(c => c.Lastname, "desc").Column(c => c.Firstname)
		//				.ToString());

		//			Start();
		//			_sqlCustomerBuilder.Alias<Customer>("t1");
		//			_sqlCustomerBuilder.Alias<Person>("p1");
		//			Write(_sqlCustomerBuilder
		//				.Select.Column(c => c.Firstname).Column(c => c.Lastname).JoinedColumn((Person p) => p.Name)
		//				.From()
		//				.Join<Person>().Column(c => c.Id).Equal.JoinedColumn((Person p) => p.Id).ToString());

		//		//			select 
		//		//				t1.Id,
		//		//				t1.StartedAt,
		//		//				t2.Name,
		//		//				t1.Contact,
		//		//				t1.CategoryId,
		//		//				t1.Problem,
		//		//				t1.Status,
		//		//				t1.UsedTimeMinutes
		//		//			from Task t1
		//		//			join Customer t2 on t1.CustomerId = t2.Id 
		//		//			where t1.Status <> 'lukket' and GroupID = 'Profil Optik butikker';

		//			var sqlBuilder2 = new SqlBuilder<Task>();
		//			_sqlCustomerBuilder.AutoClauseFormatting = true;
		//			sqlBuilder2.Alias<Task>("t1");
		//			sqlBuilder2.Alias<Customer>("t2");
		//			Write(sqlBuilder2
		//						.Select
		//						.Column(t=>t.Id)
		//						.Column(t=>t.StartdAt)
		//						.JoinedColumn((Customer c) => c.Name)
		//						.Column(t => t.Contact)
		//						.Column(t => t.CategoryId)
		//						.Column(t => t.Problem)
		//						.Column(t => t.Status)
		//						.Column(t => t.UsedTimeMinutes)
		//						.From()
		//						.Join<Customer>().Column(t => t.CustomerId).Equal.JoinedColumn((Customer c) => c.Id)
		//						.Where.Expr(t => t.Status != "lukket").And.Expr((Customer c) => c.GroupID == "Profil Optik butikker").ToString());


		////			select
		////				Date=t1.StartedAt
		////				,Shop=t2.Name
		////				,Category=t1.CategoryId
		////				,t1.Problem
		////				,t1.Status
		////				,Closed=t1.EndedAt
		////				,t1.UsedTimeMinutes
		////			from task t1
		////			join Customer t2 on t1.CustomerId = t2.Id
		////			where
		////				t2.GroupID = 'Profil Optik butikker'
		////			  and (isnull(@Shop,'') = '' or @Shop = t2.Name)
		////			  and (isnull(@Category,'') = ''or @Category = t1.CategoryId)
		////				and (@FromDate is null or @ToDate is null or (t1.StartedAt >= @FromDate and t1.StartedAt <= @ToDate)) 
		////			order by t1.StartedAt desc;


		//			sqlBuilder2.Clear();
		//			sqlBuilder2.Alias<Task>("t1");
		//			sqlBuilder2.Alias<Customer>("t2");

		//			Write(sqlBuilder2
		//				.Select
		//				.Column(t => t.StartdAt, "Date")
		//				.JoinedColumn((Customer c) => c.Name, "Shop")
		//				.Column(t => t.CategoryId, "Category")
		//				.Column(t => t.Problem)
		//				.Column(t => t.Status)
		//				.Column(t => t.EndedAt, "Closed")
		//				.Column(t => t.UsedTimeMinutes)
		//				.From()
		//				.Join<Customer>().Column(t => t.CustomerId).Equal.JoinedColumn((Customer c) => c.Id)
		//				.Where
		//					.Expr((Customer c)=>c.GroupID == "Profil Optik butikker")
		//					.And.OpenBracket.Text("isnull(@Shop,'')").Equal.EmptyString.Or.JoinedColumn((Customer c) => c.Name).Equal.Text("@Shop").CloseBracket
		//					.And.OpenBracket.Text("isnull(@Category,'')").Equal.EmptyString.Or.Column(t => t.CategoryId).Equal.Text("@Category").CloseBracket
		//					.And.OpenBracket
		//								.Text("@FromDate").IsNull.Or.Text("@ToDate").IsNull.Or
		//								.OpenBracket
		//									.Column(t => t.StartdAt).GreaterEqual.Text("@FromDate").And.Column(t => t.StartdAt).LessEqual.Text("@ToDate")
		//								.CloseBracket
		//							.CloseBracket
		//				.OrderBy.Column(t => t.StartdAt, "desc")
		//				.ToString()
		//				);
		//			Console.ReadKey();
		//		}
	}
}
