using AnnouncementModel;
using AnnouncementWebAPI.Dao;
using AnnouncementWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnnouncementWebAPI.Service
{
    public class MeetupService
    {
        /// <summary>
        /// 取得公告置頂前十筆
        /// </summary>
        /// <returns></returns>
        public List<TopAnnounce> GetTopAnnounceList()
        {
            List<TopAnnounce> result = new List<TopAnnounce>();

            using (AnnounceRepository repository = new AnnounceRepository())
            {
                result = repository.GetTopAnnounceList();
            }

            return result;
        }

        /// <summary>
        /// MeetupList
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public List<Meetup> GetMeetupList()
        {
            List<Meetup> result = new List<Meetup>();

            using (MeetupRepository repository = new MeetupRepository())
            {
                result = repository.GetMeetupList();
            }

            return result;
        }

       
        /// <summary>
        /// 新增佈告欄
        /// </summary>
        /// <param name="val">/param>
        /// <returns></returns>
        public bool InsertMeetup(string strTitle, string strImage, string strAddress, string strDescription)
        {
            var result = false;

            if (!string.IsNullOrWhiteSpace(strTitle))
                strTitle = strTitle.Trim();

            if (!string.IsNullOrWhiteSpace(strImage))
                strImage = strImage.Trim();

            if (!string.IsNullOrWhiteSpace(strAddress))
                strAddress = strAddress.Trim();

            if (!string.IsNullOrWhiteSpace(strDescription))
                strDescription = strDescription.Trim();


            Meetup meetup = new Meetup()
            {
                Title = strTitle,
                Image = strImage,
                Address = strAddress,
                Description = strDescription
            };

            using (MeetupRepository repository = new MeetupRepository())
            {
                result = repository.InsertMeetup(meetup);
            }

            return result;
        }


        /// <summary>
        /// 更新佈告欄
        /// </summary>
        /// <param name="val">主旨、內容、發佈日期、下架日期、置頂、重新上下架、重新上下架日期</param>
        /// <returns></returns>
        public bool UpdateAnnounce(UpdateAnnounceModel announce)
        {
            bool result = false;

            int intItemId = 0;
            if (!string.IsNullOrWhiteSpace(announce.ItemID))
                intItemId = Convert.ToInt32(announce.ItemID);

            #region 判斷是否上架，已經上架公告任何人都不能編輯
            Announce getAnnounce = null;
            using (AnnounceRepository repository = new AnnounceRepository())
            {
                getAnnounce = repository.GetAnnounceByItemID(intItemId);
            }

            if (getAnnounce != null)
            {
                DateTime? dtExpireDate = null;
                DateTime? dtPublishDate = null;

                if (!string.IsNullOrWhiteSpace(announce.ExpireDate))
                    dtExpireDate = Convert.ToDateTime(announce.ExpireDate);

                if (!string.IsNullOrWhiteSpace(announce.PublishDate))
                    dtPublishDate = Convert.ToDateTime(announce.PublishDate);

                // 已上架公告任何人不能編輯
                bool isPublish = false;
                if (getAnnounce.PublishDate.HasValue)
                {
                    DateTime getPublishDatet = Convert.ToDateTime(getAnnounce.PublishDate.Value);

                    isPublish = DateTime.Now >= getPublishDatet;

                    if (isPublish)
                        dtPublishDate = getPublishDatet;
                }

                // 已經下架公告，不能編輯
                bool isExpireDate = false;
                if (getAnnounce.ExpireDate.HasValue)
                {
                    DateTime getExpireDate = Convert.ToDateTime(getAnnounce.ExpireDate.Value);

                    isExpireDate = DateTime.Now >= getExpireDate;

                    if (isExpireDate)
                        dtExpireDate = getExpireDate;
                }
                #endregion

                if (!string.IsNullOrWhiteSpace(announce.Subject))
                    announce.Subject = announce.Subject.Trim();

                if (!string.IsNullOrWhiteSpace(announce.Body))
                    announce.Body = announce.Body.Trim();


                bool bitTop = bool.Parse((string)announce.Top);

                bool bitImportant = bool.Parse((string)announce.Important);

                bool bitRePublish = bool.Parse((string)announce.RePublish);

                DateTime? dtReExpireDate = null;
                DateTime? dtRePublishDate = null;

                // 處理重新上架
                if (bitRePublish)
                {
                    if (!string.IsNullOrWhiteSpace(announce.ReExpireDate))
                        dtReExpireDate = Convert.ToDateTime(announce.ReExpireDate);

                    if (!string.IsNullOrWhiteSpace(announce.RePublishDate))
                        dtRePublishDate = Convert.ToDateTime(announce.RePublishDate);

                    // 主旨重新上架時，不能修改
                    announce.Subject = getAnnounce.Subject;
                }

                Announce upAnnounce = new Announce()
                {
                    Subject = announce.Subject,
                    Body = announce.Body,
                    ExpireDate = dtExpireDate,
                    PublishDate = dtPublishDate,
                    Top = bitTop,
                    Important = bitImportant,
                    Modifier = announce.Modifier,
                    RePublish = bitRePublish,
                    ReExpireDate = dtReExpireDate,
                    RePublishDate = dtRePublishDate,
                    ItemID = intItemId
                };

                using (AnnounceRepository repository = new AnnounceRepository())
                {
                    result = repository.UpdateAnnounce(upAnnounce);
                }
            }

            return result;
        }

        /// <summary>
        /// 刪除公告欄
        /// </summary>
        /// <param name="intItemID"></param>
        /// <returns></returns>
        public bool DeleteAnnounce(int intItemID)
        {
            bool result = false;

            Announce getAnnounce = null;
            using (AnnounceRepository repository = new AnnounceRepository())
            {
                getAnnounce = repository.GetAnnounceByItemID(intItemID);
            }

            if (getAnnounce != null)
            {
                using (AnnounceRepository repository = new AnnounceRepository())
                {
                    result = repository.DeleteAnnounce(intItemID);
                }
            }

            return result;
        }

        /// <summary>
        /// 佈告欄查詢過濾頁數
        /// </summary>
        /// <param name="val">發佈起訖日、主旨、頁碼起訖、帳號角色</param>
        /// <returns></returns>
        public int GetFilteredPage(string strStartDate, string strEndDate, string strSubject, string strAccount, string strUserRole)
        {
            int filterPage = 0;

            DateTime? dtStartDate = null;
            DateTime? dtEndDate = null;

            if (!string.IsNullOrWhiteSpace(strStartDate))
                dtStartDate = Convert.ToDateTime(strStartDate);

            if (!string.IsNullOrWhiteSpace(strEndDate))
                dtEndDate = Convert.ToDateTime(strEndDate);

            if (!string.IsNullOrWhiteSpace(strSubject))
                strSubject = strSubject.Trim();
            else
                strSubject = null;

            using (AnnounceRepository repository = new AnnounceRepository())
            {
                filterPage = repository.GetFilteredPage(dtStartDate, dtEndDate, strSubject, strAccount, strUserRole);
            }

            return filterPage;
        }

        /// <summary>
        /// 取得總頁數
        /// </summary>
        /// <param name="val">帳號角色</param>
        /// <returns></returns>
        public int GetTotalPage(string strAccount, string strUserRole)
        {
            int totalPage = 0;

            using (AnnounceRepository repository = new AnnounceRepository())
            {
                totalPage = repository.GetTotalPage(strAccount, strUserRole);
            }

            return totalPage;
        }

    }
}