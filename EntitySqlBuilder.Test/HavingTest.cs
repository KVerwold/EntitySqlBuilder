using NUnit.Framework;
using EntitySqlBuilder.Test.Entities;

namespace EntitySqlBuilder.Test
{
	
	public class HavingTest
	{
		[Test]
		public void Having_Age_Greater_60()
		{
			var builder = new SqlBuilder<Customer>();
			const string compareSql = "SELECT Lastname,Avg(Age) AS [AverageAge] FROM Customer GROUP BY Lastname Having Avg(Age) > 20";
			var sql = builder.Select.Columns(c => c.Lastname).Column("Avg({0}) AS [AverageAge]", c=>c.Age)
									.From()
									.GroupBy.Columns(c => c.Lastname)
									.Having.Text("Avg({0}) > 20", c=>c.Age)
									.ToString();
			StringAssert.AreEqualIgnoringCase(compareSql, sql);
		}
	}
}
