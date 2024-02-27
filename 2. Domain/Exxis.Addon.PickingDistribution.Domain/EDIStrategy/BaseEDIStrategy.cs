using System.Collections.Generic;
using System.Linq;
using Exxis.Addon.HojadeRutaAGuia.Data;
using Exxis.Addon.HojadeRutaAGuia.Domain.EDIStrategy.Structures;
using SystemModels = Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System;
using UDOModels = Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.EDIStrategy
{
    public abstract class BaseEDIStrategy
    {
        private IEnumerable<UDOModels.Detail.FTP1> _populatedTemplateColumns;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        protected BaseEDIStrategy(string pathFile, UDOModels.Header.OFTP fileTemplate, SystemModels.Header.OCRD businessPartner, UnitOfWork unitOfWork)
        {
            PathFile = pathFile;
            FileTemplate = fileTemplate;
            BusinessPartner = businessPartner;
            UnitOfWork = unitOfWork;
        }
        
        protected string PathFile { get; }

        protected UDOModels.Header.OFTP FileTemplate { get; }

        protected SystemModels.Header.OCRD BusinessPartner { get; }

        protected UnitOfWork UnitOfWork { get; }

        protected IEnumerable<UDOModels.Detail.FTP1> PopulatedTemplateColumns
        {
            get
            {
                return _populatedTemplateColumns ?? (_populatedTemplateColumns = FileTemplate.ColumnDetail
                    .Where(t => !string.IsNullOrEmpty(t.SAPValue)));
            }
        }

        public abstract void ValidateFile();
        public abstract IEnumerable<EDIRecord> BuildEDIRecords();
        public abstract IEnumerable<SystemModels.Header.Document.ORDR> BuildSaleOrders();
        public abstract IEnumerable<RegisterResume> RegisterSaleOrders();
    }
}