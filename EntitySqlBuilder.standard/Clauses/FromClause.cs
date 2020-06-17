using System;

namespace EntitySqlBuilder.Clauses
{
	public class FromClause<TEntity> : BaseClause<TEntity>
	{
		private string _tablename;
		private string _alias;

		public FromClause(Builder<TEntity> builder)
			: base(builder)
		{
		}

		internal void Init(string tableName, string alias)
		{
			_tablename = string.IsNullOrEmpty(tableName) ? typeof(TEntity).Name : tableName;
			if (string.IsNullOrEmpty(_tablename)) throw new ArgumentException("Tablename may not be empty");
			_alias = alias;
		}

		internal override string ToSql()
		{
			return ($"FROM {_tablename}{(!string.IsNullOrEmpty(_alias) ? $" [{_alias}]" : "")}");
		}

		/// <summary>
		/// Adding the Join clause to join another entity.
		/// Supported join types:
		///		InnerJoin, (default)
		///		LeftOuterJoin,
		///		RightOuterJoin,
		///		FullOuterJoin
		///		
		/// Sample:
		/// sqlBuilder = new SqlBuilder<Customer>();
		/// sqlBuilder.AddTableAlias<Customer>("c");
		/// sqlBbuilder.AddTableAlias<CustomerGroup>("cg");
		/// sqlBuilder.Select.All.From().Join<CustomerGroup>().Expr<Customer, CustomerGroup>((c, cg) => c.CustomerGroupId == cg.Id)
		///   => SQL: Select * from Customer [c] join CustomerGroup [cg] on [c].CustomerGroupId = [cg].Id
		/// 
		/// Important: Defined aliases "c" and "cg" must be used in join expression
		/// </summary>
		/// <param name="joinType">Type of join (default InnerJoin)</param>
		/// <param name="alias">Table alias (default null)</param>
		/// <returns>FromClause for Entity</returns>
		public JoinClause<TEntity> Join<TJoin>(JoinType joinType=JoinType.InnerJoin, string alias=null)
		{
			return Builder.Join<TJoin>(joinType, alias);
		}

		/// <summary>
		/// Adding the Where clause
		/// Sample:
		/// sqlBuilder = new SqlBuilder<Customer>();
		/// sqlBuilder.Select.All.From().Where... => SQL: Select * from Customer where ...
		/// </summary>
		/// <returns>WhereClause for Entity</returns>
		public WhereClause<TEntity> Where
		{
			get { return Builder.Where(); }
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
