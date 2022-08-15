using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace AaAFP2
{
    public partial class DbEntities { }

    [AddINotifyPropertyChangedInterface]
    public partial class Order { }

    [AddINotifyPropertyChangedInterface]
    public partial class TypeProduct { }

    [AddINotifyPropertyChangedInterface]
    public partial class Product { }

    [AddINotifyPropertyChangedInterface]
    public partial class ContractPayment { }

    [AddINotifyPropertyChangedInterface]
    public partial class Contract { }

    [AddINotifyPropertyChangedInterface]
    public partial class Material { }

    [AddINotifyPropertyChangedInterface]
    public partial class ProductMaterial { }

    [AddINotifyPropertyChangedInterface]
    public partial class TypeMaterial { }

    [AddINotifyPropertyChangedInterface]
    public partial class Client { }

    [AddINotifyPropertyChangedInterface]
    public partial class StatusOrder { }

    [AddINotifyPropertyChangedInterface]
    public partial class AccrualPointsRwp { }

    [AddINotifyPropertyChangedInterface]
    public partial class Salary { }

    [AddINotifyPropertyChangedInterface]
    public partial class BonusSalary { }

    [AddINotifyPropertyChangedInterface]
    public partial class CostCategory { }

    [AddINotifyPropertyChangedInterface]
    public partial class DeductionFromSalary { }

    [AddINotifyPropertyChangedInterface]
    public partial class Employee { }

    [AddINotifyPropertyChangedInterface]
    public partial class Equipment { }

    [AddINotifyPropertyChangedInterface]
    public partial class FixedCost { }

    [AddINotifyPropertyChangedInterface]
    public partial class ManufacturingOperation { }

    [AddINotifyPropertyChangedInterface]
    public partial class MonthlyCost { }

    [AddINotifyPropertyChangedInterface]
    public partial class OperaionProduct { }

    [AddINotifyPropertyChangedInterface]
    public partial class OrderProduct { }

    [AddINotifyPropertyChangedInterface]
    public partial class OtherCost { }

    [AddINotifyPropertyChangedInterface]
    public partial class OtherRevenue { }

    [AddINotifyPropertyChangedInterface]
    public partial class PackingList { }

    [AddINotifyPropertyChangedInterface]
    public partial class PaymentFixedCost { }

    [AddINotifyPropertyChangedInterface]
    public partial class PaymentSalaryPrepay { }

    [AddINotifyPropertyChangedInterface]
    public partial class Position { }

    [AddINotifyPropertyChangedInterface]
    public partial class Recommendation { }

    [AddINotifyPropertyChangedInterface]
    public partial class RecordPackingList { }

    [AddINotifyPropertyChangedInterface]
    public partial class SalaryPayment { }

    [AddINotifyPropertyChangedInterface]
    public partial class SituationCategoryEnterprise { }

    [AddINotifyPropertyChangedInterface]
    public partial class WeeklyCost { }

    [AddINotifyPropertyChangedInterface]
    public partial class WriteOffMaterial { }

    [AddINotifyPropertyChangedInterface]
    public partial class ConditionFinance { }

    [AddINotifyPropertyChangedInterface]
    public partial class ConditionManufacturing { }

    [AddINotifyPropertyChangedInterface]
    public partial class ConditionOrdersAndEmployees { }

}
