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
    public class HistoryAnnounceRepository : IDisposable
    {
        private readonly IDbConnection dbConnection;
        public static readonly string SysConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// 資料庫連接
        /// </summary>
        public HistoryAnnounceRepository()
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
        /// 歷史公告列表
        /// </summary>
        /// <param name="dtStartDate">下架起日期起</param>
        /// <param name="dtEndDate">下架訖日訖</param>
        /// <param name="strAnnouncer">發佈者</param>
        /// <param name="intStartCount">頁第一筆</param>
        /// <param name="intEndCount">頁最後筆</param>
        /// <param name="strUserRole">權限</param>
        /// <returns></returns>
        public List<Announce> GetAnnounceList(DateTime? dtStartDate, DateTime? dtEndDate, string strAnnouncer, int intStartCount, int intEndCount, string strUserRole)
        {
            List<Announce> announceList = null;

            string strProcedure = "sp_HistoryAnnounceList";
            try
            {
                announceList = dbConnection.Query<Announce>(strProcedure, new
                {
                    Announcer = strAnnouncer,
                    StartExpireDate = dtStartDate,
                    EndExpireDate = dtEndDate,
                    StartCount = intStartCount,
                    EndCount = intEndCount,
                    UserRole = strUserRole
                }, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return announceList;
        }


        /// <summary>
        /// 取得歷史公告列表查詢過濾頁數
        /// </summary>
        /// <param name="dtStartDate">下架日期起</param>
        /// <param name="dtEndDate">下架日期訖</param>
        /// <param name="strAnnouncer">發佈者</param>
        /// <param name="strUserRole">權限</param>
        /// <returns></returns>
        public int GetFilteredPage(DateTime? dtStartDate, DateTime? dtEndDate, string strAnnouncer, string strUserRole)
        {
            int filterPage = 0;

            string strProcedure = "sp_HistoryAnnounceFilteredPage";

            try
            {
                filterPage = dbConnection.QueryFirst<Int32>(strProcedure, new
                {
                    Announcer = strAnnouncer,
                    StartExpireDate = dtStartDate,
                    EndExpireDate = dtEndDate,
                    UserRole = strUserRole
                }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                return 0;
            }

            return filterPage;
        }


        /// <summary>
        /// 取得歷史公告列表總頁數
        /// </summary>
        /// <param name="strAnnouncer">發佈者</param>
        /// <param name="strUserRole">權限</param>
        /// <returns></returns>
        public int GetTotalPage(string strAnnouncer, string strUserRole)
        {
            int totalPage = 0;

            string strProcedure = "sp_HistoryAnnounceTotalPage";

            try
            {
                totalPage = dbConnection.QueryFirst<Int32>(strProcedure, new
                {
                    Announcer = strAnnouncer,
                    UserRole = strUserRole
                }, commandType: CommandType.StoredProcedure);
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