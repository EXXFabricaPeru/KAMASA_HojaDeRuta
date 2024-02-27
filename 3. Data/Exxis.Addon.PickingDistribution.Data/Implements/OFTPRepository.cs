using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements
{
    public class OFTPRepository : BaseOFTPRepository
    {
        private const string CAMPOS_QUERY = @"select * from ""@VS_LT_OPCG"" {0}";
        private const string CAMPOS_DET_QUERY = @"select * from ""@VS_LT_PCG1"" {0}";

        public OFTPRepository(Company company) : base(company)
        {
        }

        public override Dictionary<string, string> RetrieveFields(string destino)
        {
            Dictionary<string, string> campos = new Dictionary<string, string>();
            string query = $@"select ""AliasID"",""Descr"" from CUFD where ""TableID""='{(destino == "C" ? "ORDR" : "RDR1")}'";
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();

            recordSet.DoQuery(query);
            while (!recordSet.EoF)
            {
                string id = "U_" + recordSet.GetColumnValue("AliasID").ToString();
                string val = recordSet.GetColumnValue("Descr").ToString();

                if (destino == "D" && id == "U_EXK_CFIL") continue;

                campos.Add(id, val);

                recordSet.MoveNext();
            }

            return campos;
        }

        public override IEnumerable<OFTP> retrieve_template(Expression<Func<OFTP, bool>> expression = null)
        {
            IList<OFTP> result = new List<OFTP>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var whereStatement = string.Empty;
            if (expression != null)
                whereStatement = $"where {QueryHelper.ParseToHANAQuery(expression)}";

            IEnumerable<PropertyInfo> entityProperties = typeof(OFTP)
                .RetrieveSAPProperties()
                .ToList();

            recordSet.DoQuery(string.Format(CAMPOS_QUERY, whereStatement));
            while (!recordSet.EoF)
            {
                var item = recordSet.MakeBasicSAPEntity<OFTP>(entityProperties);
                result.Add(item);
                recordSet.MoveNext();
            }

            return result;
        }

        public override IEnumerable<FTP1> retrieve_template_columns(Expression<Func<FTP1, bool>> expression = null)
        {
            IList<FTP1> result = new List<FTP1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var whereStatement = string.Empty;
            if (expression != null)
                whereStatement = $"where {QueryHelper.ParseToHANAQuery(expression)}";

            IEnumerable<PropertyInfo> entityProperties = typeof(FTP1)
                .RetrieveSAPProperties()
                .ToList();

            recordSet.DoQuery(string.Format(CAMPOS_DET_QUERY, whereStatement));
            while (!recordSet.EoF)
            {
                var item = recordSet.MakeBasicSAPEntity<FTP1>(entityProperties);
                result.Add(item);
                recordSet.MoveNext();
            }

            return result;
        }

        public override IEnumerable<OPCG> retrieve_template_pasarela(Expression<Func<OPCG, bool>> expression = null)
        {
            IList<OPCG> result = new List<OPCG>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var whereStatement = string.Empty;
            if (expression != null)
                whereStatement = $"where {QueryHelper.ParseToHANAQuery(expression)}";

            IEnumerable<PropertyInfo> entityProperties = typeof(OPCG)
                .RetrieveSAPProperties()
                .ToList();

            recordSet.DoQuery(string.Format(CAMPOS_QUERY, whereStatement));
            while (!recordSet.EoF)
            {
                var item = recordSet.MakeBasicSAPEntity<OPCG>(entityProperties);
                result.Add(item);
                recordSet.MoveNext();
            }

            return result;
        }

        public override IEnumerable<PCG1> retrieve_template_pasarela_columns(Expression<Func<PCG1, bool>> expression = null)
        {
            IList<PCG1> result = new List<PCG1>();
            var recordSet = Company.GetBusinessObject(BoObjectTypes.BoRecordsetEx).To<RecordsetEx>();
            var whereStatement = string.Empty;
            if (expression != null)
                whereStatement = $"where {QueryHelper.ParseToHANAQuery(expression)}";

            IEnumerable<PropertyInfo> entityProperties = typeof(PCG1)
                .RetrieveSAPProperties()
                .ToList();

            recordSet.DoQuery(string.Format(CAMPOS_DET_QUERY, whereStatement));
            while (!recordSet.EoF)
            {
                var item = recordSet.MakeBasicSAPEntity<PCG1>(entityProperties);
                result.Add(item);
                recordSet.MoveNext();
            }

            return result;
        }

        private string GetValue(RecordsetEx oRS, string field)
        {
            if (oRS.GetColumnValue(field) != null)
            {
                return oRS.GetColumnValue(field).ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}