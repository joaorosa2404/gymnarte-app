using System.Net.Cache;

namespace GymnArteApp.Server.Models
{
    public class BiometricData
    {
        public int BiometricDataId { get; set; }
        public User User { get; set; } = null!;
        public DateTime RecordData { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int Age { get; set; }
        public decimal FatPercent { get; set; }
        public decimal LeanMassPercent { get; set; }
        public decimal BodyWaterPercent { get; set; }
        public decimal VisceralFat { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
