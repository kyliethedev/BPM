using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models.Common
{
    ///=======================================
    /// <summary>
    /// 어드민 로그인 권한모델
    /// </summary>
    ///=======================================
    public class Enums
    {
        public enum LoginTypeCode : short
        {
            Super       = 0,
            MemberStore = 1, 
            SalesStore  = 2
        }
    }
}
