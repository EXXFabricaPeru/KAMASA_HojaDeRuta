using System;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.DisposableBO;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;
using System.Collections.Generic;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    // ReSharper disable once InconsistentNaming
    public class OPDSRepository : BaseOPDSRepository
    {
        private static readonly string SETTINGS_QUERY = $"select * from	\"@{OPDS.ID}\" where \"Code\" = '{{0}}'";

        private static readonly string SETTINGS_COUNT= $"select count(*) as \"cont\" from 	\"@{OPDS.ID}\" ";

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
                            ValueType = x.GetString(@"U_EXX_HOAS_CTIP"),
                            Value = x.GetString(@"U_EXX_HOAS_CVAL")
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

        public override void ValidData()
        {
            try
            {
                var existe = "";

                using (SafeRecordSet safeRecordSet = Company.MakeSafeRecordSet())
                {
                    safeRecordSet.ExecuteQuery(string.Format(SETTINGS_COUNT));
                    existe = safeRecordSet.RetrieveSingleRecord(x =>
                    {
                        try
                        {
                            return x.GetString(@"cont") ;
                            //return new OPDS
                            //{
                            //    Code = x.GetString(@"Code"),
                            //    //Name = x.GetString(@"Name"),
                            //    //ValueType = x.GetString(@"U_EXX_HOAS_CTIP"),
                            //    //Value = x.GetString(@"U_EXX_HOAS_CVAL")
                            //};
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
                if (existe.ToInt32() == 0)
                {

                    List<OPDS> datos = new List<OPDS>();

                    datos.Add(new OPDS
                    {
                        Code = "COMPARTIDO",
                        Name = "compartido hoja de ruta",
                        ValueType = "ST",
                        Value = "C:\\Compartida"
                    });

                    datos.Add(new OPDS
                    {
                        Code = "DBPASS",
                        Name = "Contraseña  de Base de datos",
                        ValueType = "ST",
                        Value = "Passw0rd"
                    });

                    datos.Add(new OPDS
                    {
                        Code = "DBUSER",
                        Name = "Usuario de Base de datos",
                        ValueType = "ST",
                        Value = "SAPINST"
                    });

                    datos.Add(new OPDS
                    {
                        Code = "SERVER",
                        Name = "server de la bd",
                        ValueType = "ST",
                        Value = "192.168.1.215:30015"
                    });

                    foreach (var item in datos)
                    {
                        UserTable userTable = Company.UserTables.Item(OPDS.ID);
                        //userTable.GetByKey(setting.Code);
                        userTable.Code = item.Code;
                        userTable.Name = item.Name;
                        userTable.UserFields.Fields.Item("U_EXX_HOAS_CTIP").Value = item.ValueType;
                        userTable.UserFields.Fields.Item("U_EXX_HOAS_CVAL").Value = item.Value;

                        userTable.Add();
                    }
                }
                    

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }




        }
    }
}