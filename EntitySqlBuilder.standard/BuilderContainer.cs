using EntitySqlBuilder.Clauses;
using System.Collections.Generic;

namespace EntitySqlBuilder
{
	public class BuilderContainer<TEntity>
	{
		private Builder<TEntity> _builder;

		public BuilderContainer(Builder<TEntity> builder)
		{
			_builder = builder;
			JoinClauses = new List<JoinClause<TEntity>>();
		}

		public SelectClause<TEntity> Select() 
		{
			if(SelectClause == null)
				SelectClause = new SelectClause<TEntity>(_builder);
			SelectClause.Init();
			return SelectClause;
		}

		public FromClause<TEntity> From(string tableName, string alias)
		{
			if(FromClause == null)
				FromClause = new FromClause<TEntity>(_builder);
			FromClause.Init(tableName,alias);
			return FromClause;
		}

		public JoinClause<TEntity> Join<TJoin>(JoinType joinType = JoinType.InnerJoin, string alias = null)
		{
			var joinClause = new JoinClause<TEntity>(_builder);
			joinClause.Init<TJoin>(joinType, alias);
			JoinClauses.Add(joinClause);
			return joinClause;
		}

		public WhereClause<TEntity> Where()
		{
			if(WhereClause == null)
				WhereClause = new WhereClause<TEntity>(_builder);
			WhereClause.Init();
			return WhereClause;
		}

		public GroupByClause<TEntity> GroupBy()
		{
			if(GroupByClause == null)
				GroupByClause = new GroupByClause<TEntity>(_builder);
			GroupByClause.Init();
			return GroupByClause;
		}

		public HavingClause<TEntity> Having()
		{
			if (HavingClause == null)
				HavingClause = new HavingClause<TEntity>(_builder);
			HavingClause.Init();
			return HavingClause;
		}

		public OrderByClause<TEntity> OrderBy()
		{
			if(OrderByClause == null)
				OrderByClause = new OrderByClause<TEntity>(_builder);

			OrderByClause.Init();
			return OrderByClause;
		}

		public SelectClause<TEntity> SelectClause { get; private set; }
		public FromClause<TEntity> FromClause { get; private set; }
		public WhereClause<TEntity> WhereClause { get; private set; }
		public GroupByClause<TEntity> GroupByClause { get; private set; }
		public HavingClause<TEntity> HavingClause { get; private set; }
		public OrderByClause<TEntity> OrderByClause { get; private set; }
		public ICollection<JoinClause<TEntity>> JoinClauses { get; private set; }
	}
}
