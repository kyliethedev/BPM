using System;

namespace Admin.Models.SalesStore
{
    ///=======================================
    /// <summary>
    /// 판매처 모델
    /// </summary>
    ///=======================================
    public class SalesStoreModel
    {
        public Int64  SalesStoreNo   { get; set; }
        public string SalesStoreName { get; set; }
        public string CEOName        { get; set; }
        public string LicenseNo      { get; set; }
        public Int16  UseState       { get; set; }

        public string AdminID        { get; set; }
        public string UpdDate        { get; set; }
        public string RegDate        { get; set; }

        public SalesStoreAcctModel objSalesStoreAcctModel { get; set; }
    }

    ///=======================================
    /// <summary>
    /// 판매처 계좌 모델
    /// </summary>
    ///=======================================
    public class SalesStoreAcctModel
    {
        public Int64  SalesStoreNo  { get; set; }
        public string BankCode      { get; set; }
        public string AccountNo     { get; set; }
        public string AccountHolder { get; set; }
        public string RegDate       { get; set; }

        public string UpdDate       { get; set; }
    }
}
