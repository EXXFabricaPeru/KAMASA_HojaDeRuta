using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventura.Addon.ComisionTarjetas.Batch.Code;
using Ventura.Addon.ComisionTarjetas.Batch.Entity;
using Z.Core.Extensions;
using CRIPTORVS;

namespace Ventura.Addon.ComisionTarjetas.Batch.Process
{
    public class UpdateLicenseProcess : BaseProcess
    {
        public UpdateLicenseProcess(SAPConnection sapConnection) : base(sapConnection)
        {

        }



        public override void Build()
        {
            try
            {
                if (SAPConnection.Connected)
                {
                    SAPConnection.Connect();
                }
                string rutaArchivo = ConfigurationManager.AppSettings["Licencia"];
                string licenciaValue = "";
                Licencia licencia;
                if (File.Exists(rutaArchivo))
                {
                    using (StreamReader sr = new StreamReader(rutaArchivo))
                    {
                        string linea;

                        while ((linea = sr.ReadLine()) != null)
                        {
                            licenciaValue = linea;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("El archivo no existe.");
                }
             

                licencia = GetLicense(licenciaValue);
                //UserObjectsMD udo = (UserObjectsMD)SAPConnection.GetBusinessObject(BoObjectTypes.oUserObjectsMD);

                //if (udo.GetByKey("BPVS_OPRD"))
                //{
                //    udo.
                //}

                //UserTable userTable = SAPConnection..Item("VS_PRD1");
                //if (userTable.GetByKey("VS4"))
                //{
                //    //userTable.Name
                //    //for (int i = 0; i < userTable.UserFields.Fields.Count; i++)
                //    //{
                //    //    var x = userTable.UserFields.Fields.Item(i).Value;
                //    //    //if()
                //    //}
                //    ////userTable.

                //    userTable.UserFields.Fields.Item("U_VS_LICENC").Value = licencia.Raw;
                //    userTable.UserFields.Fields.Item("U_VS_FECVIG").Value = licencia.Fechavigencia;
                //    int result = userTable.Update();
                //}

                //var userTable = SAPConnection.GetBusinessObject(BoObjectTypes.oUserTables).To<UserTables>();

                //UserTable userTable2 = SAPConnection.UserTables.Item("VS_PRD1");
                //userTable2.GetByKey("VS");


                SAPbobsCOM.GeneralService oGeneralService = null;
                SAPbobsCOM.GeneralData oGeneralData = null;
                SAPbobsCOM.GeneralDataParams oGeneralParams = null;
                SAPbobsCOM.CompanyService sCmp = SAPConnection.GetCompanyService();
                SAPbobsCOM.GeneralData oChild = null;
                SAPbobsCOM.GeneralDataCollection oChildren = null;

                oGeneralService = sCmp.GetGeneralService("BPVS_OPRD");
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("Code", "VS");      //Primary Key
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                oChildren = oGeneralData.Child("VS_PRD1");  // Child Table Of Main UDO

                //cont = oChildren.Count;
                for (int i = 0; i < oChildren.Count; i++)
                {
                    oChild = oChildren.Item(i);
                    if (oChild.GetProperty("U_VS_CODPRD").ToString()==licencia.CodProducto)
                    {
                        oChild.SetProperty("U_VS_LICENC", licencia.Raw);                        
                        oChild.SetProperty("U_VS_FECVIG", DateTime.ParseExact(licencia.Fechavigencia, "yyyyMMdd",null));
                    }
                }
                oGeneralService.Update(oGeneralData);

                //var recordSet = SAPConnection.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
                //var query = String.Format("UPDATE \"@VS_PRD1\" SET \"U_VS_LICENC\"='{0}' , \"U_VS_FECVIG\"='{1}' where \"U_VS_CODPRD\"='{2}' ", licencia.Raw, licencia.Fechavigencia, licencia.CodProducto);
                //recordSet.DoQuery(query);


            }
            catch (Exception ex)
            {

            }
            finally
            {
                GC.Collect();
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
                    throw new Exception(string.Format("Error de tipo de base", value));
            }
        }

        public Licencia GetLicense(string licenciaEncripted)
        {
            string licencia = Criptor.Desencriptar(licenciaEncripted);
            Licencia licenciaObj = null;
            if (licencia == "") return null;
            try
            {
                licenciaObj = new Licencia(licencia);
                licenciaObj.Raw = licenciaEncripted;
            }
            catch
            {
                return null;
            }
           
            return licenciaObj;
        }
    }
}
