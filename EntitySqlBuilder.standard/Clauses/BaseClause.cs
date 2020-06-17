
using System.Collections.Generic;

namespace EntitySqlBuilder.Clauses
{
	public abstract class BaseClause<TEntity>
	{
		private List<string> _elements;

		public BaseClause(Builder<TEntity> builder)
		{
			Builder = builder;
		}

		protected Builder<TEntity> Builder { get; private set; }

		internal void Add(string element)
		{
			Elements.Add(element);
		}

		internal void Add(IEnumerable<string> elements)
		{
			Elements.AddRange(elements);
		}

		internal List<string> Elements
		{
			get => _elements ?? (_elements = new List<string>());
		}

		internal abstract string ToSql();

		public override string ToString()
		{
			return Builder.ToString();
		}


	}
}
