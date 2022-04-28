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
    public class MeetupRepository : IDisposable
    {
        private readonly IDbConnection dbConnection;
        public static readonly string SysConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        /// <summary>
        /// 資料庫連接
        /// </summary>
        public MeetupRepository()
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

        public List<Meetup> GetMeetupList()
        {
            List<Meetup> meetupList = null;

             string strSql = $@"Select [Title], [Image], [Address], [Description] FROM [Meetup] ";

            try
            {
                meetupList = dbConnection.Query<Meetup>(strSql).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }

            return meetupList;
        }

        /// <summary>
        /// 新增會員帳號
        /// </summary>
        /// <param name="meetup"></param>
        /// <returns></returns>
        public bool InsertMeetup(Meetup meetup)
        {
            bool result = false;

            //string strProcedure = "sp_InsertAccount";

            try
            {
                string strSql = $@"INSERT INTO [dbo].[Meetup]([Title], [Image], [Address], [Description]) VALUES ( @Title, @Image, @Address, @Description)";

                int insertIndex = dbConnection.Execute(strSql, new
                {
                    Title = meetup.Title,
                    Image = meetup.Image,
                    Address = meetup.Address,
                    Description = meetup.Description
                });
                result = (insertIndex != -1);
            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
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