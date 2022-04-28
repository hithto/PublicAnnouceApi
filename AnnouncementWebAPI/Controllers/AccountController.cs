using AnnouncementModel;
using AnnouncementWebAPI.Dao;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AnnouncementWebAPI.Controllers
{
    public class AccountController : ApiController
    {
        private readonly AccountService service;

        public AccountController()
        {
            this.service = new AccountService();
        }

        /// <summary>
        /// 取得單一使用者帳號
        /// </summary>
        /// <param name="val">帳號</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetAccount(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                string strUserID = (string)val.EmpId;

                objReturn.Account = service.GetEmpId(strUserID);

                if (objReturn.Account != null)
                    objReturn.status = true;
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }


        /// <summary>
        /// 取得帳號管理查詢列表
        /// </summary>
        /// <param name="val">帳號、角色</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic GetAccountList(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                string strUserID = (string)val.EmpId;
                string strEmpType = (string)val.EmpType;
                int intStartCount = (int)val.StartCount;
                int intEndCount = (int)val.EndCount;

                objReturn.AccountList = service.GetAccountList(strUserID, strEmpType, intStartCount, intEndCount);
                objReturn.FilteredPage = service.GetFilteredPage(strUserID, strEmpType);
                objReturn.TotalPage = service.GetTotalPage();

                objReturn.status = true;
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;
        }

        /// <summary>
        /// 新增使用者帳號
        /// </summary>
        /// <param name="val">帳號、密碼、角色</param>
        /// <returns></returns>
        [HttpPost]
        public dynamic InsertAccount(dynamic val)
        {
            // 宣告回傳物件
            dynamic objReturn = new ExpandoObject();

            objReturn.errorMessage = null;
            objReturn.status = false;

            try
            {
                string strUserID = (string)val.EmpId;
                string strEmpPw = (string)val.EmpPw;
                string strEmpType = (string)val.EmpType;

                objReturn.status = service.InsertAccount(strUserID, strEmpPw, strEmpType);
            }
            catch (Exception e)
            {
                objReturn.errorMessage = e.Message;
            }

            return objReturn;

        }


    }
}