using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public class Query : IQuery
    {
        private IList<Filter> filters = null;

        public void AddFilter(String column, String value)
        {
            this.AddFilter(column, ExpressionType.Equal, value);
        }

        public void AddFilter(String column, ExpressionType expression, String value)
        {
            if (this.filters == null)
            {
                this.filters = new List<Filter>();
            }
            this.filters.Add(new Filter()
            {
                Column = column,
                Expression = expression,
                Value = value,
                Type = FilterType.String
            });
        }

        public void AddFilter(String column, long value)
        {
            this.AddFilter(column, ExpressionType.Equal, value);
        }

        public void AddFilter(String column, ExpressionType expression, long value)
        {
            if (this.filters == null)
            {
                this.filters = new List<Filter>();
            }
            this.filters.Add(new Filter()
            {
                Column = column,
                Expression = expression,
                Value = value,
                Type = FilterType.Numeric
            });
        }

        public override string ToString()
        {
            if (this.filters == null)
            {
                return String.Empty;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.filters.Count; i++)
            {
                Filter filter = this.filters[i];
                ExpressionValueAttribute expressionValue = filter.Expression.GetAttribute<ExpressionValueAttribute>();
                builder.Append(i == 0 ? " where " : " and ");
                builder.Append(filter.Column);
                if (filter.Value == null)
                {
                    builder.Append(" IS NULL ");
                    continue;
                }
                builder.Append(expressionValue.Value);
                if (filter.Type == FilterType.Numeric)
                {
                    builder.Append(filter.Value.ToString());
                }
                else
                {
                    builder.AppendFormat(" '{0}' ", filter.Value.ToString().Replace("'", "''"));
                }
            }
            return builder.ToString();
        }

        public class Builder
        {
            private Query query = new Query();

            public Builder AddFilter(String column, String value)
            {
                this.query.AddFilter(column, value);
                return this;
            }

            public Builder AddFilter(String column, ExpressionType expression, String value)
            {
                this.query.AddFilter(column, expression, value);
                return this;
            }

            public Builder AddFilter(String column, long value)
            {
                this.query.AddFilter(column, value);
                return this;
            }

            public Builder AddFilter(String column, ExpressionType expression, long value)
            {
                this.query.AddFilter(column, expression, value);
                return this;
            }

            public Query Build()
            {
                return this.query;
            }
        }
    }
}
