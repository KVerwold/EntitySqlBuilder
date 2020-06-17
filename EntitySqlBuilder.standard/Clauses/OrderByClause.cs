using System;
using System.Linq.Expressions;

namespace EntitySqlBuilder.Clauses
{
	public class OrderByClause<TEntity> : BaseClause<TEntity>
	{
		public OrderByClause(Builder<TEntity> builder): base(builder)
		{
		}

		internal void Init()
		{
		}

		internal override string ToSql()
		{
			return $"ORDER BY {string.Join(",", Elements)}";
		}

		/// <summary>
		/// Adding a member expression (column) to the Order by clause.
		/// Sample:
		/// ...OrderBy.Column(c=>c.Firstname) => SQL: .. ORDER BY Firstname asc
		/// ...OrderBy.Column(c=>c.Firstname,false) => SQL: .. ORDER BY Firstname desc
		/// </summary>
		/// <param name="expression">Member expression</param>
		/// <param name="asc">If true (default), sorting order is ascending, otherwise descending</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Column(Expression<Func<TEntity, object>> expression, bool asc=true)
		{
			return Column(false, expression, asc);
		}

		/// <summary>
		/// Adding a member expression (column) to the Order by clause including table alias.
		/// Sample:
		/// ...OrderBy.Column("t1",c=>c.Firstname) => SQL: .. ORDER BY [t1].Firstname asc
		/// ...OrderBy.Column("t1",c=>c.Firstname,false) => SQL: .. ORDER BY [t1].Firstname desc
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <param name="asc">If true (default), sorting order is ascending, otherwise descending</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Column(bool useExpressionTableAlias, Expression<Func<TEntity, object>> expression, bool asc=true)
		{
			Add(Builder.GetMemberExpressionWithParam(useExpressionTableAlias, expression, asc ? "ASC" : "DESC"));
			return this;
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Order by clause.
		/// Sample:
		/// ...OrderBy.Columns(c=>c.Firstname, c=>c.Lastname) => SQL: ORDER BY Firstname, Lastname
		/// </summary>
		/// <param name="expressions">Member expression</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Columns(params Expression<Func<TEntity, object>>[] expressions)
		{
			return Columns(false,expressions);
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Order by clause including table alias.
		/// Sample:
		/// ...OrderBy.Column("t1",c=>c.Firstname, c=>c.Lastname) => SQL: ORDER BY [t1].Firstname, [t1].Lastname
		/// </summary>
		/// <param name="tableAlias">Alias of the table</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Columns(bool useExpressionTableAlias, params Expression<Func<TEntity, object>>[] expressions)
		{
			foreach (var expression in expressions)
			{
				Column(useExpressionTableAlias, expression);
			}
			return this;
		}

		/// <summary>
		/// Adding a member expression (column) from join entity to the Order by clause.
		/// Sample:
		/// sqlBuilder.AddTableAlias<Customer>("c");
		/// ...OrderBy.Column<Customer>(c=>c.Firstname) => SQL: .. ORDER BY [c].Firstname asc
		/// ...OrderBy.Column<Customer>(c=>c.Firstname,false) => SQL: .. ORDER BY [c].Firstname desc
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="expression">Member expression</param>
		/// <param name="asc">If true (default), sorting order is ascending, otherwise descending</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Column<TJoin>(Expression<Func<TJoin, object>> expression, bool asc=true)
		{
			return Column<TJoin>(false, expression, asc);
		}

		/// <summary>
		/// Adding a member expression (column) from join entity to the Order by clause including table alias.
		/// Sample:
		/// ...OrderBy.Column<Customer>("c",c=>c.Firstname) => SQL: .. ORDER BY [c].Firstname asc
		/// ...OrderBy.Column<Customer>("c",c=>c.Firstname,false) => SQL: .. ORDER BY [c].Firstname desc
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expressions</param>
		/// <param name="asc">If true (default), sorting order is ascending, otherwise descending</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Column<TJoin>(bool useExpressionTableAlias, Expression<Func<TJoin, object>> expression, bool asc = true)
		{
			Add(Builder.GetMemberExpressionWithParam(useExpressionTableAlias, expression, asc ? "ASC" : "DESC"));
			return this;
		}

		/// <summary>
		/// Adds a list of member expressions (columns) from join entity to the Order by clause.
		/// Sample:
		/// sqlBuilder.AddTableAlias<Customer>("c");
		/// ...OrderBy.Column<Customer>(c=>c.Firstname, c=>c.Lastname) => SQL: .. ORDER BY [c].Firstname,[c].Firstname asc
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="expressions">Member expressions</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Columns<TJoin>(params Expression<Func<TJoin, object>>[] expressions)
		{
			return Columns<TJoin>(false, expressions);
		}

		/// <summary>
		/// Adds a list of member expressions (columns) from join entity to the Order by clause including table alias
		/// Sample:
		/// ...OrderBy.Column<Customer>("c",c=>c.Firstname, c=>c.Lastname) => SQL: .. ORDER BY [c].Firstname,[c].Firstname asc
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>OrderByClause for Entity</returns>
		public OrderByClause<TEntity> Columns<TJoin>(bool useExpressionTableAlias, params Expression<Func<TJoin, object>>[] expressions)
		{
			foreach (var expression in expressions)
			{
				Column<TJoin>(useExpressionTableAlias, expression);
			}
			return this;
		}

	}
}
