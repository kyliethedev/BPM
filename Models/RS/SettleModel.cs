using System;

namespace Admin.Models.RS
{
    ///=======================================
    /// <summary>
    /// 정산 모델
    /// </summary>
    ///=======================================
    public class SettleReqModel
    {
        public Int16  LoginType     { get; set; }
        public Int64  StoreNo       { get; set; }
        public string SettleYM      { get; set; }
        public Int16  SettleType    { get; set; }
    }

    ///=======================================
    /// <summary>
    /// 직접결제정산 모델
    /// </summary>
    ///=======================================
    public class DirectSettleResModel
    {
        public Int64  MemberStoreNo   { get; set; }
        public string MemberStoreName { get; set; }
        public Int64  UserNo          { get; set; }
        public string UserName        { get; set; }
        public int    ApplyAmt        { get; set; }  //신청금액

        public Int16  StateCode       { get; set; }  //상태(1:요청, 2:승인, 3:거절, 4:완료)
        public string ApplyDate       { get; set; }  //승인일시
        public int    ApplyCnt        { get; set; }  //신청개수
    }

    ///=======================================
    /// <summary>
    /// 간접결제정산 모델
    /// </summary>
    ///=======================================
    public class BPApplySettleResModel
    {
        public Int64  SalesStoreNo       { get; set; }
        public string SalesStoreName     { get; set; }
        public Int64  MemberStoreNo      { get; set; }
        public string MemberStoreName    { get; set; }
        public Int64  ItemNo             { get; set; }

        public Int64  ItemGroupNo        { get; set; }
        public string ItemGroupName      { get; set; }
        public string ItemName           { get; set; }
        public int    ItemPrice          { get; set; }
        public int    PurchaseCnt        { get; set; }
        
        public int    TotalAmt           { get; set; }
        public string RegDate            { get; set; }
        public int    TotalCnt           { get; set; }
        public Int64  UserNo             { get; set; }
        public string UserName           { get; set; }

        public int    ApplyAmt           { get; set; }
        public string ApplyDate          { get; set; }
    }
}
