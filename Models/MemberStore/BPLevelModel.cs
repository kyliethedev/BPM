using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models.MemberStore
{
    ///=======================================
    /// <summary>
    /// BP등급 모델
    /// </summary>
    ///=======================================
    public class BPLevelModel
    {
        public Int64  BPLevelGroupNo { get; set; }  //등급 그룹번호
        public Int64  MemberStoreNo  { get; set; }
        public string BPLevelFlag    { get; set; }  //레벨 적용 여부(Y: 적용, N: 비적용)
        public Int16  UseState       { get; set; }  //상태(1:사용, 2:미사용)
        public string AdminID        { get; set; }

        public string RegDate        { get; set; }
        public string UpdDate        { get; set; }

        public BPLevelDtlModel objBPLevelDtlModel { get; set; }
    }

    ///=======================================
    /// <summary>
    /// BP등급 상세 모델
    /// </summary>
    ///=======================================
    public class BPLevelDtlModel
    {
        public Int64  BPLevelGroupNo { get; set; }
        public Int64  BPLevelNo      { get; set; }
        public string BPLevelName    { get; set; }
        public int    BPAmt          { get; set; }
        public Int16  UseState       { get; set; }

        public string AdminID        { get; set; }
        public string RegDate        { get; set; }
        public string UpdDate        { get; set; }
        public string BPAmts         { get; set; }  //대량 수정을 위한 string BPAmts
        public Int64  MemberStoreNo  { get; set; }
    }
}
