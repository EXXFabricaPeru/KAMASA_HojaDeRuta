using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Attributes;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Utilities;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository
{
    // ReSharper disable once InconsistentNaming
    public class OWTQDocumentRepository : BaseSAPDocumentRepository<OWTQ, WTQ1>
    {
        public OWTQDocumentRepository(Company company) : base(company)
        {
        }

        public override IEnumerable<OWTQ> RetrieveDocuments(Expression<Func<OWTQ, bool>> expression)
        {
            IEnumerable<OWTQ> documents = base.RetrieveDocuments(expression)
                .ToList();

            var unitOfWork = new UnitOfWork(Company);
            
            foreach (OWTQ document in documents)
            {
                OWHS warehouse = unitOfWork.WarehouseRepository.RetrieveWarehouse(document.ToWareHouseCode);
                document.ShippingAddressCode = warehouse.Code;
                document.ShippingAddressDescription = warehouse.Street;
            }

            return documents;
        }

        public override Tuple<bool, string> UpdateCustomFieldsFromDocument(OWTQ document)
        {
            Type documentType = document.GetType();
            var sapObject = documentType.GetCustomAttribute<SAPObjectAttribute>();
            var saleOrder = Company.GetBusinessObject(sapObject.SapTypes).To<StockTransfer>();
            saleOrder.GetByKey(document.DocumentEntry);
            Fields userFields = saleOrder.UserFields.Fields;
            IEnumerable<PropertyInfo> documentProperties = documentType.GetProperties()
                .Where(property => (property.GetCustomAttribute<SAPColumnAttribute>()?.SystemField ?? true) != true &&
                                   (property.GetValue(document)?.Equals(property.PropertyType.GetDefaultValue()) ?? true) != true);

            foreach (PropertyInfo property in documentProperties)
            {
                object propertyValue = property.GetValue(document);
                var columnAttribute = property.GetCustomAttribute<SAPColumnAttribute>();

                Field userField = userFields.Item(columnAttribute.Name);
                object value = get_valid_value(userField.ValidValues, propertyValue);
                userField.Value = value;
            }

            if (saleOrder.Update().IsDefault())
                return Tuple.Create(true, string.Empty);

            var description = Company.GetLastErrorDescription();
            return Tuple.Create(false, description);
        }

        private object get_valid_value(IValidValues validValues, object value)
        {
            for (var i = 0; i < validValues.Count; i++)
            {
                ValidValue item = validValues.Item(i);
                if (item.Description == value.ToString())
                    return item.Value;
            }

            return value;
        }

        public  Tuple<bool, string> RegisterByPicking(List<CrossCutting.Model.Local.Item> items)
        {
            return Tuple.Create(true, "");
        }

        public override void CloseDocument(int documentEntry)
        {
            
            var sapDocument = Company.GetBusinessObject(BoObjectTypes.oInventoryTransferRequest).To<StockTransfer>();
            sapDocument.GetByKey(documentEntry);
            var result = sapDocument.Close();
            if (result != 0)
                throw new Exception($"[ERROR] ({result}): {Company.GetLastErrorDescription()}");
        }
    }
}