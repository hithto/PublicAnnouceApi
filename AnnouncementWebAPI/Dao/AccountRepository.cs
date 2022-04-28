using AnnouncementModel;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AnnouncementWebAPI.Dao
{
    public class AccountRepository : IDisposable
    {
        private readonly IDbConnection dbConnection;
        public static readonly string SysConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// 資料庫連接
        /// </summary>
        public AccountRepository()
        {
            try
            {
                dbConnection = new SqlConnection(SysConnectionString);
                dbConnection.Open();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 取得單一使用者帳號
        /// </summary>
        /// <param name="strEmpId"></param>
        /// <returns></returns>
        public Account GetEmpId(string strEmpId)
        {
            Account account = null;

            string strProcedure = "sp_GetAccount";
            try
            {
                account = dbConnection.Query<Account>(strProcedure, new
                {
                    EmpId = strEmpId,
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return account;
        }

       /// <summary>
       /// 取得使用者列表
       /// </summary>
       /// <param name="strEmpId">帳號</param>
       /// <param name="strEmpType">角色</param>
       /// <param name="intStartCount">頁第一筆</param>
       /// <param name="intEndCount">頁最後筆</param>
       /// <returns></returns>
        public List<Account> GetAccountList(string strEmpId, string strEmpType, int intStartCount, int intEndCount)
        {
            List<Account> accountList = null;

            string strProcedure = "sp_AccountList";
            try
            {
                accountList = dbConnection.Query<Account>(strProcedure, new
                {
                    EmpId = strEmpId,
                    EmpType = strEmpType,
                    StartCount = intStartCount,
                    EndCount = intEndCount
                }, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return accountList;
        }

        /// <summary>
        /// 新增會員帳號
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool InsertAccount(Account account)
        {
            bool result = false;

            string strProcedure = "sp_InsertAccount";

            try
            {
                int insertIndex = dbConnection.Execute(strProcedure, new
                {
                    EmpId = account.EmpId,
                    EmpPw = account.EmpPw,
                    EmpType = account.EmpType
                }, commandType: CommandType.StoredProcedure);
                result = (insertIndex != -1);
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }

        /// <summary>
        /// 取得使用者列表查詢過濾頁數
        /// </summary>
        /// <param name="strEmpId">帳號</param>
        /// <param name="strEmpType">角色</param>
        /// <returns></returns>
        public int GetFilteredPage(string strEmpId, string strEmpType)
        {
            int filterPage = 0;

            string strProcedure = "sp_FilteredPage";

            try
            {
                filterPage = dbConnection.QueryFirst<Int32>(strProcedure, new
                {
                    EmpId = strEmpId,
                    EmpType = strEmpType
                }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                return 0;
            }

            return filterPage;
        }


        /// <summary>
        /// 取得使用者列表總頁數
        /// </summary>
        /// <returns></returns>
        public int GetTotalPage()
        {
            int totalPage = 0;

            string strProcedure = "sp_TotalPage";

            try
            {
                totalPage = dbConnection.QueryFirst<Int32>(strProcedure, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                return 0;
            }

            return totalPage;
        }


        public void Dispose()
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
        }
    }
}