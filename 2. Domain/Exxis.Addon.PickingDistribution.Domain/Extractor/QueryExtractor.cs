using System;
using System.Collections.Generic;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Extractor
{
    public class QueryExtractor
    {
        private readonly string _query;

        public IList<QueryParameter> Parameters { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public QueryExtractor(string query)
        {
            _query = query;
        }

        public string Extract()
        {
            Parameters = new List<QueryParameter>();
            int index = 0;
            return get_formatted_query(ref index, string.Empty);
        }

        private string get_formatted_query(ref int index, string resultQuery)
        {
            resultQuery = read_query(ref index, i => _query[i] == '$' && _query[i + 1] == '{', resultQuery);
            if (index >= _query.Length)
                return resultQuery;

            index++;
            string parameter = read_query(ref index, i => _query[i] == '}', string.Empty);
            resultQuery += $"{{{Parameters.Count}}}";

            Parameters.Add(new QueryParameter(parameter));
            return get_formatted_query(ref index, resultQuery);
        }

        private string read_query(ref int index, Func<int, bool> filter, string resultQuery)
        {
            if (index >= _query.Length || filter(index))
            {
                index++;
                return resultQuery;
            }

            char c = _query[index];
            index++;
            return read_query(ref index, filter, resultQuery + c);
        }
    }
}