using GeolocationAPI.SAPComponents;

namespace GeolocationAPI.Repositories
{
    public class UnitOfWork
    {
        private readonly SAPConnector _connector;

        public UnitOfWork(SAPConnector connector)
        {
            _connector = connector;
        }

        private TransferOrderRepository _transferOrderRepository;

        public TransferOrderRepository TransferOrderRepository => _transferOrderRepository ??= new TransferOrderRepository(_connector);

        public TimeWindowDispatchRepository TimeWindowDispatchRepository => new TimeWindowDispatchRepository(_connector);

        public PriorityIndicatorRepository PriorityIndicatorRepository => new PriorityIndicatorRepository(_connector);

        public VelocityRepository VelocityRepository => new VelocityRepository(_connector);

        public VehicleRepository VehicleRepository => new VehicleRepository(_connector);

        public RouteMapRepository RouteMapRepository => new RouteMapRepository(_connector);

        public TimeDispatchRepository TimeDispatchRepository => new TimeDispatchRepository(_connector);

        public SettingRepository SettingRepository => new SettingRepository(_connector);

        public DeliveryControlRepository DeliveryControlRepository => new DeliveryControlRepository(_connector);

        public BusinessPartnerRepository BusinessPartnerRepository => new BusinessPartnerRepository(_connector);

        public WarehouseRepository WarehouseRepository => new WarehouseRepository(_connector);

        private AssignedCertifyRepository _assignedCertifyRepository;

        public AssignedCertifyRepository AssignedCertifyRepository => _assignedCertifyRepository ??= new AssignedCertifyRepository(_connector);

        public TransporterRepository TransporterRepository => new TransporterRepository(_connector);

        public ConfigurationRepository ConfigurationRepository => new ConfigurationRepository(_connector);
    }
}