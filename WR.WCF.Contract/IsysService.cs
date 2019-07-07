using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

using WR.WCF.DataContract;

namespace WR.WCF.Contract
{
    [ServiceContract]
    public interface IsysService
    {
        [OperationContract]
        UserInfoEntity Login(string loginname_, string password_);

        [OperationContract]
        List<TBUSER> GetUserList(string str);

        [OperationContract]
        TBUSER GetUser(string id);

        [OperationContract]
        List<TBROLE> GetRoleByUserId(string id);

        [OperationContract]
        List<TbRoleEntity> GetAllRoleByUserId(string userid);

        [OperationContract]
        string AddUser(string id, string userid, string pwd, string username, string telphone, string email, string remark,string review, string by);

        [OperationContract]
        string UpdateUser(string id, string userid, string pwd, string username, string telphone, string email, string remark, string review, string by);

        [OperationContract]
        string DelUser(string id, string by);

        [OperationContract]
        List<TBROLE> GetRoleList(string rolename);

        [OperationContract]
        TBROLE GetRole(string id);

        [OperationContract]
        List<TBMENU> GetMenuByRoleId(string id);

        [OperationContract]
        List<TbMenuEntity> GetAllMenuByRoleId(string roleid);

        [OperationContract]
        string AddRole(string id, string rolename, string remark, string by);

        [OperationContract]
        string UpdateRole(string id, string rolename, string remark, string by);

        [OperationContract]
        string DelRole(string id);

        [OperationContract]
        string AddUserRole(string userid, string roleids);

        [OperationContract]
        string AddRoleMenu(string role, string menuids);

        [OperationContract]
        string UpdatePwd(string userid, string oldpwd, string newpwd, string by);

        [OperationContract]
        List<CMNDICT> GetCmn(string dict);

        [OperationContract]
        List<CMNRULE> GetRule();

        [OperationContract]
        string AddRule(string ruleid, string rulename, string descr, string device, string layer);

        [OperationContract]
        string EditRule(string ruleid, string rulename, string descr, string device, string layer);

        [OperationContract]
        string DelRule(string ruleid);

        [OperationContract]
        List<CmnRuleEntity> GetRuleByUserid(string userid);

        [OperationContract]
        string EditUserRule(string userid, string ruleids);

        [OperationContract]
        List<TbMenuEntity> GetMenuByUserId(string id);

        [OperationContract]
        string AddDict(List<CMNDICT> list);

        [OperationContract]
        string UpdateDict(List<CMNDICT> list);

        [OperationContract]
        List<TBUSERLOG> GetHeartBeatInterval(int interval = 10);
    }
}