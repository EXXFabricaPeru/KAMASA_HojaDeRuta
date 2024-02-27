using System;
using SAPbobsCOM;
using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.CrossCutting.Resources;
using Ventura.Addon.ComisionTarjetas.Domain.Code;

namespace Ventura.Addon.ComisionTarjetas.Batch.Process
{
    public abstract class BaseProcess
    {
        protected Company SAPConnection { get; }

        protected BaseProcess(SAPConnection sapConnection)
        {
            Company company = new CompanyClass();
            company.Server = sapConnection.Server;
            company.DbServerType = get_server_type(sapConnection.Type);
            company.CompanyDB = sapConnection.Company;
            company.UserName = sapConnection.UserName;
            company.Password = sapConnection.Password;
            SAPConnection = company;

            if (!company.Connected)
            {
                var con=company.Connect();

            }
                
        }

        private BoDataServerTypes get_server_type(string value)
        {
            switch (value)
            {
                case "HANADB":
                    return BoDataServerTypes.dst_HANADB;
                case "MSSQL2005":
                    return BoDataServerTypes.dst_MSSQL2005;
                case "MSSQL2008":
                    return BoDataServerTypes.dst_MSSQL2008;
                case "MSSQL2012":
                    return BoDataServerTypes.dst_MSSQL2012;
                case "MSSQL2014":
                    return BoDataServerTypes.dst_MSSQL2014;
                case "MSSQL2016":
                    return BoDataServerTypes.dst_MSSQL2016;
                case "MSSQL2017":
                    return BoDataServerTypes.dst_MSSQL2017;
                default:
                    throw new Exception(string.Format(ErrorMessages.MissmatchSAPDatabaseType, value));
            }
        }

        public abstract void Build();

        protected T MakeDomain<T>() where T : BaseDomain 
            => (T)Activator.CreateInstance(typeof(T), SAPConnection);
    }
}