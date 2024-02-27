using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO
{
    public class SafeRecordSet : IDisposable
    {
        public class NotRecordFound : Exception
        {
            /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
            /// <param name="query">Query to uses the <see cref="T:Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO.SafeRecordSet"/></param>
            public NotRecordFound(string query) : base($"No existe registros para el query: {query}")
            {
            }
        }

        private bool _disposedValue;
        private SAPbobsCOM.RecordsetEx _recordSet;

        public string ExecutedQuery { get; private set; }

        public SafeRecordSet(SAPbobsCOM.RecordsetEx recordSet)
        {
            _recordSet = recordSet;
        }

        public IEnumerable<T> RetrieveListFromQuery<T>(string query, Func<SafeRecordSet, T> function)
        {
            IList<T> result = new List<T>();
            _recordSet.DoQuery(query);
            ExecutedQuery = query;
            

            while (!_recordSet.EoF)
            {
                result.Add(function(this));
                _recordSet.MoveNext();
            }

            return result;
        }

        public IEnumerable<T> RetrieveListFromQuery<T>(string query)
        {
            IList<T> result = new List<T>();
            _recordSet.DoQuery(query);
            ExecutedQuery = query;
            IEnumerable<PropertyInfo> entityProperties = typeof(T)
                .RetrieveSAPProperties()
                .ToList();

            while (!_recordSet.EoF)
            {
                var item = _recordSet.MakeBasicSAPEntity<T>(entityProperties);
                result.Add(item);
                _recordSet.MoveNext();
            }

            return result;
        }

        public void ExecuteQuery(string query)
        {
            ExecutedQuery = query;
            _recordSet.DoQuery(query);
        }

        public void ReadSingleRecord(Action<SafeRecordSet> action)
        {
            action(this);
        }

        public T RetrieveSingleRecord<T>(Func<SafeRecordSet, T> action)
        {
            return action(this);
        }

        public void ReadSingleRecord(Action action)
        {
            action();
        }
        
        public void Read(Action<SafeRecordSet> action)
        {
            while (!_recordSet.EoF)
            {
                action(this);
                _recordSet.MoveNext();
            }
        }

        public void Read(Action action)
        {
            while (!_recordSet.EoF)
            {
                action();
                _recordSet.MoveNext();
            }
        }

        public object GetValue(string columnAlias)
            => _recordSet.GetColumnValue(columnAlias);

        public int? GetInt32(string columnAlias)
        {
            object value = _recordSet.GetColumnValue(columnAlias);
            return value?.ToInt32();
        }

        public decimal? GetDecimal(string columnAlias)
        {
            object value = _recordSet.GetColumnValue(columnAlias);
            return value?.ToDecimal();
        }

        public string GetString(string columnAlias)
        {
            if (_recordSet.EoF)
                throw new NotRecordFound(ExecutedQuery);

            object value = _recordSet.GetColumnValue(columnAlias);
            return value == null ? string.Empty : value.ToString();
        }

        public DateTime? GetDateTime(string columnAlias)
        {
            string value = GetString(columnAlias);
            if (string.IsNullOrEmpty(value))
                return null;
            return Convert.ToDateTime(value);
        }

        public DateTime TryGetDateTime(string columnAlias)
        {
            string value = GetString(columnAlias);
            return Convert.ToDateTime(value);
        }

        public string GetString(int columnIndex)
        {
            if (_recordSet.EoF)
                throw new NotRecordFound(ExecutedQuery);
            
            object value = _recordSet.GetColumnValue(columnIndex);
            return value == null ? string.Empty : value.ToString();
        }

        public int TryGetInt32(string columnAlias)
        {
            object value = _recordSet.GetColumnValue(columnAlias);
            return value?.ToInt32() ?? default(int);
        }

        public decimal TryGetDecimal(string columnAlias)
        {
            object value = _recordSet.GetColumnValue(columnAlias);
            return value?.ToDecimal() ?? default(decimal);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
                return;

            if (disposing)
            {
                Marshal.ReleaseComObject(_recordSet);
                _recordSet = null;
                GC.Collect();
            }

            _disposedValue = true;
        }
    }
}