using AnnouncementModel;
using AnnouncementWebAPI.Dao;
using AnnouncementWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnouncementWebAPI.Service
{
    public class HistoryAnnounceService
    {
        /// <summary>
        /// 取得歷史公告查詢列表
        /// </summary>
        /// <param name="val">發佈起訖日、發佈者、頁碼起訖、帳號角色</param>
        /// <returns></returns>
        public List<Announce> GetAnnounceList(string strStartDate, string strEndDate, string strAnnouncer, int intStartCount, int intEndCount, string strUserRole)
        {
            List<Announce> result = new List<Announce>();

            DateTime? dtStartDate = null;
            DateTime? dtEndDate = null;

            if (!string.IsNullOrWhiteSpace(strStartDate))
                dtStartDate = Convert.ToDateTime(strStartDate);

            if (!string.IsNullOrWhiteSpace(strEndDate))
                dtEndDate = Convert.ToDateTime(strEndDate);

            if (!string.IsNullOrWhiteSpace(strAnnouncer))
                strAnnouncer = strAnnouncer.Trim();
            else
                strAnnouncer = null;

            using (HistoryAnnounceRepository repository = new HistoryAnnounceRepository())
            {
                result = repository.GetAnnounceList(dtStartDate, dtEndDate, strAnnouncer, intStartCount, intEndCount, strUserRole);
            }

            return result;
        }

        /// <summary>
        /// 歷史公告查詢過濾頁數
        /// </summary>
        /// <param name="val">發佈起訖日、發佈者、帳號角色</param>
        /// <returns></returns>
        public int GetFilteredPage(string strStartDate, string strEndDate, string strAnnouncer, string strUserRole)
        {
            int filterPage = 0;

            DateTime? dtStartDate = null;
            DateTime? dtEndDate = null;

            if (!string.IsNullOrWhiteSpace(strStartDate))
                dtStartDate = Convert.ToDateTime(strStartDate);

            if (!string.IsNullOrWhiteSpace(strEndDate))
                dtEndDate = Convert.ToDateTime(strEndDate);

            if (!string.IsNullOrWhiteSpace(strAnnouncer))
                strAnnouncer = strAnnouncer.Trim();
            else
                strAnnouncer = null;

            using (HistoryAnnounceRepository repository = new HistoryAnnounceRepository())
            {
                filterPage = repository.GetFilteredPage(dtStartDate, dtEndDate, strAnnouncer, strUserRole);
            }

            return filterPage;
        }

        /// <summary>
        /// 取得總頁數
        /// </summary>
        /// <param name="val">發佈者、角色</param>
        /// <returns></returns>
        public int GetTotalPage(string strAnnouncer, string strUserRole)
        {
            int totalPage = 0;

            using (HistoryAnnounceRepository repository = new HistoryAnnounceRepository())
            {
                totalPage = repository.GetTotalPage(strAnnouncer, strUserRole);
            }

            return totalPage;
        }

    }
}