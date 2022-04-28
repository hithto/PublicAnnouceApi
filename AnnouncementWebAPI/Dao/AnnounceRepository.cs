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
    public class AnnounceRepository : IDisposable
    {
        private readonly IDbConnection dbConnection;
        public static readonly string SysConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// 資料庫連接
        /// </summary>
        public AnnounceRepository()
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
        /// 取得公告置頂前十筆
        /// </summary>
        /// <returns></returns>
        public List<TopAnnounce> GetTopAnnounceList()
        {
            List<TopAnnounce> announceList = null;

            string strProcedure = "sp_GetTopAnnounceList";
            try
            {
                announceList = dbConnection.Query<TopAnnounce>(strProcedure, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return announceList;
        }

        /// <summary>
        /// 佈告欄列表
        /// </summary>
        /// <param name="dtStartDate">發布日期起</param>
        /// <param name="dtEndDate">發布日期訖</param>
        /// <param name="strSubject">主旨</param>
        /// <param name="intStartCount">頁第一筆</param>
        /// <param name="intEndCount">頁最後筆</param>
        /// <param name="strAccount">角色</param>
        /// <param name="strUserRole">權限</param>
        /// <returns></returns>
        public List<Announce> GetAnnounceList(DateTime? dtStartDate, DateTime? dtEndDate, string strSubject, int intStartCount, int intEndCount, string strAccount, string strUserRole)
        {
            List<Announce> announceList = null;

            string strProcedure = "sp_AnnounceList";
            try
            {
                announceList = dbConnection.Query<Announce>(strProcedure, new
                {
                    Subject = strSubject,
                    StartPublishDate = dtStartDate,
                    EndPublishDate = dtEndDate,
                    StartCount = intStartCount,
                    EndCount = intEndCount,
                    UserName = strAccount,
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
        /// 新增佈告欄
        /// </summary>
        /// <param name="announce"></param>
        /// <returns></returns>
        public int InsertAnnounce(Announce announce)
        {
            int Id = 0;

            string strProcedure = "sp_InsertAnnounce";

            try
            {
                var query = dbConnection.QuerySingle(strProcedure, new
                {
                    Subject = announce.Subject,
                    Body = announce.Body,
                    ExpireDate = announce.ExpireDate,
                    PublishDate = announce.PublishDate,
                    Top = announce.Top,
                    Important = announce.Important,
                    Creator = announce.Creator
                }, commandType: CommandType.StoredProcedure);

                Id = Convert.ToInt32(query.ItemID);
            }
            catch (Exception ex)
            {
                return Id;
            }

            return Id;
        }

        /// <summary>
        /// 編輯佈告欄
        /// </summary>
        /// <param name="announce"></param>
        /// <returns></returns>
        public bool UpdateAnnounce(Announce announce)
        {
            bool result = false;

            string strProcedure = "sp_UpdateAnnounce";

            try
            {
                int updateIndex = dbConnection.Execute(strProcedure, new
                {
                    ItemID = announce.ItemID,
                    Subject = announce.Subject,
                    Body = announce.Body,
                    ExpireDate = announce.ExpireDate,
                    PublishDate = announce.PublishDate,
                    RePublish = announce.RePublish,
                    ReExpireDate = announce.ReExpireDate,
                    RePublishDate = announce.RePublishDate,
                    Modifier = announce.Modifier,
                    Top = announce.Top,
                    Important = announce.Important,
                }, commandType: CommandType.StoredProcedure);
                result = (updateIndex != -1);
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }


        /// <summary>
        /// 刪除公告
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        public bool DeleteAnnounce(int ItemID)
        {
            bool result = false;

            string strProcedure = "sp_DeleteAnnounce";

            try
            {
                int deleteIndex = dbConnection.Execute(strProcedure, new
                {
                    ItemID = ItemID
                }, commandType: CommandType.StoredProcedure);
                result = (deleteIndex != -1);
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }



        /// <summary>
        /// 用流水號取得一篇佈告欄
        /// </summary>
        /// <param name="intItemID"></param>
        /// <returns></returns>
        public Announce GetAnnounceByItemID(int intItemID)
        {
            Announce announce = null;

            string strProcedure = "sp_GetAnnounce";
            try
            {
                announce = dbConnection.Query<Announce>(strProcedure, new
                {
                    ItemID = intItemID,
                }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }

            return announce;
        }



        /// <summary>
        /// 取得佈告欄列表查詢過濾頁數
        /// </summary>
        /// <param name="dtStartDate">發布日期起</param>
        /// <param name="dtEndDate">發布日期訖</param>
        /// <param name="strSubject">主旨</param>
        /// <param name="strAccount">角色</param>
        /// <param name="strUserRole">權限</param>
        /// <returns></returns>
        public int GetFilteredPage(DateTime? dtStartDate, DateTime? dtEndDate, string strSubject, string strAccount, string strUserRole)
        {
            int filterPage = 0;

            string strProcedure = "sp_AnnounceFilteredPage";

            try
            {
                filterPage = dbConnection.QueryFirst<Int32>(strProcedure, new
                {
                    Subject = strSubject,
                    StartPublishDate = dtStartDate,
                    EndPublishDate = dtEndDate,
                    UserName = strAccount,
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
        /// 取得佈告欄列表總頁數
        /// </summary>
        /// <param name="strAccount">角色</param>
        /// <param name="strUserRole">權限</param>
        /// <returns></returns>
        public int GetTotalPage(string strAccount, string strUserRole)
        {
            int totalPage = 0;

            string strProcedure = "sp_AnnounceTotalPage";

            try
            {
                totalPage = dbConnection.QueryFirst<Int32>(strProcedure, new
                {
                    UserName = strAccount,
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