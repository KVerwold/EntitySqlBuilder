using System;
using System.Linq.Expressions;

namespace EntitySqlBuilder.Clauses
{
	public class HavingClause<TEntity> : BaseClause<TEntity>
	{

		public HavingClause(Builder<TEntity> builder)
			: base(builder)
		{
		}

		internal void Init()
		{
		}

		internal override string ToSql()
		{
			return $"Having {string.Join(" ", Elements)}";
		}

		/// <summary>
		/// Adding simple text to the Select clause.
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Text("COUNT(*) AS [Total]")... => SQL: Select COUNT(*) AS [Total] ...
		/// </summary>
		/// <param name="text">Text to add to the Select clause</param>
		/// <returns>HavingClause for Entity</returns>
		public HavingClause<TEntity> Text(string text)
		{
			Add(text);
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clause.
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Text("AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG(Age) AS [AverageAge]
		/// </summary>
		/// <param name="paramText">Composite format string</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>HavingClause for Entity</returns>
		public HavingClause<TEntity> Text(string paramText, params Expression<Func<TEntity, object>>[] expressions)
		{
			Text(false, paramText, expressions);
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clause including table alias
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Text("c1", "AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG([c1].Age) AS [AverageAge]
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="paramText">Composite format string</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>HavingClause for Entity</returns>
		public HavingClause<TEntity> Text(bool useExprTypeInfereceAsTableAlias, string paramText, params Expression<Func<TEntity, object>>[] expressions)
		{
			Add(Builder.GetParamTextWithExpressions(useExprTypeInfereceAsTableAlias, paramText, expressions));
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clauseincluding table alias
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.AddTableAlias<Customer>("c1")
		/// sqlBuilder.Select.Text<Customer>("AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG([c1].Age) AS [AverageAge]
		/// </summary>
		/// <param name="paramText">Formatted text</param>
		/// <param name="expressions"></param>
		/// <returns>HavingClause for Entity</returns>
		public HavingClause<TEntity> Text<TJoin>(string paramText, params Expression<Func<TJoin, object>>[] expressions)
		{
			Text<TJoin>(false, paramText, expressions);
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clauseincluding table alias
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Text<Customer>("c1", "AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG([c1].Age) AS [AverageAge]
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="paramText">Formatted text</param>
		/// <param name="expressions"></param>
		/// <returns>HavingClause for Entity</returns>
		public HavingClause<TEntity> Text<TJoin>(bool useExprTypeInfereceAsTableAlias, string paramText, params Expression<Func<TJoin, object>>[] expressions)
		{
			Add(Builder.GetParamTextWithExpressions(useExprTypeInfereceAsTableAlias, paramText, expressions));
			return this;
		}

		/// <summary>
		/// Adding the Order By clause
		/// Sample:
		/// sqlBuilder = new SqlBuilder<Customer>();
		/// sqlBuilder.Select.All.From().OrderBy... => SQL: Select * from Customer order by ...
		/// </summary>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> OrderBy
		{
			get { return Builder.OrderBy(); }
		}

	}
}
