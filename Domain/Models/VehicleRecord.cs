namespace AssetLocater.Domain.Models
{
    public sealed class VehicleRecord
    {
        public string OldNumberPlate { get; init; }
        public string NewNumberPlate { get; init; }
        public string Description { get; init; }
        public string Chassis { get; init; }
        public string SpecialNumber { get; init; }
        public string OtherSpecialNumber { get; init; }
        public string NationalId { get; init; }
        public string Name { get; init; }
        public string Tel { get; init; }
        public string Cell { get; init; }
        public string PhysicalAddress { get; init; }
    }

}
