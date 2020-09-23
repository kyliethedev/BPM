using System;

namespace Admin.Models.Point
{
    ///=======================================
    /// <summary>
    /// 직원 모델
    /// </summary>
    ///=======================================
    public class UserModel
    {
        public Int64  UserNo          { get; set; }
        public string UserID          { get; set; }
        public string UserName        { get; set; }
        public string PassWord        { get; set; }
        public int    MemberStoreNo   { get; set; } //가맹점 번호

        public string TeamName        { get; set; } //소속팀명
        public string PhoneNo         { get; set; }
        public string EmpYMD          { get; set; } //입사일
        public int    BPLevelGroupNo  { get; set; } //BP등급 그룹번호 --연차, 직급, 커스터마이징
        public int    BPLevelNo       { get; set; } //등급 번호

        public string BPLevelName     { get; set; } //등급명
        public Int16  UseState        { get; set; } //상태(1:정상, 2:정지)
        public string AdminID         { get; set; }
        public string RegDate         { get; set; }
        public string UpdDate         { get; set; }

        public UserAcctModel objUserAccountModel { get; set; }
        public UserBPModel   objUserBPModel      { get; set; }
    }

    ///=======================================
    /// <summary>
    /// 직원 계좌 모델
    /// </summary>
    ///=======================================
    public class UserAcctModel
    {
        public Int64  UserNo        { get; set; }
        public string BankCode      { get; set; }
        public string AccountNo     { get; set; }
        public string AccountHolder { get; set; }
        public string RegDate       { get; set; }

        public string UpdDate       { get; set; }
    }

    ///=======================================
    /// <summary>
    /// 직원별 복지포인트 모델
    /// </summary>
    ///=======================================
    public class UserBPModel
    {
        public Int64  UserNo      { get; set; }
        public int    WorkBPAmt   { get; set; } //복지포인트 추감/차감 작업 금액
        public int    TotalINAmt  { get; set; }
        public int    TotalOUTAmt { get; set; }
        public int    RemainAmt   { get; set; }

        public string Description { get; set; }
        public string AdminID     { get; set; }
        public string RegDate     { get; set; }
        public string UpdDate     { get; set; }
        public int    BPAmt       { get; set; }
    }

    ///=======================================
    /// <summary>
    /// 직원별 복지포인트 사용내역 모델
    /// </summary>
    ///=======================================
    public class UserBPUseDetail
    {
        public Int64  PurchaseNo   { get; set; }
        public Int64  ItemNo       { get; set; }
        public int    ItemPrice    { get; set; }
        public int    TotalAmt     { get; set; }
        public string Description  { get; set; }

        public string Location     { get; set; } //사용 위치(1:복지포인트몰, 2:관리자)
        public string YMD          { get; set; }
        public string CYMD         { get; set; }
    }

    ///=======================================
    /// <summary>
    /// 직원별 복지포인트 지급내역 모델
    /// </summary>
    ///=======================================
    public class UserBPProvideDetail
    {
        public Int64  BPNo        { get; set; }
        public string Description { get; set; }
        public int    BPAmt       { get; set; }
        public int    RemainAmt   { get; set; }
        public Int16  UseState    { get; set; } //상태(1:정상, 2:취소)

        public string AdminID     { get; set; }
        public string RegDate     { get; set; }
    }
}

