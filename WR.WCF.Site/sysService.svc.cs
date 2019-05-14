using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

using WR.DAL.EF;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using WR.Utils;

namespace WR.WCF.Site
{
    public class sysService : ServiceBase, IsysService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginname_"></param>
        /// <param name="password_"></param>
        /// <returns></returns>
        public UserInfoEntity Login(string loginname_, string password_)
        {
            try
            {
                UserInfoEntity ent = new UserInfoEntity();

                //验证服务器是否授权
                //int aut = DataCache.IsAuth;
                //if (aut != 0)
                //{
                //    ent.IsOK = aut;
                //    return ent;
                //}

                using (BFdbContext db = new BFdbContext())
                {
                    var user = db.TBUSER.FirstOrDefault(p => p.USERID.ToLower() == loginname_.ToLower() && p.PWD == password_ && p.DELFLAG == "0");
                    if (user == null)
                    {
                        ent.IsOK = -99;
                        ent.Msg = "-1";
                    }
                    else
                    {
                        ent.IsOK = 0;
                        ent.ID = user.ID;
                        ent.USERID = user.USERID;
                        ent.USERNAME = user.USERNAME;
                        ent.PASSWORD = user.PWD;
                        ent.EMAIL = user.EMAIL;
                        ent.TELEPHONE = user.TELEPHONE;
                        ent.RE_REVIEW = user.RE_REVIEW;

                        string sql = @"select distinct a.id, a.menuname, a.menucode, a.remark,a.updatedate,a.updateid,a.createdate,a.createid 
                                    from tb_menu a,tb_rolemenurelation b,tb_userrolerelation c
                                    where a.id=b.menuid and b.roleid=c.roleid and c.userid='{0}'";
                        ent.MenuList = db.SqlQuery<TBMENU>(string.Format(sql, user.ID)).ToList();

                        //提供方法执行的上下文环境
                        OperationContext context = OperationContext.Current;
                        //获取传进的消息属性
                        MessageProperties properties = context.IncomingMessageProperties;

                        RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                        TBUSERLOG log = new TBUSERLOG();
                        log.CREATEDATE = DateTime.Now;
                        log.ID = Guid.NewGuid().ToString("N");
                        log.IP = endpoint.Address;
                        log.REMARK = "";
                        log.TYPE = "0";
                        log.USERID = ent.ID;

                        db.Insert<TBUSERLOG>(log);
                    }

                    return ent;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<TBUSER> GetUserList(string str)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.TBUSER.Where(p => (string.IsNullOrEmpty(str) || p.USERID.Contains(str) || p.USERNAME.Contains(str) || p.TELEPHONE.Contains(str))
                        && p.DELFLAG == "0").ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }


        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public List<TBROLE> GetRoleByUserId(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Format(@"select distinct a.* from Tb_Role a,Tb_UserRoleRelation b
                                            where a.id=b.roleid and b.userid='{0}'", id);

                    return db.SqlQuery<TBROLE>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取全部角色列表
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public List<TbRoleEntity> GetAllRoleByUserId(string userid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (db.DatabaseType == DatabaseType.Mysql)
                    {
                        string sql = string.Format(@"select a.*,if(isnull(b.roleid),'1','0') FLG from Tb_Role a left join
                                    (select tb.roleid from Tb_Userrolerelation tb where tb.userid='{0}') b
                                    on a.id=b.roleid", userid);

                        return db.SqlQuery<TbRoleEntity>(sql).ToList();
                    }
                    else
                    {
                        string sql = string.Format(@"select a.*,nvl2(b.roleid,'0','1') FLG from Tb_Role a,
                                    (select tb.roleid from Tb_Userrolerelation tb where tb.userid='{0}') b
                                    where a.id=b.roleid(+)", userid);

                        return db.SqlQuery<TbRoleEntity>(sql).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TBUSER GetUser(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.TBUSER.Find(id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        /// <param name="username"></param>
        /// <param name="telphone"></param>
        /// <param name="email"></param>
        /// <param name="remark"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string AddUser(string id, string userid, string pwd, string username, string telphone, string email, string remark, string review, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (db.TBUSER.Any(p => p.USERID == userid && p.DELFLAG == "0"))
                    {
                        return "已经存在相同用户";
                    }

                    TBUSER user = new TBUSER();
                    user.ID = id;
                    user.USERID = userid;
                    user.USERNAME = username;
                    user.PWD = pwd;
                    user.DELFLAG = "0";
                    user.EMAIL = email;
                    user.REMARK = remark;
                    user.TELEPHONE = telphone;
                    user.CREATEDATE = DateTime.Now;
                    user.CREATEID = by;
                    user.UPDATEDATE = DateTime.Now;
                    user.UPDATEID = by;
                    user.RE_REVIEW = review;
                    return db.Insert<TBUSER>(user).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <param name="pwd"></param>
        /// <param name="username"></param>
        /// <param name="telphone"></param>
        /// <param name="email"></param>
        /// <param name="remark"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string UpdateUser(string id, string userid, string pwd, string username, string telphone, string email, string remark, string review, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (db.TBUSER.Any(p => p.USERID == userid && p.DELFLAG == "0" && p.ID != id))
                    {
                        return "已经存在相同用户";
                    }

                    TBUSER user = db.TBUSER.Find(id);
                    user.USERID = userid;
                    user.USERNAME = username;
                    user.PWD = pwd;
                    user.EMAIL = email;
                    user.REMARK = remark;
                    user.TELEPHONE = telphone;
                    user.UPDATEDATE = DateTime.Now;
                    user.UPDATEID = by;
                    user.RE_REVIEW = review;

                    return db.Update<TBUSER>(user).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string DelUser(string id, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    TBUSER user = db.TBUSER.FirstOrDefault(p => p.USERID == id && p.DELFLAG == "0");
                    user.DELFLAG = "1";
                    user.UPDATEDATE = DateTime.Now;
                    user.UPDATEID = by;

                    return db.Update<TBUSER>(user).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public List<TBROLE> GetRoleList(string rolename)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.TBROLE.Where(p => (string.IsNullOrEmpty(rolename) || p.ROLENAME.Contains(rolename))).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public List<TBMENU> GetMenuByRoleId(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Format(@"select distinct a.* from Tb_menu a,Tb_rolemenurelation b
                                            where a.id=b.menuid and b.roleid='{0}'", id);

                    return db.SqlQuery<TBMENU>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取全部菜单列表
        /// </summary>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public List<TbMenuEntity> GetAllMenuByRoleId(string roleid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (db.DatabaseType == DatabaseType.Mysql)
                    {
                        string sql = string.Format(@"select a.*,if(isnull(b.menuid),'1','0') FLG from Tb_menu a left join
                                    (select tb.menuid from Tb_rolemenurelation tb where tb.roleid='{0}') b
                                    on a.id=b.menuid", roleid);

                        return db.SqlQuery<TbMenuEntity>(sql).ToList();
                    }
                    else
                    {
                        string sql = string.Format(@"select a.*,nvl2(b.menuid,'0','1') FLG from Tb_menu a,
                                    (select tb.menuid from Tb_rolemenurelation tb where tb.roleid='{0}') b
                                    where a.id=b.menuid(+)", roleid);

                        return db.SqlQuery<TbMenuEntity>(sql).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 根据ID获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TBROLE GetRole(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.TBROLE.Find(id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolename"></param>
        /// <param name="remark"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string AddRole(string id, string rolename, string remark, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (db.TBROLE.Any(p => p.ROLENAME == rolename))
                    {
                        return "已经存在角色名称";
                    }

                    TBROLE role = new TBROLE();
                    role.ID = id;
                    role.ROLENAME = rolename;
                    role.REMARK = remark;
                    role.CREATEDATE = DateTime.Now;
                    role.CREATEID = by;
                    role.UPDATEDATE = DateTime.Now;
                    role.UPDATEID = by;

                    return db.Insert<TBROLE>(role).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rolename"></param>
        /// <param name="remark"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string UpdateRole(string id, string rolename, string remark, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (db.TBROLE.Any(p => p.ROLENAME == rolename && p.ID != id))
                    {
                        return "已经存在角色名称";
                    }

                    TBROLE role = db.TBROLE.Find(id);
                    role.ROLENAME = rolename;
                    role.REMARK = remark;
                    role.UPDATEDATE = DateTime.Now;
                    role.UPDATEID = by;

                    return db.Update<TBROLE>(role).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DelRole(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    TBROLE role = db.TBROLE.Find(id);
                    role.ID = id;
                    return db.Delete<TBROLE>(role).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 添加关系
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleids"></param>
        /// <returns></returns>
        public string AddUserRole(string userid, string roleids)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var tran = db.BeginTransaction();
                    db.ExecuteSqlCommand(string.Format("delete from tb_userrolerelation where userid='{0}'", userid));

                    //角色ID
                    var rids = roleids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var id in rids)
                    {
                        db.ExecuteSqlCommand(string.Format("insert into tb_userrolerelation(userid, roleid) values('{0}','{1}')", userid, id));
                    }
                    tran.Commit();

                    return "";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 添加关系
        /// </summary>
        /// <param name="role"></param>
        /// <param name="menuids"></param>
        /// <returns></returns>
        public string AddRoleMenu(string role, string menuids)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var tran = db.BeginTransaction();
                    db.ExecuteSqlCommand(string.Format("delete from tb_rolemenurelation where roleid='{0}'", role));

                    //角色ID
                    var rids = menuids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var id in rids)
                    {
                        db.ExecuteSqlCommand(string.Format("insert into tb_rolemenurelation(roleid, menuid) values('{0}','{1}')", role, id));
                    }
                    tran.Commit();

                    return "";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="oldpwd"></param>
        /// <param name="newpwd"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string UpdatePwd(string userid, string oldpwd, string newpwd, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var user = db.TBUSER.Find(userid);
                    if (user.PWD != oldpwd)
                        return "-1";

                    user.PWD = newpwd;
                    user.UPDATEDATE = DateTime.Now;
                    user.UPDATEID = by;
                    return db.Update<TBUSER>(user).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public List<CMNDICT> GetCmn(string dict)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.CMNDICT.Where(p => (string.IsNullOrEmpty(dict) || p.DICTID == dict)).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<CMNRULE> GetRule()
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.CMNRULE.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string AddRule(string ruleid, string rulename, string descr, string device, string layer)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    CMNRULE ent = new CMNRULE();
                    ent.DESCRP = descr;
                    ent.DEVICE = device;
                    ent.ID = ruleid;
                    ent.LAYER = layer;
                    ent.RULENAME = rulename;
                    ent.CREATEDDATE = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    return db.Insert<CMNRULE>(ent).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string EditRule(string ruleid, string rulename, string descr, string device, string layer)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var ent = db.CMNRULE.FirstOrDefault(p => p.ID == ruleid);
                    ent.DESCRP = descr;
                    ent.DEVICE = device;
                    //ent.ID = ruleid;
                    ent.LAYER = layer;
                    ent.RULENAME = rulename;
                    return db.Update<CMNRULE>(ent).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string DelRule(string ruleid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    CMNRULE ent = new CMNRULE();
                    ent.ID = ruleid;

                    return db.Delete<CMNRULE>(ent).ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<CmnRuleEntity> GetRuleByUserid(string userid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Format(@"select a.userid,a.ruleid,a.device,a.layer,b.rulename,b.descrp from cmn_relation a,cmn_rule b 
                                                where a.ruleid=b.id and a.userid='{0}'", userid);

                    return db.SqlQuery<CmnRuleEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string EditUserRule(string userid, string ruleids)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var tran = db.BeginTransaction();
                    db.ExecuteSqlCommand(string.Format("delete from cmn_relation where userid='{0}'", userid));

                    var rids = ruleids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var id in rids)
                    {
                        db.ExecuteSqlCommand(string.Format(@"insert into cmn_relation(userid, device, layer, ruleid)
                                                             select '{0}',a.device,a.layer,a.id from cmn_rule a where a.id='{1}'", userid, id));
                    }
                    tran.Commit();

                    return "";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<TbMenuEntity> GetMenuByUserId(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Format(@"select distinct a.id,a.menuname,a.menucode from tb_menu a,tb_rolemenurelation b,tb_userrolerelation c
                                                where a.id=b.menuid and b.roleid=c.roleid and lower(c.userid)='{0}'", id.ToLower());
                    return db.SqlQuery<TbMenuEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string AddDict(List<CMNDICT> dictEntity)
        {
            try
            {
                var rs = false;

                using (BFdbContext db = new BFdbContext())
                {
                    db.ExecuteSqlCommand("delete cmn_dict where dictid='3000'");

                    using (db.BeginTransaction())
                    {
                        foreach (var entity in dictEntity)
                        {
                            db.TInsert<CMNDICT>(entity);
                        }
                    }

                    db.SaveChanges();

                    rs = true;
                }

                return rs ? "0" : "-1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string UpdateDict(List<CMNDICT> dictEntity)
        {
            try
            {
                var rs = false;

                using (BFdbContext db = new BFdbContext())
                {
                    using (db.BeginTransaction())
                    {
                        foreach (var entity in dictEntity)
                        {
                            db.TUpdate<CMNDICT>(entity);
                        }
                    }

                    db.SaveChanges();

                    rs = true;
                }

                return rs ? "0" : "-1";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }
    }
}
