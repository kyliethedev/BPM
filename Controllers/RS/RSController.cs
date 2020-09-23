using System;
using System.Collections.Generic;
using System.Data;
using Admin.App_Code;
using Admin.Models.Common;
using Admin.Models.RS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Admin.Controllers.RS
{
    ///=======================================
    /// <summary>
    /// 정산 Controller
    /// </summary>
    ///=======================================
    public class RSController : Controller
    {
        DBInitConn dbConn = new DBInitConn();

        private readonly ILogger<RSController> _logger;

        private string strLoginID      = string.Empty;
        private string strLoginType    = string.Empty;
        private string strLoginStoreNo = string.Empty;

        public RSController(ILogger<RSController> logger)
        {
            _logger = logger;
        }

        ///---------------------------------------
        /// <summary>
        /// 직접결제정산 페이지 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult DirectSettle()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage        = "직접결제정산";
            ViewBag.LoginType       = strLoginType;
            ViewBag.LoginStoreNo    = strLoginStoreNo;

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 직접결제정산 리스트 조회
        /// </summary>
        /// <param name="settleReq"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetDirectSettleList(SettleReqModel settleReq)
        {
            List<DirectSettleResModel> data = new List<DirectSettleResModel>();

            try
            {
                string   spName      = "UP_BP_SETTLE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intStoreNo", "@pi_intSettleYM", "@pi_intSettleType", "@pi_intLoginType"
                };
                object[] spParamValue = new object[] {
                    settleReq.StoreNo, settleReq.SettleYM, settleReq.SettleType, settleReq.LoginType
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        if(settleReq.LoginType.Equals(1)) //슈퍼유저
                        {
                            DirectSettleResModel obj = new DirectSettleResModel
                            {
                                MemberStoreNo   = Convert.ToInt64(row["MEMBERSTORENO"]),
                                MemberStoreName = row["MEMBERSTORENAME"].ToString(),
                                ApplyCnt        = Convert.ToInt32(row["APPLYCNT"]),
                                ApplyAmt        = Convert.ToInt32(row["APPLYAMT"])
                            };

                            data.Add(obj);
                        }
                        else if(settleReq.LoginType.Equals(2)) //가맹점
                        {
                            DirectSettleResModel obj = new DirectSettleResModel
                            {
                                MemberStoreNo   = Convert.ToInt64(row["MEMBERSTORENO"]),
                                MemberStoreName = row["MEMBERSTORENAME"].ToString(),
                                UserNo          = Convert.ToInt64(row["USERNO"]),
                                UserName        = row["USERNAME"].ToString(),
                                ApplyAmt        = Convert.ToInt32(row["APPLYAMT"]),

                                StateCode       = Convert.ToInt16(row["STATECODE"]),
                                ApplyDate       = row["APPLYDATE"].ToString() ?? ""
                            };

                            data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetDirectSettleList : " + ex.Message);
                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처정산 페이지 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult BPApplySettle()
        {
            strLoginType           = HttpContext.Session.GetString("logintype");
            strLoginStoreNo        = HttpContext.Session.GetString("loginstoreno");
            ViewBag.ThisPage       = "판매처정산";
            ViewBag.LoginType      = strLoginType;
            ViewBag.LoginStoreNo   = strLoginStoreNo;

            ViewBag.SalesStoreDDLB = CommonDDLB.SalesStoreNameALLDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처정산 리스트 조회
        /// </summary>
        /// <param name="settleReq"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetBPApplySettleList(SettleReqModel settleReq)
        {
            List<BPApplySettleResModel> data = new List<BPApplySettleResModel>();

            try
            {
                string spName        = "UP_BP_SETTLE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intStoreNo", "@pi_intSettleYM", "@pi_intSettleType", "@pi_intLoginType"
                };
                object[] spParamValue = new object[] {
                    settleReq.StoreNo, settleReq.SettleYM, settleReq.SettleType, settleReq.LoginType
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        if (settleReq.LoginType.Equals(1)) //슈퍼유저
                        {
                            BPApplySettleResModel obj = new BPApplySettleResModel
                            {
                                SalesStoreNo   = Convert.ToInt64(row["SALESSTORENO"]),
                                SalesStoreName = row["SALESSTORENAME"].ToString(),
                                TotalCnt       = Convert.ToInt32(row["TOTALCNT"]),
                                TotalAmt       = Convert.ToInt32(row["TOTALAMT"])
                            };

                            data.Add(obj);
                        }
                        else if (settleReq.LoginType.Equals(2)) //가맹점
                        {
                            BPApplySettleResModel obj = new BPApplySettleResModel
                            {
                                MemberStoreNo  = Convert.ToInt64(row["MEMBERSTORENO"]),
                                UserNo         = Convert.ToInt64(row["USERNO"]),
                                UserName       = row["USERNAME"].ToString(),
                                SalesStoreNo   = Convert.ToInt64(row["SALESSTORENO"]),
                                SalesStoreName = row["SALESSTORENAME"].ToString(),

                                ApplyAmt       = Convert.ToInt32(row["APPLYAMT"]),
                                ApplyDate      = row["APPLYDATE"].ToString()
                            };

                            data.Add(obj);
                        }
                        else if (settleReq.LoginType.Equals(3)) //판매처
                        {
                            BPApplySettleResModel obj = new BPApplySettleResModel
                            {
                                SalesStoreNo    = Convert.ToInt64(row["SALESSTORENO"]),
                                SalesStoreName  = row["SALESSTORENAME"].ToString(),
                                MemberStoreNo   = Convert.ToInt64(row["MEMBERSTORENO"]),
                                MemberStoreName = row["MEMBERSTORENAME"].ToString(),
                                ItemNo          = Convert.ToInt64(row["ITEMNO"]),

                                ItemGroupNo     = Convert.ToInt64(row["ITEMGROUPNO"]),
                                ItemGroupName   = row["ITEMGROUPNAME"].ToString(),
                                ItemName        = row["ITEMNAME"].ToString(),
                                ItemPrice       = Convert.ToInt32(row["ITEMPRICE"]),
                                PurchaseCnt     = Convert.ToInt32(row["PURCHASECNT"]),

                                TotalAmt        = Convert.ToInt32(row["TOTALAMT"]),
                                RegDate         = row["REGDATE"].ToString()
                            };

                            data.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetBPApplySettleList : " + ex.Message);
                return Json(data);
            }

            return Json(new { data });
        }
    }
}
