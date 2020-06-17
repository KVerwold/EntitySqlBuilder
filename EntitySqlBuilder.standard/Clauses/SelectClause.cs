using System;
using System.Linq.Expressions;

namespace EntitySqlBuilder.Clauses
{
	public class SelectClause<TEntity> : BaseClause<TEntity>
	{
		private string _topExpr = String.Empty;
		private string _distinctExpr = String.Empty;

		public SelectClause(Builder<TEntity> builder): base(builder)
		{
		}

		private static string GetAlias(string alias)
		{
			return string.IsNullOrEmpty(alias) ? null : $"AS [{alias}]";
		}

		internal void Init()
		{
		}

		internal override string ToSql()
		{
			return $"Select{_topExpr}{_distinctExpr} {string.Join(",",Elements)}";
		}

		/// <summary>
		/// Adding an "*" to the SELECT clause.
		/// Sample:
		/// sqlBuilder.Select.All() => SQL: Select *
		/// sqlBuilder.Select.All("c1") => SQL: Select [c1].*
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> All(string tableAlias = null)
		{
			Add($"{(string.IsNullOrEmpty(tableAlias) ? string.Empty : $"[{tableAlias}].")}*");
			return this;
		}

		/// <summary>
		/// Adding an "*" to the SELECT clause for joined Entity
		/// Sample:
		/// sqlBuilder.AddTableAlias<CustomerGroup>("cg");
		/// sqlBuilder.Select.All<CustomerGroup>() => SQL: Select [cg].*
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> All<TJoin>()
		{
			return All(Builder.GetTableAlias<TJoin>(null));
		}

		/// <summary>
		/// Adding a "DISTINCT" to the SELECT clause.
		/// Sample:
		/// sqlBuilder.Select.Distinct... => SQL: Select Distinct
		/// </summary>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Distinct
		{
			get 
			{
				_distinctExpr = " DISTINCT";
				return this;
			}
		}

		/// <summary>
		/// Adding a TOP clause to the SELECT clause.
		/// Sample:
		/// sqlBuilder.Select.Top(100)... => SQL: Select TOP 100
		/// sqlBuilder.Select.Top(100,true)... => SQL: Select TOP 100 PERCENT
		/// </summary>
		/// <param name="topValue">Number of record to select for TOP clause</param>
		/// <param name="percent">If true PERCENT is added to the TOP clause</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Top(int topValue, bool percent = false)
		{
			_topExpr = $" TOP {topValue}{(percent ? " PERCENT" : "")}";
			return this;
		}

		/// <summary>
		/// Adding a member expression (column) to the Select clause.
		/// Sample:
		/// sqlBuilder.Select.Column(c=>c.Firstname).. => SQL: Select Firstname ..
		/// sqlBuilder.Select.Column(c=>c.Firstname,"MyAlias").. => SQL: Select Firstname AS [MyAlias] ..
		/// </summary>
		/// <param name="expression">Member expression</param>
		/// <param name="columnAlias">Column alias (default is null)</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column(Expression<Func<TEntity, object>> expression, string columnAlias = null)
		{
			Column(false, expression, columnAlias);
			return this;
		}

