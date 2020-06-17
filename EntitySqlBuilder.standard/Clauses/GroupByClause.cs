using System;
using System.Linq.Expressions;

namespace EntitySqlBuilder.Clauses
{
	public class GroupByClause<TEntity> : BaseClause<TEntity>
	{
		public GroupByClause(Builder<TEntity> builder)
			: base(builder)
		{
		}

		internal void Init()
		{
		}

		internal override string ToSql()
		{
			return $"GROUP BY {string.Join(",", Elements)}";
		}

		/// <summary>
		/// Adding a member expression (column) to the Group by clause.
		/// Sample:
		/// ..GroupBy.Column(c=>c.Firstname) => SQL: GROUP BY Firstname
		/// </summary>
		/// <param name="expression">Member expression</param>
		/// <returns>GroupByClause for Entity</returns>
		public GroupByClause<TEntity> Column(Expression<Func<TEntity, object>> expression)
		{
			return Column(false, expression);
		}

		/// <summary>
		/// Adding a member expression (column) to the Group by clause including table alias.
		/// Sample:
		/// ..GroupBy.Column("c",c=>c.Firstname) => SQL: GROUP BY [c].Firstname
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <returns>GroupByClause for Entity</returns>
		public GroupByClause<TEntity> Column(bool useExprTypeInfereceAsTableAlias, Expression<Func<TEntity, object>> expression)
		{
			Add(Builder.GetMemberExpressions(useExprTypeInfereceAsTableAlias,expression));
			return this;
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Group by clause.
		/// Sample:
		/// ..GroupBy.Column(c=>c.Firstname, c=>c.Lastname) => SQL: GROUP BY Firstname,Lastname
		/// </summary>
		/// <param name="expressions">Member expressions</param>
		/// <returns>GroupByClause for Entity</returns>
		public GroupByClause<TEntity> Columns(params Expression<Func<TEntity, object>>[] expressions)
		{
			return Columns(false, expressions);
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Group by clause including table alias.
		/// Sample:
		/// ..GroupBy.Column("c", c=>c.Firstname, c=>c.Lastname) => SQL: GROUP BY [c].Firstname,[c].Lastname
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>GroupByClause for Entity</returns>
		public GroupByClause<TEntity> Columns(bool useExprTypeInfereceAsTableAlias, params Expression<Func<TEntity, object>>[] expressions)
		{
			Add(Builder.GetMemberExpressions(useExprTypeInfereceAsTableAlias, expressions));
			return this;
		}

		/// <summary>
		/// Adding a member expression (column) from joined entity to the Group clause.
		/// Sample:
		/// sqlBuilder.AddTableAlias<Customer>("c");
		/// ..GroupBy.Column<Customer>(c=>c.Firstname) => SQL: GROUP BY [c].Firstname
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="expression">Member expression</param>
		/// <returns>Reference to SqlBuilder</returns>
		public GroupByClause<TEntity> Column<TJoin>(Expression<Func<TJoin, object>> expression)
		{
			return Column<TJoin>(false, expression);
		}

		/// <summary>
		/// Adding a member expression (column) from joined entity to the Group by clause including table alias.
		/// Sample:
		/// ..GroupBy.Column<Customer>("c",c=>c.Firstname) => SQL: GROUP BY [c].Firstname
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <param name="param">Additional string parameter</param>
		/// <returns>Reference to SqlBuilder</returns>
		public GroupByClause<TEntity> Column<TJoin>(bool useExprTypeInfereceAsTableAlias, Expression<Func<TJoin, object>> expression)
		{
			Add(Builder.GetMemberExpressions(useExprTypeInfereceAsTableAlias, expression));
			return this;
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Group By clause.
		/// Sample:
		/// sqlBuilder.AddTableAlias<Customer>("c");
		/// ..GroupBy.Column<Customer>(c=>c.Firstname, c=>c.Lastname) => SQL: GROUP BY [c].Firstname,[c].Lastname
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="expressions">Member expression</param>
		/// <returns>Reference to SqlBuilder</returns>
		public GroupByClause<TEntity> Columns<TJoin>(params Expression<Func<TJoin, object>>[] expressions)
		{
			return Columns(false, expressions);
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Group By clause including table alias.
		/// Sample:
		/// ..GroupBy.Column<Customer>("c", c=>c.Firstname, c=>c.Lastname) => SQL: GROUP BY [c].Firstname,[c].Lastname
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expressions">Member expression</param>
		/// <returns>Reference to SqlBuilder</returns>
		public GroupByClause<TEntity> Columns<TJoin>(bool useExprTypeInfereceAsTableAlias, params Expression<Func<TJoin, object>>[] expressions)
		{
			Add(Builder.GetMemberExpressions(useExprTypeInfereceAsTableAlias, expressions));
			return this;
		}

		/// <summary>
		/// Adding the Having clause
		/// Sample:
		/// sqlBuilder = new SqlBuilder<Customer>();
		/// sqlBuilder.Select.All.From().GroupBy...Having... => SQL: Select * from Customer group by ... having
		/// </summary>
		/// <returns>OrderByClause for Entity</returns>
		public HavingClause<TEntity> Having
		{
			get { return Builder.Having(); }
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
