using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using EntitySqlBuilder.Clauses;


namespace EntitySqlBuilder
{
	public enum SqlClause
	{
		None,
		SelectClause,
		FromClause,
		JoinClause,
		WhereClause,
		GroupByClause,
		HavingClause,
		OrderByClause,
		UnionClause
	};

	public class Builder<TEntity>
	{
		private BuilderContainer<TEntity> _container;

		public Builder()
		{
			_container = new BuilderContainer<TEntity>(this);
			Aliases = new Dictionary<Type, string>();
		}

		#region Expression Resolver methods

		private static string GetExpressionPredicateAsSqlOperator(ExpressionType nodeType)
		{
			switch (nodeType)
			{
				case ExpressionType.Equal:
					return "=";
				case ExpressionType.GreaterThan:
					return ">";
				case ExpressionType.GreaterThanOrEqual:
					return ">=";
				case ExpressionType.LessThan:
					return "<";
				case ExpressionType.LessThanOrEqual:
					return "<=";
				case ExpressionType.NotEqual:
					return "<>";
				case ExpressionType.IsTrue:
					return " is true";
				case ExpressionType.IsFalse:
					return "is not true";
				case ExpressionType.AndAlso:
					return "AND";
				case ExpressionType.OrElse:
					return "OR";
				case ExpressionType.Not:
					return "IS NOT";
				default:
					return null;
			}
		}

		private static string GetFullMemberName(Type t, string memberName, Dictionary<Type, string> aliasDictionary)
		{
			return aliasDictionary != null && aliasDictionary.ContainsKey(t) ? $"[{aliasDictionary[t]}].{memberName}" : memberName;
		}

		private static string GetFullMemberName(string alias, string memberName)
		{
			return $"{(string.IsNullOrEmpty(alias) ? "" : $"[{alias}].")}{memberName}";
		}

