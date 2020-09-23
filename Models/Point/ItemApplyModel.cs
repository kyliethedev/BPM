using System;

namespace Admin.Models.Point
{
    ///=======================================
    /// <summary>
    /// 중고 아이템 모델
    /// </summary>
    ///=======================================
    public class ItemApplyModel
    {
        public Int64  ApplyNo         { get; set; }  //승인번호
        public Int64  UserNo          { get; set; }
        public string UserName        { get; set; }
        public string ItemName        { get; set; }
        public string Description     { get; set; }

        public int    Price           { get; set; }
        public int    TotalINCnt      { get; set; }
        public string ImageDir        { get; set; }
        public Int64  StateCode       { get; set; }  //상태(1:요청, 2:승인, 3:거절, 4:완료)
        public string ApplyAdminID    { get; set; }

        public string ApplyDate       { get; set; }
        public string DeclineAdminID  { get; set; }
        public string DeclineDate     { get; set; }
        public string CompleteAdminID { get; set; }
        public string CompleteDate    { get; set; }

        public string RegDate         { get; set; }
        public string UpdDate         { get; set; }
        public UserModel objUserModel { get; set; }
        public Int64  MemberStoreNo   { get; set; }
        public int    TotalOUTCnt     { get; set; }

        public int    RemainCnt       { get; set; }
    }
}
