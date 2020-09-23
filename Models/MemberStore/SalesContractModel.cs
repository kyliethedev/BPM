using System;

namespace Admin.Models.MemberStore
{
    ///=======================================
    /// <summary>
    /// 가맹점 - 판매처 연계 모델
    /// </summary>
    ///=======================================
    public class SalesContractModel
    {
        public Int64  ContractNo      { get; set; }
        public Int64  MemberStoreNo   { get; set; }
        public string MemberStoreName { get; set; }
        public string SalesStoreNo    { get; set; }
        public string SalesStoreName  { get; set; }

        public string AdminID         { get; set; }
        public Int16  UseState        { get; set; }
        public string RegDate         { get; set; }
        public string UpdDate         { get; set; }
    }
}
