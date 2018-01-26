using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.ServiceModel;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;

using WR.Utils;
using WR.WCF.Contract;
using WR.WCF.DataContract;
using WR.DAL.EF;
using System.Diagnostics;

namespace WR.WCF.Site
{
    public class wrService : ServiceBase, IwrService
    {
        private static LoggerEx log = LogService.Getlog(typeof(wrService));

        private void AddLog(string methodName, string info)
        {
            log.Info(string.Format("{0} {1}.....", methodName, info));
        }

        /// <summary>
        /// 获取缺陷分类
        /// </summary>
        /// <param name="schemeid"></param>
        /// <returns></returns>
        public List<WMCLASSIFICATIONITEM> GetClassificationItem(string schemeid, string userid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.SqlQuery<WMCLASSIFICATIONITEM>(string.Format(@"select a.itemid,a.schemeid,a.id,a.name,a.description,a.priority,a.isacceptable,a.type,a.userid,
                                                                                    nvl(b.hotkey,a.hotkey) hotkey,nvl(b.color,a.color) color
                                                                              from wm_classificationitem a,
                                                                                  (select * from wm_classificationuser where userid='{1}') b 
                                                                              where a.itemid=b.itemid(+) and a.schemeid='{0}'", schemeid, userid)).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<WMCLASSIFICATIONSCHEME> GetClassificationScheme(string schemeid, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据class汇总
        /// </summary>
        /// <param name="resultid"></param>
        /// <returns></returns>
        public List<WmClassificationItemEntity> GetClassSummary(string resultid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Format("select substr(t.completiontime,0,6) from wm_waferresult t where t.resultid='{0}'", resultid);

                    var yearMonth = db.SqlQuery<string>(sql).FirstOrDefault();

                    if (!string.IsNullOrEmpty(yearMonth) && yearMonth == DateTime.Now.ToString("yyyyMM"))
                        yearMonth = string.Empty;

                    sql = string.Format(@"select ta.id, ta.description, ta.name, nvl(tb.cnt, 0) Points,'Front' inspectiontype
                                                  from (select a.schemeid, a.id, a.description, b.name,a.itemid
                                                          from wm_classificationitem a, wm_classificationscheme b,wm_waferresult c 
                                                         where a.schemeid = b.schemeid and b.schemeid=c.classificationinfoid  and c.delflag='0' 
                                                               and c.resultid='{0}') ta,       
                                                       (select a.inspclassifiid,count(a.inspclassifiid) as cnt
                                                          from wm_defectlist{1} a
                                                         where a.resultid = '{0}'
                                                         group by a.inspclassifiid) tb
                                                 where ta.itemid = tb.inspclassifiid(+) order by ta.id", resultid, yearMonth);

                    return db.SqlQuery<WmClassificationItemEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取Device、Setup、Lot
        /// 去掉用户权限设置中的对rule的选择功能，默认所有用户可以看到所有的缺陷检测结果。
        /// </summary>
        /// <param name="operatorid"></param>
        /// <returns></returns>
        public List<WmidentificationEntity> GetIdentification(string operatorid, string fromday, string today, string done)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    //a.identificationid,
                    //                    string sql = @"select distinct a.lot,a.device,a.layer,b.userid  from wm_identification a,cmn_relation b,wm_waferresult c 
                    //                                    where a.identificationid=c.identificationid and c.delflag='0' and instr(a.device||'-'||a.layer,b.device||'-'||decode(b.layer,'*','',b.layer))>0";

                    string sql = string.Empty;

                    if (!string.IsNullOrEmpty(operatorid))
                    {
                        sql = @"select distinct a.lot,a.device,a.layer,b.userid  from wm_identification a,cmn_relation b,wm_waferresult c 
                                    where a.identificationid=c.identificationid and c.delflag='0' and instr(a.device||'-'||a.layer,b.device||'-'||decode(b.layer,'*','',b.layer))>0";
                    }
                    else
                    {
                        sql = @"select distinct a.lot,a.device,a.layer  from wm_identification a,wm_waferresult c 
                                    where a.identificationid=c.identificationid and c.delflag='0'";
                    }

                    if (!string.IsNullOrEmpty(operatorid))
                        sql += string.Format(" and b.userid='{0}'", operatorid);

                    sql += string.Format(" and ((c.completiontime>={0} and c.completiontime<={1}) or {2})", fromday, today, done == "1" ? "c.ischecked in ('0','1')" : "1=2");

                    sql += " order by a.device,a.layer,a.lot";

                    return db.SqlQuery<WmidentificationEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取需要Review的Lot信息
        /// </summary>
        /// <param name="operatorid"></param>
        /// <returns></returns>
        public List<WmwaferResultEntity> GetWaferResult(string operatorid, string fromday, string today, string done)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    //                    string sql = string.Format(@"select rownum Id,a.device,a.layer,a.lot,a.substrate_slot,a.substrate_id,a.substrate_notchlocation,decode(t.SFIELD, 0, round(((d.cnt-nvl(b.defectivedie, 0))/d.cnt)*100,2), t.SFIELD) SFIELD,
                    //                                                    decode(t.numdefect,0,nvl(b.defectivedie,0),t.numdefect) NUMDEFECT,t.ischecked,t.classificationinfoid,
                    //                                                    t.computername,t.completiontime,t.checkeddate,t.createddate,'Front' filetype,t.disposition,b.defectdensity,
                    //                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid from wm_waferresult t,wm_identification a,wm_inspectioninfo b,cmn_relation c,
                    //                                                    (select ta.layoutid,count(tb.id) cnt from wm_dielayout ta,wm_dielayoutlist tb where ta.layoutid=tb.layoutid and lower(trim(tb.disposition))<>'notexist' group by ta.layoutid) d
                    //                                                    where t.identificationid=a.identificationid and t.dielayoutid=d.layoutid and t.resultid=b.resultid and instr(a.device||'-'||a.layer,c.device||'-'||decode(c.layer,'*','',c.layer))>0 
                    //                                                        and ((t.completiontime>={1} and t.completiontime<={2}) or {3}) and c.userid='{0}' and t.delflag='0' order by a.device,a.layer,a.lot,a.substrate_id",
                    //                                                    operatorid, fromday, today, done == "1" ? "t.ischecked in ('0','1')" : "1=2"); //and t.createddate> to_number(to_char(sysdate,'yyyymmddhh24miss'))

                    string sql = string.Empty;

                    if (!string.IsNullOrEmpty(operatorid))
                    {
                        sql = string.Format(@"select rownum Id,a.device,a.layer,a.lot,a.substrate_slot,a.substrate_id,a.substrate_notchlocation,t.SFIELD,
                                                    decode(t.numdefect,0,nvl(b.defectivedie,0),t.numdefect) NUMDEFECT,t.ischecked,t.classificationinfoid,
                                                    t.computername,t.completiontime,t.checkeddate,t.createddate,'Front' filetype,t.disposition,b.defectdensity,
                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid,b.recipe_id,
                                                   case when b.inspecteddie>0 and b.maska_die>0 then round((b.inspecteddie- b.maska_defect)/b.inspecteddie*100,2) else 0 end maska_die,
                                                   case when b.inspecteddie>0 and b.maskb_die>0 then round((b.inspecteddie- b.maskb_defect)/b.inspecteddie*100,2) else 0 end maskb_die,
                                                   case when b.inspecteddie>0 and b.maskc_die>0 then round((b.inspecteddie- b.maskc_defect)/b.inspecteddie*100,2) else 0 end maskc_die,
                                                   case when b.inspecteddie>0 and b.maskd_die>0 then round((b.inspecteddie- b.maskd_defect)/b.inspecteddie*100,2) else 0 end maskd_die,
                                                   case when b.inspecteddie>0 and b.maske_die>0 then round((b.inspecteddie- b.maske_defect)/b.inspecteddie*100,2) else 0 end maske_die
                                                    from wm_waferresult t,wm_identification a,wm_inspectioninfo b,cmn_relation c
                                                    where t.identificationid=a.identificationid and t.resultid=b.resultid and instr(a.device||'-'||a.layer,c.device||'-'||decode(c.layer,'*','',c.layer))>0 
                                                        and ((t.completiontime>={1} and t.completiontime<={2}) or {3}) and c.userid='{0}' and t.delflag='0' order by a.device,a.layer,a.lot,a.substrate_id",
                                                    operatorid, fromday, today, done == "1" ? "t.ischecked in ('0','1')" : "1=2"); //and t.createddate> to_number(to_char(sysdate,'yyyymmddhh24miss'))

                    }
                    else
                    {
                        sql = string.Format(@"select rownum Id,a.device,a.layer,a.lot,a.substrate_slot,a.substrate_id,a.substrate_notchlocation,t.SFIELD,
                                                    decode(t.numdefect,0,nvl(b.defectivedie,0),t.numdefect) NUMDEFECT,t.ischecked,t.classificationinfoid,
                                                    t.computername,t.completiontime,t.checkeddate,t.createddate,'Front' filetype,t.disposition,b.defectdensity,
                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid,b.recipe_id,
                                                    case when b.inspecteddie>0 and b.maska_die>0 then round((b.inspecteddie- b.maska_defect)/b.inspecteddie*100,2) else 0 end maska_die,
                                                   case when b.inspecteddie>0 and b.maskb_die>0 then round((b.inspecteddie- b.maskb_defect)/b.inspecteddie*100,2) else 0 end maskb_die,
                                                   case when b.inspecteddie>0 and b.maskc_die>0 then round((b.inspecteddie- b.maskc_defect)/b.inspecteddie*100,2) else 0 end maskc_die,
                                                   case when b.inspecteddie>0 and b.maskd_die>0 then round((b.inspecteddie- b.maskd_defect)/b.inspecteddie*100,2) else 0 end maskd_die,
                                                   case when b.inspecteddie>0 and b.maske_die>0 then round((b.inspecteddie- b.maske_defect)/b.inspecteddie*100,2) else 0 end maske_die
                                                    from wm_waferresult t,wm_identification a,wm_inspectioninfo b
                                                    where t.identificationid=a.identificationid and t.resultid=b.resultid
                                                        and ((t.completiontime>={0} and t.completiontime<={1}) or {2}) and t.delflag='0' order by a.device,a.layer,a.lot,a.substrate_id",
                                                fromday, today, done == "1" ? "t.ischecked in ('0','1')" : "1=2");
                    }

                    return db.SqlQuery<WmwaferResultEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Stream GetPic(string filename)
        {
            try
            {
                string file = Utils.PathCombine(Utils.ImgUpdatePath, filename);
                if (!File.Exists(file))
                    file = AppDomain.CurrentDomain.BaseDirectory + "default.png";

                FileStream fstream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

                var incomingRequest = WebOperationContext.Current.IncomingRequest;
                var outgoingResponse = WebOperationContext.Current.OutgoingResponse;
                long offset = 0, count = fstream.Length;

                if (incomingRequest.Headers.AllKeys.Contains("Range"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(incomingRequest.Headers["Range"], @"(?<=bytes\b*=)(\d*)-(\d*)");
                    if (match.Success)
                    {
                        outgoingResponse.StatusCode = System.Net.HttpStatusCode.PartialContent;
                        string v1 = match.Groups[1].Value;
                        string v2 = match.Groups[2].Value;
                        if (!match.NextMatch().Success)
                        {
                            if (v1 == "" && v2 != "")
                            {
                                var r2 = long.Parse(v2);
                                offset = count - r2;
                                count = r2;
                            }
                            else if (v1 != "" && v2 == "")
                            {
                                var r1 = long.Parse(v1);
                                offset = r1;
                                count -= r1;
                            }
                            else if (v1 != "" && v2 != "")
                            {
                                var r1 = long.Parse(v1);
                                var r2 = long.Parse(v2);
                                offset = r1;
                                count -= r2 - r1 + 1;
                            }
                            else
                            {
                                outgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                            }
                        }
                    }
                    outgoingResponse.ContentType = "application/force-download";
                    outgoingResponse.ContentLength = count;

                    StreamReaderEx fs = new StreamReaderEx(fstream, offset, count);
                    fs.Position = 0;
                    fs.Reading += (t) =>
                    {
                        //限速代码,实际使用时可以去掉，或者精确控制
                        //Thread.Sleep(300);
                        //Console.WriteLine();
                    };
                }

                return fstream;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取xml文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Stream GetXml(string filename)
        {
            try
            {
                //if (!File.Exists(Utils.PathCombine(Utils.ImgUpdatePath, filename)))
                //    throw new FaultException("非法请求。");

                var files = Directory.GetFiles(Utils.PathCombine(Utils.ImgUpdatePath, filename), "*.xml");
                if (files.Length == 0)
                    throw new FaultException("非法请求。");

                //FileStream fstream = new FileStream(Utils.PathCombine(Utils.ImgUpdatePath, filename), FileMode.Open, FileAccess.Read, FileShare.Read);
                FileStream fstream = new FileStream(files[0], FileMode.Open, FileAccess.Read, FileShare.Read);

                var incomingRequest = WebOperationContext.Current.IncomingRequest;
                var outgoingResponse = WebOperationContext.Current.OutgoingResponse;
                long offset = 0, count = fstream.Length;

                if (incomingRequest.Headers.AllKeys.Contains("Range"))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(incomingRequest.Headers["Range"], @"(?<=bytes\b*=)(\d*)-(\d*)");
                    if (match.Success)
                    {
                        outgoingResponse.StatusCode = System.Net.HttpStatusCode.PartialContent;
                        string v1 = match.Groups[1].Value;
                        string v2 = match.Groups[2].Value;
                        if (!match.NextMatch().Success)
                        {
                            if (v1 == "" && v2 != "")
                            {
                                var r2 = long.Parse(v2);
                                offset = count - r2;
                                count = r2;
                            }
                            else if (v1 != "" && v2 == "")
                            {
                                var r1 = long.Parse(v1);
                                offset = r1;
                                count -= r1;
                            }
                            else if (v1 != "" && v2 != "")
                            {
                                var r1 = long.Parse(v1);
                                var r2 = long.Parse(v2);
                                offset = r1;
                                count -= r2 - r1 + 1;
                            }
                            else
                            {
                                outgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                            }
                        }
                    }
                    outgoingResponse.ContentType = "application/force-download";
                    outgoingResponse.ContentLength = count;

                    StreamReaderEx fs = new StreamReaderEx(fstream, offset, count);
                    fs.Position = 0;
                }

                return fstream;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取缺陷列表
        /// </summary>
        /// <param name="resultid"></param>
        /// <returns></returns>
        public List<WmdefectlistEntity> GetDefectList(string resultid, string ischecked)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Format("select substr(t.completiontime,0,6) from wm_waferresult t where t.resultid='{0}'", resultid);

                    var yearMonth = db.SqlQuery<string>(sql).FirstOrDefault();

                    if (!string.IsNullOrEmpty(yearMonth) && yearMonth == DateTime.Now.ToString("yyyyMM"))
                        yearMonth = string.Empty;

                    string ischk = "";
                    if (!string.IsNullOrEmpty(ischecked) && ischecked == "0")
                        ischk = "and a.modifieddefect is not null";
                    else if (!string.IsNullOrEmpty(ischecked) && ischecked == "1")
                        ischk = "and a.modifieddefect is null";

                    sql = string.Format(@"select a.id,a.passid,a.inspid,a.modifieddefect,a.inspclassifiid,a.imagename,a.area_,d.color,
                                                    a.ischecked,a.checkeddate,d.name as description,a.dieaddress,b.inspectedsurface,'0' adc,d.schemeid,d.id cclassid, a.size_, a.masktype 
                                                    from wm_defectlist{2} a,wm_inspectionpass b,wm_inspectioninfo c,wm_classificationitem d 
                                                    where a.passid=b.passid and a.inspid=b.inspid and b.inspid=c.inspid and a.inspclassifiid=d.itemid and a.resultid= c.resultid
                                                    and c.resultid='{0}' {1} order by a.id", resultid, ischk, yearMonth);

                    return db.SqlQuery<WmdefectlistEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 更新快捷键
        /// </summary>
        /// <param name="hotkeys"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int UpdateClassificationItemUser(string hotkeys, string userid)
        {
            using (BFdbContext db = new BFdbContext())
            {
                var tran = db.BeginTransaction();
                try
                {
                    string[] keyArr = hotkeys.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in keyArr)
                    {
                        string[] key = item.Split(new char[] { '|' });
                        db.ExecuteSqlCommand(string.Format("delete wm_classificationuser where itemid = '{0}' and userid = '{1}'", key[0], userid));
                        db.ExecuteSqlCommand(string.Format("insert into wm_classificationuser(itemid, hotkey, color, userid) values('{0}', '{1}', '{2}', '{3}')", key[0], key[1], key[2], userid));
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    log.Error(ex);
                    throw GetFault(ex);
                }
            }
        }

        /// <summary>
        /// 更新缺陷结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="passid"></param>
        /// <param name="inspid"></param>
        /// <param name="checkedby"></param>
        /// <param name="mclassid"></param>
        /// <returns></returns>
        public WmwaferResultEntity UpdateDefect(string resultid, string checkedby, string mclassid, string finish)
        {
            AddLog("UpdateDefect", "start");

            var entity = new WmwaferResultEntity();

            StringBuilder sbt = new StringBuilder();
            using (BFdbContext db = new BFdbContext())
            {
                //                sbt.AppendFormat(@"select a.resultid,a.checkeddate,b.yieldnum,c.defectnum,a.dielayoutid from wm_waferresult a, 
                //                                 (select ba.layoutid,count(ba.id) yieldnum from wm_dielayoutlist ba where lower(trim(ba.disposition))<>'notexist' group by ba.layoutid) b,
                //                                (select ba.layoutid, count(ba.id) defectnum from wm_dielayoutlist ba where lower(trim(ba.disposition)) <> 'notexist' and ba.inspclassifiid<>'0' group by ba.layoutid) c
                //                                where a.dielayoutid=b.layoutid and a.dielayoutid = c.layoutid and a.resultid='{0}'", resultid);
                sbt.AppendFormat(@"select a.resultid,a.checkeddate,a.dielayoutid from wm_waferresult a
                                    where a.resultid='{0}' and a.ischecked <> '2'", resultid);

                var info = db.SqlQuery<WmwaferInfoEntity>(sbt.ToString()).ToList();
                if (info == null || info.Count < 1)
                {
                    AddLog("UpdateDefect", "Data does not exist or completed" + sbt.ToString());
                    //return -1;
                    entity.Id = -1;
                    return entity;
                }

                var tran = db.BeginTransaction();

                try
                {
                    string sql = string.Format("select substr(t.completiontime,0,6) from wm_waferresult t where t.resultid='{0}'", resultid);

                    var yearMonth = db.SqlQuery<string>(sql).FirstOrDefault();

                    if (!string.IsNullOrEmpty(yearMonth) && yearMonth == DateTime.Now.ToString("yyyyMM"))
                        yearMonth = string.Empty;

                    sbt.Clear();
                    //更新defect表
                    //                    sbt.AppendFormat(@"update wm_defectlist set ischecked='1',checkeddate={0:yyyyMMddHHmmss},checkedby='{1}'
                    //                                     where (passid,inspid) in (select a.passid,a.inspid from wm_inspectionpass a,wm_inspectioninfo b where a.inspid=b.inspid and b.resultid='{2}')",
                    //                                     DateTime.Now, checkedby, resultid);

                    sbt.AppendFormat(@"update wm_defectlist{3} set ischecked='1',checkeddate={0:yyyyMMddHHmmss},checkedby='{1}'
                                     where resultid='{2}'",
                                    DateTime.Now, checkedby, resultid, yearMonth);

                    db.ExecuteSqlCommand(sbt.ToString());
                    AddLog("", sbt.ToString());

                    //修改后的defect
                    if (!string.IsNullOrEmpty(mclassid))
                    {
                        var modf = mclassid.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (modf.Length > 0)
                        {
                            foreach (var item in modf)
                            {
                                var ids = item.Split(new char[] { ',' });

                                sbt.Clear();
                                sbt.AppendFormat("update wm_defectlist{4} set inspclassifiid='{3}',modifieddefect=inspclassifiid where id={0} and passid={1} and inspid='{2}'", ids[0], ids[1], ids[2], ids[3], yearMonth);
                                db.ExecuteSqlCommand(sbt.ToString());

                                sbt.Clear();
                                sbt.AppendFormat("update wm_dielayoutlist{4} t set t.inspclassifiid='{0}' where t.layoutid='{1}' and t.dieaddressx={2} and t.dieaddressy='{3}'", ids[6], info[0].dielayoutid, ids[4], ids[5], yearMonth);
                                db.ExecuteSqlCommand(sbt.ToString());
                            }
                        }

                        AddLog("", mclassid);
                    }

                    //更新masktype
                    sbt.Clear();
                    sbt.AppendFormat(@"update wm_inspectioninfo t set (t.maska_defect,t.maskb_defect,t.maskc_defect,t.maskd_defect,t.maske_defect)= 
                                    (select sum(case when d.masktype = 'A' and c.id <> 0 then 1 else 0 end) maska_defect , 
                                    sum(case when d.masktype = 'B' and c.id <> 0 then 1 else 0 end) maskb_defect , 
                                    sum(case when d.masktype = 'C' and c.id <> 0 then 1 else 0 end) maskc_defect , 
                                    sum(case when d.masktype = 'D' and c.id <> 0 then 1 else 0 end) maskd_defect , 
                                    sum(case when d.masktype = 'E' and c.id <> 0 then 1 else 0 end) maske_defect 
                                    from wm_defectlist{0} d 
                                    inner join wm_classificationitem c 
                                    on c.itemid = d.inspclassifiid 
                                    where d.inspid = t.inspid) 
                                    where exists(select 1 from wm_defectlist{0} d inner join wm_classificationitem c on c.itemid = d.inspclassifiid 
                                    where d.inspid = t.inspid) and t.resultid='{1}' ", yearMonth, resultid);
                    db.ExecuteSqlCommand(sbt.ToString());

                    //更新waferresult表
                    sbt.Clear();
                    //                    sbt.AppendFormat(@"select a.resultid,a.checkeddate,b.yieldnum,nvl(c.defectnum,0) defectnum from wm_waferresult a, 
                    //                                 (select ba.layoutid,count(ba.id) yieldnum from wm_dielayoutlist ba where lower(trim(ba.disposition))<>'notexist' group by ba.layoutid) b,
                    //                                (select ba.layoutid, count(ba.id) defectnum from wm_dielayoutlist ba where lower(trim(ba.disposition)) <> 'notexist' and ba.inspclassifiid<>'0' group by ba.layoutid) c
                    //                                where a.dielayoutid=b.layoutid and a.dielayoutid = c.layoutid(+) and a.resultid='{0}'", resultid);
                    //                    sbt.AppendFormat(@"select a.resultid,a.checkeddate,b.yieldnum,nvl(c.defectnum,0) defectnum from wm_waferresult a, 
                    //                                 (select ba.layoutid,d.rows_ * d.columns_ -count(ba.id) yieldnum from wm_dielayoutlist{1} ba inner join wm_dielayout d on d.layoutid = ba.layoutid where lower(trim(ba.disposition))='notexist' group by ba.layoutid, d.rows_, d.columns_) b,
                    //                                (select ba.layoutid, count(ba.id) defectnum from wm_dielayoutlist{1} ba where lower(trim(ba.disposition)) <> 'notexist' and ba.inspclassifiid<>'0' group by ba.layoutid) c
                    //                                where a.dielayoutid=b.layoutid and a.dielayoutid = c.layoutid(+) and a.resultid='{0}'", resultid, yearMonth);
                    sbt.AppendFormat(@"select a.resultid,a.checkeddate,b.yieldnum,nvl(c.defectnum,0) defectnum from wm_waferresult a, 
                                 (select ba.layoutid,d.rows_ * d.columns_ -count(ba.id) yieldnum from wm_dielayoutlist{1} ba inner join wm_dielayout d on d.layoutid = ba.layoutid where lower(trim(ba.disposition)) in ( 'notexist','notprocess') group by ba.layoutid, d.rows_, d.columns_) b,
                                (select ba.layoutid, count(ba.id) defectnum from wm_dielayoutlist{1} ba where lower(trim(ba.disposition)) <> 'notexist' and ba.inspclassifiid<>'0' group by ba.layoutid) c
                                where a.dielayoutid=b.layoutid and a.dielayoutid = c.layoutid(+) and a.resultid='{0}'", resultid, yearMonth);
                    info = db.SqlQuery<WmwaferInfoEntity>(sbt.ToString()).ToList();
                    var y = (info[0].yieldnum.Value * 1.0 - info[0].defectnum.Value * 1.0) / info[0].yieldnum.Value;
                    AddLog("", sbt.ToString());

                    sbt.Clear();
                    sbt.AppendFormat("update wm_waferresult set ischecked='{0}',checkeddate={1},checkedby='{2}',sfield={3},numdefect={4} where resultid='{5}'",
                        string.IsNullOrEmpty(finish) ? "0" : finish,
                        info[0].checkeddate.HasValue ? info[0].checkeddate.Value.ToString() : DateTime.Now.ToString("yyyyMMddHHmmss"),
                        checkedby,
                        Math.Round(y, 4) * 100,
                        info[0].defectnum,
                        resultid);

                    db.ExecuteSqlCommand(sbt.ToString());

                    sbt.Clear();
                    sbt.AppendFormat(@"select rownum Id,a.device,a.layer,a.lot,a.substrate_slot,a.substrate_id,a.substrate_notchlocation,t.SFIELD,
                                                    decode(t.numdefect,0,nvl(b.defectivedie,0),t.numdefect) NUMDEFECT,t.ischecked,t.classificationinfoid,
                                                    t.computername,t.completiontime,t.checkeddate,t.createddate,'Front' filetype,t.disposition,b.defectdensity,
                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid,b.recipe_id,
                                                    case when b.inspecteddie>0 and b.maska_die>0 then round((b.inspecteddie- b.maska_defect)/b.inspecteddie*100,2) else 0 end maska_die,
                                                   case when b.inspecteddie>0 and b.maskb_die>0 then round((b.inspecteddie- b.maskb_defect)/b.inspecteddie*100,2) else 0 end maskb_die,
                                                   case when b.inspecteddie>0 and b.maskc_die>0 then round((b.inspecteddie- b.maskc_defect)/b.inspecteddie*100,2) else 0 end maskc_die,
                                                   case when b.inspecteddie>0 and b.maskd_die>0 then round((b.inspecteddie- b.maskd_defect)/b.inspecteddie*100,2) else 0 end maskd_die,
                                                   case when b.inspecteddie>0 and b.maske_die>0 then round((b.inspecteddie- b.maske_defect)/b.inspecteddie*100,2) else 0 end maske_die
                                                    from wm_waferresult t,wm_identification a,wm_inspectioninfo b
                                                    where t.identificationid=a.identificationid and t.resultid=b.resultid
                                                    and t.resultid='{0}'", resultid);

                    entity = db.SqlQuery<WmwaferResultEntity>(sbt.ToString()).FirstOrDefault();

                    tran.Commit();

                    AddLog("", sbt.ToString());
                    AddLog("UpdateDefect", "end");

                    return entity;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    log.Error(ex);
                    throw GetFault(ex);
                }
            }
        }

        /// <summary>
        /// 逻辑删除wafer
        /// </summary>
        /// <param name="waferid"></param>
        /// <returns></returns>
        public int DeleteWafer(string waferid, string delby)
        {
            StringBuilder sbt = new StringBuilder();
            using (BFdbContext db = new BFdbContext())
            {
                sbt.AppendFormat("update wm_waferresult set delflag='1',delby='{1}' where resultid='{0}'", waferid, delby);
                return db.ExecuteSqlCommand(sbt.ToString());
            }
        }

        /// <summary>
        /// 更新Wafer状态
        /// </summary>
        /// <param name="waferid"></param>
        /// <param name="reby"></param>
        /// <returns></returns>
        public int UpdateWaferStatus(string waferid, string status, string reby)
        {
            StringBuilder sbt = new StringBuilder();
            using (BFdbContext db = new BFdbContext())
            {
                sbt.AppendFormat("update wm_waferresult set ischecked='{0}' where resultid='{1}'", status, waferid);
                return db.ExecuteSqlCommand(sbt.ToString());
            }
        }

        /// <summary>
        /// 根据id获取result
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WMWAFERRESULT GetWaferResultById(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.WMWAFERRESULT.FirstOrDefault(p => p.RESULTID == id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 或者DieLayout信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WMDIELAYOUT GetDielayoutById(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.WMDIELAYOUT.FirstOrDefault(p => p.LAYOUTID == id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取DieLayout列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WmdielayoutlistEntitiy> GetDielayoutListById(string id)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    //                    string sql = string.Format(@"select t.dieaddressx,t.dieaddressy,t.inspclassifiid,t.isinspectable,t.disposition,a.columns_,a.rows_ from wm_dielayoutlist t,wm_dielayout a
                    //                                                    where t.layoutid=a.layoutid and t.layoutid='{0}' and t.disposition in ('NotProcess     ','NotInspectable ','Fail           ','Pass           ','Processed      ')", id);

                    string sql = string.Format("select substr(t.completiontime,0,6) from wm_waferresult t where t.dielayoutid='{0}'", id);

                    var date = db.SqlQuery<string>(sql).FirstOrDefault();

                    if (!string.IsNullOrEmpty(date) && date != DateTime.Now.ToString("yyyyMM"))
                    {
                        sql = string.Format(@"select t.dieaddressx,t.dieaddressy,t.inspclassifiid,t.isinspectable,t.disposition,a.columns_,a.rows_ from wm_dielayoutlist{1} t,wm_dielayout a
                                                    where t.layoutid=a.layoutid and t.layoutid='{0}'", id, date);
                    }
                    else
                    {
                        sql = string.Format(@"select t.dieaddressx,t.dieaddressy,t.inspclassifiid,t.isinspectable,t.disposition,a.columns_,a.rows_ from wm_dielayoutlist t,wm_dielayout a
                                                    where t.layoutid=a.layoutid and t.layoutid='{0}'", id);
                    }

                    return db.SqlQuery<WmdielayoutlistEntitiy>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取密度报表
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<WmdensityReport> GetDensityReport(string lot, string stDate, string edDate, string isFilter)
        {
            using (BFdbContext db = new BFdbContext())
            {
                AddLog("GetDensityReport", "start");

                var dateList = GetCompletiontime(stDate, edDate);
                var list = new List<WmdensityReport>();
                var currentYearMonth = DateTime.Now.ToString("yyyyMM");
                var isDistinct = isFilter.Equals("1") ? "distinct" : "";

                foreach (var yearMonth in dateList)
                {
                    var tablename = yearMonth;

                    if (yearMonth == currentYearMonth)
                        tablename = string.Empty;

                    string sql = string.Format(@"select {4} b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,1 cnt,th.defectdensity,c.completiontime,a.DIEADDRESS 
                                                from wm_defectlist{3} a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_inspectionpass th,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.passid=th.passid and a.inspid=th.inspid and th.inspid=c.inspid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'|'||b.lot||'|','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate, tablename, isDistinct);
                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select {4} b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,1 cnt,th.defectdensity,c.completiontime,a.DIEADDRESS 
                                                from wm_defectlist{3} a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_inspectionpass th,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.passid=th.passid and a.inspid=th.inspid and th.inspid=c.inspid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|||','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate, tablename, isDistinct);
                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select {4} b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,1 cnt,th.defectdensity,c.completiontime,a.DIEADDRESS 
                                                from wm_defectlist{3} a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_inspectionpass th,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.passid=th.passid and a.inspid=th.inspid and th.inspid=c.inspid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'||','{0}')>0 and te.delflag='0'and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate, tablename, isDistinct);

                    //return db.SqlQuery<WmdensityReport>(sql).ToList();
                    list.AddRange(db.SqlQuery<WmdensityReport>(sql).ToList());

                    AddLog("GetDensityReport end", sql);
                }

                return list;
            }
        }

        /// <summary>
        /// 获取检测总数
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int[] GetCountInspected(string lot, string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {
                var dateList = GetCompletiontime(stDate, edDate);
                var lst = new List<WmdensityReport>();
                var currentYearMonth = DateTime.Now.ToString("yyyyMM");

                var count = 0;
                var wafercnt = 0;

                foreach (var yearMonth in dateList)
                {
                    var tablename = yearMonth;

                    if (yearMonth == currentYearMonth)
                        tablename = string.Empty;

                    string sql = string.Format(@"select sum(b.inspecteddie) cnt,count(distinct c.resultid) wafercnt 
                                            from wm_inspectioninfo b,wm_waferresult c,wm_identification d
                                            where b.resultid=c.resultid and c.identificationid=d.identificationid
                                                and instr(d.device||'|'||d.layer||'|'||d.lot||'|','{0}')>0 and c.delflag='0' and b.completiontime>={1} and b.completiontime<={2}", lot, stDate, edDate, tablename);
                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select sum(b.inspecteddie) cnt,count(distinct c.resultid) wafercnt 
                                            from wm_inspectioninfo b,wm_waferresult c,wm_identification d
                                            where b.resultid=c.resultid and c.identificationid=d.identificationid
                                                and instr(d.device||'|||','{0}')>0 and c.delflag='0' and b.completiontime>={1} and b.completiontime<={2}", lot, stDate, edDate, tablename);
                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select sum(b.inspecteddie) cnt,count(distinct c.resultid) wafercnt 
                                            from wm_inspectioninfo b,wm_waferresult c,wm_identification d
                                            where b.resultid=c.resultid and c.identificationid=d.identificationid
                                                and instr(d.device||'|'||d.layer||'||','{0}')>0 and c.delflag='0' and b.completiontime>={1} and b.completiontime<={2}", lot, stDate, edDate, tablename);

                    //lst.AddRange(db.SqlQuery<WmdensityReport>(sql).ToList());

                    if (lst != null && lst.Count > 0)
                    {
                        count += lst[0].CNT;
                        wafercnt += lst[0].WAFERCNT;
                    }
                }

                //if (lst != null && lst.Count > 0)
                //    return new int[] { lst[0].CNT, lst[0].WAFERCNT };
                //else
                //    return new int[] { 0, 0 };

                return new int[] { count, wafercnt };
            }
        }

        /// <summary>
        /// 获取缺陷分类汇总报表
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<WmCategoryReport> GetCategoryReport(string lot, string stDate, string edDate, string isFilter)
        {
            using (BFdbContext db = new BFdbContext())
            {
                var dateList = GetCompletiontime(stDate, edDate);
                var list = new List<WmCategoryReport>();
                var currentYearMonth = DateTime.Now.ToString("yyyyMM");
                var isDistinct = isFilter.Equals("1") ? "distinct" : "";

                foreach (var yearMonth in dateList)
                {
                    var tablename = yearMonth;

                    if (yearMonth == currentYearMonth)
                        tablename = string.Empty;

                    string sql = string.Format(@"select {4} c.device,c.lot,c.layer,b.completiontime,c.substrate_id,a.id,a.name description,b.numdefect defectnum
                                                from wm_classificationitem a,wm_waferresult b,wm_identification c
                                            where b.identificationid=c.identificationid
                                            and instr(c.device||'|'||c.layer||'|'||c.lot||'|','{0}')>0 and b.delflag='0' and b.completiontime>={1} and b.completiontime<={2} order by c.substrate_id,a.id", lot, stDate, edDate, tablename, isDistinct);

                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select {4} c.device,c.lot,c.layer,b.completiontime,c.substrate_id,a.id,a.name description,b.numdefect defectnum
                                                from wm_classificationitem a,wm_waferresult b,wm_identification c
                                            where b.identificationid=c.identificationid
                                            and instr(c.device||'|||','{0}')>0 and b.delflag='0' and b.completiontime>={1} and b.completiontime<={2} order by c.substrate_id,a.id", lot, stDate, edDate, tablename, isDistinct);

                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select {4} c.device,c.lot,c.layer,b.completiontime,c.substrate_id,a.id,a.name description,b.numdefect defectnum
                                                from wm_classificationitem a,wm_waferresult b,wm_identification c
                                            where b.identificationid=c.identificationid
                                            and instr(c.device||'|'||c.layer||'||','{0}')>0 and b.delflag='0' and b.completiontime>={1} and b.completiontime<={2} order by c.substrate_id,a.id", lot, stDate, edDate, tablename, isDistinct);

                    list.AddRange(db.SqlQuery<WmCategoryReport>(sql).ToList());
                }

                return list;
            }
        }

        /// <summary>
        /// 获取缺陷的die
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<WmDefectiveDieReport> GetDefectiveDieReport(string lot, string stDate, string edDate, string isFilter)
        {
            using (BFdbContext db = new BFdbContext())
            {
                var dateList = GetCompletiontime(stDate, edDate);
                var list = new List<WmDefectiveDieReport>();
                var currentYearMonth = DateTime.Now.ToString("yyyyMM");
                var isDistinct = isFilter.Equals("1") ? "distinct" : "";

                foreach (var yearMonth in dateList)
                {
                    var tablename = yearMonth;

                    if (yearMonth == currentYearMonth)
                        tablename = string.Empty;

                    string sql = string.Format(@"select {4} b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,c.completiontime,a.area_,a.size_,a.DIEADDRESS 
                                                from wm_defectlist{3} a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.resultid=c.resultid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'|'||b.lot||'|','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate, tablename, isDistinct);

                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select {4} b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,c.completiontime,a.area_,a.size_,a.DIEADDRESS 
                                                from wm_defectlist{3} a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.resultid=c.resultid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|||','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate, tablename, isDistinct);

                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select {4} b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,c.completiontime,a.area_,a.size_,a.DIEADDRESS 
                                                from wm_defectlist{3} a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.resultid=c.resultid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'||','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate, tablename, isDistinct);

                    list.AddRange(db.SqlQuery<WmDefectiveDieReport>(sql).ToList());
                }

                return list;
            }
        }

        private List<string> GetCompletiontime(string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {
                string sql = string.Format(@"select distinct substr(t.completiontime,0,6) from wm_waferresult t where t.completiontime>={0} and t.completiontime<={1} group by t.completiontime", stDate, edDate);

                return db.SqlQuery<string>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取检测后的die
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<WmInpDieReport> GetDieInspDieReport(string lot, string stDate, string edDate, string isFilter)
        {
            using (BFdbContext db = new BFdbContext())
            {
                //                string sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.dieqty
                //                                                from wm_waferresult ta,wm_identification tb,
                //                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                //                                                (select c.resultid,b.rows_ * b.columns_ -count(a.id) dieqty 
                //                                                        from wm_dielayoutlist a,wm_dielayout b,wm_waferresult c
                //                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))='notexist' group by c.resultid ,b.rows_, b.columns_) td
                //                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                //                                                        and instr(tb.device||'|'||tb.layer||'|'||tb.lot||'|','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate);

                //                if (lot.EndsWith("|||"))
                //                    sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.dieqty
                //                                                from wm_waferresult ta,wm_identification tb,
                //                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                //                                                (select c.resultid,b.rows_ * b.columns_ -count(a.id) dieqty 
                //                                                        from wm_dielayoutlist a,wm_dielayout b,wm_waferresult c
                //                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))='notexist' group by c.resultid, b.rows_, b.columns_) td
                //                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                //                                                        and instr(tb.device||'|||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate);

                //                else if (lot.EndsWith("||"))
                //                    sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.dieqty
                //                                                from wm_waferresult ta,wm_identification tb,
                //                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                //                                                (select c.resultid,b.rows_ * b.columns_ -count(a.id) dieqty 
                //                                                        from wm_dielayoutlist a,wm_dielayout b,wm_waferresult c
                //                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))='notexist' group by c.resultid, b.rows_, b.columns_) td
                //                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                //                                                        and instr(tb.device||'|'||tb.layer||'||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate);

                //                return db.SqlQuery<WmInpDieReport>(sql).ToList();

                var dateList = GetCompletiontime(stDate, edDate);
                var list = new List<WmInpDieReport>();
                var currentYearMonth = DateTime.Now.ToString("yyyyMM");
                var isDistinct = isFilter.Equals("1") ? "distinct" : "";

                foreach (var yearMonth in dateList)
                {
                    var tablename = yearMonth;

                    if (yearMonth == currentYearMonth)
                        tablename = string.Empty;

                    string sql = string.Format(@"select {4} tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,ta.numdefect defectnum,td.dieqty
                                                from wm_waferresult ta,wm_identification tb,
                                                (select c.resultid,b.rows_ * b.columns_ -count(a.id) dieqty 
                                                        from wm_dielayoutlist{3} a,wm_dielayout b,wm_waferresult c
                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))='notexist' group by c.resultid ,b.rows_, b.columns_) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'|'||tb.lot||'|','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate, tablename, isDistinct);

                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select {4} tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,ta.numdefect defectnum,td.dieqty
                                                from wm_waferresult ta,wm_identification tb,
                                                (select c.resultid,b.rows_ * b.columns_ -count(a.id) dieqty 
                                                        from wm_dielayoutlist{3} a,wm_dielayout b,wm_waferresult c
                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))='notexist' group by c.resultid, b.rows_, b.columns_) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate, tablename, isDistinct);

                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select {4} tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,ta.numdefect defectnum,td.dieqty
                                                from wm_waferresult ta,wm_identification tb,
                                                (select c.resultid,b.rows_ * b.columns_ -count(a.id) dieqty 
                                                        from wm_dielayoutlist{3} a,wm_dielayout b,wm_waferresult c
                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))='notexist' group by c.resultid, b.rows_, b.columns_) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate, tablename, isDistinct);

                    list.AddRange(db.SqlQuery<WmInpDieReport>(sql).ToList());
                }

                return list;
            }
        }

        public List<WmDefectListReport> GetDefectListReport(string lot, string stDate, string edDate, string isFilter)
        {
            using (BFdbContext db = new BFdbContext())
            {
                AddLog("GetDefectListReport", "start");

                var dateList = GetCompletiontime(stDate, edDate);
                var list = new List<WmDefectListReport>();
                var currentYearMonth = DateTime.Now.ToString("yyyyMM");
                var isDistinct = isFilter.Equals("1") ? "distinct" : "";

                foreach (var yearMonth in dateList)
                {
                    var tablename = yearMonth;

                    if (yearMonth == currentYearMonth)
                        tablename = string.Empty;

                    string sql = string.Format(@"select {4} d.lot,d.device,d.layer,d.substrate_id,d.substrate_slot,a.id defectnumber,b.id,b.name description,
                                            a.dieaddress,a.area_,a.size_,c.completiontime,a.DIEADDRESS 
                                            from wm_defectlist{3} a,wm_classificationitem b,wm_waferresult c,wm_identification d
                                            where a.inspclassifiid=b.itemid and a.resultid=c.resultid and c.identificationid=d.identificationid 
                                                and instr(d.device||'|'||d.layer||'|'||d.lot||'|','{0}')>0 and c.delflag='0' and c.completiontime>={1} and c.completiontime<={2} order by d.substrate_id,a.id", lot, stDate, edDate, tablename, isDistinct);

                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select {4} d.lot,d.device,d.layer,d.substrate_id,d.substrate_slot,a.id defectnumber,b.id,b.name description,
                                            a.dieaddress,a.area_,a.size_,c.completiontime,a.DIEADDRESS 
                                            from wm_defectlist{3} a,wm_classificationitem b,wm_waferresult c,wm_identification d
                                            where a.inspclassifiid=b.itemid and a.resultid=c.resultid and c.identificationid=d.identificationid 
                                                and instr(d.device||'|||','{0}')>0 and c.delflag='0' and c.completiontime>={1} and c.completiontime<={2} order by d.substrate_id,a.id", lot, stDate, edDate, tablename, isDistinct);

                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select {4} d.lot,d.device,d.layer,d.substrate_id,d.substrate_slot,a.id defectnumber,b.id,b.name description,
                                            a.dieaddress,a.area_,a.size_,c.completiontime,a.DIEADDRESS 
                                            from wm_defectlist{3} a,wm_classificationitem b,wm_waferresult c,wm_identification d
                                            where a.inspclassifiid=b.itemid and a.resultid=c.resultid and c.identificationid=d.identificationid 
                                                and instr(d.device||'|'||d.layer||'||','{0}')>0 and c.delflag='0' and c.completiontime>={1} and c.completiontime<={2} order by d.substrate_id,a.id", lot, stDate, edDate, tablename, isDistinct);

                    //return db.SqlQuery<WmDefectListReport>(sql).ToList();
                    list.AddRange(db.SqlQuery<WmDefectListReport>(sql).ToList());
                    AddLog("GetDefectListReport end", sql);
                }

                return list;
            }
        }

        public List<WmGoodDieReport> GetGoodDieReport(string lot, string stDate, string edDate, string isFilter)
        {
            using (BFdbContext db = new BFdbContext())
            {
                AddLog("GetGoodDieReport", "start");

                var dateList = GetCompletiontime(stDate, edDate);
                var list = new List<WmGoodDieReport>();
                var currentYearMonth = DateTime.Now.ToString("yyyyMM");
                var isDistinct = isFilter.Equals("1") ? "distinct" : "";

                foreach (var yearMonth in dateList)
                {
                    var tablename = yearMonth;

                    if (yearMonth == currentYearMonth)
                        tablename = string.Empty;

                    string sql = string.Format(@"select {4} tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,ta.numdefect defectnum,td.inspecteddie inspcnt,ta.resultid
                                                from wm_waferresult ta,wm_identification tb,wm_inspectioninfo td
                                                where ta.identificationid=tb.identificationid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'|'||tb.lot||'|','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2}  order by tb.substrate_id", lot, stDate, edDate, tablename, isDistinct);

                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select {4} tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,ta.numdefect defectnum,td.inspecteddie inspcnt,ta.resultid
                                                from wm_waferresult ta,wm_identification tb,wm_inspectioninfo td
                                                where ta.identificationid=tb.identificationid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate, tablename, isDistinct);

                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select {4} tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,ta.numdefect defectnum,td.inspecteddie inspcnt,ta.resultid
                                                from wm_waferresult ta,wm_identification tb,wm_inspectioninfo td
                                                where ta.identificationid=tb.identificationid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2}  order by tb.substrate_id", lot, stDate, edDate, tablename, isDistinct);

                    //return db.SqlQuery<WmGoodDieReport>(sql).ToList();
                    list.AddRange(db.SqlQuery<WmGoodDieReport>(sql).ToList());

                    AddLog("GetGoodDieReport end", sql);
                }

                //list = (from l in list
                //        group l by new { l.LOT, l.DEVICE, l.LAYER, l.SUBSTRATE_ID, l.SUBSTRATE_SLOT, l.COMPLETIONTIME, l.RESULTID } into tmp
                //        select new WmGoodDieReport
                //        {
                //            LOT = tmp.Key.LOT,
                //            DEVICE = tmp.Key.DEVICE,
                //            LAYER = tmp.Key.LAYER,
                //            SUBSTRATE_ID = tmp.Key.SUBSTRATE_ID,
                //            SUBSTRATE_SLOT = tmp.Key.SUBSTRATE_SLOT,
                //            COMPLETIONTIME = tmp.Key.COMPLETIONTIME,
                //            RESULTID = tmp.Key.RESULTID,
                //            DEFECTNUM = tmp.Sum(t => t.DEFECTNUM),
                //            INSPCNT = tmp.Sum(t => t.INSPCNT)
                //        }).ToList();

                return list;
            }
        }

        public List<WMCLASSIFICATIONITEM> GetClassificationItemsByLayer(string lot, string stDate, string edDate)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (lot.EndsWith("|||"))
                    {
                        return db.SqlQuery<WMCLASSIFICATIONITEM>(string.Format(@"select a.*
                                                                              from wm_classificationitem a
                                                                             where a.schemeid in (select ta.classificationinfoid
                                                                                                    from wm_waferresult ta, wm_identification tb
                                                                                                   where instr(tb.device || '|||', '{0}') > 0  and ta.identificationid=tb.identificationid
                                                                                                     and rownum < 2)", lot)).ToList();
                    }
                    else if (lot.EndsWith("||"))
                    {
                        return db.SqlQuery<WMCLASSIFICATIONITEM>(string.Format(@"select a.*
                                                                              from wm_classificationitem a
                                                                             where a.schemeid in (select ta.classificationinfoid
                                                                                                    from wm_waferresult ta, wm_identification tb 
                                                                                                   where instr(tb.device ||'|'||tb.layer||'||', '{0}') > 0  and ta.identificationid=tb.identificationid
                                                                                                     and rownum < 2)", lot)).ToList();
                    }
                    else
                    {
                        return db.SqlQuery<WMCLASSIFICATIONITEM>(string.Format(@"select a.*
                                                                              from wm_classificationitem a
                                                                             where a.schemeid in (select ta.classificationinfoid
                                                                                                    from wm_waferresult ta, wm_identification tb 
                                                                                                   where instr(tb.device ||'|'||tb.layer||'|'||tb.lot||'|', '{0}') > 0  and ta.identificationid=tb.identificationid
                                                                                                     and rownum < 2)", lot)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<WMCLASSIFICATIONITEM> GetClassificationItemsByLot(string lot, string stDate, string edDate)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.SqlQuery<WMCLASSIFICATIONITEM>(string.Format(@"select a.*
                                                                              from wm_classificationitem a
                                                                             where a.schemeid in (select ta.classificationinfoid
                                                                                                    from wm_waferresult ta, wm_identification tb
                                                                                                   where instr(tb.device||'|'||tb.layer||'|'||tb.lot||'|','{0}')>0  and ta.identificationid=tb.identificationid
                                                                                                     and rownum < 2)", lot)).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<WmItemsSummaryEntity> GetItemsSummaryByLot(string lot, string stDate, string edDate)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = "";

                    var dateList = GetCompletiontime(stDate, edDate);
                    var list = new List<WmItemsSummaryEntity>();
                    var currentYearMonth = DateTime.Now.ToString("yyyyMM");

                    foreach (var yearMonth in dateList)
                    {
                        var tablename = yearMonth;

                        if (yearMonth == currentYearMonth)
                            tablename = string.Empty;

                        if (lot.EndsWith("|||"))
                            sql = string.Format(@"select c.device, c.layer,c.lot, a.inspclassifiid, c.substrate_id,count(a.id) NumCnt, a.resultid
                                                  from wm_defectlist{3}       a,
                                                       wm_waferresult      b,
                                                       wm_identification   c
                                                 where a.resultid = b.resultid
                                                   and b.identificationid = c.identificationid
                                                   and b.delflag='0' and instr(c.device||'|||','{0}')>0 and b.completiontime>={1} and b.completiontime<={2} 
                                                 group by c.device, c.layer,c.lot,c.substrate_id, a.inspclassifiid, a.resultid", lot, stDate, edDate, tablename);
                        else if (lot.EndsWith("||"))
                            sql = string.Format(@"select c.device, c.layer, c.lot,a.inspclassifiid, c.substrate_id,count(a.id) NumCnt, a.resultid
                                                  from wm_defectlist{3}       a,
                                                       wm_waferresult      b,
                                                       wm_identification   c
                                                 where a.resultid = b.resultid
                                                   and b.identificationid = c.identificationid
                                                   and b.delflag='0' and instr(c.device||'|'||c.layer||'||','{0}')>0 and b.completiontime>={1} and b.completiontime<={2} 
                                                 group by c.device, c.layer,c.lot,c.substrate_id, a.inspclassifiid, a.resultid", lot, stDate, edDate, tablename);
                        else
                            sql = string.Format(@"select c.device, c.layer,c.lot, a.inspclassifiid,c.substrate_id, count(a.id) NumCnt, a.resultid
                                                  from wm_defectlist{3}       a,
                                                       wm_waferresult      b,
                                                       wm_identification   c
                                                 where a.resultid = b.resultid
                                                   and b.identificationid = c.identificationid
                                                   and b.delflag='0' and instr(c.device||'|'||c.layer||'|'||c.lot||'|','{0}')>0 and b.completiontime>={1} and b.completiontime<={2} 
                                                 group by c.device, c.layer,c.lot, c.substrate_id,a.inspclassifiid, a.resultid", lot, stDate, edDate, tablename);

                        //log.Error(sql);
                        list.AddRange(db.SqlQuery<WmItemsSummaryEntity>(sql).ToList());
                    }

                    list = (from l in list
                            group l by new { l.Device, l.Layer, l.Lot, l.Inspclassifiid, l.Substrate_id, l.RESULTID } into tmp
                            select new WmItemsSummaryEntity
                            {
                                Device = tmp.Key.Device,
                                Layer = tmp.Key.Layer,
                                Lot = tmp.Key.Lot,
                                Inspclassifiid = tmp.Key.Inspclassifiid,
                                Substrate_id = tmp.Key.Substrate_id,
                                RESULTID = tmp.Key.RESULTID,
                                NumCnt = tmp.Sum(t => t.NumCnt)
                            }).ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<WmItemsSummaryEntity> GetDefectSummaryByLot(string lot, string stDate, string edDate)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = "";

                    var dateList = GetCompletiontime(stDate, edDate);
                    var list = new List<WmItemsSummaryEntity>();
                    var currentYearMonth = DateTime.Now.ToString("yyyyMM");

                    foreach (var yearMonth in dateList)
                    {
                        var tablename = yearMonth;

                        if (yearMonth == currentYearMonth)
                            tablename = string.Empty;

                        if (lot.EndsWith("|||"))
                            sql = string.Format(@"select d.device, d.layer, count(a.inspecteddieid) NumCnt
                                               from wm_inspecteddielist{3} a,
                                                    wm_inspectioninfo   b,
                                                    wm_waferresult      c,
                                                    wm_identification   d
                                              where a.inspid = b.inspid
                                                and b.resultid = c.resultid
                                                and c.identificationid = d.identificationid
                                                and c.delflag = '0' and instr(d.device||'|||','{0}')>0  and c.completiontime>={1} and c.completiontime<={2} 
                                              group by d.device, d.layer", lot, stDate, edDate, tablename);
                        else if (lot.EndsWith("||"))
                            sql = string.Format(@"select d.device, d.layer, count(a.inspecteddieid) NumCnt
                                               from wm_inspecteddielist{3} a,
                                                    wm_inspectioninfo   b,
                                                    wm_waferresult      c,
                                                    wm_identification   d
                                              where a.inspid = b.inspid
                                                and b.resultid = c.resultid
                                                and c.identificationid = d.identificationid
                                                and c.delflag = '0' and instr(d.device||'|'||d.layer||'||','{0}')>0  and c.completiontime>={1} and c.completiontime<={2} 
                                              group by d.device, d.layer", lot, stDate, edDate, tablename);
                        else
                            sql = string.Format(@"select d.device, d.layer,d.lot,d.substrate_id, count(a.inspecteddieid) NumCnt
                                               from wm_inspecteddielist{3} a,
                                                    wm_inspectioninfo   b,
                                                    wm_waferresult      c,
                                                    wm_identification   d
                                              where a.inspid = b.inspid
                                                and b.resultid = c.resultid
                                                and c.identificationid = d.identificationid
                                                and c.delflag = '0' and instr(d.device||'|'||d.layer||'|'||d.lot||'|','{0}')>0  and c.completiontime>={1} and c.completiontime<={2}
                                              group by d.device, d.layer,d.lot,d.substrate_id", lot, stDate, edDate, tablename);

                        list.AddRange(db.SqlQuery<WmItemsSummaryEntity>(sql).ToList());
                    }

                    list = (from l in list
                            group l by new { l.Device, l.Layer } into tmp
                            select new WmItemsSummaryEntity
                            {
                                Device = tmp.Key.Device,
                                Layer = tmp.Key.Layer,
                                NumCnt = tmp.Sum(t => t.NumCnt)
                            }).ToList();

                    return list;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 获取基础的分类
        /// </summary>
        /// <returns></returns>
        public List<WmClassificationItemEntity> GetBaseClassificationItem()
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = "select t.id,t.name from wm_classificationitem t group by t.id,t.name order by t.id";

                    return db.SqlQuery<WmClassificationItemEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string DataArchive(string sdate, string edate, string type)
        {
            try
            {
                StringBuilder sbt = new StringBuilder();
                using (BFdbContext db = new BFdbContext())
                {
                    BFParameters param = BFParameters.CreateParameters();
                    param.Add(":sdate_", sdate, DbType.String, ParameterDirection.Input);
                    param.Add(":edate_", edate, DbType.String, ParameterDirection.Input);
                    param.Add(":type_", type, DbType.String, ParameterDirection.Input);

                    param.Add(":o_errcode", System.DBNull.Value, DbType.String, ParameterDirection.Output, 10);
                    param.Add(":o_message", System.DBNull.Value, DbType.String, ParameterDirection.Output, 100);

                    //return db.ExecuteProcedure<MessageEntity>("sp_date_archive", param);
                    var rs = db.ExecuteProcedure("sp_date_archive", param);

                    log.Info(string.Format("sdate_:{0} edate_:{1} type_:{2} o_errcode:{3} o_message:{4}", sdate, edate, type, param.DbParameters[3].Value, param.DbParameters[4].Value));

                    return param.DbParameters[4].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        /// <summary>
        /// 数据库的备份和恢复
        /// </summary>
        /// <param name="type">0:备份 1:恢复</param>
        /// <returns></returns>
        public int ImpOrExpDatabase(string type, string date = null)
        {
            if (type == "0")
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBFile", System.DateTime.Today.ToString("yyyyMMdd"));

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //var currentTime = System.DateTime.Now.ToString("yyyyMMdd");

                //创建一个进程实例
                Process p = new Process();
                //生成备份文件的文件名称
                string filename = string.Format("{0}\\{1}.dmp", path, System.DateTime.Today.ToString("yyyyMMdd"));
                string logname = string.Format("{0}\\{1}_exp.log", path, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

                try
                {
                    //导出程序路径
                    p.StartInfo.FileName = "C:\\app\\yang.luo\\product\\11.2.0\\dbhome_2\\BIN\\exp.exe";
                    //启用操作系统外壳程序执行
                    p.StartInfo.UseShellExecute = true;
                    //显示dos窗口执行过程
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.Arguments = "idmp/idmp@its file=" + filename + " TABLES=(acc_cell_bak) log=" + logname;
                    p.Start();
                    p.WaitForExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw GetFault(ex);
                }
                finally
                {
                    p.Dispose();

                    log.Info(string.Format("type:{0} filename:{1} logname:{2}", type, filename, logname));
                }
            }
            else
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBFile", date.Substring(0, 8));
                var filename = string.Format("{0}\\{1}.dmp", path, date);
                var logname = string.Format("{0}\\{1}_imp.log", path, System.DateTime.Now.ToString("yyyyMMddHHmmss"));

                if (!File.Exists(filename))
                    return -2;

                //创建一个进程实例
                Process p = new Process();

                try
                {
                    //导出程序路径
                    p.StartInfo.FileName = "C:\\app\\yang.luo\\product\\11.2.0\\dbhome_2\\BIN\\imp.exe";
                    //启用操作系统外壳程序执行
                    p.StartInfo.UseShellExecute = true;
                    //显示dos窗口执行过程
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.Arguments = "idmp/idmp@its file=" + filename + " TABLES=(acc_cell_bak) ignore=y log=" + logname;
                    p.Start();
                    p.WaitForExit();
                    p.Dispose();

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw GetFault(ex);
                }
                finally
                {
                    p.Dispose();
                    log.Info(string.Format("type:{0} filename:{1} logname:{2}", type, filename, logname));
                }
            }

            return 0;
        }

        public List<string> GetDBFilesList()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBFile");

                DirectoryInfo dirInfo = new DirectoryInfo(path);

                return dirInfo.GetFiles("*.dmp", SearchOption.AllDirectories).Select(s => s.Name.Split('.')[0]).ToList();
                //var pathlist= System.IO.Directory.GetFiles(path, "*.dmp",SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<DiskInfoEntity> GetDiskList()
        {
            var list = new List<DiskInfoEntity>();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                list.Add(new DiskInfoEntity()
                {
                    Name = drive.Name,

                    TotalSize = Convert.ToDouble(drive.TotalSize / (1024 * 1024 * 1024)),
                    FreeSpace = Convert.ToDouble(drive.TotalFreeSpace / (1024 * 1024 * 1024)),
                    UsedSpace = Convert.ToDouble(drive.TotalSize / (1024 * 1024 * 1024)) - Convert.ToDouble(drive.TotalFreeSpace / (1024 * 1024 * 1024))
                });
            }

            return list;
        }

        public bool AddTableSpace(string name)
        {
            using (BFdbContext db = new BFdbContext())
            {
                try
                {
                    var sql = string.Empty;

                    if (!string.IsNullOrEmpty(name))
                    {
                        sql = string.Format("select file_name from dba_data_files t where t.TABLESPACE_NAME='{0}' order by t.FILE_ID desc", name);
                    }
                    else
                    {
                        name = "USERS";
                        sql = "select file_name from dba_data_files t where t.TABLESPACE_NAME='USERS' order by t.FILE_ID desc";
                    }

                    var filePath = db.SqlQuery<string>(sql).FirstOrDefault();


                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var array = filePath.Split(new char[] { '\\' });

                        var fileName = array[array.Length - 1];
                        var fileNameArray = fileName.Split('.');

                        var index = Convert.ToInt32(fileNameArray[0].Substring(name.Length)) + 1;

                        var newFileName = string.Format("{0}0{1}.DBF", name.ToUpper(), index);

                        var newFilePath = string.Format("{0}{1}", filePath.Substring(0, filePath.LastIndexOf("\\") + 1), newFileName);

                        sql = string.Format("alter tablespace {0} add datafile '{1}' size 50M autoextend on next 50M maxsize UNLIMITED", name.ToUpper(), newFilePath);

                        db.ExecuteSqlCommand(sql);

                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw GetFault(ex);
                }
            }
        }

        #region 考试系统

        public List<EMCLASSIFICATIONMARK> GetCLASSIFICATIONMARK(string filter)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Empty;

                    if (string.IsNullOrEmpty(filter))
                        sql = @"select to_char(c.id) cid, c.name, nvl(e.mark, 0) mark
                                from (select t.id, t.name
                                        from wm_classificationitem t
                                        group by t.id, t.name) c
                                left join em_classificationmark e
                                on e.cid = c.id order by c.id";
                    else
                        sql = string.Format(@"select to_char(c.id) cid, c.name, nvl(e.mark, 0) mark
                                          from (select t.id, t.name
                                                   from wm_classificationitem t
                                                  group by t.id, t.name) c
                                          left join em_classificationmark e
                                            on e.cid = c.id
                                         where c.id like '{0}%'
                                            or c.name like '{0}%' order by c.id", filter);

                    return db.SqlQuery<EMCLASSIFICATIONMARK>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public string EditCLASSIFICATIONMARK(string id, string name, int mark)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var ent = db.EMCLASSIFICATIONMARK.FirstOrDefault(p => p.CID == id);

                    if (ent != null)
                    {
                        ent.NAME = name;
                        ent.MARK = mark;

                        return db.Update<EMCLASSIFICATIONMARK>(ent).ToString();
                    }
                    else
                    {
                        var model = new EMCLASSIFICATIONMARK();
                        model.CID = id;
                        model.NAME = name;
                        model.MARK = mark;

                        return db.Insert<EMCLASSIFICATIONMARK>(model).ToString();
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
        /// 获取需要Review的Lot信息
        /// </summary>
        /// <param name="operatorid"></param>
        /// <returns></returns>
        public List<WmwaferResultEntity> GetWaferResultHis(string stDate, string edDate, string lot)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var sql = new StringBuilder();

                    //                    sql.AppendFormat(@"select rownum Id,a.device,a.layer,a.lot,a.substrate_slot,a.substrate_id,a.substrate_notchlocation,t.SFIELD,
                    //                                                    decode(t.numdefect,0,nvl(b.defectivedie,0),t.numdefect) NUMDEFECT,t.ischecked,t.classificationinfoid,
                    //                                                    t.computername,t.completiontime,t.checkeddate,t.createddate,'Front' filetype,t.disposition,b.defectdensity,
                    //                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid,b.recipe_id from wm_waferresult t,wm_identification a,wm_inspectioninfo b
                    //                                                    where t.identificationid=a.identificationid and t.resultid=b.resultid
                    //                                                       and t.delflag = '0' and t.ischecked='2' and t.completiontime between {0} and {1}", stDate, edDate);

                    sql.AppendFormat(@"select 0 Id,a.device,a.layer,a.lot,a.substrate_slot,a.substrate_id,a.substrate_notchlocation,t.SFIELD,
                                                    decode(t.numdefect,0,nvl(b.defectivedie,0),t.numdefect) NUMDEFECT,t.ischecked,t.classificationinfoid,
                                                    t.computername,t.completiontime,t.checkeddate,t.createddate,'Front' filetype,t.disposition,b.defectdensity,
                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid,b.recipe_id from wm_waferresult t,wm_identification a,wm_inspectioninfo b
                                                    where t.identificationid=a.identificationid and t.resultid=b.resultid
                                                       and t.delflag = '0' and t.completiontime between {0} and {1}", stDate, edDate);

                    if (lot.EndsWith("|||"))
                        sql.AppendFormat("and instr(a.device||'|||','{0}')>0 ", lot);
                    else if (lot.EndsWith("||"))
                        sql.AppendFormat("and instr(a.device||'|'||a.layer||'||','{0}')>0 ", lot);
                    else
                        sql.AppendFormat("and instr(a.device||'|'||a.layer||'|'||a.lot||'|','{0}')>0", lot);

                    sql.Append("order by a.device,a.layer,a.lot,a.substrate_id");

                    return db.SqlQuery<WmwaferResultEntity>(sql.ToString()).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<EMLIBRARY> GetLIBRARY(string filter)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Empty;

                    if (string.IsNullOrEmpty(filter))
                        sql = @"select * from em_library t where t.delflag = '0' order by t.createdate";
                    else
                        sql = string.Format(@"select * from em_library t
                                         where t.delflag = '0' and t.papername like '{0}%' order by t.createdate", filter);

                    return db.SqlQuery<EMLIBRARY>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int AddLibray(string resultId, string papername, string remark, string by, string status)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var cnt = 0;
                    var model = new EMLIBRARY();

                    model.LID = Guid.NewGuid().ToString();
                    model.PAPERNAME = papername;
                    model.REMARK = remark;
                    model.RESULTID = resultId;
                    model.STATUS = status;
                    model.DELFLAG = "0";

                    model.CREATEID = by;
                    model.CREATEDATE = DateTime.Now;
                    model.UPDATEID = by;
                    model.UPDATEDATE = DateTime.Now;

                    if (!string.IsNullOrEmpty(resultId))
                    {
                        var newResultId = string.Join("','", resultId.Split(','));

                        string sql = string.Format(@"insert into em_defectlist
                                              (id, passid, inspid, inspectiontype, swcscoordinates, inspclassifiid,
                                               size_, majoraxissize, majorminoraxisaspectratio, area_, dieaddress,
                                               imagename, style, pixelsize, resultid, oldresultid,omodifieddefect)
                                              select rownum id, passid, '{0}' inspid, inspectiontype, swcscoordinates, nvl(modifieddefect, inspclassifiid) inspclassifiid,
                                                     size_, majoraxissize, majorminoraxisaspectratio, area_, dieaddress,
                                                     imagename, style, pixelsize, '{0}',resultid,inspclassifiid omodifieddefect
                                                from wm_defectlist t
                                               where t.resultid in('{1}')", model.LID, newResultId);

                        cnt = db.ExecuteSqlCommand(sql);

                    }

                    model.NUMDEFECT = cnt;

                    //if (cnt > 0)
                    cnt = db.Insert<EMLIBRARY>(model);

                    return cnt;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int UpdateLibray(string id, string name, string remark, string status, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    EMLIBRARY model = db.EMLIBRARY.Find(id);

                    if (model != null)
                    {
                        model.PAPERNAME = name;
                        model.REMARK = remark;
                        model.STATUS = status;

                        model.UPDATEID = by;
                        model.UPDATEDATE = DateTime.Now;
                    }

                    return db.Update<EMLIBRARY>(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int DeleteLibray(string id, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    EMLIBRARY model = db.EMLIBRARY.Find(id);

                    model.DELFLAG = "1";
                    model.UPDATEID = by;
                    model.UPDATEDATE = DateTime.Now;

                    var rs = db.Update<EMLIBRARY>(model);

                    var sql = string.Format("update em_plan t set t.delflag='1',t.updateid='{1}',t.updatedate=sysdate where t.lid='{0}'", id, by);

                    db.ExecuteSqlCommand(sql);

                    return rs;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<EMPLAN> GetEmPlan(string lid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Empty;

                    if (!string.IsNullOrEmpty(lid))
                        sql = string.Format("select * from em_plan t where t.lid='{0}' and t.delflag = '0'", lid);
                    else
                        sql = string.Format("select * from em_plan t where t.delflag = '0'");

                    return db.SqlQuery<EMPLAN>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int AddPlan(string lid, string name, string stDate, string edDate, int usernum, int defectnum, string remark, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var model = new EMPLAN();

                    model.PID = Guid.NewGuid().ToString();
                    model.LID = lid;
                    model.PLANNAME = name;
                    model.REMARK = remark;
                    model.STARTDATE = Convert.ToDateTime(stDate);
                    model.ENDDATE = Convert.ToDateTime(edDate);
                    model.USERNUM = usernum;

                    model.NUMDEFECT = defectnum;

                    model.UPDATEID = by;
                    model.UPDATEDATE = DateTime.Now;
                    model.CREATEID = by;
                    model.CREATEDATE = DateTime.Now;

                    model.DELFLAG = "0";

                    return db.Insert<EMPLAN>(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int UpdatePlan(string id, string name, string stDate, string edDate, int usernum, int defectnum, string remark, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    EMPLAN model = db.EMPLAN.Find(id);

                    if (model != null)
                    {
                        model.PLANNAME = name;
                        model.REMARK = remark;
                        model.STARTDATE = Convert.ToDateTime(stDate);
                        model.ENDDATE = Convert.ToDateTime(edDate);
                        model.USERNUM = usernum;
                        model.NUMDEFECT = defectnum;

                        model.UPDATEID = by;
                        model.UPDATEDATE = DateTime.Now;
                    }

                    return db.Update<EMPLAN>(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int DeletePlan(string id, string by)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    EMPLAN model = db.EMPLAN.Find(id);

                    model.DELFLAG = "1";
                    model.UPDATEID = by;
                    model.UPDATEDATE = DateTime.Now;

                    return db.Update<EMPLAN>(model);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int AddExamResult(string userid, string pid)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var model = new EMEXAMRESULT();

                    model.EID = Guid.NewGuid().ToString();
                    model.PLANID = pid;
                    model.USERID = userid;
                    //model.RESULTID = Guid.NewGuid().ToString();

                    model.CREATEID = userid;
                    model.CREATEDATE = DateTime.Now;
                    model.UPDATEID = userid;
                    model.UPDATEDATE = DateTime.Now;
                    model.STARTDATE = DateTime.Now;
                    model.DELFLAG = "0";

                    string sql = string.Format(@"insert into em_defectlist
                                            (id, passid, inspid, inspectiontype, swcscoordinates, inspclassifiid,
                                               size_, majoraxissize, majorminoraxisaspectratio, area_, dieaddress,
                                               imagename, style, pixelsize, resultid, oldresultid, omodifieddefect)
                                             select rownum id, passid, inspid, inspectiontype, swcscoordinates, inspclassifiid,
                                             size_, majoraxissize, majorminoraxisaspectratio, area_, dieaddress,
                                             imagename, style, pixelsize, resultid, oldresultid, omodifieddefect
                                              from (select id, passid, '{0}' inspid, inspectiontype, swcscoordinates, inspclassifiid,
                                                     size_, majoraxissize, majorminoraxisaspectratio, area_, dieaddress,
                                                     imagename, style, pixelsize,'{0}' resultid,oldresultid, omodifieddefect
                                                    from em_defectlist order by dbms_random.random)
                                             where rownum <= (select case  p.numdefect when 0 then 200 else p.numdefect end from em_plan p where p.pid='{1}')", model.EID, model.PLANID);

                    var cnt = db.ExecuteSqlCommand(sql);


                    if (cnt > 0)
                    {
                        cnt = db.Insert<EMEXAMRESULT>(model);

                        db.ExecuteSqlCommand(string.Format(@"update em_examresult t
                                                           set t.numdefect =
                                                               (select count(1) from em_defectlist d where d.resultid = t.eid)
                                                         where t.eid = '{0}'", model.EID));


                        db.ExecuteSqlCommand(string.Format(@"update em_defectlist t
                                                   set t.inspclassifiid =
                                                       (select m.itemid
                                                          from wm_classificationitem c
                                                         inner join wm_classificationitem m
                                                            on m.id = '0'
                                                           and m.schemeid = c.schemeid
                                                         where c.itemid = t.inspclassifiid)
                                                 where exists (select 1
                                                          from wm_classificationitem c
                                                         inner join wm_classificationitem m
                                                            on m.id = '0'
                                                           and m.schemeid = c.schemeid
                                                         where c.itemid = t.inspclassifiid)
                                                    and t.resultid='{0}'", model.EID));
                    }

                    return cnt;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int UpdateExamResult(string resultid, string checkedby, string mclassid, string finish)
        {
            StringBuilder sbt = new StringBuilder();
            using (BFdbContext db = new BFdbContext())
            {
                var tran = db.BeginTransaction();

                try
                {
                    sbt.Clear();
                    //更新defect表
                    sbt.AppendFormat(@"update em_defectlist set ischecked='1',checkeddate={0:yyyyMMddHHmmss},checkedby='{1}'
                                     where resultid='{2}'",
                                     DateTime.Now, checkedby, resultid);

                    db.ExecuteSqlCommand(sbt.ToString());

                    //修改后的defect
                    if (!string.IsNullOrEmpty(mclassid))
                    {
                        var modf = mclassid.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        if (modf.Length > 0)
                        {
                            foreach (var item in modf)
                            {
                                var ids = item.Split(new char[] { ',' });

                                sbt.Clear();
                                sbt.AppendFormat("update em_defectlist set inspclassifiid='{3}',modifieddefect=inspclassifiid where id={0} and passid={1} and inspid='{2}'", ids);
                                db.ExecuteSqlCommand(sbt.ToString());
                            }
                        }
                    }

                    if (finish == "2")
                    {
                        sbt.Clear();
                        sbt.AppendFormat("update em_examresult t set t.enddate = sysdate where t.eid ='{0}'", resultid);

                        db.ExecuteSqlCommand(sbt.ToString());

                        //totalscore
                        db.ExecuteSqlCommand(string.Format(@"update em_examresult t
                                                           set t.totalscore =
                                                               (select sum(case
                                                                             when d.inspclassifiid = d.omodifieddefect then
                                                                              m.mark
                                                                             else
                                                                              0
                                                                           end)
                                                                  from em_defectlist d
                                                                 inner join wm_classificationitem c
                                                                    on c.itemid = d.inspclassifiid
                                                                 inner join em_classificationmark m
                                                                    on m.cid = c.id
                                                                 where d.resultid = t.eid)
                                                         where t.eid = '{0}'", resultid));

                        //rightnum
                        db.ExecuteSqlCommand(string.Format(@"update em_examresult t
                                                           set t.rightnum =
                                                               (select count(1)
                                                                  from em_defectlist d
                                                                 where d.resultid = t.eid
                                                                   and d.inspclassifiid = d.omodifieddefect)
                                                         where t.eid = '{0}'", resultid));

                        //errornum
                        db.ExecuteSqlCommand(string.Format(@"update em_examresult t
                                                           set t.errornum =
                                                               (select count(1)
                                                                  from em_defectlist d
                                                                 where d.resultid = t.eid
                                                                   and d.inspclassifiid <> d.omodifieddefect)
                                                         where t.eid = '{0}'", resultid));
                    }

                    tran.Commit();
                    return 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    log.Error(ex);
                    throw GetFault(ex);
                }
            }
        }

        /// <summary>
        /// 获取缺陷列表
        /// </summary>
        /// <param name="resultid"></param>
        /// <returns></returns>
        public List<EmdefectlistEntity> GetPaperDefectList(string resultid, string ischecked)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string ischk = "";
                    if (!string.IsNullOrEmpty(ischecked) && ischecked == "0")
                        ischk = "and a.modifieddefect is not null";
                    else if (!string.IsNullOrEmpty(ischecked) && ischecked == "1")
                        ischk = "and a.modifieddefect is null";

                    string sql = string.Format(@"select a.id, a.passid, a.inspid, a.modifieddefect, a.inspclassifiid,
                                                       a.oldresultid || '\' ||a.imagename imagename, a.area_, d.color, a.ischecked, a.checkeddate,
                                                       d.name as description, a.dieaddress, 'Front' inspectedsurface,
                                                       '0' adc, d.schemeid, d.id cclassid, a.size_,o.id occlassid
                                                  from em_defectlist a, wm_classificationitem d,wm_classificationitem o
                                                 where a.inspclassifiid = d.itemid and a.omodifieddefect = o.itemid
                                                    and a.resultid='{0}' {1} order by a.id", resultid, ischk);

                    return db.SqlQuery<EmdefectlistEntity>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<EmExamResultEntity> GetExamResultReport(string sdate, string edate, string pname)
        {
            using (BFdbContext db = new BFdbContext())
            {
                string sql = string.Empty;

                if (string.IsNullOrEmpty(pname))
                {
                    sql = string.Format(@"select p.planname, u.userid, t.totalscore, t.startdate, t.enddate,
                                               t.numdefect, t.rightnum, t.errornum, t.eid
                                          from em_examresult t
                                         inner join em_plan p
                                            on p.pid = t.planid
                                        inner join tb_user u
                                            on u.id = t.userid
                                         where 1 = 1
                                           and t.delflag = '0'
                                           and t.startdate>=to_date('{0}','yyyyMMdd')
                                           and t.startdate <=to_date('{1}235959','yyyyMMddhh24miss')", sdate, edate);
                }
                else
                {
                    sql = string.Format(@"select p.planname, u.userid, t.totalscore, t.startdate, t.enddate,
                                               t.numdefect, t.rightnum, t.errornum, t.eid
                                          from em_examresult t
                                         inner join em_plan p
                                            on p.pid = t.planid
                                        inner join tb_user u
                                            on u.id = t.userid
                                         where 1 = 1
                                           and t.delflag = '0'
                                           and t.startdate>=to_date('{0}','yyyyMMdd')
                                           and t.startdate <=to_date('{1}235959','yyyyMMddhh24miss') and t.planid ='{2}'", sdate, edate, pname);
                }

                return db.SqlQuery<EmExamResultEntity>(sql).ToList();
            }
        }

        public int GetPaper(string by)
        {
            using (BFdbContext db = new BFdbContext())
            {
                var sql = @"select * from em_plan t where sysdate between t.startdate and t.enddate order by t.startdate desc";

                var listPlan = db.SqlQuery<EMPLAN>(sql).ToList();

                if (listPlan.Count <= 0)
                    return -1;

                var rs = -2;

                foreach (var p in listPlan)
                {
                    sql = string.Format(@"select t.*
                              from em_examresult t
                             inner join em_plan p
                                on p.pid = t.planid
                             where t.userid = '{0}'
                               and p.pid='{1}'
                               and sysdate between p.startdate and p.enddate", by, p.PID);

                    var listExamResult = db.SqlQuery<EMEXAMRESULT>(sql).FirstOrDefault();

                    if (listExamResult == null)
                    {
                        AddExamResult(by, p.PID);

                        rs = 0;
                    }
                    else
                    {
                        if (listExamResult.ENDDATE == null)
                            rs = 0;
                    }
                }

                return rs;
            }
        }

        public List<EmExamResultEntity> GetExamResult(string by, string eid)
        {
            using (BFdbContext db = new BFdbContext())
            {
                var sql = string.Empty;

                if (!string.IsNullOrEmpty(by))
                {
                    sql = string.Format(@"select t.*,p.enddate planenddate
                              from em_examresult t
                             inner join em_plan p
                                on p.pid = t.planid
                             where t.userid = '{0}'
                               and sysdate between p.startdate and p.enddate", by);
                }
                else
                {
                    sql = string.Format(@"select t.*,p.enddate planenddate
                              from em_examresult t
                            inner join em_plan p
                                on p.pid = t.planid
                             where t.eid = '{0}'", eid);

                }

                return db.SqlQuery<EmExamResultEntity>(sql).ToList();
            }
        }

        public List<WMCLASSIFICATIONITEM> GetClassificationItemByResultId(string resultid)
        {
            using (BFdbContext db = new BFdbContext())
            {
                var sql = string.Format(@"select c.*
                                      from wm_classificationitem c
                                     inner join em_defectlist t
                                        on c.itemid = t.inspclassifiid
                                     where t.resultid = '{0}'
                                       and rownum < 2", resultid);
                return db.SqlQuery<WMCLASSIFICATIONITEM>(sql).ToList();
            }
        }
        #endregion

        /// <summary>
        /// 更新检查结果的isreview状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isreview"></param>
        /// <returns></returns>
        public int UpdateWaferResultToReadOnly(string id, string isreview)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    string sql = string.Format("update wm_waferresult t set t.isreview='{0}' where t.resultid='{1}'", isreview, id);

                    return db.ExecuteSqlCommand(sql);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public List<WMYIELDSETTING> GetAllYieldSetting()
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    return db.FindAll<WMYIELDSETTING>().ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int AddYield(string repiceId, decimal layeryield, decimal waferyield,decimal maskayield,decimal maskbyield,decimal maskcyield,decimal maskdyield,decimal maskeyield)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (db.WMYIELDSETTING.Any(p => p.RECIPE_ID == repiceId))
                    {
                        return -2;
                    }

                    var entity = new WMYIELDSETTING
                      {
                          RECIPE_ID = repiceId,
                          LOT_YIELD = layeryield,
                          WAFER_YIELD = waferyield,
                          MASKA_YIELD = maskayield,
                          MASKB_YIELD=maskbyield,
                          MASKC_YIELD=maskcyield,
                          MASKD_YIELD=maskdyield,
                          MASKE_YIELD=maskeyield
                      };

                    return db.Insert<WMYIELDSETTING>(entity);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int EditYield(string repiceId, decimal layeryield, decimal waferyield, decimal maskayield, decimal maskbyield, decimal maskcyield, decimal maskdyield, decimal maskeyield)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var entity = new WMYIELDSETTING
                    {
                        RECIPE_ID = repiceId,
                        LOT_YIELD = layeryield,
                        WAFER_YIELD = waferyield,
                        MASKA_YIELD = maskayield,
                        MASKB_YIELD = maskbyield,
                        MASKC_YIELD = maskcyield,
                        MASKD_YIELD = maskdyield,
                        MASKE_YIELD = maskeyield
                    };

                    return db.Update<WMYIELDSETTING>(entity);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }

        public int DelYield(string repiceId)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    var entity = db.Find<WMYIELDSETTING>(s => s.RECIPE_ID == repiceId);

                    if (entity != null)
                        return db.Delete<WMYIELDSETTING>(entity);
                    else
                        return -1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw GetFault(ex);
            }
        }
    }
}
