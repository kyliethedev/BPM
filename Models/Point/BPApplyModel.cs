using System;

namespace Admin.Models.Point
{
    ///=======================================
    /// <summary>
    /// 복지포인트 신청내역 모델
    /// </summary>
    ///=======================================
    public class BPApplyModel
    {
        public int    RowNum          { get; set; }
        public Int64  MemberStoreNo   { get; set; }
        public Int64  UserNo          { get; set; }
        public string UserName        { get; set; }
        public Int64  ApplyNo         { get; set; }  //승인번호

        public string Description     { get; set; }  //사용처 설명
        public int    UseAmt          { get; set; }  //사용금액
        public int    ApplyAmt        { get; set; }  //요청금액
        public string ImageDir        { get; set; }
        public Int16  StateCode       { get; set; }  //상태(1:요청, 2:승인, 3:거절, 4:완료)

        public string DepositYMD      { get; set; }  //입금일(예정일)
        public string ApplyAdminID    { get; set; }
        public string ApplyDate       { get; set; }  //승인일시
        public string DeclineAdminID  { get; set; }
        public string DeclineDate     { get; set; }  //거절일시

        public string CompleteAdminID { get; set; }
        public string CompleteDate    { get; set; }  //완료일시
        public string AdminID         { get; set; }
        public string RegDate         { get; set; }
        public string UpdDate         { get; set; }
        
        public string    Memo         { get; set; }  //상태 변경 시 사유
        public string    ApplyNos     { get; set; }  //대량 상태 변경 용 string ApplyNos
        public UserModel objUserModel { get; set; }
    }
}
