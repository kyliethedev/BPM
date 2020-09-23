using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Models.Common
{
    ///=======================================
    /// <summary>
    /// DB 조회 결과 모델
    /// </summary>
    ///=======================================
    public class Result
    {
        public int    RetVal { get; set; } = 0;
        public string ErrMsg { get; set; } = string.Empty;
    }

    ///=======================================
    /// <summary>
    /// DB 조회 데이터 결과 모델
    /// </summary>
    ///=======================================
    public class Result<T>
    {
        public int    RetVal { get; set; } = 0;
        public string ErrMsg { get; set; } = string.Empty;
        public T      data   { get; set; } = default;
    }
}
