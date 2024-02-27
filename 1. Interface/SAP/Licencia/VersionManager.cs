using SAPbobsCOM;
using System;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Licencia
{
      public class VersionManager
    {
        private Company Company;
        private const string productTable = "VS_OPRD";
        private const string udoCode = "BPVS_OPRD";
        public VersionManager(Company company)
        {
            this.Company = company;
        }

        public void ValidarVersion(string productCode, string productDescription, string version, out int resultCode)
        {
            try
            {
                resultCode = 0;

                //if (!ProductTableExists())
                //CreateProductStructure();
                //RegisterProduct("VS_PD", "Picking y Distribución", "1.1.1.0");
                if (!ProductExists(productCode))
                {
                    RegisterProduct(productCode, productDescription, version);
                    resultCode = 1;
                    return;
                }

                if (!IsUpToDate(productCode, version))
                {
                    UpdateVersion(productCode, version);
                    resultCode = 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsUpToDate(string productCode, string actualVersion)
        {
            try
            {
                string databaseVersion = GetDatabaseVersion(productCode);

                if (string.IsNullOrEmpty(databaseVersion))
                {
                    return false;
                }

                int comp = string.Compare(actualVersion, databaseVersion, StringComparison.Ordinal);

                return comp <= 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetDatabaseVersion(string productCode)
        {
            try
            {
                string query = $"SELECT \"U_VS_VRSION\" FROM \"@VS_PRD1\" WHERE \"U_VS_CODPRD\" = '{productCode}'";
                Recordset recordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                recordset.DoQuery(query);
                return recordset.Fields.Item(0).Value.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateVersion(string productId, string actualVersion)
        {
            Recordset oRecordSet = null;

            try
            {
                string query = $"UPDATE \"@VS_PRD1\" SET \"U_VS_VRSION\" ='{actualVersion}' WHERE \"U_VS_CODPRD\"='{productId}'";
                oRecordSet = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRecordSet.DoQuery(query);
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                if (oRecordSet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                oRecordSet = null;
                GC.Collect();
            }
        }

        public void RegisterProduct(string productId, string productDescription, string version)
        {
            //registrando cabecera de productos
            SAPbobsCOM.ICompanyService cs = null;
            try
            {
                bool existe = false;
                cs = Company.GetCompanyService();
                IGeneralService oGService = cs.GetGeneralService(udoCode);
                GeneralDataParams oGDParams = (GeneralDataParams)oGService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams);

                GeneralData oGData = null;
                GeneralData oChild = null;
                GeneralDataCollection oChildren = null;

                oGDParams.SetProperty("Code", "VS");

                try
                {
                    oGData = oGService.GetByParams(oGDParams);
                    existe = true;
                }
                catch (Exception ex1) { }

                if (!existe)
                    oGData = (GeneralData)oGService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData);

                oGData.SetProperty("Code", "VS");
                oGData.SetProperty("Name", "Ventura Soluciones");

                oChildren = oGData.Child("VS_PRD1");

                oChild = oChildren.Add();
                oChild.SetProperty("U_VS_CODPRD", productId);
                oChild.SetProperty("U_VS_DSCPRD", productDescription);
                oChild.SetProperty("U_VS_VRSION", version);

                if (!existe)
                    oGService.Add(oGData);
                else
                    oGService.Update(oGData);
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                if (cs != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(cs);
                cs = null;
                GC.Collect();
            }
        }

        public bool ProductExists(string productCode)
        {
            try
            {
                string query = $"select COUNT(*) from \"@VS_PRD1\" WHERE \"U_VS_CODPRD\" = '{productCode}'";
                Recordset recordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                recordset.DoQuery(query);
                return Convert.ToInt32(recordset.Fields.Item(0).Value.ToString()) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ProductTableExists()
        {
            try
            {
                string query = $"select COUNT(*) from OUDO where \"Code\" = '{udoCode}'";
                Recordset recordset = (Recordset)Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                recordset.DoQuery(query);
                return Convert.ToInt32(recordset.Fields.Item(0).Value.ToString()) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public void CreateProductStructure()
        //{
        //    Util.CargarEstructuraListaProducto();
        //}
    }
}
