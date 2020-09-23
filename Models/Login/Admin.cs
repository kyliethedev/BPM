using System;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models.Login
{
    ///=======================================
    /// <summary>
    /// Request 로그인 모델
    /// </summary>
    ///=======================================
    public class ReqAdminModel
    {
        [Required]
        public string ID       { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
