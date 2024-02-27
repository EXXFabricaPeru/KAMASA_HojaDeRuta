using System;
using System.Reflection;
using System.Resources;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Utilities
{
    public class QueryBuilder
    {
        private readonly Company _company;
        private const string NOT_SUPPORT_DATA_BASE_TYPE = "";

        public QueryBuilder(Company company)
        {
            _company = company;
        }

        public QueryType Build()
        {
            switch (_company.DbServerType)
            {
                case BoDataServerTypes.dst_HANADB:
                    return QueryType.HANA;
                case BoDataServerTypes.dst_MSSQL:
                case BoDataServerTypes.dst_MSSQL2005:
                case BoDataServerTypes.dst_MSSQL2008:
                case BoDataServerTypes.dst_MSSQL2012:
                case BoDataServerTypes.dst_MSSQL2014:
                case BoDataServerTypes.dst_MSSQL2016:
                case BoDataServerTypes.dst_MSSQL2017:
                //case BoDataServerTypes.dst_MSSQL2019:
                    return QueryType.SQL;
                default:
                    throw new Exception(NOT_SUPPORT_DATA_BASE_TYPE);
            }
        }
    }

    public class QueryDecorator
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ResourceManager _resourceManager;

        protected QueryDecorator(string resourceName)
        {
            _resourceManager = new ResourceManager(resourceName, GetType().Assembly);
        }

        // public ElementTuple<string> VS_AGRO_GET_PEP_ID
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetItemCode
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetQuimico
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetUbigeo
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetRecurso
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetPEPs
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetGrupoEtapaProduccion
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetLaborCampo
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetPersonal
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetSector
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetEmisiónProduccion
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetCampaña
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetFundo
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetLote
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetCultivo
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetVariedadSegunCultivo
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetGrupodeEtapa
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetCodigoRiego
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetMenu
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }
        // public ElementTuple<string> HANA_VS_AGR_GetPerfilUsuario
        // {
        //     get
        //     {
        //         string currentPropertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
        //         string resourceValue = _resourceManager.GetString(currentPropertyName);
        //         return ElementTuple<string>.MakeTuple(currentPropertyName, resourceValue);
        //     }
        // }

        public static QueryDecorator MakeSQLQuery
            => new QueryDecorator("Ventura.SAP.AGRO.CrossCutting.Resources.SQL_querys");

        public static QueryDecorator MakeHANAQuery
            => new QueryDecorator("Ventura.SAP.AGRO.CrossCutting.Resources.HANA_querys");
    }
}
