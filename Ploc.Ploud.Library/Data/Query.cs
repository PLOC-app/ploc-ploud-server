using System;
using System.Collections.Generic;
using System.Text;

namespace Ploc.Ploud.Library
{
    public class Query : IQuery
    {
        private IList<Filter> filters = null;

        public void AddFilter(string column, string value)
        {
            this.AddFilter(column, ExpressionType.Equal, value);
        }

        public void AddFilter(string column, ExpressionType expression, string value)
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

        public void AddFilter(string column, long value)
        {
            this.AddFilter(column, ExpressionType.Equal, value);
        }

        public void AddFilter(string column, ExpressionType expression, long value)
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
                return string.Empty;
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

            public Builder AddFilter(string column, string value)
            {
                this.query.AddFilter(column, value);

                return this;
            }

            public Builder AddFilter(string column, ExpressionType expression, string value)
            {
                this.query.AddFilter(column, expression, value);

                return this;
            }

            public Builder AddFilter(string column, long value)
            {
                this.query.AddFilter(column, value);
                return this;
            }

            public Builder AddFilter(string column, ExpressionType expression, long value)
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
