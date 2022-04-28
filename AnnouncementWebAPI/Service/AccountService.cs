using AnnouncementModel;
using AnnouncementWebAPI.Dao;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

public class AccountService
{
    public Account GetEmpId(string strEmpId)
    {
        if (!string.IsNullOrWhiteSpace(strEmpId))
            strEmpId = strEmpId.Trim();

        using (AccountRepository repository = new AccountRepository())
        {
            return repository.GetEmpId(strEmpId);
        }
    }


    public List<Account> GetAccountList(string strEmpId, string strEmpType, int intStartCount, int intEndCount)
    {
        List<Account> result = new List<Account>();

        if (!string.IsNullOrWhiteSpace(strEmpId))
            strEmpId = strEmpId.Trim();
        else
            strEmpId = null;

        if (!string.IsNullOrWhiteSpace(strEmpType))
            strEmpType = strEmpType.Trim();
        else
            strEmpType = null;

        using (AccountRepository repository = new AccountRepository())
        {
            result = repository.GetAccountList(strEmpId, strEmpType, intStartCount, intEndCount);
        }

        return result;
    }

    public bool InsertAccount(string strUserID, string strEmpPw, string strEmpType)
    {
        bool result = false;

        if (!string.IsNullOrWhiteSpace(strUserID))
            strUserID = strUserID.Trim();

        if (!string.IsNullOrWhiteSpace(strEmpPw))
            strEmpPw = DataHelper.Encrypt(strEmpPw.Trim());

        Account account = new Account()
        {
            EmpId = strUserID,
            EmpPw = strEmpPw,
            EmpType = strEmpType
        };

        using (AccountRepository repository = new AccountRepository())
        {
            result = repository.InsertAccount(account);
        }

        return result;
    }

    public int GetFilteredPage(string strEmpId, string strEmpType)
    {
        int filterPage = 0;

        using (AccountRepository repository = new AccountRepository())
        {
            filterPage = repository.GetFilteredPage(strEmpId, strEmpType);
        }

        return filterPage;
    }

    public int GetTotalPage()
    {
        int totalPage = 0;

        using (AccountRepository repository = new AccountRepository())
        {
            totalPage = repository.GetTotalPage();
        }

        return totalPage;
    }



}