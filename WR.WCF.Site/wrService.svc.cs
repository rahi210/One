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

namespace WR.WCF.Site
{
    public class wrService : ServiceBase, IwrService
    {
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
                    string sql = string.Format(@"select ta.id, ta.description, ta.name, nvl(tb.cnt, 0) Points,'Front' inspectiontype
                                                  from (select a.schemeid, a.id, a.description, b.name,a.itemid
                                                          from wm_classificationitem a, wm_classificationscheme b,wm_waferresult c 
                                                         where a.schemeid = b.schemeid and b.schemeid=c.classificationinfoid  and c.delflag='0' 
                                                               and c.resultid='{0}') ta,       
                                                       (select a.inspclassifiid,count(a.inspclassifiid) as cnt
                                                          from wm_defectlist a
                                                         where a.resultid = '{0}'
                                                         group by a.inspclassifiid) tb
                                                 where ta.itemid = tb.inspclassifiid(+) order by ta.id", resultid);

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
                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid,b.recipe_id from wm_waferresult t,wm_identification a,wm_inspectioninfo b,cmn_relation c
                                                    where t.identificationid=a.identificationid and t.resultid=b.resultid and instr(a.device||'-'||a.layer,c.device||'-'||decode(c.layer,'*','',c.layer))>0 
                                                        and ((t.completiontime>={1} and t.completiontime<={2}) or {3}) and c.userid='{0}' and t.delflag='0' order by a.device,a.layer,a.lot,a.substrate_id",
                                                    operatorid, fromday, today, done == "1" ? "t.ischecked in ('0','1')" : "1=2"); //and t.createddate> to_number(to_char(sysdate,'yyyymmddhh24miss'))

                    }
                    else
                    {
                        sql = string.Format(@"select rownum Id,a.device,a.layer,a.lot,a.substrate_slot,a.substrate_id,a.substrate_notchlocation,t.SFIELD,
                                                    decode(t.numdefect,0,nvl(b.defectivedie,0),t.numdefect) NUMDEFECT,t.ischecked,t.classificationinfoid,
                                                    t.computername,t.completiontime,t.checkeddate,t.createddate,'Front' filetype,t.disposition,b.defectdensity,
                                                    t.lotcompletiontime,t.identificationid,t.resultid,t.dielayoutid,b.recipe_id from wm_waferresult t,wm_identification a,wm_inspectioninfo b
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
                if (!File.Exists(Utils.PathCombine(Utils.ImgUpdatePath, filename)))
                    throw new FaultException("非法请求。");

                FileStream fstream = new FileStream(Utils.PathCombine(Utils.ImgUpdatePath, filename), FileMode.Open, FileAccess.Read, FileShare.Read);

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
                    string ischk = "";
                    if (!string.IsNullOrEmpty(ischecked) && ischecked == "0")
                        ischk = "and a.modifieddefect is not null";
                    else if (!string.IsNullOrEmpty(ischecked) && ischecked == "1")
                        ischk = "and a.modifieddefect is null";

                    string sql = string.Format(@"select a.id,a.passid,a.inspid,a.modifieddefect,a.inspclassifiid,a.imagename,a.area_,d.color,
                                                    a.ischecked,a.checkeddate,d.name as description,a.dieaddress,b.inspectedsurface,'0' adc,d.schemeid,d.id cclassid, a.size_ 
                                                    from wm_defectlist a,wm_inspectionpass b,wm_inspectioninfo c,wm_classificationitem d 
                                                    where a.passid=b.passid and a.inspid=b.inspid and b.inspid=c.inspid and a.inspclassifiid=d.itemid and a.resultid= c.resultid
                                                    and c.resultid='{0}' {1} order by a.id", resultid, ischk);

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
        public int UpdateDefect(string resultid, string checkedby, string mclassid, string finish)
        {
            StringBuilder sbt = new StringBuilder();
            using (BFdbContext db = new BFdbContext())
            {
                //                sbt.AppendFormat(@"select a.resultid,a.checkeddate,b.yieldnum,c.defectnum,a.dielayoutid from wm_waferresult a, 
                //                                 (select ba.layoutid,count(ba.id) yieldnum from wm_dielayoutlist ba where lower(trim(ba.disposition))<>'notexist' group by ba.layoutid) b,
                //                                (select ba.layoutid, count(ba.id) defectnum from wm_dielayoutlist ba where lower(trim(ba.disposition)) <> 'notexist' and ba.inspclassifiid<>'0' group by ba.layoutid) c
                //                                where a.dielayoutid=b.layoutid and a.dielayoutid = c.layoutid and a.resultid='{0}'", resultid);
                sbt.AppendFormat(@"select a.resultid,a.checkeddate,a.dielayoutid from wm_waferresult a
                                    where a.resultid='{0}'", resultid);

                var info = db.SqlQuery<WmwaferInfoEntity>(sbt.ToString()).ToList();
                if (info == null || info.Count < 1)
                    return -1;

                var tran = db.BeginTransaction();

                try
                {
                    sbt.Clear();
                    //更新defect表
                    sbt.AppendFormat(@"update wm_defectlist set ischecked='1',checkeddate={0:yyyyMMddHHmmss},checkedby='{1}'
                                     where (passid,inspid) in (select a.passid,a.inspid from wm_inspectionpass a,wm_inspectioninfo b where a.inspid=b.inspid and b.resultid='{2}')",
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
                                sbt.AppendFormat("update wm_defectlist set inspclassifiid='{3}',modifieddefect=inspclassifiid where id={0} and passid={1} and inspid='{2}'", ids);
                                db.ExecuteSqlCommand(sbt.ToString());

                                sbt.Clear();
                                sbt.AppendFormat(" update wm_dielayoutlist t set t.inspclassifiid='{0}' where t.layoutid='{1}' and t.dieaddressx={2} and t.dieaddressy='{3}'", ids[6], info[0].dielayoutid, ids[4], ids[5]);
                                db.ExecuteSqlCommand(sbt.ToString());
                            }
                        }
                    }

                    //更新waferresult表
                    sbt.Clear();
                    sbt.AppendFormat(@"select a.resultid,a.checkeddate,b.yieldnum,c.defectnum from wm_waferresult a, 
                                 (select ba.layoutid,count(ba.id) yieldnum from wm_dielayoutlist ba where lower(trim(ba.disposition))<>'notexist' group by ba.layoutid) b,
                                (select ba.layoutid, count(ba.id) defectnum from wm_dielayoutlist ba where lower(trim(ba.disposition)) <> 'notexist' and ba.inspclassifiid<>'0' group by ba.layoutid) c
                                where a.dielayoutid=b.layoutid and a.dielayoutid = c.layoutid and a.resultid='{0}'", resultid);
                    info = db.SqlQuery<WmwaferInfoEntity>(sbt.ToString()).ToList();
                    var y = (info[0].yieldnum.Value * 1.0 - info[0].defectnum.Value * 1.0) / info[0].yieldnum.Value;

                    sbt.Clear();
                    sbt.AppendFormat("update wm_waferresult set ischecked='{0}',checkeddate={1},checkedby='{2}',sfield={3},numdefect={4} where resultid='{5}'",
                        string.IsNullOrEmpty(finish) ? "0" : finish,
                        info[0].checkeddate.HasValue ? info[0].checkeddate.Value.ToString() : DateTime.Now.ToString("yyyyMMddHHmmss"),
                        checkedby,
                        Math.Round(y, 4) * 100,
                        info[0].defectnum,
                        resultid);

                    db.ExecuteSqlCommand(sbt.ToString());

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
                    string sql = string.Format(@"select t.dieaddressx,t.dieaddressy,t.inspclassifiid,t.isinspectable,t.disposition,a.columns_,a.rows_ from wm_dielayoutlist t,wm_dielayout a
                                                    where t.layoutid=a.layoutid and t.layoutid='{0}' and t.disposition in ('NotProcess     ','NotInspectable ','Fail           ','Pass           ','Processed      ')", id);

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
        public List<WmdensityReport> GetDensityReport(string lot, string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {

                string sql = string.Format(@"select b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,1 cnt,th.defectdensity,c.completiontime,a.DIEADDRESS 
                                                from wm_defectlist a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_inspectionpass th,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.passid=th.passid and a.inspid=th.inspid and th.inspid=c.inspid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'|'||b.lot||'|','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate);
                if (lot.EndsWith("|||"))
                    sql = string.Format(@"select b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,1 cnt,th.defectdensity,c.completiontime,a.DIEADDRESS 
                                                from wm_defectlist a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_inspectionpass th,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.passid=th.passid and a.inspid=th.inspid and th.inspid=c.inspid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|||','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate);
                else if (lot.EndsWith("||"))
                    sql = string.Format(@"select b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,1 cnt,th.defectdensity,c.completiontime,a.DIEADDRESS 
                                                from wm_defectlist a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_inspectionpass th,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.passid=th.passid and a.inspid=th.inspid and th.inspid=c.inspid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'||','{0}')>0 and te.delflag='0'and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate);

                return db.SqlQuery<WmdensityReport>(sql).ToList();
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
                string sql = string.Format(@"select count(a.inspecteddieid) cnt,count(distinct c.resultid) wafercnt 
                                            from wm_inspecteddielist a,wm_inspectioninfo b,wm_waferresult c,wm_identification d
                                            where a.inspid=b.inspid and b.resultid=c.resultid and c.identificationid=d.identificationid
                                                and instr(d.device||'|'||d.layer||'|'||d.lot||'|','{0}')>0 and c.delflag='0' and b.completiontime>={1} and b.completiontime<={2}", lot, stDate, edDate);
                if (lot.EndsWith("|||"))
                    sql = string.Format(@"select count(a.inspecteddieid) cnt,count(distinct c.resultid) wafercnt 
                                            from wm_inspecteddielist a,wm_inspectioninfo b,wm_waferresult c,wm_identification d
                                            where a.inspid=b.inspid and b.resultid=c.resultid and c.identificationid=d.identificationid
                                                and instr(d.device||'|||','{0}')>0 and c.delflag='0' and b.completiontime>={1} and b.completiontime<={2}", lot, stDate, edDate);
                else if (lot.EndsWith("||"))
                    sql = string.Format(@"select count(a.inspecteddieid) cnt,count(distinct c.resultid) wafercnt 
                                            from wm_inspecteddielist a,wm_inspectioninfo b,wm_waferresult c,wm_identification d
                                            where a.inspid=b.inspid and b.resultid=c.resultid and c.identificationid=d.identificationid
                                                and instr(d.device||'|'||d.layer||'||','{0}')>0 and c.delflag='0' and b.completiontime>={1} and b.completiontime<={2}", lot, stDate, edDate);

                var lst = db.SqlQuery<WmdensityReport>(sql).ToList();

                if (lst != null && lst.Count > 0)
                    return new int[] { lst[0].CNT, lst[0].WAFERCNT };
                else
                    return new int[] { 0, 0 };
            }
        }

        /// <summary>
        /// 获取缺陷分类汇总报表
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<WmCategoryReport> GetCategoryReport(string lot, string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {
                string sql = string.Format(@"select c.device,c.lot,c.layer,b.completiontime,c.substrate_id,a.id,a.name description,d.defectnum
                                                from wm_classificationitem a,wm_waferresult b,wm_identification c,
                                            (select t.resultid,t.inspclassifiid,count(t.id) defectnum
                                            from wm_defectlist t group by t.resultid,t.inspclassifiid) d
                                            where a.itemid=d.inspclassifiid and d.resultid=b.resultid and b.identificationid=c.identificationid
                                            and instr(c.device||'|'||c.layer||'|'||c.lot||'|','{0}')>0 and b.delflag='0' and b.completiontime>={1} and b.completiontime<={2} order by c.substrate_id,a.id", lot, stDate, edDate);

                if (lot.EndsWith("|||"))
                    sql = string.Format(@"select c.device,c.lot,c.layer,b.completiontime,c.substrate_id,a.id,a.name description,d.defectnum
                                                from wm_classificationitem a,wm_waferresult b,wm_identification c,
                                            (select t.resultid,t.inspclassifiid,count(t.id) defectnum
                                            from wm_defectlist t group by t.resultid,t.inspclassifiid) d
                                            where a.itemid=d.inspclassifiid and d.resultid=b.resultid and b.identificationid=c.identificationid
                                            and instr(c.device||'|||','{0}')>0 and b.delflag='0' and b.completiontime>={1} and b.completiontime<={2} order by c.substrate_id,a.id", lot, stDate, edDate);

                else if (lot.EndsWith("||"))
                    sql = string.Format(@"select c.device,c.lot,c.layer,b.completiontime,c.substrate_id,a.id,a.name description,d.defectnum
                                                from wm_classificationitem a,wm_waferresult b,wm_identification c,
                                            (select t.resultid,t.inspclassifiid,count(t.id) defectnum
                                            from wm_defectlist t group by t.resultid,t.inspclassifiid) d
                                            where a.itemid=d.inspclassifiid and d.resultid=b.resultid and b.identificationid=c.identificationid
                                            and instr(c.device||'|'||c.layer||'||','{0}')>0 and b.delflag='0' and b.completiontime>={1} and b.completiontime<={2} order by c.substrate_id,a.id", lot, stDate, edDate);

                return db.SqlQuery<WmCategoryReport>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取缺陷的die
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<WmDefectiveDieReport> GetDefectiveDieReport(string lot, string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {
                string sql = string.Format(@"select b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,c.completiontime,a.area_,a.size_,a.DIEADDRESS 
                                                from wm_defectlist a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.resultid=c.resultid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'|'||b.lot||'|','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate);

                if (lot.EndsWith("|||"))
                    sql = string.Format(@"select b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,c.completiontime,a.area_,a.size_,a.DIEADDRESS 
                                                from wm_defectlist a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.resultid=c.resultid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|||','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate);

                else if (lot.EndsWith("||"))
                    sql = string.Format(@"select b.device,b.layer,b.lot,b.substrate_id,a.dieaddress,d.id,d.name description,c.completiontime,a.area_,a.size_,a.DIEADDRESS 
                                                from wm_defectlist a,wm_waferresult te,wm_identification b,wm_inspectioninfo c,wm_classificationitem d
                                                where a.resultid=te.resultid and te.identificationid=b.identificationid and a.resultid=c.resultid
                                                    and a.inspclassifiid=d.itemid and instr(b.device||'|'||b.layer||'||','{0}')>0 and te.delflag='0' and c.completiontime>={1} and c.completiontime<={2} 
                                                order by b.lot,b.substrate_id,d.id", lot, stDate, edDate);

                return db.SqlQuery<WmDefectiveDieReport>(sql).ToList();
            }
        }

        /// <summary>
        /// 获取检测后的die
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<WmInpDieReport> GetDieInspDieReport(string lot, string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {
                string sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.dieqty
                                                from wm_waferresult ta,wm_identification tb,
                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                                                (select c.resultid,count(a.id) dieqty 
                                                        from wm_dielayoutlist a,wm_dielayout b,wm_waferresult c
                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))<>'notexist' group by c.resultid) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'|'||tb.lot||'|','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate);

                if (lot.EndsWith("|||"))
                    sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.dieqty
                                                from wm_waferresult ta,wm_identification tb,
                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                                                (select c.resultid,count(a.id) dieqty 
                                                        from wm_dielayoutlist a,wm_dielayout b,wm_waferresult c
                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))<>'notexist' group by c.resultid) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate);

                else if (lot.EndsWith("||"))
                    sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.dieqty
                                                from wm_waferresult ta,wm_identification tb,
                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                                                (select c.resultid,count(a.id) dieqty 
                                                        from wm_dielayoutlist a,wm_dielayout b,wm_waferresult c
                                                        where a.layoutid=b.layoutid and b.layoutid=c.dielayoutid and lower(trim(a.disposition))<>'notexist' group by c.resultid) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate);

                return db.SqlQuery<WmInpDieReport>(sql).ToList();
            }
        }

        public List<WmDefectListReport> GetDefectListReport(string lot, string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {
                string sql = string.Format(@"select d.lot,d.device,d.layer,d.substrate_id,d.substrate_slot,a.id defectnumber,b.id,b.name description,
                                            a.dieaddress,a.area_,a.size_,c.completiontime,a.DIEADDRESS 
                                            from wm_defectlist a,wm_classificationitem b,wm_waferresult c,wm_identification d
                                            where a.inspclassifiid=b.itemid and a.resultid=c.resultid and c.identificationid=d.identificationid 
                                                and instr(d.device||'|'||d.layer||'|'||d.lot||'|','{0}')>0 and c.delflag='0' and c.completiontime>={1} and c.completiontime<={2} order by d.substrate_id,a.id", lot, stDate, edDate);

                if (lot.EndsWith("|||"))
                    sql = string.Format(@"select d.lot,d.device,d.layer,d.substrate_id,d.substrate_slot,a.id defectnumber,b.id,b.name description,
                                            a.dieaddress,a.area_,a.size_,c.completiontime,a.DIEADDRESS 
                                            from wm_defectlist a,wm_classificationitem b,wm_waferresult c,wm_identification d
                                            where a.inspclassifiid=b.itemid and a.resultid=c.resultid and c.identificationid=d.identificationid 
                                                and instr(d.device||'|||','{0}')>0 and c.delflag='0' and c.completiontime>={1} and c.completiontime<={2} order by d.substrate_id,a.id", lot, stDate, edDate);

                else if (lot.EndsWith("||"))
                    sql = string.Format(@"select d.lot,d.device,d.layer,d.substrate_id,d.substrate_slot,a.id defectnumber,b.id,b.name description,
                                            a.dieaddress,a.area_,a.size_,c.completiontime,a.DIEADDRESS 
                                            from wm_defectlist a,wm_classificationitem b,wm_waferresult c,wm_identification d
                                            where a.inspclassifiid=b.itemid and a.resultid=c.resultid and c.identificationid=d.identificationid 
                                                and instr(d.device||'|'||d.layer||'||','{0}')>0 and c.delflag='0' and c.completiontime>={1} and c.completiontime<={2} order by d.substrate_id,a.id", lot, stDate, edDate);

                return db.SqlQuery<WmDefectListReport>(sql).ToList();
            }
        }

        public List<WmGoodDieReport> GetGoodDieReport(string lot, string stDate, string edDate)
        {
            using (BFdbContext db = new BFdbContext())
            {
                string sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.inspcnt
                                                from wm_waferresult ta,wm_identification tb,
                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                                                (select b.resultid,count(inspecteddieid) inspcnt 
                                                        from wm_inspecteddielist a,wm_inspectioninfo b where a.inspid=b.inspid  group by b.resultid) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'|'||tb.lot||'|','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2}  order by tb.substrate_id", lot, stDate, edDate);

                if (lot.EndsWith("|||"))
                    sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.inspcnt
                                                from wm_waferresult ta,wm_identification tb,
                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                                                (select b.resultid,count(inspecteddieid) inspcnt 
                                                        from wm_inspecteddielist a,wm_inspectioninfo b where a.inspid=b.inspid  group by b.resultid) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2} order by tb.substrate_id", lot, stDate, edDate);

                else if (lot.EndsWith("||"))
                    sql = string.Format(@"select tb.lot,tb.device,tb.layer,tb.substrate_id,tb.substrate_slot,ta.completiontime,tc.defectnum,td.inspcnt
                                                from wm_waferresult ta,wm_identification tb,
                                                (select a.resultid,count(distinct a.dieaddress) defectnum from wm_defectlist a,wm_classificationitem b where a.inspclassifiid=b.itemid and b.id<>0 group by a.resultid) tc,
                                                (select b.resultid,count(inspecteddieid) inspcnt 
                                                        from wm_inspecteddielist a,wm_inspectioninfo b where a.inspid=b.inspid  group by b.resultid) td
                                                where ta.identificationid=tb.identificationid and ta.resultid=tc.resultid and ta.resultid=td.resultid 
                                                        and instr(tb.device||'|'||tb.layer||'||','{0}')>0 and ta.delflag='0' and ta.completiontime>={1} and ta.completiontime<={2}  order by tb.substrate_id", lot, stDate, edDate);

                return db.SqlQuery<WmGoodDieReport>(sql).ToList();
            }
        }

        public List<WMCLASSIFICATIONITEM> GetClassificationItemsByLayer(string lot, string stDate, string edDate)
        {
            try
            {
                using (BFdbContext db = new BFdbContext())
                {
                    if (lot.EndsWith("|||"))
                        return db.SqlQuery<WMCLASSIFICATIONITEM>(string.Format(@"select a.*
                                                                              from wm_classificationitem a
                                                                             where a.schemeid in (select ta.classificationinfoid
                                                                                                    from wm_waferresult ta, wm_identification tb
                                                                                                   where instr(tb.device || '|||', '{0}') > 0  and ta.identificationid=tb.identificationid
                                                                                                     and rownum < 2)", lot)).ToList();
                    else
                    {
                        return db.SqlQuery<WMCLASSIFICATIONITEM>(string.Format(@"select a.*
                                                                              from wm_classificationitem a
                                                                             where a.schemeid in (select ta.classificationinfoid
                                                                                                    from wm_waferresult ta, wm_identification tb 
                                                                                                   where instr(tb.device ||'|'||tb.layer||'||', '{0}') > 0  and ta.identificationid=tb.identificationid
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

                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select c.device, c.layer,c.lot, a.inspclassifiid, c.substrate_id,count(a.id) NumCnt
                                                  from wm_defectlist       a,
                                                       wm_waferresult      b,
                                                       wm_identification   c
                                                 where a.resultid = b.resultid
                                                   and b.identificationid = c.identificationid
                                                   and b.delflag='0' and instr(c.device||'|||','{0}')>0 and b.completiontime>={1} and b.completiontime<={2} 
                                                 group by c.device, c.layer,c.lot,c.substrate_id, a.inspclassifiid", lot, stDate, edDate);
                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select c.device, c.layer, c.lot,a.inspclassifiid, c.substrate_id,count(a.id) NumCnt
                                                  from wm_defectlist       a,
                                                       wm_waferresult      b,
                                                       wm_identification   c
                                                 where a.resultid = b.resultid
                                                   and b.identificationid = c.identificationid
                                                   and b.delflag='0' and instr(c.device||'|'||c.layer||'||','{0}')>0 and b.completiontime>={1} and b.completiontime<={2} 
                                                 group by c.device, c.layer,c.lot,c.substrate_id, a.inspclassifiid", lot, stDate, edDate);
                    else
                        sql = string.Format(@"select c.device, c.layer,c.lot, a.inspclassifiid,c.substrate_id, count(a.id) NumCnt
                                                  from wm_defectlist       a,
                                                       wm_waferresult      b,
                                                       wm_identification   c
                                                 where a.resultid = b.resultid
                                                   and b.identificationid = c.identificationid
                                                   and b.delflag='0' and instr(c.device||'|'||c.layer||'|'||c.lot||'|','{0}')>0 and b.completiontime>={1} and b.completiontime<={2} 
                                                 group by c.device, c.layer,c.lot, c.substrate_id,a.inspclassifiid", lot, stDate, edDate);

                    //log.Error(sql);
                    return db.SqlQuery<WmItemsSummaryEntity>(sql).ToList();
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

                    if (lot.EndsWith("|||"))
                        sql = string.Format(@"select d.device, d.layer, count(a.inspecteddieid) NumCnt
                                               from wm_inspecteddielist a,
                                                    wm_inspectioninfo   b,
                                                    wm_waferresult      c,
                                                    wm_identification   d
                                              where a.inspid = b.inspid
                                                and b.resultid = c.resultid
                                                and c.identificationid = d.identificationid
                                                and c.delflag = '0' and instr(d.device||'|||','{0}')>0  and c.completiontime>={1} and c.completiontime<={2} 
                                              group by d.device, d.layer", lot, stDate, edDate);
                    else if (lot.EndsWith("||"))
                        sql = string.Format(@"select d.device, d.layer, count(a.inspecteddieid) NumCnt
                                               from wm_inspecteddielist a,
                                                    wm_inspectioninfo   b,
                                                    wm_waferresult      c,
                                                    wm_identification   d
                                              where a.inspid = b.inspid
                                                and b.resultid = c.resultid
                                                and c.identificationid = d.identificationid
                                                and c.delflag = '0' and instr(d.device||'|'||d.layer||'||','{0}')>0  and c.completiontime>={1} and c.completiontime<={2} 
                                              group by d.device, d.layer", lot, stDate, edDate);
                    else
                        sql = string.Format(@"select d.device, d.layer,d.lot,d.substrate_id, count(a.inspecteddieid) NumCnt
                                               from wm_inspecteddielist a,
                                                    wm_inspectioninfo   b,
                                                    wm_waferresult      c,
                                                    wm_identification   d
                                              where a.inspid = b.inspid
                                                and b.resultid = c.resultid
                                                and c.identificationid = d.identificationid
                                                and c.delflag = '0' and instr(d.device||'|'||d.layer||'|'||d.lot||'|','{0}')>0  and c.completiontime>={1} and c.completiontime<={2}
                                              group by d.device, d.layer,d.lot,d.substrate_id", lot, stDate, edDate);

                    return db.SqlQuery<WmItemsSummaryEntity>(sql).ToList();
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
    }
}
