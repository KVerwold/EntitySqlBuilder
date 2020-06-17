using System;
using System.Linq.Expressions;

namespace EntitySqlBuilder.Clauses
{
	public enum JoinType
	{
		InnerJoin,
		LeftOuterJoin,
		RightOuterJoin,
		FullOuterJoin
	}

	public class JoinClause<TEntity> : BaseClause<TEntity>
	{
		private JoinType _joinType;
		private string _table;
		private string _alias;

		public JoinClause(Builder<TEntity> builder): base(builder)
		{
		}

		protected virtual string GetJoinType(JoinType joinType)
		{
			switch (joinType)
			{
				case JoinType.LeftOuterJoin:
					return "LEFT OUTER JOIN";
				case JoinType.RightOuterJoin:
					return "RIGHT OUTER JOIN";
				case JoinType.FullOuterJoin:
					return "FULL OUTER JOIN";
				default:
					return "JOIN";
			}
		}

		internal void Init<T>(JoinType joinType, string alias)
		{
			_joinType = joinType;
			_table = typeof(T).Name;
			_alias = alias;
			if (string.IsNullOrEmpty(_alias)) throw new ArgumentException("Alias may not be empty");
		}

		internal override string ToSql()
		{
			return $"{GetJoinType(_joinType)} {_table} [{_alias}] ON {string.Join(" ", Elements)}";
		}

		/// <summary>
		/// Adding the expression for the join clause.
		/// IMPORTANT !!
		/// Alias used for fields in expression, must match alias from entity
		/// 
		/// Example
		/// builder.AddTableAlias<Customer>("c")
		/// builder.AddTableAlias<CustomerGroup>("cg")
		/// builder.Select.All.From().Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id).ToString();
		///
		/// 'Customer' Entity alias "c" is also used as fields alias for expresssion "c.CustomerGroupId"
		/// </summary>
		/// <typeparam name="T1">Entity for left join expression</typeparam>
		/// <typeparam name="T2">Entity for right join expression</typeparam>
		/// <param name="expression">Expression, which describes the join filters</param>
		/// <returns>JoinClause for Entity</returns>
		public JoinClause<TEntity> Expr<T1, T2>(Expression<Func<T1, T2, bool>> expression)
		{
			Add(Builder.GetExpression(expression));
			return this;
		}

		public JoinClause<TEntity> Join<TJoin>(JoinType joinType = JoinType.InnerJoin, string alias = null)
		{
			return Builder.Join<TJoin>(joinType, alias);
		}

		public WhereClause<TEntity> Where
		{
			get { return Builder.Where(); }
		}

		public GroupByClause<TEntity> GroupBy
		{
			get { return Builder.GroupBy(); }
		}

		public OrderByClause<TEntity> OrderBy
		{
			get { return Builder.OrderBy(); }
		}

	}
}
