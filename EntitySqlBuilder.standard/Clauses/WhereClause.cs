using System;
using System.Linq.Expressions;

namespace EntitySqlBuilder.Clauses
{
	public class WhereClause<TEntity> : BaseClause<TEntity>
	{

		public WhereClause(Builder<TEntity> builder)
			: base(builder)
		{
		}

		internal void Init()
		{
		}

		internal override string ToSql()
		{
			return $"Where {string.Join(" ", Elements)}";
		}

		/// <summary>
		/// Adding an excpression to the Where clause
		/// Sample:
		/// ...Where.Expr(c=> c.Age > 20) => SQL: where Age > 20
		/// ...Where.Expr(c=> c.LastName == "Doe") => SQL: where Lastname = 'Doe'
		/// </summary>
		/// <param name="expression">Member expression</param>
		/// <returns>WhereClause for Entity</returns>
		public WhereClause<TEntity> Expr(Expression<Func<TEntity, bool>> expression)
		{
			return Expr(false, expression);
		}

		/// <summary>
		/// Adding an excpression to the Where clause including table alias
		/// Sample:
		/// ...Where.Expr("c",c=> c.Age > 20) => SQL: where [c].Age > 20
		/// </summary>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <returns>WhereClause for Entity</returns>
		public WhereClause<TEntity> Expr(bool useExpressionTableAlias, Expression<Func<TEntity, bool>> expression)
		{
			Add(Builder.GetExpression(useExpressionTableAlias, expression));
			return this;
		}

		/// <summary>
		/// Adding an excpression for joined entity to the Where clause
		/// Sample:
		/// ...Where.Expr<Customer>(c=> c.Age > 20) => SQL: where (Age > 20)
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="expression">Member expression</param>
		/// <returns>WhereClause for Entity</returns>
		public WhereClause<TEntity> Expr<TJoin>(Expression<Func<TJoin, bool>> expression)
		{
			return Expr(false,expression);
		}

		/// <summary>
		/// Adding an excpression for joined entity to the Where clause including table alias
		/// Sample:
		/// ...Where.Expr<Customer>("c",c=> c.Age > 20) => SQL: where ([c].Age > 20)
		/// </summary>
		/// <typeparam name="TJoin">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <returns>WhereClause for Entity</returns>
		public WhereClause<TEntity> Expr<TJoin>(bool useExpressionTableAlias, Expression<Func<TJoin, bool>> expression)
		{
			Add(Builder.GetExpression(useExpressionTableAlias, expression));
			return this;
		}

		/// <summary>
		/// Adding an excpression for join entities to the Where clause
		/// Sample:
		/// sqlBuilder.AddTableAlias<Customer>("c");
		/// sqlBuilder.AddTableAlias<CustomerGroup>("cg");
		/// ...Where.Expr<Customer,CustomerGroup>((c,cg)=> c.CustomerGroupId == cg.Id) => SQL: where ([c].CustomerGroupId = [cg].Id)
		/// </summary>
		/// <typeparam name="TJoin1">Joined Entity</typeparam>
		/// <typeparam name="TJoin2">Joined Entity</typeparam>
		/// <param name="useExprTypeInfereceAsTableAlias">Using type inferecens from expression as table alias</param>
		/// <param name="expression">Member expression</param>
		/// <returns>WhereClause for Entity</returns>
		public WhereClause<TEntity> Expr<TJoin1,TJoin2>(Expression<Func<TJoin1,TJoin2, bool>> expression)
		{
			Add(Builder.GetExpression(expression));
			return this;
		}

		public WhereClause<TEntity> Expr<TJoin1,TJoin2,TJoin3>(Expression<Func<TJoin1,TJoin2,TJoin3, bool>> expression)
		{
			Add(Builder.GetExpression(expression));
			return this;
		}

		public WhereClause<TEntity> Expr<TJoin1,TJoin2,TJoin3,TJoin4>(Expression<Func<TJoin1, TJoin2, TJoin3, TJoin4, bool>> expression)
		{
			Add(Builder.GetExpression(expression));
			return this;
		}

		public WhereClause<TEntity> Expr<TJoin1,TJoin2,TJoin3,TJoin4,TJoin5>(Expression<Func<TJoin1, TJoin2, TJoin3, TJoin4, TJoin5, bool>> expression)
		{
			Add(Builder.GetExpression(expression));
			return this;
		}

		public WhereClause<TEntity> Expr<TJoin1,TJoin2,TJoin3,TJoin4,TJoin5,TJoin6>(Expression<Func<TJoin1, TJoin2, TJoin3,TJoin4,TJoin5,TJoin6, bool>> expression)
		{
			Add(Builder.GetExpression(expression));
			return this;
		}

		public WhereClause<TEntity> Expr<TJoin1,TJoin2,TJoin3,TJoin4,TJoin5,TJoin6,TJoin7>(Expression<Func<TJoin1,TJoin2,TJoin3,TJoin4,TJoin5,TJoin6,TJoin7, bool>> expression)
		{
			Add(Builder.GetExpression(expression));
			return this;
		}

		/// <summary>
		/// Adding the Group By clause
		/// Sample:
		/// sqlBuilder = new SqlBuilder<Customer>();
		/// sqlBuilder.Select.All.From().GroupBy... => SQL: Select * from Customer group by ...
		/// </summary>
		/// <returns>GroupByClause for Entity</returns>
		public GroupByClause<TEntity> GroupBy
		{
			get { return Builder.GroupBy(); }
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
