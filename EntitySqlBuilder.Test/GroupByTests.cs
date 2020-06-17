using NUnit.Framework;
using EntitySqlBuilder.Test.Entities;

namespace EntitySqlBuilder.Test
{
	[TestFixture]
	public class GroupByTests
	{
		[Test]
		public void GroupBy_Lastname()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT Lastname,COUNT(*) AS [Cnt] FROM Customer GROUP BY Lastname";
			var sql = builder.Select.Columns(c => c.Lastname).Column("COUNT(*) AS [Cnt]")
									.From()
									.GroupBy.Columns(c => c.Lastname).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}

		[Test]
		public void GroupBy_Lastname_Age()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT Lastname,Age,COUNT(*) AS [Cnt] FROM Customer GROUP BY Lastname,Age";
			var sql = builder.Select.Columns(c=>c.Lastname, c=>c.Age).Column("COUNT(*) AS [Cnt]")
									.From()
									.GroupBy.Columns(c => c.Lastname, c=>c.Age).ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}


	}
}
