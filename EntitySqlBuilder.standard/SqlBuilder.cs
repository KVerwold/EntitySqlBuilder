using EntitySqlBuilder.Clauses;

namespace EntitySqlBuilder
{
	public class SqlBuilder<TEntity>
	{
		private readonly Builder<TEntity> _builder;

		public SqlBuilder()
		{
			_builder = new Builder<TEntity>();
		}

		public SelectClause<TEntity> Select
		{
			get { return _builder.Select(); }
		}

		public void AddTableAlias<T>(string alias)
		{
			var t = typeof(T);
			if (!_builder.Aliases.ContainsKey(t))
				_builder.Aliases.Add(t, alias);
		}

		public void Clear()
		{
			_builder.Clear();
		}
	}
}
