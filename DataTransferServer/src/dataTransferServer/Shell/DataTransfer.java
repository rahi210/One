package dataTransferServer.Shell;

import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Observable;
import java.util.Properties;
import java.util.concurrent.Executors;
import oracle.jdbc.driver.OracleDriver;

import dataTransferServer.DataAccess.*;
import dataTransferServer.Base.PropertyManager;
import dataTransferServer.Base.LogWriter;
import dataTransferServer.Base.Utils;

import org.dom4j.DocumentException;

public class DataTransfer extends Observable implements Runnable {

	public void doBusiness() {
		if (true) {
			super.setChanged();
		}
		notifyObservers();
	}

	public void run() {
		boolean isrun = true;

		LogWriter.getWriter().start();

		try {

			SimpleDateFormat df1 = new SimpleDateFormat("yyyy-MM-dd");// 设置日期格式
			SimpleDateFormat df2 = new SimpleDateFormat("HH:mm:ss");// 设置日期格式
			SimpleDateFormat df3 = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");// 设置日期格式

			// System.out.println("------------------------------------------------");
			System.out
					.println("                        *******************************");
			System.out
					.println("                        *                             *");
			System.out
					.println("                        *   DataTransferServer        *");
			System.out
					.println("                        *   Version：1.1.9.0091       *");
			System.out
					.println("                        *******************************");
			System.out.println();
			System.out.println("            	   	 start date:"
					+ df1.format(new Date()) + "       time:"
					+ df2.format(new Date()));
			System.out.println();

			Properties properties = PropertyManager.getProperties();

			if (properties != null) {
				System.out
						.println("----------------------------------------------------------------------------");

				String connectionString = properties
						.getProperty("ConnectionString");
				String userName = properties.getProperty("UserName");
				String password = properties.getProperty("Password");

				if (connectionString.indexOf("mysql") > 0) {
					Class.forName("com.mysql.jdbc.Driver");

					OracleHelper.dbType = 1;
				} else {
					DriverManager.registerDriver(new OracleDriver());

					OracleHelper.dbType = 0;
				}

				Connection currConnect = DriverManager.getConnection(
						connectionString, userName, password);
				DriverManager.setLoginTimeout(10);
				OracleHelper.SetConnection(currConnect);

				// Thread Size
				/*
				 * String threadSize = properties.getProperty("ThreadSize"); if
				 * (threadSize != null) { OracleHelper.threadSize =
				 * Integer.parseInt(threadSize); }
				 * 
				 * String maxInsertCount = properties
				 * .getProperty("MaxInsertCount"); if (maxInsertCount != null) {
				 * OracleHelper.maxInsertCount = Integer
				 * .parseInt(maxInsertCount); }
				 */

				String deviceName = properties.getProperty("DeviceName");
				if (deviceName != null) {
					Utils.DeviceName = deviceName;
				}
				
				String isWriteLog = properties.getProperty("IsWriteLog");
				if (isWriteLog != null) {
					Utils.IsWriteLog =Boolean.parseBoolean(isWriteLog);
				}
				
				properties
						.setProperty("LastResetTime", Utils.GetCurrDateTime());

				System.out.println(df3.format(new Date())
						+ "  has been successfully run-->>>");
				LogWriter
						.getWriter()
						.AddLogItem(
								df3.format(new Date())
										+ "  has been successfully run,Version：1.1.9.0091-->>>");
			} else {
				System.out
						.println("----------------------------------------------------------------------------");
				System.out.println();
				System.out
						.println("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
				// throw new IOException("配置文件不存在，请检查");
				System.out
						.println("		    Startup failed: the configuration file does not exist, please check it");
				System.out
						.println("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
				LogWriter
						.getWriter()
						.AddLogItem(
								"Startup failed: the configuration file does not exist, please check it");
			}

		} catch (SQLException e) {
			e.printStackTrace();
			// LogWriter.getWriter().AddLogItem(e.getStackTrace());
			StringBuffer sb = new StringBuffer();
			StackTraceElement[] stackArray = e.getStackTrace();
			for (int i = 0; i < stackArray.length; i++) {
				StackTraceElement element = stackArray[i];
				sb.append(element.toString() + "\n");
			}

			LogWriter.getWriter().AddLogItem(sb.toString());
			isrun = false;

		} catch (Exception e) {
			e.printStackTrace();

			StringBuffer sb = new StringBuffer();
			StackTraceElement[] stackArray = e.getStackTrace();
			for (int i = 0; i < stackArray.length; i++) {
				StackTraceElement element = stackArray[i];
				sb.append(element.toString() + "\n");
			}

			LogWriter.getWriter().AddLogItem(sb.toString());
			isrun = false;
		}

		// OracleHelper.executor = new
		// ScheduledThreadPoolExecutor(OracleHelper.threadSize);
		/*
		 * OracleHelper.executor = Executors
		 * .newFixedThreadPool(OracleHelper.threadSize);
		 */

		if (isrun == false) {
			try {

				Utils.ResetSystem(true);
			} catch (IOException e) {
				e.printStackTrace();

				StringBuffer sb = new StringBuffer();
				StackTraceElement[] stackArray = e.getStackTrace();
				for (int i = 0; i < stackArray.length; i++) {
					StackTraceElement element = stackArray[i];
					sb.append(element.toString() + "\n");
				}

				LogWriter.getWriter().AddLogItem(sb.toString());
			}
		}

		while (isrun) {
			try {
				// Runtime.getRuntime().exec("cmd /c start "+" run.bat");

				Properties properties = PropertyManager.getProperties();

				String filepath = properties.getProperty("FilePath");
				String fileName = properties.getProperty("FileName");
				String destFilePath = properties.getProperty("DestFilePath");
				String[] filePathArray = filepath.split("\\|");

				OracleHelper.InsertLog();
				
				for (String f : filePathArray) {
					new XmlSerialize(f, fileName, destFilePath);
				}
				
				// 判断是否需要重启
				Utils.ResetSystem(false);

				Thread.sleep(1000);

			} catch (SQLException e) {
				e.printStackTrace();
				// LogWriter.getWriter().AddLogItem(e.getStackTrace());
				StringBuffer sb = new StringBuffer();
				StackTraceElement[] stackArray = e.getStackTrace();
				for (int i = 0; i < stackArray.length; i++) {
					StackTraceElement element = stackArray[i];
					sb.append(element.toString() + "\n");
				}

				LogWriter.getWriter().AddLogItem(sb.toString());
				// isrun = false;
			} catch (Exception e) {
				e.printStackTrace();
				// LogWriter.getWriter().AddLogItem(e.getStackTrace());
				StringBuffer sb = new StringBuffer();
				StackTraceElement[] stackArray = e.getStackTrace();
				for (int i = 0; i < stackArray.length; i++) {
					StackTraceElement element = stackArray[i];
					sb.append(element.toString() + "\n");
				}

				LogWriter.getWriter().AddLogItem(sb.toString());
				// isrun = false;
			}

			// isrun = false;
		}
	}

	/**
	 * @param args
	 * @throws DocumentException
	 */
	public static void main(String[] args) throws DocumentException {
		// TODO Auto-generated method stub
		DataTransfer dataTransfer = new DataTransfer();

		DataTransferListener listener = new DataTransferListener();
		dataTransfer.addObserver(listener);
		Thread t = new Thread(dataTransfer);

		t.setUncaughtExceptionHandler(new Thread.UncaughtExceptionHandler() {
			public void uncaughtException(Thread t, Throwable e) {
				try {
					System.out
							.println("uncaughtExceptionHandler catch a Exception---------");
					System.out.println(e.getMessage());

					LogWriter.getWriter().AddLogItem(
							"Exception:reset system......");
					LogWriter.getWriter().AddLogItem(e.getMessage());

					Thread.sleep(1000);
					String userPath = System.getProperty("user.dir");

					Runtime.getRuntime().exec(
							"cmd /c start " + userPath + "\\run.bat");
				} catch (IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
					// LogWriter.getWriter().AddLogItem(e.getStackTrace());
					StringBuffer sb = new StringBuffer();
					StackTraceElement[] stackArray = e1.getStackTrace();
					for (int i = 0; i < stackArray.length; i++) {
						StackTraceElement element = stackArray[i];
						sb.append(element.toString() + "\n");
					}

					LogWriter.getWriter().AddLogItem(sb.toString());
				} catch (InterruptedException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
					// LogWriter.getWriter().AddLogItem(e.getStackTrace());
					StringBuffer sb = new StringBuffer();
					StackTraceElement[] stackArray = e1.getStackTrace();
					for (int i = 0; i < stackArray.length; i++) {
						StackTraceElement element = stackArray[i];
						sb.append(element.toString() + "\n");
					}

					LogWriter.getWriter().AddLogItem(sb.toString());
				}

				System.exit(0);
			}
		});

		t.start();
	}
}