		/// <summary>
		/// Adding a member expression (column) to the Select clause including table alias.
		/// Sample:
		/// sqlBuilder.Select.Column("t1",c=>c.Firstname) => SQL: Select [t1].Firstname ...
		/// sqlBuilder.Select.Column("t1",c=>c.Firstname,"MyAlias") => SQL: Select [t1].Firstname AS [MyAlias]
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <param name="columnAlias">Column alias (default is null)</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column(bool useExprTypeInfereceAsTableAlias, Expression <Func<TEntity, object>> expression, string columnAlias=null)
		{
			Add(Builder.GetMemberExpressionWithParam(useExprTypeInfereceAsTableAlias, expression, GetAlias(columnAlias)));
			return this;
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Select clause.
		/// Sample:
		/// sqlBuilder.Select.Columns(c=>c.Firstname, c=>c.Lastname) => SQL: Select Firstname, Lastname
		/// </summary>
		/// <param name="expressions">Member expressions</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Columns(params Expression<Func<TEntity, object>>[] expressions)
		{
			return Columns(false, expressions);
		}

		/// <summary>
		/// Adding a list of member expressions (columns) to the Select clause including table alias
		/// Sample:
		/// sqlBuilder.Select.Column("t1",c=>c.Firstname, c=>c.Lastname) => SQL: Select [t1].Firstname, [t1].Lastname
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Columns(bool useExprTypeInfereceAsTableAlias, params Expression<Func<TEntity, object>>[] expressions)
		{
			Add(Builder.GetMemberExpressions(useExprTypeInfereceAsTableAlias,expressions));
			return this;
		}

		/// <summary>
		/// Adding a member expression (column) from a joined entity to the Select clause.
		/// Sample:
		/// sqlBuilder.Aliases<CustomerGroup>("cg")
		/// sqlBuilder.Select.Column<CustomerGroup>(cg=>cg.Name) => SQL: Select [cg].Name
		/// sqlBuilder.Select.Column<CustomerGroup>(cg=>cg.Name,"MyAlias") => SQL: Select [cg].Name AS [MyAlias]
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="expression">Member expression</param>
		/// <param name="columnAlias">Column alias (default is null)</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column<TJoin>(Expression<Func<TJoin, object>> expression, string columnAlias = null)
		{
			return Column<TJoin>(false, expression, columnAlias);
		}

		/// <summary>
		/// Adding a member expression (column) from a joined entity to the Select clause including table alias.
		/// Sample:
		/// sqlBuilder.Select.Column<CustomerGroup>("c1", c=>c.Name) => SQL: Select [c1].Name
		/// sqlBuilder.Select.Column<CustomerGroup>("c1", c=>c.Name,"MyAlias") => SQL: Select [c1].Name AS [MyAlias]
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <param name="columnAlias">Additional field alias</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column<TJoin>(bool useExprTypeInfereceAsTableAlias, Expression<Func<TJoin, object>> expression, string columnAlias=null)
		{
			Add(Builder.GetMemberExpressionWithParam(useExprTypeInfereceAsTableAlias, expression, GetAlias(columnAlias)));
			return this;
		}

		/// <summary>
		/// Adding a member expressions (columns) from a joined entity to the Select clause.
		/// Sample:
		/// sqlBuilder.Aliases<CustomerGroup>("c")
		/// sqlBuilder.Select.Columns<Customer>(c=>c.Firstname,c=>c.Lastname) => SQL: Select [c].Firstname,[c].Lastname
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="expressions">Member expressions</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Columns<TJoin>(params Expression<Func<TJoin, object>>[] expressions)
		{
			return Columns<TJoin>(false, expressions);
		}

		/// <summary>
		/// Adding a member expressions (columns) from a joined entity to the Select clause including table alias.
		/// Sample:
		/// sqlBuilder.Select.Column<Customer>("c1", c=>c.Firstname,c=>c.Lastname) => SQL: Select [c1].Firstname,[c1].Lastname
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Columns<TJoin>(bool useExprTypeInfereceAsTableAlias, params Expression<Func<TJoin, object>>[] expressions)
		{
			Add(Builder.GetMemberExpressions(useExprTypeInfereceAsTableAlias, expressions));
			return this;
		}

		/// <summary>
		/// Adding simple text to the Select clause.
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Column("COUNT(*) AS [Total]")... => SQL: Select COUNT(*) AS [Total] ...
		/// </summary>
		/// <param name="text">Text to add to the Select clause</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column(string text)
		{
			Add(text);
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clause.
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Column("AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG(Age) AS [AverageAge]
		/// </summary>
		/// <param name="paramText">Composite format string</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column(string paramText, params Expression<Func<TEntity, object>>[] expressions)
		{
			Column(false,paramText, expressions);
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clause including table alias
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Column("c1", "AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG([c1].Age) AS [AverageAge]
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="paramText">Composite format string</param>
		/// <param name="expressions">Member expressions</param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column(bool useExprTypeInfereceAsTableAlias, string paramText, params Expression<Func<TEntity, object>>[] expressions)
		{
			Add(Builder.GetParamTextWithExpressions(useExprTypeInfereceAsTableAlias, paramText, expressions));
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clauseincluding table alias
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.AddTableAlias<Customer>("c1")
		/// sqlBuilder.Select.Column<Customer>("AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG([c1].Age) AS [AverageAge]
		/// </summary>
		/// <param name="paramText">Formatted text</param>
		/// <param name="expressions"></param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column<TJoin>(string paramText, params Expression<Func<TJoin, object>>[] expressions)
		{
			Column<TJoin>(false, paramText, expressions);
			return this;
		}

		/// <summary>
		/// Adding a formatted string as text to the Select clauseincluding table alias
		/// Text is added to columns list and follows columns seperation from clause.
		/// Sample:
		/// sqlBuilder.Select.Column<Customer>("c1", "AVG({0}) AS [AverageAge]", c=>c.Age)... => SQL: Select AVG([c1].Age) AS [AverageAge]
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="paramText">Formatted text</param>
		/// <param name="expressions"></param>
		/// <returns>SelectClause for Entity</returns>
		public SelectClause<TEntity> Column<TJoin>(bool useExprTypeInfereceAsTableAlias, string paramText, params Expression<Func<TJoin, object>>[] expressions)
		{
			Add(Builder.GetParamTextWithExpressions(useExprTypeInfereceAsTableAlias, paramText, expressions));
			return this;
		}

		/// <summary>
		/// Adding the FROM clause
		/// </summary>
		/// <param name="table">Name of the table (if null, Entity name us used)</param>
		/// <param name="alias">Table alias</param>
		/// <returns>FromClause for Entity</returns>
		public FromClause<TEntity> From(string table=null, string alias=null)
		{
			return Builder.From(table, alias);
		}

	}
}
