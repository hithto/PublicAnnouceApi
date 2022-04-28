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
    public class AnnounceController : ApiController
    {
        private readonly AnnounceService service;

        public AnnounceController()
        {
            this.service = new AnnounceService();
        }


        /// <summary>
        /// 取得公告前十筆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetTopAnnounceList(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                objReturn.AnnounceList = service.GetTopAnnounceList();
                objReturn.status = true;
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }


        /// <summary>
        /// 取得佈告欄查詢列表
        /// </summary>
        /// <param name="val">發佈起訖日、主旨</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetAnnounceList(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                string strStartPublishDate = (string)val.StartPublishDate;
                string strEndPublishDate = (string)val.EndPublishDate;
                string strSubject = (string)val.Subject;
                int intStartCount = (int)val.StartCount;
                int intEndCount = (int)val.EndCount;

                string strAccount = (string)val.Account;
                string strUserRole = (string)val.UserRole;

                objReturn.AnnounceList = service.GetAnnounceList(strStartPublishDate, strEndPublishDate, strSubject, intStartCount, intEndCount, strAccount, strUserRole);
                objReturn.FilteredPage = service.GetFilteredPage(strStartPublishDate, strEndPublishDate, strSubject, strAccount, strUserRole);
                objReturn.TotalPage = service.GetTotalPage(strAccount, strUserRole);

                objReturn.status = true;
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }

        /// <summary>
        /// 新增佈告欄
        /// </summary>
        /// <param name="val">主旨、內容、發佈日期、下架日期、置頂</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic InsertAnnounce(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {

                string strSubject = (string)val.Subject;
                string strBody = (string)val.Body;
                string strExpireDate = (string)val.ExpireDate;
                string strPublishDate = (string)val.PublishDate;
                bool bitTop = bool.Parse((string)val.Top);
                bool bitImportant = bool.Parse((string)val.Important);
                string strCreator = (string)val.Creator;

                int Id = service.InsertAnnounce(strSubject, strBody, strExpireDate, strPublishDate, bitTop, bitImportant, strCreator);

                objReturn.Id = Id;
                objReturn.status = Id > 0;
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }


        /// <summary>
        /// 更新佈告欄
        /// </summary>
        /// <param name="val">主旨、內容、發佈日期、下架日期、置頂、重新上下架、重新上下架日期</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic UpdateAnnounce(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                string strItemID = (string)val.ItemID;
                string strSubject = (string)val.Subject;
                string strBody = (string)val.Body;
                string strExpireDate = (string)val.ExpireDate;
                string strPublishDate = (string)val.PublishDate;
                string strRePublish = (string)val.RePublish;
                string strReExpireDate = (string)val.ReExpireDate;
                string strRePublishDate = (string)val.RePublishDate;
                string strTop = (string)val.Top;
                string strImportant = (string)val.Important;
                string strModifier = (string)val.Modifier;

                UpdateAnnounceModel updateModel = new UpdateAnnounceModel()
                {
                    ItemID = strItemID,
                    Subject = strSubject,
                    Body = strBody,
                    ExpireDate = strExpireDate,
                    PublishDate = strPublishDate,
                    RePublish = strRePublish,
                    ReExpireDate = strReExpireDate,
                    RePublishDate = strRePublishDate,
                    Top = strTop,
                    Important = strImportant,
                    Modifier = strModifier
                };

                objReturn.status = service.UpdateAnnounce(updateModel);
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }


        /// <summary>
        /// 刪除佈告欄
        /// </summary>
        /// <param name="val">序列號</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic DeleteAnnounce(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                int intItemID = (int)val.ItemID;

                objReturn.status = service.DeleteAnnounce(intItemID);
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }


        /// <summary>
        /// 用流水號取得一篇佈告欄
        /// </summary>
        /// <param name="val">序列</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetAnnounce(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                int intItemID = (int)val.ItemID;

                objReturn.Announce = service.GetAnnounceByItemID(intItemID);

                if (objReturn.Announce != null)
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