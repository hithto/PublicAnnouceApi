using AnnouncementModel;
using AnnouncementWebAPI.Dao;
using AnnouncementWebAPI.Models;
using AnnouncementWebAPI.Service;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AnnouncementWebAPI.Controllers
{
    public class HistoryAnnounceController : ApiController
    {
        private readonly HistoryAnnounceService service;

        public HistoryAnnounceController()
        {
            this.service = new HistoryAnnounceService();
        }

        /// <summary>
        /// 取得歷史公告查詢列表
        /// </summary>
        /// <param name="val">下架起訖日、發佈者</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetHistoryAnnounceList(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                string strStartExpireDate = (string)val.StartExpireDate;
                string strEndExpireDate = (string)val.EndExpireDate;
                string strAnnouncer = (string)val.Announcer;
                int intStartCount = (int)val.StartCount;
                int intEndCount = (int)val.EndCount;

                string strUserRole = (string)val.UserRole;

                objReturn.AnnounceList = service.GetAnnounceList(strStartExpireDate, strEndExpireDate, strAnnouncer, intStartCount, intEndCount, strUserRole);
                objReturn.FilteredPage = service.GetFilteredPage(strStartExpireDate, strEndExpireDate, strAnnouncer, strUserRole);
                objReturn.TotalPage = service.GetTotalPage(strAnnouncer, strUserRole);

                objReturn.status = true;
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }


    }
}