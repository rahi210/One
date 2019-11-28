package dataTransferServer.DataAccess;

import java.sql.*;
import java.util.Date;
import java.util.List;
import java.util.Properties;
import java.util.concurrent.CountDownLatch;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.ScheduledThreadPoolExecutor;
import java.util.concurrent.ThreadPoolExecutor;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.sql.DataSource;

import com.alibaba.fastjson.JSONArray;

//import org.json.JSONArray;

import dataTransferServer.Base.LogWriter;
import dataTransferServer.Base.PropertyManager;
import dataTransferServer.Base.Utils;
import dataTransferServer.Entity.*;

import oracle.jdbc.driver.OracleDriver;

public class OracleHelper {

	private OracleHelper() {
		super();
	}

	private static Connection currConnect;
	private static String yearMonth;
	private static String connectionString;
	private static String userName;
	private static String password;
	private static boolean isExistsLotLayout;

	public static int threadSize = 4;
	public static int maxInsertCount = 50000;
	public static int finishCount = 0;
	public static ExecutorService executor;

	public static int dbType = 0; // 0-oracle 1-mysql

	// static CountDownLatch cdl = new CountDownLatch(threadSize);

	// java:comp/env/jdbc/oracle
	@SuppressWarnings("finally")
	public static boolean SetConnection(String jndiname) {

		try {
			Context ic = new InitialContext();
			DataSource source = (DataSource) ic.lookup(jndiname);

			currConnect = source.getConnection();
		} catch (NamingException e) {
			// e.printStackTrace();
			LogWriter.getWriter().AddLogItem(e.getStackTrace());
		} catch (SQLException e) {
			// e.printStackTrace();
			LogWriter.getWriter().AddLogItem(e.getStackTrace());
		} finally {
			if (currConnect == null) {
				return false;
			}
			return true;
		}
	}

	public static boolean SetConnection(Connection connect) {
		currConnect = connect;
		return true;
	}

	public static Connection GetDataBaseConnect() throws SQLException {
		if (currConnect != null && !currConnect.isClosed()) {
			return currConnect;
		}

		return CreateConnect();
	}

	/*
	 * private static ThreadLocal<Connection> connectionHolder = new
	 * ThreadLocal<Connection>() {
	 * 
	 * @Override protected Connection initialValue() { try {
	 * 
	 * if (connectionString == null) { Properties properties =
	 * PropertyManager.getProperties();
	 * 
	 * connectionString = properties .getProperty("ConnectionString"); userName
	 * = properties.getProperty("UserName"); password =
	 * properties.getProperty("Password"); }
	 * 
	 * return DriverManager.getConnection(connectionString, userName, password);
	 * } catch (SQLException e) { LogWriter.getWriter().AddLogItem(
	 * "Database connection timeout：" + e.getStackTrace()); } return null; } };
	 * 
	 * public static Connection getThreadLocalConnection() { return
	 * connectionHolder.get(); }
	 */

	private static ThreadLocal<Connection> connectionHolder = new ThreadLocal<Connection>();

	public static Connection getThreadLocalConnection() {
		// 获得线程变量connectionHolder的值conn
		Connection conn = connectionHolder.get();
		try {
			if (conn == null) {
				// 如果连接为空，则创建连接，另一个工具类，创建连接
				Properties properties = PropertyManager.getProperties();

				connectionString = properties.getProperty("ConnectionString");
				userName = properties.getProperty("UserName");
				password = properties.getProperty("Password");

				DriverManager.registerDriver(new OracleDriver());
				conn = DriverManager.getConnection(connectionString, userName,
						password);
				// 将局部变量connectionHolder的值设置为conn
				connectionHolder.set(conn);
			}
		} catch (SQLException e) {
			e.printStackTrace();
			LogWriter.getWriter().AddLogItem(
					"Database connection timeout：" + e.getStackTrace());
		}

		return conn;
	}

	public static void closeThreadLocalConnection() {
		Connection conn = connectionHolder.get();
		if (conn != null) {
			try {
				conn.close();
				// 从ThreadLocal中清除Connection
				connectionHolder.remove();
			} catch (SQLException e) {
				e.printStackTrace();
				LogWriter.getWriter().AddLogItem(
						"Database connection timeout：" + e.getStackTrace());
			}
		}
	}

