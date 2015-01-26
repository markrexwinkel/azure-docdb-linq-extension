using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfoSupport.Azure.DocumentDb
{
    class ConfigurableDocumentQuery<T> : IOrderedQueryable<T>
    {
        private IOrderedQueryable<T> _docQuery;
        private JsonSerializerSettings _settings;
        private DocumentClient _client;
        private string _docsLink;
        private IQueryProvider _provider;

        public ConfigurableDocumentQuery(DocumentClient client, string docsLink, IOrderedQueryable<T> query, JsonSerializerSettings settings)
        {
            _client = client;
            _docsLink = docsLink;
            _docQuery = query;
            _settings = settings;
            _provider = new ConfigurableDocumentQueryProvider<T>(query.Provider, client, docsLink, settings);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _docQuery.AsEnumerable<T>(_client, _docsLink, _settings).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Type ElementType
        {
            get { return _docQuery.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return _docQuery.Expression; }
        }

        public IQueryProvider Provider
        {
            get { return _provider; }
        }
    }
}
