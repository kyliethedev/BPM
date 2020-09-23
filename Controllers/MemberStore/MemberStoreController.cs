using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Admin.Models.MemberStoreModel;
using Admin.App_Code;
using System.Data;
using Admin.Models.Common;
using Admin.Models.MemberStore;
using Microsoft.AspNetCore.Http;

namespace Admin.Controllers.MemberStore
{
    ///=======================================
    /// <summary>
    /// 가맹점 Controller
    /// </summary>
    ///=======================================
    public class MemberStoreController : Controller
    {
        DBInitConn dbConn = new DBInitConn();

        private readonly ILogger<MemberStoreController> _logger;

        private string strLoginID      = string.Empty;
        private string strLoginType    = string.Empty;
        private string strLoginStoreNo = string.Empty;

        public MemberStoreController(ILogger<MemberStoreController> logger)
        {
            _logger = logger;
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult MemberStore()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage        = "가맹점관리";
            ViewBag.LoginType       = strLoginType;
            ViewBag.LoginStoreNo    = strLoginStoreNo;
            ViewBag.UseStateDDLB    = CommonDDLB.UseStateDDLB("상태선택");
            ViewBag.UseTypeDDLB     = CommonDDLB.UseTypeDDLB();

            ViewBag.ProvideTypeDDLB = CommonDDLB.ProvideTypeDDLB();
            ViewBag.MemberStoreDDLB = CommonDDLB.MemberStoreNameAllDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점 리스트 조회
        /// </summary>
        /// <param name="memberStore"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetMemberStoreList(MemberStoreModel memberStore)
        {
            List<MemberStoreModel> data = new List<MemberStoreModel>();

            try
            {
                string   spName       = "UP_MEMBER_STORE_AR_LST";

                string[] spParamName  = new string[] {
                    "@pi_intMemberStoreNo", "@pi_strMemberStoreName", "@pi_strCEOName",        "@pi_strLicenseNo",  "@pi_intUseType",
                    "@pi_intProvideType",   "@pi_intProvideCycle",    "@pi_intLastProvideYMD", "@pi_intExpireFlag", "@pi_intExpireCycle",
                    "@pi_intLastExpireYMD", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    memberStore.MemberStoreNo, DBNull.Value,         DBNull.Value, DBNull.Value, memberStore.UseType,
                    memberStore.ProvideType,   DBNull.Value,         DBNull.Value, DBNull.Value, DBNull.Value,
                    DBNull.Value,              memberStore.UseState
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if(result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        MemberStoreModel obj = new MemberStoreModel
                        {
                            MemberStoreNo   = Convert.ToInt64(row["MEMBERSTORENO"]),
                            MemberStoreName = row["MEMBERSTORENAME"].ToString() ?? "",
                            UseType         = Convert.ToInt16(row["USETYPE"]),
                            ProvideType     = Convert.ToInt16(row["PROVIDETYPE"]),
                            ProvideCycle    = Convert.ToInt16(row["PROVIDECYCLE"]),

                            LastProvideYMD  = row["LASTPROVIDEYMD"].ToString() ?? "",
                            ExpireFlag      = row["EXPIREFLAG"].ToString(),
                            ExpireCycle     = Convert.ToInt16(row["EXPIRECYCLE"]),
                            LastExpireYMD   = row["LASTEXPIREYMD"].ToString() ?? "",
                            UseState        = Convert.ToInt16(row["USESTATE"]),

                            AdminID         = row["ADMINID"].ToString(),
                            RegDate         = row["REGDATE"].ToString(),
                            UpdDate         = row["UPDDATE"].ToString() ?? "",
                            CEOName         = row["CEONAME"].ToString(),
                            LicenseNo       = row["LICENSENO"].ToString()
                        };
                        
                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetMemberStoreList : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점 상세조회
        /// </summary>
        /// <param name="memberStore"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailMemberStore(MemberStoreModel memberStore)
        {
            Result<MemberStoreModel> data = null;

            try
            {
                data = new Result<MemberStoreModel>();

                string   spName      = "UP_MEMBER_STORE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_strMemberStoreName", "@pi_strCEOName",        "@pi_strLicenseNo",  "@pi_intUseType",
                    "@pi_intProvideType",   "@pi_intProvideCycle",    "@pi_intLastProvideYMD", "@pi_intExpireFlag", "@pi_intExpireCycle",
                    "@pi_intLastExpireYMD", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    memberStore.MemberStoreNo, DBNull.Value, DBNull.Value, DBNull.Value, memberStore.UseState,
                    DBNull.Value,              DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                    DBNull.Value,              DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new MemberStoreModel
                        {
                            MemberStoreNo   = Convert.ToInt64(row["MEMBERSTORENO"]),
                            MemberStoreName = row["MEMBERSTORENAME"].ToString() ?? "",
                            UseType         = Convert.ToInt16(row["USETYPE"]),
                            ProvideType     = Convert.ToInt16(row["PROVIDETYPE"]),
                            ProvideCycle    = Convert.ToInt16(row["PROVIDECYCLE"]),

                            LastProvideYMD  = row["LASTPROVIDEYMD"].ToString() ?? "",
                            ExpireFlag      = row["EXPIREFLAG"].ToString(),
                            ExpireCycle     = Convert.ToInt16(row["EXPIRECYCLE"]),
                            LastExpireYMD   = row["LASTEXPIREYMD"].ToString() ?? "",
                            UseState        = Convert.ToInt16(row["USESTATE"]),

                            AdminID         = row["ADMINID"].ToString(),
                            RegDate         = row["REGDATE"].ToString(),
                            UpdDate         = row["UPDDATE"].ToString() ?? "",
                            CEOName         = row["CEONAME"].ToString(),
                            LicenseNo       = row["LICENSENO"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 999;
                data.ErrMsg = "Exception in DetailMemberStore : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점 등록
        /// </summary>
        /// <param name="memberStore"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult InsMemberStore(MemberStoreModel memberStore)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_MEMBER_STORE_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_strMemberStoreName", "@pi_strCEOName",       "@pi_strLicenseNo",  "@pi_intUseType",    "@pi_intProvideType", 
                    "@pi_intProvideCycle",    "@pi_intLastProvideYMD","@pi_strExpireFlag", "@pi_intExpireCycle","@pi_intLastExpireYMD",
                    "@pi_strAdminID",         "@po_intRetVal",        "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    memberStore.MemberStoreName, memberStore.CEOName,  memberStore.LicenseNo, memberStore.UseType,     memberStore.ProvideType,
                    memberStore.ProvideCycle,    DBNull.Value,         DBNull.Value,          memberStore.ExpireCycle, DBNull.Value,
                    strLoginID,                  DBNull.Value,         DBNull.Value,
                };

               data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 990;
                data.ErrMsg = "Exception in InsMemberStore : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점 수정
        /// </summary>
        /// <param name="memberStore"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdMemberStore(MemberStoreModel memberStore)
        {
            Result data = null;

            try
            {
                data = new Result();

                string   spName      = "UP_MEMBER_STORE_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_strMemberStoreName", "@pi_strCEOName",        "@pi_strLicenseNo",  "@pi_intUseType",
                    "@pi_intProvideType",   "@pi_intProvideCycle",    "@pi_strLastProvideYMD", "@pi_strExpireFlag", "@pi_intExpireCycle",
                    "@pi_strLastExpireYMD", "@pi_intUseState",        "@po_intRetVal",         "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    memberStore.MemberStoreNo, memberStore.MemberStoreName, memberStore.CEOName, memberStore.LicenseNo, memberStore.UseType,
                    memberStore.ProvideType,   memberStore.ProvideCycle,    DBNull.Value,        DBNull.Value,          memberStore.ExpireCycle,
                    DBNull.Value,              memberStore.UseState,        DBNull.Value,        DBNull.Value,
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 980;
                data.ErrMsg = "Exception in UpdMemberStore : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP등급관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult BPLevel()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage        = "BP등급관리";
            ViewBag.LoginType       = strLoginType;
            ViewBag.LoginStoreNo    = strLoginStoreNo;
            ViewBag.MemberStoreDDLB = CommonDDLB.MemberStoreNameDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// BP등급그룹 관리 리스트 조회
        /// </summary>
        /// <param name="bpLevel"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetBPLevelGroupList(BPLevelModel bpLevel)
        {
            List<BPLevelModel> data = new List<BPLevelModel>();

            try
            {
                string   spName      = "UP_MEMBERSTORE_BPLEVELGROUP_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo"
                };
                object[] spParamValue = new object[] {
                    bpLevel.MemberStoreNo
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        BPLevelModel obj = new BPLevelModel
                        {
                            BPLevelGroupNo = Convert.ToInt64(row["BPLEVELGROUPNO"]),
                            MemberStoreNo  = Convert.ToInt64(row["MEMBERSTORENO"]),
                            BPLevelFlag    = row["BPLEVELFLAG"].ToString(),
                            UseState       = Convert.ToInt16(row["USESTATE"]),
                            RegDate        = row["REGDATE"].ToString(),

                            UpdDate        = row["UPDDATE"].ToString() ?? ""
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetBPLevelGroupList : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP등급그룹 등록
        /// </summary>
        /// <param name="bpLevel"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult InsBPLevelGroup(BPLevelModel bpLevel)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_BP_LEVEL_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_strBPLevelFlag", "@pi_strAdminID", "@po_intRetVal", "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    bpLevel.MemberStoreNo, bpLevel.BPLevelFlag, strLoginID, DBNull.Value, DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 970;
                data.ErrMsg = "Exception in InsBPLevelGroup : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP등급그룹 수정
        /// </summary>
        /// <param name="bpLevel"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdBPLevelGroup(BPLevelModel bpLevel)
        {
            Result data = null;

            try
            {
                data = new Result();

                string   spName      = "UP_BP_LEVEL_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intBPLevelGroupNo", "@pi_intUseState", "@po_intRetVal", "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    bpLevel.MemberStoreNo, bpLevel.BPLevelGroupNo, bpLevel.UseState, DBNull.Value, DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 960;
                data.ErrMsg = "Exception in UpdBPLevelGroup : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP등급 세부사항 조회
        /// </summary>
        /// <param name="bpLevelDtl"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailBPLevelDtl(BPLevelDtlModel bpLevelDtl)
        {
            List<BPLevelDtlModel> data = new List<BPLevelDtlModel>();

            try
            {
                string   spName      = "UP_MEMBERSTORE_BPLEVEL_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intBPLevelGroupNo", "@pi_intBPLevelNo"
                };
                object[] spParamValue = new object[] {
                    bpLevelDtl.MemberStoreNo, bpLevelDtl.BPLevelGroupNo, DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        BPLevelDtlModel obj = new BPLevelDtlModel
                        {
                            BPLevelName = row["BPLEVELNAME"].ToString(),
                            BPAmt       = Convert.ToInt32(row["BPAMT"])
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DetailBPLevelDtl : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// BP등급 세부사항 수정
        /// </summary>
        /// <param name="bpLevelDtl"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdBPLevelDtl(BPLevelDtlModel bpLevelDtl)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_BP_LEVEL_DTL_TX_INS_BULK";

                string[] spParamName = new string[] {
                    "@pi_intBPLevelGroupNo", "@pi_strBPLevelNameList", "@pi_strBPAmtList", "@pi_strAdminID", "@po_intRetVal",
                    "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    bpLevelDtl.BPLevelGroupNo, bpLevelDtl.BPLevelName, bpLevelDtl.BPAmts, strLoginID, DBNull.Value,
                    DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 940;
                data.ErrMsg = "Exception in UpdBPLevelDtl : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처계약관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public ActionResult SalesContract()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage        = "판매처계약관리";
            ViewBag.LoginType       = strLoginType;
            ViewBag.LoginStoreNo    = strLoginStoreNo;
            ViewBag.UseStateDDLB    = CommonDDLB.UseStateDDLB("가맹점상태선택");
            ViewBag.MemberStoreDDLB = CommonDDLB.MemberStoreNameDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처계약 리스트 조회 
        /// </summary>
        /// <param name="memberStore"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetMemberSalesContractList(MemberStoreModel memberStore)
        {
            List<MemberStoreModel> data = new List<MemberStoreModel>();

            try
            {
                string   spName      = "UP_MEMBER_STORE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_strMemberStoreName", "@pi_strCEOName",        "@pi_strLicenseNo",  "@pi_intUseType",
                    "@pi_intProvideType",   "@pi_intProvideCycle",    "@pi_intLastProvideYMD", "@pi_intExpireFlag", "@pi_intExpireCycle",
                    "@pi_intLastExpireYMD", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    memberStore.MemberStoreNo, DBNull.Value,         DBNull.Value, DBNull.Value,  DBNull.Value,
                    DBNull.Value,              DBNull.Value,         DBNull.Value, DBNull.Value,  DBNull.Value,
                    DBNull.Value,              memberStore.UseState
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        MemberStoreModel obj = new MemberStoreModel
                        {
                            MemberStoreNo   = Convert.ToInt64(row["MEMBERSTORENO"]),
                            MemberStoreName = row["MEMBERSTORENAME"].ToString(),
                            UseState        = Convert.ToInt16(row["USESTATE"]),
                            RegDate         = row["REGDATE"].ToString(),
                            UpdDate         = row["UPDDATE"].ToString()
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetMemberSalesContractList : " + ex.Message);

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점별 계약된 판매처 DDLB
        /// </summary>
        /// <param name="salesContract"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult ServicedSalesStoreDDLB(SalesContractModel salesContract)
        {
            string ServicedSalesStoreDDLB = CommonDDLB.ServicedSalesStoreDDLB(salesContract);
            return Json(ServicedSalesStoreDDLB);
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처계약 수정작업
        /// </summary>
        /// <param name="salesContract"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdMemberSalesContract(SalesContractModel salesContract)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_SALES_CONTRACT_TX_INS_BULK";

                string[] spParamName = new string[] {
                    "@pi_intMemberStoreNo", "@pi_intSalesStoreNoList", "@pi_strAdminID", "@po_intRetVal", "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    salesContract.MemberStoreNo, salesContract.SalesStoreNo, strLoginID, DBNull.Value, DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 930;
                data.ErrMsg = "Exception in UpdMemberSalesContract : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }
    }
}
