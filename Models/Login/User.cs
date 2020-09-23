using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models.Login
{
    public class User
    {
        public int    UserNo          { get; set; }
        public string UserID          { get; set; }
        public string UserName        { get; set; }
        [DataType(DataType.Password)]
        public string PassWord        { get; set; }
        public int    MemberStoreNo   { get; set; } //가맹점 번호

        public string MemberStoreName { get; set; } //가맹점명
        public string TeamName        { get; set; } //소속팀명
        public int    BPLevelGroupNo  { get; set; } //BP등급 그룹번호
        public int    BPLevelNo       { get; set; } //등급 번호
        public string BPLevelName     { get; set; } //등급명

        public int    ProvideBPAmt    { get; set; } //지급금액
        public Int16  UseState        { get; set; } //상태(1:정상, 2:정지)
        public string AdminID         { get; set; }
        public string RegDate         { get; set; }
        public string UpdDate         { get; set; }
    }
}
