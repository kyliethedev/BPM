using System;

namespace Admin.Models.MemberStoreModel
{
    ///=======================================
    /// <summary>
    /// 가맹점 모델
    /// </summary>
    ///=======================================
    public class MemberStoreModel
    {
        public Int64  MemberStoreNo   { get; set; }
        public string MemberStoreName { get; set; }
        public Int16  UseType         { get; set; } //사용방식(1:직접, 2:간접, 3:복합)
        public Int16  ProvideType     { get; set; } //지급방식(1:일괄, 2:분할)
        public Int16  ProvideCycle    { get; set; } //지급주기(0:일괄, 1:년, 2:월, 3:일)

        public string LastProvideYMD  { get; set; } //최종 지급일
        public string ExpireFlag      { get; set; } //만료 여부(Y:만료O, N:만료X)
        public Int16  ExpireCycle     { get; set; } //만료 주기(0:만료X, 1:년, 2:월, 3:일)
        public string LastExpireYMD   { get; set; } //최종 만료일
        public Int16  UseState        { get; set; } //상태(1:정상, 2:해지)

        public string AdminID         { get; set; }
        public string RegDate         { get; set; }
        public string UpdDate         { get; set; }
        public string CEOName         { get; set; }
        public string LicenseNo       { get; set; } //사업자 등록번호
    }
}
