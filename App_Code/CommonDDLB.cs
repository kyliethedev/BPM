using Admin.Models.Common;
using Admin.Models.MemberStore;
using System;
using System.Data;
using System.Text;

namespace Admin.App_Code
{
    ///=======================================
    /// <summary>
    /// Common DDLB Class
    /// </summary>
    ///=======================================
    public class CommonDDLB
    {
        private static DBInitConn dbConn = new DBInitConn();

        ///---------------------------------------
        /// <summary>
        /// Print DDLB Option Html
        /// </summary>
        ///---------------------------------------
        public static string PrintDDLBOption(string value, string name)
        {
            return string.Format("<option value='{0}'>{1}</option>", value, name);
        }

        ///---------------------------------------
        /// <summary>
        /// Print DDLB Selected Option Html
        /// </summary>
        ///---------------------------------------
        public static string PrintDDLBSelectedOption(string value, string name)
        {
            return string.Format("<option value='{0}' Selected>{1}</option>", value, name);
        }

        #region DB 연동없는 DDLB
        ///---------------------------------------
        /// <summary>
        /// Print UseState DDLB
        /// </summary>
        ///---------------------------------------
        public static string UseStateDDLB(string strSelName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PrintDDLBOption("0", strSelName));
            sb.Append(PrintDDLBOption("1", "정상"));
            sb.Append(PrintDDLBOption("2", "해지"));

            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// Print UseType DDLB (For MemberStore)
        /// </summary>
        ///---------------------------------------
        public static string UseTypeDDLB()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PrintDDLBOption("0", "사용방식선택"));
            sb.Append(PrintDDLBOption("1", "직접결제"));
            sb.Append(PrintDDLBOption("2", "간접결제"));
            sb.Append(PrintDDLBOption("3", "복합결제"));

            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// Print ProvideType DDLB (For MemberStore)
        /// </summary>
        ///---------------------------------------
        public static string ProvideTypeDDLB()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PrintDDLBOption("0", "지급방식선택"));
            sb.Append(PrintDDLBOption("1", "일괄"));
            sb.Append(PrintDDLBOption("2", "분할"));

            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// Print BPApply StateCode DDLB
        /// </summary>
        ///---------------------------------------
        public static string StateCodeDDLB()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(PrintDDLBOption("0", "상태선택"));
            sb.Append(PrintDDLBOption("1", "요청"));
            sb.Append(PrintDDLBOption("2", "승인"));
            sb.Append(PrintDDLBOption("3", "거절"));
            sb.Append(PrintDDLBOption("4", "완료"));

            return sb.ToString();
        }
        #endregion

        #region DB 연동 DDLB
        ///---------------------------------------
        /// <summary>
        /// 가맹점 별 팀명 DDLB
        /// </summary>
        ///---------------------------------------
        public static string TeamNameDDLB(long memberStoreNo)
        {
            StringBuilder sb = new StringBuilder();

            string   spName      = "UP_TEAMNAME_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intMemberStoreNo"
            };
            object[] spParamValue = new object[] {
                memberStoreNo
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                sb.Append(PrintDDLBOption("", "소속팀선택"));

                foreach (DataRow row in result.data.Rows)
                {
                    sb.Append(PrintDDLBOption(row["TEAMNAME"].ToString(), row["TEAMNAME"].ToString()));
                }
            }
            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점 별 지급등급 DDLB
        /// </summary>
        ///---------------------------------------
        public static string BPLevelDDLB(long memberStoreNo)
        {
            StringBuilder sb = new StringBuilder();

            string   spName      = "UP_MEMBERSTORE_BPLEVEL_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intMemberStoreNo"
            };
            object[] spParamValue = new object[] {
                memberStoreNo
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                sb.Append(PrintDDLBOption("", "지급등급선택"));

                foreach (DataRow row in result.data.Rows)
                {
                    sb.Append(PrintDDLBOption(row["BPLEVELNO"].ToString(), row["BPLEVELNAME"].ToString()));
                }
            }
            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처 DDLB (For 판매처 연계)
        /// </summary>
        ///---------------------------------------
        public static string ServicedSalesStoreDDLB(SalesContractModel salesContract)
        {
            StringBuilder sb = new StringBuilder();

            string   spName      = "UP_MEMBERSTORE_SALES_CONTRACT_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intMemberStoreNo"
            };
            object[] spParamValue = new object[] {
                salesContract.MemberStoreNo
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                foreach (DataRow row in result.data.Rows)
                {
                    if (Convert.ToInt64(row["CONTRACTNO"]).Equals(0)) //매핑 안 된 판매처
                    {
                        sb.Append(PrintDDLBOption(row["SALESSTORENO"].ToString(), row["SALESSTORENAME"].ToString()));
                    }
                    else //매핑 된 판매처
                    {
                        sb.Append(PrintDDLBSelectedOption(row["SALESSTORENO"].ToString(), row["SALESSTORENAME"].ToString()));
                    }
                }
            }

            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// 사용중인 판매처명 DDLB
        /// </summary>
        ///---------------------------------------
        public static string SalesStoreNameDDLB()
        {
            StringBuilder sb = new StringBuilder();

            string   spName      = "UP_SALES_STORE_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intSalesStoreNo", "@pi_strSalesStoreName", "@pi_strCEOName", "@pi_intUseState"
            };
            object[] spParamValue = new object[] {
                DBNull.Value, DBNull.Value, DBNull.Value, 1
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                sb.Append(PrintDDLBOption("", "판매처선택"));

                foreach (DataRow row in result.data.Rows)
                {
                    sb.Append(PrintDDLBOption(row["SALESSTORENO"].ToString(), row["SALESSTORENAME"].ToString()));
                }
            }
            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// 판매처명 DDLB (상태 상관없이)
        /// </summary>
        ///---------------------------------------
        public static string SalesStoreNameALLDDLB()
        {
            StringBuilder sb = new StringBuilder();

            string spName = "UP_SALES_STORE_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intSalesStoreNo", "@pi_strSalesStoreName", "@pi_strCEOName", "@pi_intUseState"
            };
            object[] spParamValue = new object[] {
                DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                sb.Append(PrintDDLBOption("", "판매처선택"));

                foreach (DataRow row in result.data.Rows)
                {
                    sb.Append(PrintDDLBOption(row["SALESSTORENO"].ToString(), row["SALESSTORENAME"].ToString()));
                }
            }
            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// 상품분류 DDLB
        /// </summary>
        ///---------------------------------------
        public static string ItemGroupNameDDLB()
        {
            StringBuilder sb = new StringBuilder();

            string   spName      = "UP_ITEM_GROUP_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intItemGroupNo", "@pi_strItemGroupName", "@pi_intUseState"
            };
            object[] spParamValue = new object[] {
                DBNull.Value, DBNull.Value, DBNull.Value
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                sb.Append(PrintDDLBOption("", "상품분류선택"));

                foreach (DataRow row in result.data.Rows)
                {
                    sb.Append(PrintDDLBOption(row["ITEMGROUPNO"].ToString(), row["ITEMGROUPNAME"].ToString()));
                }
            }
            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// 사용중인 가맹점명 DDLB
        /// </summary>
        ///---------------------------------------
        public static string MemberStoreNameDDLB()
        {
            StringBuilder sb = new StringBuilder();

            string spName        = "UP_MEMBER_STORE_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intMemberStoreNo",  "@pi_strMemberStoreName", "@pi_strCEOName",        "@pi_strLicenseNo",  "@pi_intUseType",
                "@pi_intProvideType",    "@pi_intProvideCycle",    "@pi_intLastProvideYMD", "@pi_intExpireFlag", "@pi_intExpireCycle",
                "@pi_intLastExpireYMD" , "@pi_intUseState"
            };
            object[] spParamValue = new object[] {
                DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                DBNull.Value, 1
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                sb.Append(PrintDDLBOption("", "가맹점선택"));

                foreach (DataRow row in result.data.Rows)
                {
                    sb.Append(PrintDDLBOption(row["MEMBERSTORENO"].ToString(), row["MEMBERSTORENAME"].ToString()));
                }
            }
            return sb.ToString();
        }

        ///---------------------------------------
        /// <summary>
        /// 가맹점명 DDLB (상태 상관없이)
        /// </summary>
        ///---------------------------------------
        public static string MemberStoreNameAllDDLB()
        {
            StringBuilder sb = new StringBuilder();

            string spName = "UP_MEMBER_STORE_AR_LST";

            string[] spParamName = new string[] {
                "@pi_intMemberStoreNo",  "@pi_strMemberStoreName", "@pi_strCEOName",        "@pi_strLicenseNo",  "@pi_intUseType",
                "@pi_intProvideType",    "@pi_intProvideCycle",    "@pi_intLastProvideYMD", "@pi_intExpireFlag", "@pi_intExpireCycle",
                "@pi_intLastExpireYMD" , "@pi_intUseState"
            };
            object[] spParamValue = new object[] {
                DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value,
                DBNull.Value, DBNull.Value
            };

            Result<DataTable> result = dbConn.SqlExecuteSPReturnTable(spName, spParamName, spParamValue);

            if (result.RetVal.Equals(0))
            {
                sb.Append(PrintDDLBOption("", "가맹점선택"));

                foreach (DataRow row in result.data.Rows)
                {
                    sb.Append(PrintDDLBOption(row["MEMBERSTORENO"].ToString(), row["MEMBERSTORENAME"].ToString()));
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