	public static Connection CreateConnect() {
		try {

			Properties properties = PropertyManager.getProperties();
			/*
			 * String connectionString = properties
			 * .getProperty("ConnectionString"); String userName =
			 * properties.getProperty("UserName"); String password =
			 * properties.getProperty("Password");
			 */
			connectionString = properties.getProperty("ConnectionString");
			userName = properties.getProperty("UserName");
			password = properties.getProperty("Password");

			password = properties.getProperty("Password");

			if (connectionString.indexOf("mysql") > 0) {
				try {
					Class.forName("com.mysql.jdbc.Driver");

					OracleHelper.dbType = 1;
				} catch (ClassNotFoundException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			} else {
				DriverManager.registerDriver(new OracleDriver());

				OracleHelper.dbType = 0;
			}

			Connection currConnect = DriverManager.getConnection(
					connectionString, userName, password);

			DriverManager.setLoginTimeout(60);
			OracleHelper.SetConnection(currConnect);

			// LogWriter.getWriter().AddLogItem("Database automatic connection");

			return currConnect;
		} catch (SQLException e) {
			e.printStackTrace();
			LogWriter.getWriter().AddLogItem(
					"Database connection timeout：" + e.getStackTrace());

			return null;
		}
	}

	public static void Close() {
		if (currConnect != null) {
			try {
				currConnect.close();
			} catch (SQLException e) {
				e.printStackTrace();
			}
		}
	}

	public static boolean GetDataArchiveStatus() throws SQLException {

		boolean status = false;
		String sql = "select count(1) from cmn_dict t where t.dictid='3020' and t.code='1'";

		currConnect = GetDataBaseConnect();
		PreparedStatement prest = currConnect.prepareStatement(sql.toString());

		try {
			ResultSet rs = prest.executeQuery();

			while (rs.next()) {
				status = rs.getInt(1) > 0;
			}
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
			// currConnect.close();
		}
		return status;
	}

	public static boolean IsExistsLotLayout(String layer, String lotName)
			throws SQLException {

		boolean status = false;
		String sql = "select max(w.dielayoutid),max(f.inspecteddie)  from wm_waferresult w inner join wm_identification i "
				+ "on w.identificationid =i.identificationid "
				+ "inner join wm_inspectioninfo f on f.resultid= w.resultid where i.lot=? and i.layer=?";

		currConnect = GetDataBaseConnect();
		PreparedStatement prest = currConnect.prepareStatement(sql.toString());

		try {
			prest.setString(1, lotName);
			prest.setString(2, layer);

			ResultSet rs = prest.executeQuery();

			while (rs.next()) {
				Utils.LotId = rs.getString(1);
				Utils.LotDieNum = rs.getLong(2);
			}

			if (Utils.LotId != null && Utils.LotId.length() > 0) {
				status = true;
			}

		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
			// currConnect.close();
		}

		isExistsLotLayout = status;
		return status;
	}

	public static boolean InsertLog() throws SQLException {

		boolean status = false;
		String sql = "";

		if (Utils.DeviceName == null || Utils.DeviceName.length() == 0) {
			return status;
		}

		if (dbType == 1) {
			sql = "update tb_userlog t set t.createdate =sysdate()  where t.type='9' and t.remark=?";
		} else {
			sql = "update tb_userlog t set t.createdate =sysdate  where t.type='9' and t.remark=?";
		}

		currConnect = GetDataBaseConnect();
		PreparedStatement prest = currConnect.prepareStatement(sql.toString());

		try {
			prest.setString(1, Utils.DeviceName);

			status = prest.executeUpdate() > 0;

			if (!status) {
				if (dbType == 1) {
					sql = "insert into tb_userlog (id, userid, type, ip, remark, createdate) values (?, 'AFA01A646A93453FA2B8EDF1B7B6FE48', '9', '127.0.0.1', ?, sysdate())";
				} else {
					sql = "insert into tb_userlog (id, userid, type, ip, remark, createdate) values (?, 'AFA01A646A93453FA2B8EDF1B7B6FE48', '9', '127.0.0.1', ?, sysdate)";
				}

				PreparedStatement prest1 = currConnect.prepareStatement(sql
						.toString());

				prest1.setString(1, Utils.NewGuid());
				prest1.setString(2, Utils.DeviceName);

				status = prest1.executeUpdate() > 0;
			}
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		return status;
	}

	/*
	 * 将业务实体中的数据插入到数据库中 通过事务保证数据的一致性 防止垃圾数据的生成
	 */
	public static boolean InsertEntityToDataTable(WaferEntity entity)
			throws SQLException {
		boolean rs = false;
		try {
			currConnect = GetDataBaseConnect();
			// currConnect.setAutoCommit(false);

			InsertLog();

			String schemeid = GetClassificationSchemeId(entity
					.getClassificationSchemeEntity().getComputerName(), entity
					.getClassificationSchemeEntity().getName());

			if (schemeid.length() == 0) {
				InsertClassificationSchemeEntity(entity
						.getClassificationSchemeEntity());

				InsertClassificationItemEntity(entity
						.getClassificationItemEntity());

			} else {
				entity.getWaferResultEntity().setClassificationInfoID(schemeid);
				UpdateInspclassifiId(schemeid, entity.getDefectListEntity());
			}

			String completiontime = Long.toString(entity.getWaferResultEntity()
					.getCompletionTime());
			yearMonth = completiontime.substring(0, 6);

			InsertWaferResult(entity.getWaferResultEntity());

			String msg = String.format("%s,InsertWaferResult.........", Utils
					.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			InsertIdentificationEntity(entity.getIdentificationEntity());

			msg = String.format("%s,InsertIdentificationEntity.........", Utils
					.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			if (!isExistsLotLayout) {

				msg = String.format("%s,InsertDieLayoutEntity.........", Utils
						.GetCurrDateTime());
				LogWriter.getWriter().AddLogItem(msg);

				rs = InsertDieLayoutEntity(entity.getDieLayoutEntity(), entity
						.getDieLayoutListEntity());

				if (rs == false) {

					Utils.IsReImport = true;
					return false;
				}

				/*
				 * if (entity.getDieLayoutListEntity().size() <= 50000) {
				 * InsertDieLayoutListEntity(entity.getDieLayoutListEntity());
				 * msg = String.format(
				 * "%s,%s,InsertDieLayoutListEntity.........", Utils
				 * .GetCurrDateTime(), entity .getDieLayoutListEntity().size());
				 * } else { rs = ThreadInsertDieLayoutListEntity(entity
				 * .getDieLayoutListEntity()); msg = String .format(
				 * "%s,%s,ThreadInsertDieLayoutListEntity,Status=%s.........",
				 * Utils.GetCurrDateTime(), entity
				 * .getDieLayoutListEntity().size(), rs);
				 * 
				 * if (rs == false) {
				 * 
				 * Utils.IsReImport=true; return false; } }
				 * 
				 * LogWriter.getWriter().AddLogItem(msg);
				 */
			}

			InsertInspectionInfoEntity(entity.getInspectionInfoEntity());

			msg = String.format("%s,InsertInspectionInfoEntity.........", Utils
					.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			InsertInspectionPassEntity(entity.getInspectionPassEntity());

			msg = String.format("%s,InsertInspectionPassEntity.........", Utils
					.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			InsertInspectedDieListEntity(entity.getInspectedDieListEntity());

			msg = String.format("%s,InsertInspectedDieListEntity.........",
					Utils.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			InsertDefectListEntity(entity.getDefectListEntity());

			msg = String.format("%s,InsertDefectListEntity.........", Utils
					.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			UpdateInspectionInfoEntity(entity.getInspectionInfoEntity());

			msg = String.format("%s,UpdateInspectionInfoEntity.........", Utils
					.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			/*
			 * if (schemeid.length() > 0) {
			 * UpdateInspclassifiId(schemeid,entity.getDefectListEntity()); }
			 */

			// currConnect.commit();
			msg = String.format("%s,commit.........", Utils.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			rs = true;

		} catch (Exception e) {
			e.printStackTrace();
			rs = false;

			StringBuffer sb = new StringBuffer();
			StackTraceElement[] stackArray = e.getStackTrace();
			for (int i = 0; i < stackArray.length; i++) {
				StackTraceElement element = stackArray[i];
				sb.append(element.toString() + "\n");
			}

			LogWriter.getWriter().AddLogItem(sb.toString());
			// currConnect.rollback();

		} finally {
			// currConnect.close();
		}

		return rs;
	}

	// wm_waferresult
	private static boolean InsertWaferResult(WaferResultEntity entity)
			throws SQLException {
		boolean rs = false;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_waferresult ");
		lsquery
				.append("(resultid, starttime, completiontime, reviewstarttime, ");
		lsquery
				.append("reviewcompletiontime, lotstarttime, lotcompletiontime, disposition, ");
		lsquery
				.append("mastertoolname, mastersoftwareversion, mastertoolcomputername, ");
		lsquery
				.append("mastertooltohostlinkstate, modulename, computername, softwareversion, ");
		lsquery
				.append("primarysurface, identificationid, classificationinfoid, dielayoutid,createddate,sfield,numdefect)");
		lsquery
				.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());
		try {
			prest.setString(1, entity.getResultId());
			prest.setLong(2, entity.getStartTime());
			prest.setLong(3, entity.getCompletionTime());
			prest.setLong(4, entity.getReviewStartTime());
			prest.setLong(5, entity.getReviewCompletionTime());
			prest.setLong(6, entity.getLotStartTime());
			prest.setLong(7, entity.getLotCompletionTime());
			prest.setString(8, entity.getDisposition());
			prest.setString(9, entity.getMasterToolName());
			prest.setString(10, entity.getMasterSoftwareVersion());
			prest.setString(11, entity.getMasterToolComputerName());
			prest.setString(12, entity.getMasterToolToHostLinkState());
			prest.setString(13, entity.getModuleName());
			prest.setString(14, entity.getComputerName());
			prest.setString(15, entity.getSoftwareVersion());
			prest.setString(16, entity.getPrimarySurface());
			prest.setString(17, entity.getIdentificationID());
			prest.setString(18, entity.getClassificationInfoID());
			prest.setString(19, entity.getDieLayoutID());
			// prest.setString(20,entity.getIsChecked());
			// prest.setLong(21,entity.getCheckedDate());
			// prest.setString(22,entity.getCheckedBy());
			prest.setLong(20, entity.getCreatedDate());

			prest.setDouble(21, entity.getSFIELD());
			prest.setLong(22, entity.getNUMDEFECT());

			int rst = prest.executeUpdate();

			if (rst > 0)
				rs = true;
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}

		return rs;
	}

	// wm_identification
	private static boolean InsertIdentificationEntity(
			IdentificationEntity entity) throws SQLException {
		boolean rs = false;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_identification ");
		lsquery
				.append("(identificationid, lot, operator, carrier_id, carrier_station, device, layer, ppname, modificationtime, lastauthor, substrate_id, substrate_number, substrate_diametermm, substrate_slot, substrate_type, substrate_fiducialtype, substrate_notchlocation )");
		lsquery
				.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		try {
			prest.setString(1, entity.getIdentificationId());
			prest.setString(2, entity.getLot());
			prest.setString(3, entity.getOperator());
			prest.setString(4, entity.getCarrier_ID());
			prest.setString(5, entity.getCarrier_Station());
			prest.setString(6, entity.getDevice());
			prest.setString(7, entity.getLayer());
			prest.setString(8, entity.getPPName());
			prest.setLong(9, entity.getModificationTime());
			prest.setString(10, entity.getLastAuthor());
			prest.setString(11, entity.getSubstrate_ID());
			prest.setString(12, entity.getSubstrate_Number());
			prest.setString(13, entity.getSubstrate_DiameterMM());
			prest.setString(14, entity.getSubstrate_Slot());
			prest.setString(15, entity.getSubstrate_Type());
			prest.setString(16, entity.getSubstrate_FiducialType());
			prest.setString(17, entity.getSubstrate_NotchLocation());

			int rst = prest.executeUpdate();

			if (rst > 0)
				rs = true;
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}

		return rs;
	}

	private static String GetClassificationSchemeId(String computername,
			String name) throws SQLException {
		String schemeid = "";
		String sql = "select schemeid from wm_classificationscheme t where t.computername=? and t.name=?";

		PreparedStatement prest = currConnect.prepareStatement(sql.toString());

		try {
			prest.setString(1, computername);
			prest.setString(2, name);

			ResultSet rs = prest.executeQuery();

			while (rs.next()) {
				schemeid = rs.getString(1);
			}
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		return schemeid;
	}

	private static boolean UpdateInspclassifiId(String schemeid,
			List<DefectListEntity> defectListEntity) throws SQLException {
		boolean rs = false;

		String sql = "select itemid,id from wm_classificationitem t where t.schemeid=?";

		PreparedStatement prest = currConnect.prepareStatement(sql.toString());

		try {
			prest.setString(1, schemeid);

			ResultSet rsItem = prest.executeQuery();

			while (rsItem.next()) {
				String itemid = rsItem.getString(1);
				int id = rsItem.getInt(2);

				for (DefectListEntity c : defectListEntity) {
					if (c.getClassificationitemId() == id) {
						c.setInspClassifiID(itemid);
					}
				}
			}
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		/*
		 * String sql =
		 * "update wm_defectlist t set t.inspclassifiid = (select c.itemid from wm_classificationitem c where c.id =t.id and c.schemeid=?) where exists (select 1 from wm_classificationitem c where c.id =t.id and c.schemeid=?)"
		 * ;
		 * 
		 * PreparedStatement prest =
		 * currConnect.prepareStatement(sql.toString());
		 * 
		 * prest.setString(1, schemeid); prest.setString(2, schemeid);
		 * 
		 * int rst = prest.executeUpdate();
		 * 
		 * if (rst > 0) rs = true;
		 */
		// prest.close();
		return rs;
	}

	// wm_classificationscheme
	private static boolean InsertClassificationSchemeEntity(
			ClassificationSchemeEntity entity) throws SQLException {
		boolean rs = false;

		String sql = "insert into wm_classificationscheme (schemeid, computername, name) values(?, ?, ?)";

		PreparedStatement prest = currConnect.prepareStatement(sql.toString());

		try {
			prest.setString(1, entity.getSchemeId());
			prest.setString(2, entity.getComputerName());
			prest.setString(3, entity.getName());

			int rst = prest.executeUpdate();

			if (rst > 0)
				rs = true;
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		// prest.close();
		return rs;
	}

	// wm_classificationitem
	private static boolean InsertClassificationItemEntity(
			List<ClassificationItemEntity> listClassificationItemEntity)
			throws SQLException {
		// boolean rs = false;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_classificationitem ");
		lsquery
				.append("(itemid, schemeid, id, name, description, color, priority, hotkey, isacceptable) ");
		lsquery.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		try {
			for (ClassificationItemEntity entity : listClassificationItemEntity) {
				prest.setString(1, entity.getItemId());
				prest.setString(2, entity.getSchemeId());
				prest.setLong(3, entity.getId());
				prest.setString(4, entity.getName());
				prest.setString(5, entity.getDescription());
				prest.setString(6, entity.getColor());
				prest.setLong(7, entity.getPriority());
				prest.setString(8, entity.getHotkey());
				prest.setString(9, entity.getIsAcceptable());
				// prest.setString(10, entity.getType());

				prest.addBatch();
			}

			prest.executeBatch();
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		// prest.close();
		return true;
	}

	// wm_inspectioninfo
	private static boolean InsertInspectionInfoEntity(
			InspectionInfoEntity entity) throws SQLException {
		boolean rs = false;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_inspectioninfo ");
		lsquery
				.append("(inspid, resultid, modulename, starttime, completiontime, device, layer, name, modificationtime, lastauthor, disposition, classificationschemeid, defectdensity, randomdefectdensity, defectratio, defectivearea, defectivedie, imagesdirectoryname,recipe_id,inspecteddie) ");
		lsquery
				.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		try {
			prest.setString(1, entity.getInspId());
			prest.setString(2, entity.getResultId());
			prest.setString(3, entity.getModuleName());
			prest.setDouble(4, entity.getStartTime());
			prest.setDouble(5, entity.getCompletionTime());
			prest.setString(6, entity.getDevice());
			prest.setString(7, entity.getLayer());
			prest.setString(8, entity.getName());
			prest.setLong(9, entity.getModificationTime());
			prest.setString(10, entity.getLastAuthor());
			prest.setString(11, entity.getDisposition());
			prest.setString(12, entity.getClassificationSchemeID());
			prest.setDouble(13, entity.getDefectDensity());
			prest.setDouble(14, entity.getRandomDefectDensity());
			prest.setDouble(15, entity.getDefectRatio());
			prest.setDouble(16, entity.getDefectiveArea());
			prest.setDouble(17, entity.getDefectiveDie());
			prest.setString(18, entity.getImagesDirectoryName());
			prest.setString(19, entity.getRecipe_id());
			prest.setLong(20, entity.getInspectedDie());

			int rst = prest.executeUpdate();

			if (rst > 0)
				rs = true;
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		// prest.close();
		return rs;
	}

	private static boolean UpdateInspectionInfoEntity(
			InspectionInfoEntity entity) throws SQLException {
		boolean rs = false;

		StringBuffer lsquery = new StringBuffer();
		StringBuffer lsquery1 = new StringBuffer();
		StringBuffer lsquery2 = new StringBuffer();
		StringBuffer lsquery3 = new StringBuffer();
		StringBuffer lsquery4 = new StringBuffer();
		/*
		 * lsquery.append("update wm_inspectioninfo t set (t.maska_die,t.maskb_die,t.maskc_die,t.maskd_die,t.maske_die, "
		 * );lsquery.append(
		 * "t.maska_defect,t.maskb_defect,t.maskc_defect,t.maskd_defect,t.maske_defect)= "
		 * );
		 * lsquery.append("(select sum(decode(d.masktype, 'A', 1, 0)) maska_die , "
		 * ); lsquery.append("sum(decode(d.masktype, 'B', 1, 0)) maskb_die , ");
		 * lsquery.append("sum(decode(d.masktype, 'C', 1, 0)) maskc_die , ");
		 * lsquery.append("sum(decode(d.masktype, 'D', 1, 0)) maskd_die , ");
		 * lsquery.append("sum(decode(d.masktype, 'E', 1, 0)) maske_die , ");
		 * lsquery.append(
		 * "sum(case when d.masktype = 'A' and c.id <> 0 then 1 else 0 end) maska_defect , "
		 * );lsquery.append(
		 * "sum(case when d.masktype = 'B' and c.id <> 0 then 1 else 0 end) maskb_defect , "
		 * );lsquery.append(
		 * "sum(case when d.masktype = 'C' and c.id <> 0 then 1 else 0 end) maskc_defect , "
		 * );lsquery.append(
		 * "sum(case when d.masktype = 'D' and c.id <> 0 then 1 else 0 end) maskd_defect , "
		 * );lsquery.append(
		 * "sum(case when d.masktype = 'E' and c.id <> 0 then 1 else 0 end) maske_defect "
		 * ); lsquery.append("from wm_defectlist d ");
		 * lsquery.append("inner join wm_classificationitem c ");
		 * lsquery.append("on c.itemid = d.inspclassifiid ");
		 * lsquery.append("where d.inspid = t.inspid) ");lsquery.append(
		 * "where exists(select 1 from wm_defectlist d inner join wm_classificationitem c on c.itemid = d.inspclassifiid "
		 * ); lsquery.append("where d.inspid = t.inspid) and t.inspid=? ");
		 */

		if (dbType == 1) {
			lsquery.append("update wm_inspectioninfo t INNER JOIN ");
			lsquery
					.append("(select d.inspid, sum(case d.masktype when 'A' then 1 else 0 end) maska_die , ");
			lsquery
					.append("sum(case d.masktype when 'B' then 1 else 0 end) maskb_die , ");
			lsquery
					.append("sum(case d.masktype when 'C' then 1 else 0 end) maskc_die , ");
			lsquery
					.append("sum(case d.masktype when 'D' then 1 else 0 end) maskd_die , ");
			lsquery
					.append("sum(case d.masktype when 'E' then 1 else 0 end) maske_die ");
			lsquery.append("from wm_defectlist" + yearMonth + " d ");
			// lsquery.append("inner join wm_classificationitem c ");
			lsquery.append("where d.RESULTID=? ");
			lsquery.append("GROUP BY d.inspid) m ");
			lsquery.append("on m.inspid = t.inspid ");
			lsquery
					.append("SET t.maska_die = m.maska_die,t.maskb_die = m.maskb_die,t.maskc_die = m.maskc_die,t.maskd_die = m.maskd_die,t.maske_die = m.maske_die ");
			// lsquery.append("where t.inspid=?");
			lsquery.append("where t.RESULTID=?");

			// maska_defect
			lsquery1.append("update wm_inspectioninfo t INNER JOIN ");
			lsquery1
					.append("(select inspid,sum(case when d.masktype = 'A' then cnt else 0 end) maska_defect, ");
			lsquery1
					.append("sum(case when d.masktype = 'B' then cnt else 0 end) maskb_defect,");
			lsquery1
					.append("sum(case when d.masktype = 'C' then cnt else 0 end) maskc_defect, ");
			lsquery1
					.append("sum(case when d.masktype = 'D' then cnt else 0 end) maskd_defect, ");
			lsquery1
					.append("sum(case when d.masktype = 'E' then cnt else 0 end) maske_defect ");
			lsquery1
					.append("from (select de.inspid,de.masktype, count(distinct de.dieaddress) cnt from wm_defectlist"
							+ yearMonth + " de ");
			lsquery1.append("inner join wm_classificationitem c ");
			lsquery1.append("on c.itemid = de.inspclassifiid ");
			lsquery1.append("where de.RESULTID=? and c.id<>'0' ");
			lsquery1
					.append("group by de.masktype) d) m  on m.inspid = t.inspid ");
			lsquery1
					.append("SET t.maska_defect = m.maska_defect,t.maskb_defect = m.maskb_defect,t.maskc_defect = m.maskc_defect,t.maskd_defect = m.maskd_defect,t.maske_defect = m.maske_defect ");
			lsquery1.append("where t.RESULTID=?");
		} else {
			// maska_die
			lsquery
					.append("update wm_inspectioninfo t set (t.maska_die,t.maskb_die,t.maskc_die,t.maskd_die,t.maske_die)= ");
			lsquery
					.append("(select sum(case d.masktype when 'A' then 1 else 0 end) maska_die , ");
			lsquery
					.append("sum(case d.masktype when 'B' then 1 else 0 end) maskb_die , ");
			lsquery
					.append("sum(case d.masktype when 'C' then 1 else 0 end) maskc_die , ");
			lsquery
					.append("sum(case d.masktype when 'D' then 1 else 0 end) maskd_die , ");
			lsquery
					.append("sum(case d.masktype when 'E' then 1 else 0 end) maske_die ");
			lsquery.append("from wm_defectlist" + yearMonth + " d ");
			lsquery.append("inner join wm_classificationitem c ");
			lsquery.append("on c.itemid = d.inspclassifiid ");
			lsquery.append("where d.inspid = t.inspid) ");
			lsquery
					.append("where exists(select 1 from wm_defectlist"
							+ yearMonth
							+ " d inner join wm_classificationitem c on c.itemid = d.inspclassifiid ");
			lsquery.append("where d.inspid = t.inspid) and t.RESULTID=? ");

			// maska_defect
			lsquery1
					.append("update wm_inspectioninfo t set (t.maska_defect,t.maskb_defect,t.maskc_defect,t.maskd_defect,t.maske_defect)= ");
			lsquery1
					.append("(select sum(case when d.masktype = 'A' then cnt else 0 end) maska_defect, ");
			lsquery1
					.append("sum(case when d.masktype = 'B' then cnt else 0 end) maskb_defect,");
			lsquery1
					.append("sum(case when d.masktype = 'C' then cnt else 0 end) maskc_defect, ");
			lsquery1
					.append("sum(case when d.masktype = 'D' then cnt else 0 end) maskd_defect, ");
			lsquery1
					.append("sum(case when d.masktype = 'E' then cnt else 0 end) maske_defect ");
			lsquery1
					.append("from (select de.masktype, count(distinct de.dieaddress) cnt from wm_defectlist"
							+ yearMonth + " de ");
			lsquery1.append("inner join wm_classificationitem c ");
			lsquery1.append("on c.itemid = de.inspclassifiid ");
			lsquery1.append("where de.inspid=? and c.id<>'0' ");
			lsquery1.append("group by de.masktype) d) where t.inspid=?");
		}

		// maska_die
		lsquery2
				.append("update wm_inspectioninfo t set t.maska_die= case when instr(?,'A')>=1 or t.maska_die>0 then t.maska_die end,");
		lsquery2
				.append("t.maskb_die =case when instr(?,'B')>=1 or t.maskb_die>0 then t.maskb_die end,");
		lsquery2
				.append("t.maskc_die =case when instr(?,'C')>=1 or t.maskc_die>0 then t.maskc_die end,");
		lsquery2
				.append("t.maskd_die=case when instr(?,'D')>=1 or t.maskd_die>0 then t.maskd_die end,");
		lsquery2
				.append("t.maske_die=case when instr(?,'E')>=1 or t.maske_die>0 then t.maske_die end");
		lsquery2.append(" where t.RESULTID=?");

		// defectivedie
		lsquery3.append("update wm_inspectioninfo t ");
		lsquery3
				.append("set t.defectivedie =(select count(distinct de.dieaddress) ");
		lsquery3
				.append("from wm_defectlist"
						+ yearMonth
						+ " de inner join wm_classificationitem c on c.itemid = de.inspclassifiid ");
		lsquery3.append("where de.RESULTID = ? and c.id <> '0') ");
		lsquery3.append("where t.RESULTID = ? ");

		// numdefect
		lsquery4.append("update wm_waferresult t ");
		lsquery4
				.append("set t.numdefect =(select f.defectivedie from wm_inspectioninfo f where f.resultid=?), ");
		lsquery4
				.append(" t.sfield = (select case when round((f.inspecteddie-f.defectivedie)*100/f.inspecteddie,2)=100 and f.defectivedie>0 then 99.99 else round((f.inspecteddie-f.defectivedie)*100/f.inspecteddie,2) end from wm_inspectioninfo f where f.resultid=?)");
		lsquery4.append("where t.resultid = ? ");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		PreparedStatement prest1 = currConnect.prepareStatement(lsquery1
				.toString());

		PreparedStatement prest2 = currConnect.prepareStatement(lsquery2
				.toString());

		PreparedStatement prest3 = currConnect.prepareStatement(lsquery3
				.toString());

		PreparedStatement prest4 = currConnect.prepareStatement(lsquery4
				.toString());

		try {

			if (dbType == 1) {
				prest.setString(1, entity.getResultId());
				prest.setString(2, entity.getResultId());

				prest1.setString(1, entity.getResultId());
				prest1.setString(2, entity.getResultId());
			} else {
				prest.setString(1, entity.getInspId());

				prest1.setString(1, entity.getInspId());
				prest1.setString(2, entity.getInspId());
			}

			prest2.setString(1, entity.getMaskArray());
			prest2.setString(2, entity.getMaskArray());
			prest2.setString(3, entity.getMaskArray());
			prest2.setString(4, entity.getMaskArray());
			prest2.setString(5, entity.getMaskArray());
			prest2.setString(6, entity.getResultId());

			prest3.setString(1, entity.getResultId());
			prest3.setString(2, entity.getResultId());

			prest4.setString(1, entity.getResultId());
			prest4.setString(2, entity.getResultId());
			prest4.setString(3, entity.getResultId());

			LogWriter.getWriter().AddLogItem(
					entity.getInspId() + "|" + entity.getMaskArray() + "|"
							+ entity.getResultId());

			int rst = prest.executeUpdate();
			// LogWriter.getWriter().AddLogItem(lsquery.toString());
			rst = prest1.executeUpdate();
			// LogWriter.getWriter().AddLogItem(lsquery1.toString());
			rst = prest2.executeUpdate();
			// LogWriter.getWriter().AddLogItem(lsquery2.toString());
			rst = prest3.executeUpdate();
			// LogWriter.getWriter().AddLogItem(lsquery3.toString());
			rst = prest4.executeUpdate();
			// LogWriter.getWriter().AddLogItem(lsquery4.toString());

			if (rst > 0)
				rs = true;
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
			prest1.close();
			prest2.close();
			prest3.close();
			prest4.close();
		}
		// prest.close();
		return rs;
	}

	// wm_dielayout
	private static boolean InsertDieLayoutEntity(DieLayoutEntity entity,
			List<DieLayoutListEntity> dieLayoutListEntity) throws SQLException {
		boolean rs = false;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_dielayout ");
		lsquery
				.append("(layoutid, dieaddressrange, anchordie, size_, pitch, dieaddress, swcscoordinates, dieaddressincrement, columns_, rows_,layoutdetails) ");
		lsquery.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		try {
			String msg = String.format(
					"%s,Serializer LayoutListEntity.........", Utils
							.GetCurrDateTime());
			LogWriter.getWriter().AddLogItem(msg);

			Blob blob = currConnect.createBlob();
			String jsonString = JSONArray.toJSONString(dieLayoutListEntity);
			blob.setBytes(1, jsonString.getBytes());

			// blob.setBytes(1, layoutList.toString().getBytes());
			prest.setString(1, entity.getLayoutId());
			prest.setString(2, entity.getDieAddressRange());
			prest.setString(3, entity.getAnchorDie());
			prest.setString(4, entity.getSize());
			prest.setString(5, entity.getPitch());
			prest.setString(6, entity.getDieAddress());
			prest.setString(7, entity.getSWCSCoordinates());
			prest.setString(8, entity.getDieAddressIncrement());
			prest.setLong(9, entity.getColumns());
			prest.setLong(10, entity.getRows());
			prest.setBlob(11, blob);

			int rst = prest.executeUpdate();

			if (rst > 0)
				rs = true;

		} catch (SQLException ex) {
			throw ex;
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			prest.close();
		}
		// prest.close();
		return rs;
	}

	// wm_dielayoutlist
	private static boolean InsertDieLayoutListEntity(
			List<DieLayoutListEntity> dieLayoutListEntity) throws SQLException {
		// boolean rs = false;
		int count = 0;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_dielayoutlist" + yearMonth);
		lsquery
				.append(" (id, layoutid, dieaddressx, dieaddressy, disposition, inspclassifiid, reviewclassifcationid, adccid, psdcid, isinspectable, isedgedie) ");
		lsquery.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		try {
			for (DieLayoutListEntity entity : dieLayoutListEntity) {
				prest.setString(1, entity.getId());
				prest.setString(2, entity.getLayoutId());
				prest.setInt(3, entity.getDieAddressX());
				prest.setInt(4, entity.getDieAddressY());
				prest.setString(5, entity.getDisposition());
				prest.setString(6, entity.getInspClassifiID());
				prest.setString(7, entity.getReviewClassifcationID());
				prest.setString(8, entity
						.getAutoDieClassifierClassificationID());
				prest.setString(9, entity
						.getProcessSentinelDieClassificationID());
				prest.setString(10, entity.getIsInspectable());
				prest.setString(11, entity.getIsEdgeDie());

				prest.addBatch();

				count = count + 1;
				if (count >= 50000) {
					prest.executeBatch();
					prest.clearBatch();
					count = 0;
				}
			}

			prest.executeBatch();
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		// prest.close();
		return true;
	}

	// wm_inspectionpass
	private static boolean InsertInspectionPassEntity(
			InspectionPassEntity entity) throws SQLException {
		boolean rs = false;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_inspectionpass ");
		lsquery
				.append("(passid, inspid, orientation, inspectedsurface, inspectiontype, defectdensity, defectivearea, defectratio, randomdefectdensity, defectivedie) ");
		lsquery.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		try {
			prest.setInt(1, entity.getPassId());
			prest.setString(2, entity.getInspId());
			prest.setString(3, entity.getOrientation());
			prest.setString(4, entity.getInspectedSurface());
			prest.setString(5, entity.getInspectionType());
			prest.setDouble(6, entity.getDefectDensity());
			prest.setDouble(7, entity.getDefectiveArea());
			prest.setDouble(8, entity.getDefectRatio());
			prest.setDouble(9, entity.getRandomDefectDensity());
			prest.setInt(10, entity.getDefectiveDie());

			int rst = prest.executeUpdate();

			if (rst > 0)
				rs = true;
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		// prest.close();
		return rs;
	}

	// wm_inspecteddielist
	private static boolean InsertInspectedDieListEntity(
			List<InspectedDieListEntity> inspectedDieListEntity)
			throws SQLException {
		// boolean rs = false;
		StringBuffer lsquery = new StringBuffer();
		
		if (dbType == 1) {
			lsquery.append("insert into wm_inspecteddielist" + yearMonth);
			/*
			 * lsquery.append(
			 * " (inspecteddieid, passid, inspid, dieaddress, classificationid, disposition) "
			 * ); lsquery.append("values (?, ?, ?, ?, ?, ?)");
			 */
			lsquery
					.append(" (inspecteddieid, passid, inspid, dieaddress, classificationid, disposition,dielist,dielistcount) ");
			lsquery.append("values (?, ?, ?, ?, ?, ?, ?, ?)");
		}
		else
		{
			lsquery.append("insert into wm_inspecteddielist" + yearMonth);

			lsquery
					.append(" (inspecteddieid, passid, inspid, dieaddress, classificationid, disposition) ");
			lsquery.append("values (?, ?, ?, ?, ?, ?)");
		}
		
		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());
		
		try {
			
			if (dbType == 1) {
				if (inspectedDieListEntity.size() > 0) {
					String msg = String.format(
							"%s,Serializer InspectedDieListEntity.........",
							Utils.GetCurrDateTime());
					LogWriter.getWriter().AddLogItem(msg);

					Blob blob = currConnect.createBlob();
					String jsonString = JSONArray
							.toJSONString(inspectedDieListEntity);
					blob.setBytes(1, jsonString.getBytes());

					prest.setString(1, inspectedDieListEntity.get(0)
							.getInspectedDieId());
					prest.setInt(2, inspectedDieListEntity.get(0).getPassId());
					prest.setString(3, inspectedDieListEntity.get(0)
							.getInspId());
					prest.setString(4, inspectedDieListEntity.get(0)
							.getDieAddress());
					prest.setString(5, inspectedDieListEntity.get(0)
							.getClassificationID());
					prest.setString(6, inspectedDieListEntity.get(0)
							.getDisposition());
					prest.setBlob(7, blob);
					prest.setInt(8, inspectedDieListEntity.size());

					prest.executeUpdate();
				}
			} else {
				for (InspectedDieListEntity entity : inspectedDieListEntity) {
					prest.setString(1, entity.getInspectedDieId());
					prest.setInt(2, entity.getPassId());
					prest.setString(3, entity.getInspId());
					prest.setString(4, entity.getDieAddress());
					prest.setString(5, entity.getClassificationID());
					prest.setString(6, entity.getDisposition());

					prest.addBatch();
				}

				prest.executeBatch();
			}

		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		// prest.close();
		return true;
	}

	// wm_defectlist
	private static boolean InsertDefectListEntity(
			List<DefectListEntity> defectListEntity) throws SQLException {
		// boolean rs = false;

		StringBuffer lsquery = new StringBuffer();

		lsquery.append("insert into wm_defectlist" + yearMonth);
		lsquery
				.append(" (id, passid, inspid, inspectiontype, swcscoordinates, inspclassifiid, size_, majoraxissize, majorminoraxisaspectratio, area_, dieaddress, imagename, style, pixelsize,resultid,masktype) ");
		lsquery
				.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

		PreparedStatement prest = currConnect.prepareStatement(lsquery
				.toString());

		try {
			for (DefectListEntity entity : defectListEntity) {
				prest.setInt(1, entity.getId());
				prest.setLong(2, entity.getPassId());
				prest.setString(3, entity.getInspId());
				prest.setString(4, entity.getInspectionType());
				prest.setString(5, entity.getSWCSCoordinates());
				prest.setString(6, entity.getInspClassifiID());
				prest.setString(7, entity.getSize());
				prest.setString(8, entity.getMajorAxisSize());
				prest.setString(9, entity.getMajorMinorAxisAspectRatio());
				prest.setString(10, entity.getArea());
				prest.setString(11, entity.getDieAddress());
				prest.setString(12, entity.getImageName());
				prest.setString(13, entity.getStyle());
				prest.setString(14, entity.getPixelSize());
				// prest.setString(15, entity.getIsChecked());
				// prest.setString(16, entity.getCheckedBy());
				// prest.setLong(17, entity.getCheckedDate());
				// prest.setString(18, entity.getModifiedDefect());
				prest.setString(15, entity.getResultId());
				prest.setString(16, entity.getMaskType());

				prest.addBatch();
			}

			prest.executeBatch();
		} catch (SQLException ex) {
			throw ex;
		} finally {
			prest.close();
		}
		// prest.close();
		return true;
	}

	/*
	 * private static boolean UpdateDieInfo(String schemeid,
	 * List<DefectListEntity> defectListEntity) throws SQLException { boolean rs
	 * = false;
	 * 
	 * String sql =
	 * "select itemid,id from wm_classificationitem t where t.schemeid=?";
	 * 
	 * PreparedStatement prest = currConnect.prepareStatement(sql.toString());
	 * 
	 * try { prest.setString(1, schemeid);
	 * 
	 * ResultSet rsItem = prest.executeQuery();
	 * 
	 * while (rsItem.next()) { String itemid = rsItem.getString(1); int id =
	 * rsItem.getInt(2);
	 * 
	 * for (DefectListEntity c : defectListEntity) { if
	 * (c.getClassificationitemId() == id) { c.setInspClassifiID(itemid); } } }
	 * } catch (SQLException ex) { throw ex; } finally { prest.close(); }
	 * 
	 * return rs; }
	 */

	private static boolean ThreadInsertDieLayoutListEntity(
			List<DieLayoutListEntity> dieLayoutListEntity)
			throws InterruptedException {

		finishCount = 0;
		long starttime = System.currentTimeMillis();
		/*
		 * ThreadPoolExecutor executor = new ScheduledThreadPoolExecutor(
		 * threadSize);
		 */
		// CountDownLatch cdl = new CountDownLatch(threadSize);
		/*
		 * ThreadDieLayout threadDieLayout = new OracleHelper().new
		 * ThreadDieLayout( cdl, dieLayoutListEntity);
		 */

		CountDownLatch cdl = new CountDownLatch(threadSize);
		int step = (dieLayoutListEntity.size() - dieLayoutListEntity.size()
				% threadSize)
				/ threadSize;
		int stepCnt;

		for (int i = 0; i < threadSize; i++) {
			// new Thread(threadDieLayout).start();
			if (i == threadSize - 1) {
				stepCnt = step * (i + 1) + dieLayoutListEntity.size()
						% threadSize;
			} else {
				stepCnt = step * (i + 1);
			}

			/*
			 * System.out.println("step*i:" + step * i);
			 * System.out.println("stepCnt:" + stepCnt);
			 */

			executor.execute(new OracleHelper().new ThreadDieLayout(
					dieLayoutListEntity, stepCnt, step * i, cdl));
		}

		cdl.await();
		// executor.shutdown();
		long spendtime = System.currentTimeMillis() - starttime;
		// System.out.println("InsertDieLayoutList a total of " + spendtime);
		LogWriter.getWriter().AddLogItem(
				"InsertDieLayoutList a total of " + spendtime / 1000
						+ " seconds," + finishCount);

		return finishCount == threadSize;
	}

	public class ThreadDieLayout implements Runnable {
		List<DieLayoutListEntity> dieLayoutListEntity;
		int listCnt;
		int index;
		CountDownLatch latch;

		public ThreadDieLayout(List<DieLayoutListEntity> dieLayoutListEntity,
				int listCnt, int index, CountDownLatch latch) {
			this.dieLayoutListEntity = dieLayoutListEntity;

			this.listCnt = listCnt;
			this.index = index;
			this.latch = latch;
		}

		public void run() {
			StringBuffer lsquery = new StringBuffer();

			lsquery.append("insert into wm_dielayoutlist" + yearMonth);
			lsquery
					.append(" (id, layoutid, dieaddressx, dieaddressy, disposition, inspclassifiid, reviewclassifcationid, adccid, psdcid, isinspectable, isedgedie) ");
			lsquery.append("values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)");

			PreparedStatement prest = null;

			try {
				// prest = currConnect.prepareStatement(lsquery.toString());
				prest = getThreadLocalConnection().prepareStatement(
						lsquery.toString());
			} catch (SQLException e) {
				e.printStackTrace();
				LogWriter.getWriter().AddLogItem(e.getMessage());
			}

			try {
				/*
				 * System.out.println(Thread.currentThread().getName() + ":" +
				 * index);
				 */
				// while (index < LayoutListEntity.size()) {
				for (int i = index; i < listCnt; i++) {
					DieLayoutListEntity entity = dieLayoutListEntity.get(i);

					prest.setString(1, entity.getId());
					prest.setString(2, entity.getLayoutId());
					prest.setInt(3, entity.getDieAddressX());
					prest.setInt(4, entity.getDieAddressY());
					prest.setString(5, entity.getDisposition());
					prest.setString(6, entity.getInspClassifiID());
					prest.setString(7, entity.getReviewClassifcationID());
					prest.setString(8, entity
							.getAutoDieClassifierClassificationID());
					prest.setString(9, entity
							.getProcessSentinelDieClassificationID());
					prest.setString(10, entity.getIsInspectable());
					prest.setString(11, entity.getIsEdgeDie());

					prest.addBatch();

					// index = GetIndex();

					if (i != 0 && i % maxInsertCount == 0) {
						LogWriter.getWriter().AddLogItem(
								Thread.currentThread().getName() + ":"
										+ Utils.GetCurrDateTime() + " ["
										+ index + "," + i + "] LayoutId:"
										+ entity.getLayoutId()
										+ " start insert database");
						prest.executeBatch();
						prest.clearBatch();
						LogWriter.getWriter().AddLogItem(
								Thread.currentThread().getName() + ":"
										+ Utils.GetCurrDateTime() + " ["
										+ index + "," + i + "] LayoutId:"
										+ entity.getLayoutId()
										+ " finish insert database");
					}
				}

				LogWriter.getWriter().AddLogItem(
						Thread.currentThread().getName() + ":"
								+ Utils.GetCurrDateTime() + " [" + index + ","
								+ listCnt + "] start insert database");

				prest.executeBatch();
				finishCount++;
				LogWriter.getWriter().AddLogItem(
						Thread.currentThread().getName() + ":"
								+ Utils.GetCurrDateTime() + " [" + index + ","
								+ listCnt + "] finish insert database");
			} catch (SQLException e) {
				closeThreadLocalConnection();
				e.printStackTrace();
				LogWriter.getWriter().AddLogItem(e.getMessage());
			} catch (Exception e) {
				closeThreadLocalConnection();
				e.printStackTrace();
				LogWriter.getWriter().AddLogItem(e.getMessage());
			} finally {
				try {
					prest.close();
					latch.countDown();

					// closeThreadLocalConnection();
				} catch (SQLException e) {
					e.printStackTrace();
					LogWriter.getWriter().AddLogItem(
							"finally:" + e.getMessage());
				} catch (Exception e) {
					e.printStackTrace();
					LogWriter.getWriter().AddLogItem(
							"finally:" + e.getMessage());
				}
			}
		}
	}
}