using System;
using Admin.Models.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.App_Code;
using System.Collections.Generic;
using System.Data;
using Admin.Models.Common;

namespace Admin.Controllers
{
    ///=======================================
    /// <summary>
    /// 로그인 Controller
    /// </summary>
    ///=======================================
    public class LoginController : Controller
    {
        DBInitConn dbConn = new DBInitConn();

        ///---------------------------------------
        /// <summary>
        /// 로그인 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 로그인
        /// </summary>
        /// <param name="reqAdmin"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public ActionResult Login(ReqAdminModel reqAdmin)
        {
            try
            {
                string   spName      = "UP_ADMIN_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intAdminNo", "@pi_strAdminID", "@pi_strPassword", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    DBNull.Value, reqAdmin.ID, reqAdmin.Password, DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    if (result.data.Rows.Count > 0)
                    {
                        foreach (DataRow row in result.data.Rows)
                        {
                            HttpContext.Session.SetString("loginid",      row["ADMINID"].ToString());
                            HttpContext.Session.SetString("logintype",    row["ADMINTYPE"].ToString());
                            HttpContext.Session.SetString("loginstoreno", row["STORENO"].ToString());
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "아이디 또는 비밀번호가 일치하지 않습니다.";

                        return View("Login");
                    }
                }

                return View("Dashboard");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Occured Exception Error");
                Console.WriteLine("Exception in Login : " + ex.Message);

                return View("Login");
            }
        }

        ///---------------------------------------
        /// <summary>
        /// 대시보드
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 로그아웃
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();

            return View("Login");
        }
    }
}

