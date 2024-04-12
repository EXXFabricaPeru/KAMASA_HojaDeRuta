using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query;
using Exxis.Addon.HojadeRutaAGuia.Interface.Utilities;
using VersionDLL;
using VersionDLL.FlagElements.Attributes;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup.Versions
{
    public abstract class Versioner
    {
        private IList<string> _categories;

        
        public Flag CurrentFlag { get; }
        public string CurrentVersion { get; }

        protected QueryType QueryType { get; private set; }
        protected QueryDecorator QueryDecorator { get; private set; }

        protected Versioner(string version)
        {
            CurrentVersion = version;
            _categories = new List<string>();
            CurrentFlag = new Flag(CurrentVersion);
            InitializeTables();
        }

        protected virtual void InitializeTables()
        {
        }

        protected virtual void InitializeFormattedSearch()
        {
        }

        public bool HasFormattedSearch
        {
            get
            {
                Type versionerType = GetType();
                MethodInfo method = versionerType.GetMethod(nameof(InitializeFormattedSearch), BindingFlags.NonPublic | BindingFlags.Instance);
                if (method == null)
                    throw new Exception($"The class {versionerType.Name} doesn't inference the method {nameof(InitializeFormattedSearch)}");

                return method.DeclaringType != typeof(Versioner);
            }
        }

        protected void CreateTable(Type classType)
        {
            CurrentFlag.UserTables.LoadClass(classType);
        }

        protected void CreateObject(Type classType)
        {
            CurrentFlag.UDOs.LoadClass(classType);
        }

        protected void SyncSystemTable(Type classType)
        {
            CurrentFlag.LoadSystemTable(classType);
        }

        protected void ForceCreateFormattedSearch<T>(Expression<Func<T, object>> queryProperty, string udoName, string propertyIndexForm, string columnId, BoYesNoEnum isField)
            where T : SAPQueryManager
        {
            var queryManager = QueryInstancer.Make<T>(QueryType);
            if (_categories.Count(t => t == queryManager.Category) == 0)
                CurrentFlag.Categories.Put(queryManager.Category);

            var queryReference = get_query_tuple(queryManager, queryProperty);
            CurrentFlag.Queries.Put(queryManager.Category, queryReference.Id, queryReference.Description);
            CurrentFlag.FmtSearchs.Put(queryReference.Id, queryManager.Category, udoName, propertyIndexForm, columnId, string.Empty, false, false, isField);
        }

        //TODO: Refactor this
        protected void ForceCreateFormattedSearch(string category, string udoName, string propertyIndexForm, string columnId, BoYesNoEnum isField, ElementTuple<string> query)
        {
            CurrentFlag.Categories.Put(category);
            CurrentFlag.Queries.Put(category, query.Id, query.Description);
            CurrentFlag.FmtSearchs.Put(query.Id, category, udoName, propertyIndexForm, columnId, string.Empty, false, false, isField);
        }

        /// <summary>
        /// Apply only UDFs. This works only udo class has defined properties:
        /// <para>Master data: 'Code' and 'Name' like property system</para>
        /// <para>Master document: 'DocEntry', 'DocNum', 'Series', 'CreateDate' like property system</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="queryProperty"></param>
        /// <param name="udoProperty"></param>
        protected void CreateFormattedSearch<T, TK>(Expression<Func<T, object>> queryProperty, Expression<Func<TK, object>> udoProperty)
            where T : SAPQueryManager
        {
            var queryManager = QueryInstancer.Make<T>(QueryType);
            if (_categories.Count(t => t == queryManager.Category) == 0)
                CurrentFlag.Categories.Put(queryManager.Category);

            ElementTuple<string> queryReference = get_query_tuple(queryManager, queryProperty);
            Tuple<string, string, string, BoYesNoEnum> udoTuple = get_udo_tuple(udoProperty);
            CurrentFlag.Queries.Put(queryManager.Category, queryReference.Id, queryReference.Description);
            CurrentFlag.FmtSearchs.Put(queryReference.Id, queryManager.Category, udoTuple.Item1, udoTuple.Item2,
                udoTuple.Item3, null, false, false, udoTuple.Item4);
        }

        private ElementTuple<string> get_query_tuple<T>(T queryManager, Expression<Func<T, object>> queryProperty) where T : SAPQueryManager
        {
            var memberExpression = (MemberExpression) queryProperty.Body;
            string queryPropertyName = memberExpression.Member.Name;
            PropertyInfo property = queryManager.GetType().GetProperty(queryPropertyName);
            return (ElementTuple<string>) property.GetValue(queryManager);
        }

        private Tuple<string, string, string, BoYesNoEnum> get_udo_tuple<T>(Expression<Func<T, object>> udoProperty)
        {
            var columnOrder = default(int);
            var columnName = string.Empty;
            var memberExpression = (MemberExpression) udoProperty.Body;
            var propertyInfo = (PropertyInfo) memberExpression.Member;
            Attribute[] attributes = propertyInfo.GetCustomAttributes();
            foreach (Attribute attribute in attributes)
            {
                var formColumn = attribute as FormColumn;
                if (formColumn != null)
                    columnOrder = formColumn.Order;

                var enhancedColumn = attribute as EnhancedColumn;
                if (enhancedColumn != null)
                    columnOrder = enhancedColumn.Order;

                var fieldNoRelated = attribute as FieldNoRelated;
                if (fieldNoRelated != null)
                    columnName = fieldNoRelated.Descr;
            }

            if (columnOrder == default(int) && string.IsNullOrEmpty(columnName))
                throw new Exception("The assigners is bad setter");

            var udoBase = typeof(T).GetCustomAttribute<Udo>();
            if (udoBase != null)
            {
                var countField = udoBase.Type == BoUDOObjType.boud_MasterData ? 11 : 16;
                columnOrder += columnOrder == 0 ? 0 : countField;
                return Tuple.Create($"UDO_FT_{udoBase.UdoName}", $"{columnOrder}_U_E", "-1", BoYesNoEnum.tYES);
            }

            var fatherReference = typeof(T).GetCustomAttribute<UDOFatherReferenceAttribute>();
            if (fatherReference != null)
            {
                var childIndex = fatherReference.ChildNumber - 1;
                return Tuple.Create($"UDO_FT_{fatherReference.UDOId}", $"{childIndex}_U_G", $"C_{childIndex}_{columnOrder}", BoYesNoEnum.tNO);
            }

            return null;
        }

        public Versioner AppendFormattedSearch(Company company)
        {
            var builder = new QueryBuilder(company);
            QueryType = builder.Build();
            InitializeFormattedSearch();
            return this;
        }
    }
}