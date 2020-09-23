using System;
using System.Collections.Generic;
using System.Data;
using Admin.App_Code;
using Admin.Models.Common;
using Admin.Models.SalesStore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Admin.Controllers.SalesStore
{
    ///=======================================
    /// <summary>
    /// 판매처 Controller
    /// </summary>
    ///=======================================
    public class SalesStoreController : Controller
    {
        DBInitConn dbConn = new DBInitConn();

        private readonly ILogger<SalesStoreController> _logger;

        private string strLoginID      = string.Empty;
        private string strLoginType    = string.Empty;
        private string strLoginStoreNo = string.Empty;

        public SalesStoreController(ILogger<SalesStoreController> logger)
        {
            _logger = logger;
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult SalesStore()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("2"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage       = "판매처관리";
            ViewBag.LoginType      = strLoginType;
            ViewBag.LoginStoreNo   = strLoginStoreNo;
            ViewBag.UseStateDDLB   = CommonDDLB.UseStateDDLB("상태선택");
            ViewBag.SalesStoreDDLB = CommonDDLB.SalesStoreNameALLDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처관리 리스트 조회
        /// </summary>
        /// <param name="salesStore"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetSalesStoreList(SalesStoreModel salesStore)
        {
            List<SalesStoreModel> data = new List<SalesStoreModel>();

            try
            {
                string   spName      = "UP_SALES_STORE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intSalesStoreNo", "@pi_strSalesStoreName", "@pi_strCEOName", "@pi_intUseState" 
                };
                object[] spParamValue = new object[] {
                    salesStore.SalesStoreNo, DBNull.Value, salesStore.CEOName ?? "", salesStore.UseState
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        SalesStoreModel obj = new SalesStoreModel
                        {
                            SalesStoreNo   = Convert.ToInt64(row["SALESSTORENO"]),
                            SalesStoreName = row["SALESSTORENAME"].ToString(),
                            CEOName        = row["CEONAME"].ToString(),
                            LicenseNo      = row["LICENSENO"].ToString(),
                            UseState       = Convert.ToInt16(row["USESTATE"]),

                            AdminID        = row["ADMINID"].ToString(),
                            RegDate        = row["REGDATE"].ToString(),
                            UpdDate        = row["UPDDATE"].ToString() ?? ""
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetSalesStoreList : " + ex.Message);
                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처 상세조회
        /// </summary>
        /// <param name="salesStore"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailSalesStore(SalesStoreModel salesStore)
        {
            Result<SalesStoreModel> data = null;

            try
            {
                data = new Result<SalesStoreModel>();

                string   spName      = "UP_SALES_STORE_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intSalesStoreNo", "@pi_strSalesStoreName", "@pi_strCEOName", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    salesStore.SalesStoreNo, DBNull.Value, DBNull.Value, DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new SalesStoreModel
                        {
                            SalesStoreNo   = Convert.ToInt64(row["SALESSTORENO"]),
                            SalesStoreName = row["SALESSTORENAME"].ToString(),
                            CEOName        = row["CEONAME"].ToString(),
                            LicenseNo      = row["LICENSENO"].ToString(),
                            UseState       = Convert.ToInt16(row["USESTATE"]),

                            AdminID        = row["ADMINID"].ToString(),
                            RegDate        = row["REGDATE"].ToString(),
                            UpdDate        = row["UPDDATE"].ToString() ?? "",

                            objSalesStoreAcctModel = new SalesStoreAcctModel
                            {
                                BankCode      = row["BANKCODE"].ToString(),
                                AccountNo     = row["ACCOUNTNO"].ToString(),
                                AccountHolder = row["ACCOUNTHOLDER"].ToString()
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 899;
                data.ErrMsg = "Exception in DetailSalesStore : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처 등록
        /// </summary>
        /// <param name="salesStore"></param>
        /// <param name="salesStoreAcct"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult InsSalesStore(SalesStoreModel salesStore, SalesStoreAcctModel salesStoreAcct)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_SALES_STORE_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_strSalesStoreName", "@pi_strCEOName", "@pi_strLicenseNo", "@pi_strBankCode", "@pi_strAccountNo",
                    "@pi_strAccountHolder",  "@pi_strAdminID", "@po_intRetVal",    "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    salesStore.SalesStoreName,    salesStore.CEOName, salesStore.LicenseNo, salesStoreAcct.BankCode, salesStoreAcct.AccountNo,
                    salesStoreAcct.AccountHolder, strLoginID,         DBNull.Value,         DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 890;
                data.ErrMsg = "Exception in InsSalesStore : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처 수정
        /// </summary>
        /// <param name="salesStore"></param>
        /// <param name="salesStoreAcct"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdSalesStore(SalesStoreModel salesStore, SalesStoreAcctModel salesStoreAcct)
        {
            Result data = null;

            try
            {
                data = new Result();

                string   spName      = "UP_SALES_STORE_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intSalesStoreNo", "@pi_strSalesStoreName", "@pi_strCEOName",       "@pi_strLicenseNo", "@pi_intUseState",
                    "@pi_strBankCode",     "@pi_strAccountNo",      "@pi_strAccountHolder", "@po_intRetVal",    "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    salesStore.SalesStoreNo, salesStore.SalesStoreName, salesStore.CEOName,           salesStore.LicenseNo, salesStore.UseState,
                    salesStoreAcct.BankCode, salesStoreAcct.AccountNo,  salesStoreAcct.AccountHolder, DBNull.Value,         DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 880;
                data.ErrMsg = "Exception in UpdSalesStore : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult Item()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            strLoginStoreNo = HttpContext.Session.GetString("loginstoreno");
            if (strLoginType.Equals("2"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage           = "상품관리";
            ViewBag.LoginType          = strLoginType;
            ViewBag.LoginStoreNo       = strLoginStoreNo;
            ViewBag.UseStateDDLB       = CommonDDLB.UseStateDDLB("상태선택");
            ViewBag.SalesStoreNameDDLB = CommonDDLB.SalesStoreNameDDLB();

            ViewBag.ItemGroupNameDDLB  = CommonDDLB.ItemGroupNameDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 상품 리스트 조회
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetItemList(ItemModel item)
        {
            List<ItemModel> data = new List<ItemModel>();

            try
            {
                string   spName      = "UP_SALES_STORE_ITEM_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intItemGroupNo", "@pi_intItemNo", "@pi_strItemName", "@pi_intSalesStoreNo", "@pi_strSalesStoreName",
                    "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    item.ItemGroupNo, item.ItemNo, item.ItemName ?? "", item.SalesStoreNo, item.SalesStoreName ?? "",
                    item.UseState
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        ItemModel obj = new ItemModel
                        {
                            ItemNo         = Convert.ToInt64(row["ITEMNO"]),
                            ItemName       = row["ITEMNAME"].ToString(),
                            ItemPrice      = Convert.ToInt32(row["PRICE"]),
                            ItemGroupName  = row["ITEMGROUPNAME"].ToString(),
                            SalesStoreNo   = Convert.ToInt64(row["SALESSTORENO"]),

                            SalesStoreName = row["SALESSTORENAME"].ToString(),
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
                Console.WriteLine("Exception in GetSalesStoreList : " + ex.Message);
                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품 상세조회
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailItem(ItemModel item)
        {
            Result<ItemModel> data = null;

            try
            {
                data = new Result<ItemModel>();

                string   spName      = "UP_SALES_STORE_ITEM_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intItemGroupNo", "@pi_strItemName", "@pi_intSalesStoreNo", "@pi_strSalesStoreName", "@pi_intItemNo"
                };
                object[] spParamValue = new object[] {
                    DBNull.Value, DBNull.Value, item.SalesStoreNo, DBNull.Value, item.ItemNo
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new ItemModel
                        {
                            ItemNo         = Convert.ToInt64(row["ITEMNO"]),
                            ItemName       = row["ITEMNAME"].ToString(),
                            ItemPrice      = Convert.ToInt32(row["PRICE"]),
                            ItemGroupNo    = Convert.ToInt64(row["ITEMGROUPNO"]),
                            SalesStoreNo   = Convert.ToInt64(row["SALESSTORENO"]),

                            SalesStoreName = row["SALESSTORENAME"].ToString(),
                            UseState       = Convert.ToInt16(row["USESTATE"]),
                            RegDate        = row["REGDATE"].ToString(),
                            UpdDate        = row["UPDDATE"].ToString() ?? "",
                            TotalINCnt     = Convert.ToInt32(row["TOTALINCNT"]),

                            TotalOUTCnt    = Convert.ToInt32(row["TOTALOUTCNT"]),
                            RemainCnt      = Convert.ToInt32(row["REMAINCNT"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 850;
                data.ErrMsg = "Exception in DetailItem : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품 등록
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult InsItem(ItemModel item)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_SALES_STORE_ITEM_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_intItemGroupNo", "@pi_strItemName", "@pi_intSalesStoreNo", "@pi_intPrice", "@pi_intTotalINCnt",
                    "@pi_strAdminID",     "@po_intRetVal",   "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    item.ItemGroupNo, item.ItemName, item.SalesStoreNo, item.ItemPrice, item.TotalINCnt,
                    strLoginID,       DBNull.Value,  DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 840;
                data.ErrMsg = "Exception in InsItem : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품 수정
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdItem(ItemModel item)
        {
            Result data = null;

            try
            {
                data = new Result();

                string   spName      = "UP_SALES_STORE_ITEM_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intItemNo",   "@pi_intSalesStoreNo", "@pi_intItemGroupNo", "@pi_strItemName", "@pi_intPrice",
                    "@pi_intStockCnt", "@pi_intUseState",     "@po_intRetVal",      "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    item.ItemNo,     item.SalesStoreNo, item.ItemGroupNo, item.ItemName, item.ItemPrice,
                    item.TotalINCnt, item.UseState,     DBNull.Value,     DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 830;
                data.ErrMsg = "Exception in UpdItem : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품분류관리 진입함수
        /// </summary>
        /// <returns></returns>
        ///---------------------------------------
        [HttpGet]
        public IActionResult ItemGroup()
        {
            strLoginType    = HttpContext.Session.GetString("logintype");
            if (strLoginType.Equals("2") || strLoginType.Equals("3"))
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ThisPage      = "상품분류관리";
            ViewBag.UseStateDDLB  = CommonDDLB.UseStateDDLB("상태선택");
            ViewBag.ItemGroupDDLB = CommonDDLB.ItemGroupNameDDLB();

            return View();
        }

        ///---------------------------------------
        /// <summary>
        /// 상품분류 리스트 조회
        /// </summary>
        /// <param name="itemGroup"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult GetItemGroupList(ItemGroupModel itemGroup)
        {
            List<ItemGroupModel> data = new List<ItemGroupModel>();

            try
            {
                string   spName      = "UP_ITEM_GROUP_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intItemGroupNo", "@pi_strItemGroupName", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    itemGroup.ItemGroupNo, DBNull.Value, itemGroup.UseState
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        ItemGroupModel obj = new ItemGroupModel
                        {
                            ItemGroupNo   = Convert.ToInt64(row["ITEMGROUPNO"]),  
                            ItemGroupName = row["ITEMGROUPNAME"].ToString(),
                            UseState      = Convert.ToInt16(row["USESTATE"]),
                            RegDate       = row["REGDATE"].ToString(),
                            UpdDate       = row["UPDDATE"].ToString() ?? ""
                        };

                        data.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetItemGroupList : " + ex.Message);
                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품분류 상세조회
        /// </summary>
        /// <param name="itemGroup"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult DetailItemGroup(ItemGroupModel itemGroup)
        {
            Result<ItemGroupModel> data = null;

            try
            {
                data = new Result<ItemGroupModel>();

                string   spName      = "UP_ITEM_GROUP_AR_LST";

                string[] spParamName = new string[] {
                    "@pi_intItemGroupNo", "@pi_strItemGroupName", "@pi_intUseState"
                };
                object[] spParamValue = new object[] {
                    itemGroup.ItemGroupNo, DBNull.Value, DBNull.Value
                };

                Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

                if (result.RetVal.Equals(0))
                {
                    foreach (DataRow row in result.data.Rows)
                    {
                        data.data = new ItemGroupModel
                        {
                            ItemGroupNo   = Convert.ToInt64(row["ITEMGROUPNO"]),
                            ItemGroupName = row["ITEMGROUPNAME"].ToString(),
                            ItemGroupDesc = row["ITEMGROUPDESC"].ToString() ?? "",
                            UseState      = Convert.ToInt16(row["USESTATE"]),
                            RegDate       = row["REGDATE"].ToString(),

                            UpdDate       = row["UPDDATE"].ToString() ?? ""
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                data.RetVal = 820;
                data.ErrMsg = "Exception in DetailItemGroup : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품분류 등록
        /// </summary>
        /// <param name="itemGroup"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult InsItemGroup(ItemGroupModel itemGroup)
        {
            Result data = null;

            try
            {
                strLoginID = HttpContext.Session.GetString("loginid");

                data = new Result();

                string   spName      = "UP_ITEM_GROUP_TX_INS";

                string[] spParamName = new string[] {
                    "@pi_strItemGroupName", "@pi_strItemGroupDesc", "@pi_strAdminID", "@po_intRetVal", "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    itemGroup.ItemGroupName, itemGroup.ItemGroupDesc ?? "", strLoginID, DBNull.Value,  DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 810;
                data.ErrMsg = "Exception in InsItemGroup : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }

        ///---------------------------------------
        /// <summary>
        /// 상품분류 수정
        /// </summary>
        /// <param name="itemGroup"></param>
        /// <returns></returns>
        ///---------------------------------------
        [HttpPost]
        public JsonResult UpdItemGroup(ItemGroupModel itemGroup)
        {
            Result data = null;

            try
            {
                data = new Result();

                string   spName      = "UP_ITEM_GROUP_TX_UPD";

                string[] spParamName = new string[] {
                    "@pi_intItemGroupNo", "@pi_strItemGroupName", "@pi_strItemGroupDesc", "@pi_intUseState", "@po_intRetVal",
                    "@po_strErrMsg"
                };
                object[] spParamValue = new object[] {
                    itemGroup.ItemGroupNo, itemGroup.ItemGroupName, itemGroup.ItemGroupDesc, itemGroup.UseState, DBNull.Value,
                    DBNull.Value
                };

                data = dbConn.SqlExecuteSPReturnValue(spName, spParamName, spParamValue);
            }
            catch (Exception ex)
            {
                data.RetVal = 800;
                data.ErrMsg = "Exception in UpdItemGroup : " + ex.Message;

                return Json(data);
            }

            return Json(new { data });
        }
    }
}
