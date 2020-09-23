using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Models.Point;
using Microsoft.Extensions.Logging;
using Admin.Models.Common;
using Admin.App_Code;
using System.Data;

namespace Admin.Controllers.Point
{
    ///=======================================
    /// <summary>
    /// BP Controller
    /// </summary>
    ///=======================================
    public class PointController : Controller
    {
        DBInitConn dbConn = new DBInitConn();

        private readonly ILogger<PointController> _logger;

        private string strLoginID      = string.Empty;
        private string strLoginType    = string.Empty;
        private string strLoginStoreNo = string.Empty;

        public PointController(ILogger<PointController> logger)
        {
            _logger = logger;
        }

        ///---------------------------------------
        /// <summary>
        /// 직원관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult User()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage        = "직원관리";
            ViewBag.LoginType       = strLoginType;
            ViewBag.LoginStoreNo    = strLoginStoreNo;
            ViewBag.TeamNameDDLB    = CommonDDLB.TeamNameDDLB(Convert.ToInt64(strLoginStoreNo));
            ViewBag.BPLevelDDLB     = CommonDDLB.BPLevelDDLB(Convert.ToInt64(strLoginStoreNo));

            ViewBag.UseStateDDLB    = CommonDDLB.UseStateDDLB("상태선택");
            ViewBag.MemberStoreDDLB = CommonDDLB.MemberStoreNameDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 직원 리스트 조회
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetUserList(UserModel user)
        {
            List<UserModel> data = new List<UserModel>();

            try
            {
                string   spName      = "UP_USER_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intUserNo",        "@pi_strUserID",   "@pi_strUserName", "@pi_strMemberStoreName",
                    "@pi_strPhoneNo",       "@pi_intBPLevelGroupNo","@pi_intBPLevelNo","@pi_strTeamName", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    user.MemberStoreNo, user.UserNo,         user.UserID ?? "",    user.UserName ?? "", DBNull.Value,
                    user.PhoneNo ?? "", user.BPLevelGroupNo, user.BPLevelNo,       user.TeamName ?? "", user.UseState
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        UserModel obj = new UserModel
                        {
                            MemberStoreNo  = Convert.ToInt32(row["MEMBERSTORENO"]),
                            UserNo         = Convert.ToInt64(row["USERNO"]),
                            UserName       = row["USERNAME"].ToString(),
                            TeamName       = row["TEAMNAME"].ToString(),
                            BPLevelName    = row["BPLEVELNAME"].ToString(),

                            EmpYMD         = row["EMPYMD"].ToString(),
                            UseState       = Convert.ToInt16(row["USESTATE"]),
                            BPLevelGroupNo = Convert.ToInt32(row["BPLEVELGROUPNO"]),
                            objUserBPModel = new UserBPModel
                            {
                                TotalINAmt  = Convert.ToInt32(row["TOTALINAMT"]),
                                TotalOUTAmt = Convert.ToInt32(row["TOTALOUTAMT"]),
                                RemainAmt   = Convert.ToInt32(row["REMAINAMT"])
                            }
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetUserList : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 직원 상세조회
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailUser(UserModel user)
        {
            Result<UserModel> data = null;

            try
            {
                data = new Result<UserModel>();

                string   spName      = "UP_USER_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intUserNo",        "@pi_strUserID",   "@pi_strUserName", "@pi_strMemberStoreName",
                    "@pi_strPhoneNo",       "@pi_intBPLevelGroupNo","@pi_intBPLevelNo","@pi_strTeamName", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    user.MemberStoreNo, user.UserNo,  DBNull.Value, DBNull.Value, DBNull.Value,
                    DBNull.Value,       DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new UserModel
                        {
                            UserNo        = Convert.ToInt64(row["USERNO"]),
                            MemberStoreNo = Convert.ToInt32(row["MEMBERSTORENO"]),
                            TeamName      = row["TEAMNAME"].ToString(),
                            BPLevelName   = row["BPLEVELNAME"].ToString(),
                            UserName      = row["USERNAME"].ToString(),

                            EmpYMD        = row["EMPYMD"].ToString(),
                            UserID        = row["USERID"].ToString(),
                            UseState      = Convert.ToInt16(row["USESTATE"]),
                            BPLevelNo     = Convert.ToInt32(row["BPLevelNo"]),
                            objUserAccountModel = new UserAcctModel
                            {
                                BankCode      = row["BANKCODE"].ToString(),
                                AccountNo     = row["ACCOUNTNO"].ToString(),
                                AccountHolder = row["ACCOUNTHOLDER"].ToString(),
                            },

                            objUserBPModel = new UserBPModel
                            {
                                TotalINAmt  = Convert.ToInt32(row["TOTALINAMT"]),
                                TotalOUTAmt = Convert.ToInt32(row["TOTALOUTAMT"]),
                                RemainAmt   = Convert.ToInt32(row["REMAINAMT"]),
                                BPAmt       = Convert.ToInt32(row["BPAMT"])
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 799;
                data.ErrMsg = "Exception in DetailMemberStore : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 직원 등록 시 직급 별 지급포인트 세팅
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetBPAmtByBPLevelNo(UserModel user)
        {
            Result<UserModel> data = null;

            try
            {
                data = new Result<UserModel>();

                string   spName      = "UP_MEMBERSTORE_BPLEVEL_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intBPLevelGroupNo", "@pi_intBPLevelNo"
                };
                object[] spParamValue = new object[] {
                    user.MemberStoreNo, DBNull.Value, user.BPLevelNo
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new UserModel
                        {
                            objUserBPModel = new UserBPModel
                            {
                                BPAmt = Convert.ToInt32(row["BPAMT"]),
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 798;
                data.ErrMsg = "Exception in GetBPAmtByBPLevelNo : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 슈퍼유저인 경우 가맹점 변경시 팀 명 DDLB 변경
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetChangedTeamNameDDLB(UserModel user)
        {
            string TeamNameDDLB = CommonDDLB.TeamNameDDLB(user.MemberStoreNo);

            return Json(TeamNameDDLB);
        }

        ///---------------------------------------
        /// <summary>
        /// 슈퍼유저인 경우 가맹점 변경시 BP등급 DDLB 변경
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetChangedBPLevelDDLB(UserModel user)
        {
            string BPLevelDDLB = CommonDDLB.BPLevelDDLB(user.MemberStoreNo);

            return Json(BPLevelDDLB);
        }

        ///---------------------------------------
        /// <summary>
        /// 직원 개별 등록
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userAcct"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult InsUser(UserModel user, UserAcctModel userAcct)
        {
            Result data = null;

            try
            {
                strLoginID      = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_USER_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_strUserID",         "@pi_strUserName",      "@pi_intMemberStoreNo", "@pi_strTeamName", "@pi_strPhoneNo",
                    "@pi_intBPLevelGroupNo", "@pi_intBPLevelNo",     "@pi_strAdminID",       "@pi_strEmpYMD",   "@pi_strBankCode",
                    "@pi_strAccountNo",      "@pi_strAccountHolder", "@po_intRetVal",        "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    user.UserID,         user.UserName,          user.MemberStoreNo, user.TeamName, DBNull.Value,
                    user.BPLevelGroupNo, user.BPLevelNo,         strLoginID,         user.EmpYMD,   userAcct.BankCode,
                    userAcct.AccountNo,  userAcct.AccountHolder, DBNull.Value,       DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 790;
                data.ErrMsg = "Exception in InsUser : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 직원 수정
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userAcct"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdUser(UserModel user, UserAcctModel userAcct)
        {
            Result data = null;

            try
            {
                data = new Result();

                string spName = "UP_USER_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intUserNo",  "@pi_intMemberStoreNo",  "@pi_strUserID",    "@pi_strUserName", "@pi_strTeamName",                    
                    "@pi_strPhoneNo", "@pi_intBPLevelGroupNo", "@pi_intBPLevelNo", "@pi_intUseState", "@pi_strEmpYMD",
                    "@po_intRetVal",  "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    user.UserNo,  user.MemberStoreNo, user.UserID ?? "", user.UserName, user.TeamName,
                    DBNull.Value, DBNull.Value,       user.BPLevelNo,    user.UseState, user.EmpYMD,
                    DBNull.Value, DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 770;
                data.ErrMsg = "Exception in UpdUser : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 직원 복지포인트 지급
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userBP"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult AddUserBP(UserModel user, UserBPModel userBP)
        {
            Result data = null;

            try
            {
                strLoginID      = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_BP_PROVIDE_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_intUserNo", "@pi_intMemberStoreNo", "@pi_intBPAmt", "@pi_strDescription", "@pi_strAdminID",
                    "@po_intRetVal", "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    userBP.UserNo, user.MemberStoreNo, userBP.WorkBPAmt, userBP.Description, strLoginID, 
                    DBNull.Value,  DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 760;
                data.ErrMsg = "Exception in AddUserBP : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 직원 복지포인트 차감
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userBP"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult MinusUserBP(UserModel user, UserBPModel userBP)
        {
            Result data = null;

            try
            {
                strLoginID      = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_BP_DEDUCTION_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_intUserNo", "@pi_intMemberStoreNo", "@pi_intDeductionAmt", "@pi_strDescription", "@pi_strAdminID", 
                    "@po_intRetVal", "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    userBP.UserNo, user.MemberStoreNo, userBP.WorkBPAmt, userBP.Description, strLoginID,
                    DBNull.Value,  DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 750;
                data.ErrMsg = "Exception in MinusUserBP : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 복지포인트 지급 내역 조회
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailBPProvide(UserModel user)
        {
            List<UserBPProvideDetail> data = new List<UserBPProvideDetail>();

            try
            {
                string   spName      = "UP_BP_PROVIDE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intUserNo", "@pi_intMemberStoreNo"
                };
                object[] spParamValue = new object[] {
                    user.UserNo, user.MemberStoreNo
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        UserBPProvideDetail obj = new UserBPProvideDetail
                        {
                            BPNo        = Convert.ToInt64(row["BPNO"]),
                            BPAmt       = Convert.ToInt32(row["BPAMT"]),
                            RemainAmt   = Convert.ToInt32(row["REMAINAMT"]),
                            Description = row["DESCRIPTION"].ToString(),
                            UseState    = Convert.ToInt16(row["USESTATE"]),

                            AdminID     = row["ADMINID"].ToString(),
                            RegDate     = row["REGDATE"].ToString()
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DetailBPProvide : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 복지포인트 사용,차감 내역 조회
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailBPUse(UserModel user)
        {
            List<UserBPUseDetail> data = new List<UserBPUseDetail>();

            try
            {
                string   spName      = "UP_BP_USE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intUserNo", "@pi_intMemberStoreNo"
                };
                object[] spParamValue = new object[] {
                    user.UserNo, user.MemberStoreNo
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        UserBPUseDetail obj = new UserBPUseDetail
                        {
                            PurchaseNo  = Convert.ToInt64(row["PURCHASENO"]),
                            ItemNo      = Convert.ToInt64(row["ITEMNO"]),
                            ItemPrice   = Convert.ToInt32(row["ITEMPRICE"]),
                            TotalAmt    = Convert.ToInt32(row["TOTALAMT"]),
                            Description = row["DESCRIPTION"].ToString(),

                            Location    = row["LOCATION"].ToString().Equals("1") ? "복지포인트몰" : "관리자",
                            YMD         = row["YMD"].ToString(),
                            CYMD        = row["CYMD"].ToString()
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DetailBPUse : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP사용신청관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult BPApply()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage        = "BP사용신청관리";
            ViewBag.LoginType       = strLoginType;
            ViewBag.LoginStoreNo    = strLoginStoreNo;
            ViewBag.StateCodeDDLB   = CommonDDLB.StateCodeDDLB();
            ViewBag.MemberStoreDDLB = CommonDDLB.MemberStoreNameDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// BP사용신청 리스트 조회
        /// </summary>
        /// <param name="bpApply"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetBPApplyList(BPApplyModel bpApply)
        {
            List<BPApplyModel> data = new List<BPApplyModel>();

            try
            {
                string   spName      = "UP_BP_APPLY_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intApplyNo", "@pi_intStateCode"
                };
                object[] spParamValue = new object[] {
                    bpApply.MemberStoreNo, DBNull.Value, bpApply.StateCode
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        BPApplyModel obj = new BPApplyModel
                        {
                            RowNum       = Convert.ToInt32(row["ROWNUM"]),
                            ApplyNo      = Convert.ToInt64(row["APPLYNO"]),
                            UserNo       = Convert.ToInt64(row["USERNO"]),
                            UserName     = row["USERNAME"].ToString(),
                            Description  = row["DESCRIPTION"].ToString(),

                            UseAmt       = Convert.ToInt32(row["USEAMT"]),
                            ApplyAmt     = Convert.ToInt32(row["APPLYAMT"]),
                            StateCode    = Convert.ToInt16(row["STATECODE"]),
                            RegDate      = row["REGDATE"].ToString(),
                            UpdDate      = row["UPDDATE"].ToString()
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetBPApplyList : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP사용신청 상세내역 조회
        /// </summary>
        /// <param name="bpApply"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailBPApply(BPApplyModel bpApply)
        {
            Result<BPApplyModel> data = null;

            try
            {
                data = new Result<BPApplyModel>();

                string   spName      = "UP_BP_APPLY_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intApplyNo", "@pi_intStateCode"
                };
                object[] spParamValue = new object[] {
                    bpApply.MemberStoreNo, bpApply.ApplyNo, DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new BPApplyModel
                        {
                            UserNo       = Convert.ToInt64(row["USERNO"]),
                            UserName     = row["USERNAME"].ToString(),
                            Description  = row["DESCRIPTION"].ToString(),
                            UseAmt       = Convert.ToInt32(row["USEAMT"]),
                            ApplyAmt     = Convert.ToInt32(row["APPLYAMT"]),

                            ImageDir     = row["IMAGEDIR"].ToString(),
                            StateCode    = Convert.ToInt16(row["STATECODE"]),
                            Memo         = row["MEMO"].ToString() ?? "",
                            objUserModel = new UserModel
                            {
                                TeamName    = row["TEAMNAME"].ToString(),
                                BPLevelName = row["BPLEVELNAME"].ToString()
                            }
                            
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 749;
                data.ErrMsg = "Exception in DetailBPApply : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP사용신청 상태 수정
        /// </summary>
        /// <param name="bpApply"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdBPApply(BPApplyModel bpApply)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_BP_APPLY_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intApplyNo", "@pi_intStateCode", "@pi_strDepositYMD", "@pi_strMemo", "@pi_strAdminID", 
                    "@po_intRetVal",  "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    bpApply.ApplyNo, bpApply.StateCode, bpApply.DepositYMD ?? "", bpApply.Memo, strLoginID, 
                    DBNull.Value,    DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 740;
                data.ErrMsg = "Exception in UpdBPApply : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP사용신청 대량 상태 수정
        /// </summary>
        /// <param name="bpApply"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdBPApplyBulk(BPApplyModel bpApply)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_BP_APPLY_TX_UPD_BULK";

                string[] spParamName = new string[] {
                    "@v_intApplyNoList","@pi_intStateCode","@pi_strDepositYMD","@pi_strAdminID","@po_intRetVal",
                    "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    bpApply.ApplyNos, bpApply.StateCode, DBNull.Value, strLoginID, DBNull.Value,
                    DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 720;
                data.ErrMsg = "Exception in UpdBPApplyBulk : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 중고상품관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult ItemApply()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage        = "중고상품관리";
            ViewBag.LoginType       = strLoginType;
            ViewBag.LoginStoreNo    = strLoginStoreNo;
            ViewBag.StateCodeDDLB   = CommonDDLB.StateCodeDDLB();
            ViewBag.MemberStoreDDLB = CommonDDLB.MemberStoreNameDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 중고상품 리스트 조회
        /// </summary>
        /// <param name="itemApply"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetItemApplyList(ItemApplyModel itemApply)
        {
            List<ItemApplyModel> data = new List<ItemApplyModel>();

            try
            {
                string   spName      = "UP_USER_ITEM_APPLY_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intApplyNo", "@pi_intStateCode"
                };
                object[] spParamValue = new object[] {
                    itemApply.MemberStoreNo, DBNull.Value, itemApply.StateCode
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        ItemApplyModel obj = new ItemApplyModel
                        {
                            ApplyNo     = Convert.ToInt64(row["APPLYNO"]),
                            UserNo      = Convert.ToInt64(row["USERNO"]),
                            UserName    = row["USERNAME"].ToString(),
                            Description = row["DESCRIPTION"].ToString(),
                            Price       = Convert.ToInt32(row["PRICE"]),

                            TotalINCnt  = Convert.ToInt32(row["TOTALINCNT"]),
                            StateCode   = Convert.ToInt16(row["STATECODE"]),
                            RegDate     = row["REGDATE"].ToString(),
                            UpdDate     = row["UPDDATE"].ToString(),
                            TotalOUTCnt = Convert.ToInt32(row["TOTALOUTCNT"]),

                            RemainCnt   = Convert.ToInt32(row["REMAINCNT"]),
                            ItemName    = row["ITEMNAME"].ToString()
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetItemApplyList : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 중고상품 상세내역 조회
        /// </summary>
        /// <param name="itemApply"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailItemApply(ItemApplyModel itemApply)
        {
            Result<ItemApplyModel> data = null;

            try
            {
                data = new Result<ItemApplyModel>();

                string   spName      = "UP_USER_ITEM_APPLY_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intApplyNo", "@pi_intStateCode"
                };
                object[] spParamValue = new object[] {
                    itemApply.MemberStoreNo, itemApply.ApplyNo, DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new ItemApplyModel
                        {
                            UserNo       = Convert.ToInt64(row["USERNO"]),
                            UserName     = row["USERNAME"].ToString(),
                            Description  = row["DESCRIPTION"].ToString(),
                            Price        = Convert.ToInt32(row["PRICE"]),
                            TotalINCnt   = Convert.ToInt32(row["TOTALINCNT"]),

                            ImageDir     = row["IMAGEDIR"].ToString(),
                            StateCode    = Convert.ToInt16(row["STATECODE"]),
                            objUserModel = new UserModel
                            {
                                TeamName    = row["TEAMNAME"].ToString(),
                                BPLevelName = row["BPLEVELNAME"].ToString()
                            },
                            TotalOUTCnt  = Convert.ToInt32(row["TOTALOUTCNT"]),
                            RemainCnt    = Convert.ToInt32(row["REMAINCNT"]),
                            ItemName     = row["ITEMNAME"].ToString()

                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 739;
                data.ErrMsg = "Exception in DetailItemApply : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 중고상품 상세내역 수정
        /// </summary>
        /// <param name="bpApply"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdItemApply(BPApplyModel bpApply)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_USER_ITEM_APPLY_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intApplyNo", "@pi_intStateCode", "@pi_strAdminID", "@po_intRetVal", "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    bpApply.ApplyNo, bpApply.StateCode, strLoginID, DBNull.Value, DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 730;
                data.ErrMsg = "Exception in UpdItemApply : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }
    }
}
