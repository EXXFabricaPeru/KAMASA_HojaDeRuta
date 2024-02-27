using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Repository
{
    public abstract class BaseOFTPRepository : Code.Repository<OFTP>
    {
        protected BaseOFTPRepository(Company company) : base(company)
        {

        }

        public abstract Dictionary<string,string> RetrieveFields(string destino);
        public abstract IEnumerable<OFTP> retrieve_template(Expression<Func<OFTP, bool>> expression = null);
        public abstract IEnumerable<FTP1> retrieve_template_columns(Expression<Func<FTP1, bool>> expression = null);

        public abstract IEnumerable<OPCG> retrieve_template_pasarela(Expression<Func<OPCG, bool>> expression = null);

        public abstract IEnumerable<PCG1> retrieve_template_pasarela_columns(Expression<Func<PCG1, bool>> expression = null);

    }
}