		private string GetExpressionData(Expression expression, bool useExpressionAlias = false)
		{
			if (expression == null) throw new ArgumentException("Value from expression may not be empty");

			if (expression is BinaryExpression)
			{
				var be = expression as BinaryExpression;
				var leftvalue = GetExpressionData(be.Left, useExpressionAlias);
				var predicate = GetExpressionPredicateAsSqlOperator(be.NodeType);
				if (string.IsNullOrEmpty(predicate)) throw new ArgumentException(string.Format("operator {0} from expression is not supported ", be.NodeType));
				var rightValue = GetExpressionData(be.Right, useExpressionAlias);
				return $"({leftvalue} {predicate} {rightValue})";
			}

			if (expression is ConstantExpression)
			{
				var ce = expression as ConstantExpression;
				var value = ce.Value;
				if (value == null)
					return null;
				switch (value.GetType().Name)
				{
					case "String":
						return $"'{value}'";
					case "Boolean":
						return (bool) value ? "1=1" : "1=0";
					default:
						return value.ToString();
				}
			}

			if (expression is MemberExpression)
			{
				var me = expression as MemberExpression;
				if ((me.Member is FieldInfo))
				{
					var value = Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object))).Compile().Invoke();
					return value == null ? null : (value is string ? string.Format("'{0}'", value) : value.ToString());
				}
				if(me.Expression is ParameterExpression && useExpressionAlias)
				{
					return GetFullMemberName((me.Expression as ParameterExpression).Name, me.Member.Name);
				}
				return GetFullMemberName(me.Expression.Type, me.Member.Name, Aliases);
			}

			if (expression is UnaryExpression)
			{
				var ue = expression as UnaryExpression;
				var predicate = GetExpressionPredicateAsSqlOperator(ue.NodeType);
				return $"{(predicate != null ? predicate + " " : null)}{GetExpressionData(ue.Operand, useExpressionAlias)}";
			}

			return null;
		}

		internal string GetTableAlias<T>(string alias)
		{
			if (string.IsNullOrEmpty(alias))
				Aliases.TryGetValue(typeof(T), out alias);
			return alias;
		}

		#endregion

		#region Expression Methods

		public string GetExpression<T>(bool useExprTypeInfereceAsTableAlias, Expression<Func<T, bool>> expression)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			return GetExpressionData(expression.Body, useExprTypeInfereceAsTableAlias);
		}

		public string GetExpression<T1, T2>(Expression<Func<T1, T2, bool>> expression)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			return GetExpressionData(expression.Body, true);
		}

		public string GetExpression<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> expression)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			return GetExpressionData(expression.Body, true);
		}
		public string GetExpression<T1, T2, T3,T4>(Expression<Func<T1, T2, T3,T4, bool>> expression)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			return GetExpressionData(expression.Body, true);
		}
		public string GetExpression<T1, T2, T3,T4,T5>(Expression<Func<T1, T2, T3,T4,T5, bool>> expression)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			return GetExpressionData(expression.Body, true);
		}
		public string GetExpression<T1, T2, T3,T4,T5,T6>(Expression<Func<T1, T2, T3,T4,T5,T6, bool>> expression)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			return GetExpressionData(expression.Body, true);
		}
		public string GetExpression<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> expression)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			return GetExpressionData(expression.Body, true);
		}

		#endregion

		#region private Member expressions

		/// <summary>
		/// Adding a member expression from the entity to the current clause including a parameter
		/// </summary>
		/// <typeparam name="T">Class type</typeparam>
		/// <param name="tableAlias">Alias to the table</param>
		/// <param name="expression">Member expressions of entity type</param>
		/// <param name="param">Parameter to the member expression</param>
		/// <returns>Reference to SqlBuilder</returns>
		private string BuildMemberExpression<T>(bool useExprTypeInfereceAsTableAlias, Expression<Func<T, object>> expression, string param)
		{
			if (expression == null) throw new ArgumentException("Expression may not be empty");
			var expr = GetExpressionData(expression.Body, useExprTypeInfereceAsTableAlias);
			return !string.IsNullOrEmpty(param) ? $"{expr} {param}" : expr;
		}

		/// <summary>
		/// Adding member expressions from the entity to the current clause
		/// </summary>
		/// <typeparam name="T">Class type</typeparam>
		/// <param name="tableAlias">Alias to the table</param>
		/// <param name="expressions">List of member expressions of entity type</param>
		/// <returns>Reference to SqlBuilder</returns>
		private IEnumerable<string> BuildMemberExpressions<T>(bool useExprTypeInfereceAsTableAlias, params Expression<Func<T, object>>[] expressions)
		{
			if (expressions == null) throw new ArgumentException("Expression may not be empty");
			var exprList = new List<string>();
			foreach (var expression in expressions)
			{
				exprList.Add(BuildMemberExpression(useExprTypeInfereceAsTableAlias, expression, null));
			}
			return exprList;
		}

		#endregion

		#region public Member expressions

		public IEnumerable<string> GetMemberExpressions<T>(bool useExprTypeInfereceAsTableAlias, params Expression<Func<T, object>>[] expressions)
		{
			if (expressions == null) throw new ArgumentException("Expressions may not be empty");
			return BuildMemberExpressions(useExprTypeInfereceAsTableAlias, expressions);
		}

		public string GetMemberExpressionWithParam<T>(bool useExprTypeInfereceAsTableAlias, Expression<Func<T, object>> expression, string param)
		{
			return BuildMemberExpression(useExprTypeInfereceAsTableAlias, expression, param);
		}

		#endregion

		#region Text expressions 

		public string GetParamTextWithExpressions<T>(bool useExprTypeInfereceAsTableAlias, string paramText, params Expression<Func<T, object>>[] expressions)
		{
			var members = expressions.Select(expression => GetExpressionData(expression.Body, useExprTypeInfereceAsTableAlias)).Cast<object>().ToList();
			return string.Format(paramText, members.ToArray());
		}

		#endregion

		#region Clause methods

		public SelectClause<TEntity> Select()
		{
			return _container.Select();
		}

		/// <summary>
		/// Activates the FROM clause
		/// </summary>
		/// <param name="table">Name of the table (if null, Entity name is used)</param>
		/// <param name="alias">Alias to the table (default null)</param>
		/// <returns>FromClause for Entity</returns>
		public FromClause<TEntity> From(string table, string alias)
		{
			return _container.From(table, GetTableAlias<TEntity>(alias));
		}

		public JoinClause<TEntity> Join<T>(JoinType joinType, string alias)
		{
			return _container.Join<T>(joinType, GetTableAlias<T>(alias));
		}

		public WhereClause<TEntity> Where()
		{
			return _container.Where();
		}

		public GroupByClause<TEntity> GroupBy()
		{
			return _container.GroupBy();
		}

		public HavingClause<TEntity> Having()
		{
			return _container.Having();
		}

		public OrderByClause<TEntity> OrderBy()
		{
			return _container.OrderBy();
		}

		#endregion

		public void Clear()
		{
			_container = new BuilderContainer<TEntity>(this);
			Aliases.Clear();
		}

		public Dictionary<Type, string> Aliases { get; private set; }

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(_container.SelectClause.ToSql());
			sb.Append($" {_container.FromClause.ToSql()}");
			foreach (var joinClause in _container.JoinClauses)
				sb.Append($" {joinClause.ToSql()}");

			if (_container.WhereClause != null)
			{
				sb.Append($" {_container.WhereClause.ToSql()}");
			}
			if (_container.GroupByClause != null)
			{
				sb.Append($" {_container.GroupByClause.ToSql()}");
			}
			if (_container.HavingClause != null)
			{
				sb.Append($" {_container.HavingClause.ToSql()}");
			}
			if (_container.OrderByClause != null)
			{
				sb.Append($" {_container.OrderByClause.ToSql()}");
			}
			return sb.ToString();
		}
	}
}
