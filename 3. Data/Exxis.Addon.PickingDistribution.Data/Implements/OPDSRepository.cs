using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OPDSRepository : BaseOPDSRepository
    {
        private static readonly string SETTINGS_QUERY = $"select * from	\"@{OPDS.ID}\" where \"Code\" = '{{0}}'";

        public OPDSRepository(Company company) : base(company)
        {
        }

        public override OPDS Update(OPDS setting)
        {
            UserTable userTable = Company.UserTables.Item(OPDS.ID);
            userTable.GetByKey(setting.Code);
            userTable.UserFields.Fields.Item(setting.GetFieldWithPrefix(nameof(setting.ValueType))).Value = setting.ValueType;
            userTable.UserFields.Fields.Item(setting.GetFieldWithPrefix(nameof(setting.Value))).Value = setting.Value;
            userTable.Update();
            return setting;
        }

        public override OPDS Setting(string id)
        {
            using (SafeRecordSet safeRecordSet = Company.MakeSafeRecordSet())
            {
                safeRecordSet.ExecuteQuery(string.Format(SETTINGS_QUERY, id));
                return safeRecordSet.RetrieveSingleRecord(x =>
                {
                    try
                    {
                        return new OPDS
                        {
                            Code = x.GetString(@"Code"),
                            Name = x.GetString(@"Name"),
                            ValueType = x.GetString(@"U_VS_LT_CTIP"),
                            Value = x.GetString(@"U_VS_LT_CVAL")
                        };
                    }
                    catch (SafeRecordSet.NotRecordFound)
                    {
                        throw;
                    }
                    catch (Exception exception)
                    {
                        throw new Exception($"[Error] Cannot retrieve information for 'OPDS'. {exception.Message}.");
                    }
                });
            }
        }
    }
}