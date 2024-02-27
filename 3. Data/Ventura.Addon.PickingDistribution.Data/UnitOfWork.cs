// ReSharper disable InconsistentNaming

using SAPbobsCOM;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Detail.DocumentLine;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.Data.Implements;
using Exxis.Addon.HojadeRutaAGuia.Data.Implements.DocumentRepository;
using Exxis.Addon.HojadeRutaAGuia.Data.Repository;

namespace Exxis.Addon.HojadeRutaAGuia.Data
{
    public class UnitOfWork
    {
        private BaseOITMRepository _itemsRepository;
        private BaseOTDIRepository _mappingTableRepository;
        private BaseOADMRepository _administratorRepository;
        private BaseOWHSRepository _warehouseRepository;
        private BaseBusinessPartnerRepository _businessPartnerRepository;
        private BaseOFTPRepository _filesRepository;
        private BaseOFUPRepository _filesUpRepository;
        private BaseOARDRepository _routeRepository;
        private BaseORTRRepository _transferOrderRepository;
        private BaseCounterRepository _counterRepository;
        private BaseTransporterLiquidateRepository _transporterLiquidateRepository;
        private BaseMessageRepository _messageRepository;
        private BaseOCSTRepository _departamentsRepository;
        private BaseLiquidacionTarjetasRepository _liquidacionTarjetasRepository;

        private readonly Company _company;
        private readonly SAPbouiCOM.Company _companyUI;

        public UnitOfWork(Company company,SAPbouiCOM.Company companyUI=null)
        {
            _company = company;
            _companyUI = companyUI;
        }

        public BaseTariffMotiveRepository TariffMotiveRepository => new TariffMotiveRepository(_company);

        public BaseSAPDocumentRepository<ORDR, RDR1> SaleOrderRepository => new ORDRDocumentRepository(_company);

        public BaseSAPDocumentRepository<ORRR, RRR1> ReturnRequestOrderRepository => new ORRRDocumentRepository(_company);

        public BaseSAPDocumentRepository<OWTQ, WTQ1> InventoryTransferRequestRepository => new OWTQDocumentRepository(_company);

        public BaseSAPDocumentRepository<OINV, INV1> InvoiceRepository => new OINVDocumentRepository(_company);

        public BaseSAPDocumentRepository<ODPI, DPI1> DownPaymentInvoiceRepository => new ODPIDocumentRepository(_company);

        public BaseSAPDocumentRepository<ORIN, RIN1> CreditNoteRepository => new ORINDocumentRepository(_company);

        public BaseSAPDocumentRepository<ODLN, DLN1> DeliveryRepository => new ODLNDocumentRepository(_company);

        public BaseSAPDocumentRepository<ORDN, RDN1> ReturnOrderRepository => new ORDNDocumentRepository(_company);

        public BaseSAPDocumentRepository<OQUT, QUT1> SaleQuotationRepository => new OQUTDocumentRepository(_company);

        public BaseSAPDocumentRepository<OPOR, POR1> PurchaseOrderRepository => new OPORDocumentRepository(_company);

        public BaseSAPDocumentRepository<OPRQ, PRQ1> RequestPurchaseOrderRepository => new OPRQDocumentRepository(_company);

        public BaseSAPDocumentRepository<OPDN, PDN1> GoodReceiptFromPurchaseOrderRepository => new OPDNDocumentRepository(_company);

        public BaseOIBTRepository BatchRepository => new OIBTRepository(_company);

        public BaseOPDSRepository SettingsRepository => new OPDSRepository(_company);

        public BaseORTRRepository TransferOrderRepository
            => _transferOrderRepository ?? (_transferOrderRepository = new ORTRRepository(_company));

        public BaseTariffRepository TariffRepository => new TariffRepository(_company);

        public BaseODSPRepository LoadChargePriorityRepository => new ODSPRepository(_company);

        public BaseInfrastructureRepository InfrastructureRepository => new InfrastructureRepository(_company);

        public BaseOTRDRepository TransporterRepository => new OTRDRepository(_company);

        public BaseEmployeeRepository EmployeeRepository => new EmployeeRepository(_company);

        public BaseDriverRepository DriverRepository => new DriverRepository(_company);

        public BaseVehicleRepository VehicleRepository => new VehicleRepository(_company);

        public BaseDistributionHistoryRepository DistributionHistoryRepository => new DistributionHistoryRepository(_company);

        public BaseCertificateAssignmentRepository CertificateAssignmentRepository => new CertificateAssignmentRepository(_company);

        public BaseTransferItemRepository TransferItemRepository => new TransferItemRepository(_company);

        public BaseTransporterLiquidateRepository TransporterLiquidateRepository
            => _transporterLiquidateRepository ?? (_transporterLiquidateRepository = new TransporterLiquidateRepository(_company));

        public BaseOARDRepository RouteRepository
            => _routeRepository ?? (_routeRepository = new OARDRepository(_company));

        public BaseOWHSRepository WarehouseRepository
            => _warehouseRepository ?? (_warehouseRepository = new OWHSRepository(_company));

        public BaseOTDIRepository MappingTableRepository
            => _mappingTableRepository ?? (_mappingTableRepository = new OTDIRepository(_company));

        public BaseOADMRepository AdministratorRepository
            => _administratorRepository ?? (_administratorRepository = new OADMRepository(_company));

        public BaseOITMRepository ItemsRepository
            => _itemsRepository ?? (_itemsRepository = new OITMRepository(_company));

        public BaseOFTPRepository FilesRepository
            => _filesRepository ?? (_filesRepository = new OFTPRepository(_company));

        public BaseOFUPRepository FileUploadRepository
            => _filesUpRepository ?? (_filesUpRepository = new OFUPRepository(_company));

        public BaseBusinessPartnerRepository BusinessPartnerRepository
            => _businessPartnerRepository ?? (_businessPartnerRepository = new BusinessPartnerRepository(_company));

        public BaseCounterRepository CounterRepository
            => _counterRepository ?? (_counterRepository = new CounterRepository(_company));

        public BaseMessageRepository MessageRepository
            => _messageRepository ?? (_messageRepository = new MessageRepository(_company));

        public BaseOCSTRepository DepartmentRepository
            => _departamentsRepository ?? (_departamentsRepository = new OCSTRepository(_company));

        public BaseLiquidacionTarjetasRepository LiquidacionTarjetasRepository
           => _liquidacionTarjetasRepository ?? (_liquidacionTarjetasRepository = new LiquidacionTarjetasRepository(_company));
    }
}