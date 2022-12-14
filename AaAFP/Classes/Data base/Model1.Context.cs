//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AaAFP2
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DbEntities : DbContext
    {
        public DbEntities()
            : base("name=DbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccrualPointsRwp> AccrualsPointsRwp { get; set; }
        public virtual DbSet<BonusSalary> BonusSalaries { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ConditionFinance> ConditionsFinance { get; set; }
        public virtual DbSet<ConditionManufacturing> ConditionsManufacturing { get; set; }
        public virtual DbSet<ConditionOrdersAndEmployees> ConditionsOrdersAndEmployees { get; set; }
        public virtual DbSet<ContractPayment> ContractPayments { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<CostCategory> CostCategories { get; set; }
        public virtual DbSet<DeductionFromSalary> DeductionsFromSalary { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeePosition> EmployeesPositions { get; set; }
        public virtual DbSet<EnterpriseInfo> EnterprisesInfo { get; set; }
        public virtual DbSet<FixedCost> FixedCosts { get; set; }
        public virtual DbSet<ManufacturingOperation> ManufacturingOperations { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MonthlyCost> MonthlyCosts { get; set; }
        public virtual DbSet<OperationProduct> OperationsProducts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OtherCost> OtherCosts { get; set; }
        public virtual DbSet<OtherRevenue> OtherRevenues { get; set; }
        public virtual DbSet<PackingList> PackingLists { get; set; }
        public virtual DbSet<PaymentFixedCost> PaymentsFixedCosts { get; set; }
        public virtual DbSet<PaymentSalaryPrepay> PaymentsSalaryPrepay { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductMaterial> ProductsMaterials { get; set; }
        public virtual DbSet<Recommendation> Recommendations { get; set; }
        public virtual DbSet<RecordPackingList> RecordsPackingList { get; set; }
        public virtual DbSet<SalaryPayment> SalaryPayments { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<TypeMaterial> TypesMaterials { get; set; }
        public virtual DbSet<TypeProduct> TypesProducts { get; set; }
        public virtual DbSet<UnitMeasure> UnitsMeasures { get; set; }
        public virtual DbSet<WeeklyCost> WeeklyCosts { get; set; }
        public virtual DbSet<WriteOffMaterial> WriteOffMaterials { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
    }
}
