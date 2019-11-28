package dataTransferServer.Base;

import java.util.Date;
import java.util.Properties;
import java.io.IOException;
import java.text.ParseException;
import java.text.SimpleDateFormat;

public class Utils {

	public static String NewGuid() {
		return java.util.UUID.randomUUID().toString();
	}

	@SuppressWarnings("deprecation")
	public static long DataTimeFormat(String datetime) {
		if (datetime == null || datetime.length() <= 0) {
			return 0;
		}
		SimpleDateFormat simpleDateFormat = new SimpleDateFormat(
				"yyyyMMddHHmmss");
		return Long.parseLong(simpleDateFormat.format(new Date(datetime)));
	}

	public static long GetCurrDate() {
		SimpleDateFormat simpleDateFormat = new SimpleDateFormat(
				"yyyyMMddHHmmss");
		return Long.parseLong(simpleDateFormat.format(new Date()));
	}

	public static String GetCurrDateTime() {
		SimpleDateFormat simpleDateFormat = new SimpleDateFormat(
				"yyyy-MM-dd HH:mm:ss");
		return simpleDateFormat.format(new Date());
	}

	public static void ResetSystem(boolean isNowReset) throws IOException {
		Properties properties = PropertyManager.getProperties();
		boolean isReset = false;

		if (isNowReset) {
			LogWriter.getWriter().AddLogItem("error:reset system......");

			try {
				Thread.sleep(10000);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			// properties.setProperty("LastResetTime", GetCurrDateTime());
			String userPath = System.getProperty("user.dir");

			Runtime.getRuntime().exec("cmd /c start " + userPath + "\\run.bat");

			System.exit(0);

		} else {
			if (properties != null) {
				String lastResetTime = properties.getProperty("LastResetTime");

				if (lastResetTime != null) {
					SimpleDateFormat simpleDateFormat = new SimpleDateFormat(
							"yyyy-MM-dd HH:mm:ss");
					Date dResetTime;

					try {
						dResetTime = simpleDateFormat.parse(lastResetTime);

						long diff = (new Date().getTime() - dResetTime
								.getTime()) / 60000;

						if (diff > 60 * 12) {
							isReset = true;
						}
					} catch (ParseException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			}

			if (isReset) {

				LogWriter.getWriter().AddLogItem("info:reset system......");

				try {
					Thread.sleep(10000);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

				//properties.setProperty("LastResetTime", GetCurrDateTime());
				String userPath = System.getProperty("user.dir");

				Runtime.getRuntime().exec(
						"cmd /c start " + userPath + "\\run.bat");

				System.exit(0);
			}
		}
	}

	public static String LastLayer;
	public static String LastLotName;
	public static String LotId;
	public static long LotDieNum;
	public static long ReImportNum;// 重复导入次数
	public static boolean IsReImport = false; // 是否重复导入
	public static String DeviceName;
	public static boolean IsWriteLog = true; // 是否生成日志
}
