using AnnouncementWebAPI.Cors;
using AnnouncementWebAPI.Service;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AnnouncementWebAPI.Controllers
{
    public class MeetupController : ApiController
    {

        private readonly MeetupService service;

        public MeetupController()
        {
            this.service = new MeetupService();
        }


        /// <summary>
        /// 新增 Add New Meetup
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [HttpPost]
        public dynamic InsertMeetup(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                string strTitle = (string)val.Title;
                string strImage = (string)val.Image;
                string strAddress = (string)val.Address;
                string strDescription = (string)val.Description;

                objReturn.status = service.InsertMeetup(strTitle, strImage, strAddress, strDescription);
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }

        [HttpPost]
        [EnableCors(origins:"http://localhost:3000", headers:"*", methods: "*")]
        public dynamic GetMeetupList(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                objReturn.MeetupList = service.GetMeetupList();

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
