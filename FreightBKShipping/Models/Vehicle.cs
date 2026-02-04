using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreightBKShipping.Models
{
    [Table("vehicles")]
    public class Vehicle
    {
        [Key]
        [Column("vehicle_id")]
        public int VehicleId { get; set; }

        [Column("vehicle_comppany_id")]
        public int VehicleCompanyId { get; set; }

        [Column("vehicle_addby_user_id")]
        public int VehicleAddedByUserId { get; set; }

        [Column("vehicle_updatedby_user_id")]
        public int? VehicleUpdatedByUserId { get; set; }

        [Column("vehicle_no")]
        public string VehicleNo { get; set; } = string.Empty;

        [Column("vehicle_owner_type")]
        public string? VehicleOwnerType { get; set; }

        [Column("vehicle_account_id")]
        public int VehicleAccountId { get; set; }

        [Column("vehicle_type_id")]
        public int? VehicleTypeId { get; set; }

        [Column("vehicle_group_id")]
        public int? VehicleGroupId { get; set; }

        [Column("vehicle_average")]
        public int? VehicleAverage { get; set; }

        [Column("vehicle_RTO")]
        public string? VehicleRTO { get; set; }

        [Column("vehicle_engine_no")]
        public string? VehicleEngineNo { get; set; }

        [Column("vehicle_chassis_no")]
        public string? VehicleChassisNo { get; set; }

        [Column("vehicle_load_capacity")]
        public int? VehicleLoadCapacity { get; set; }

        [Column("vehicle_make")]
        public string? VehicleMake { get; set; }

        [Column("vehicle_model")]
        public string? VehicleModel { get; set; }

        [Column("vehicle_remarks")]
        public string? VehicleRemarks { get; set; }

        // 1 => Active, 0 => Deactivate, 2 => Delete
        [Column("vehicle_status")]
        public int VehicleStatus { get; set; } = 1;

        [Column("vehicle_fastage")]
        public string? VehicleFastage { get; set; }

        [Column("vehicle_gpsno")]
        public string? VehicleGpsNo { get; set; }

        [Column("vehicle_created")]
        public DateTime VehicleCreated { get; set; } = DateTime.UtcNow;

        [Column("vehicle_updated")]
        public DateTime VehicleUpdated { get; set; } = DateTime.UtcNow;

        [Column("vehicle_tax")]
        public DateTime? VehicleTax { get; set; }

        [Column("vehicle_fitness")]
        public DateTime? VehicleFitness { get; set; }

        [Column("vehicle_statepermit")]
        public DateTime? VehicleStatePermit { get; set; }

        [Column("vehicle_national")]
        public DateTime? VehicleNationalPermit { get; set; }

        [Column("vehicle_insurance")]
        public DateTime? VehicleInsurance { get; set; }

        [Column("vehicle_puc")]
        public DateTime? VehiclePUC { get; set; }

        [Column("vehicle_form9")]
        public DateTime? VehicleForm9 { get; set; }

        [Column("vehicle_calibration")]
        public DateTime? VehicleCalibration { get; set; }

        [Column("vehicle_emi")]
        public DateTime? VehicleEmi { get; set; }

        [Column("vehicle_driver_id")]
        public int? VehicleDriverId { get; set; }
    }
}
