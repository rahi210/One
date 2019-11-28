package dataTransferServer.Base;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.Properties;

public class LogWriter extends Thread {
	private boolean isRunning = false;
	private ArrayList<Object> logItemList = new ArrayList<Object>();
	private String filePath;
	private String fileName;
	private String fileExtendName;
	private String fullFileName;
	private int fileIndex = 0;
	private SimpleDateFormat dateFormat = new SimpleDateFormat(
			"yyyy.MM.dd HH:mm:ss");
	private double logFileLimitSize;
	private boolean writeLog = false;

	private LogWriter() {
		super();

		try {
			Properties properties = PropertyManager.getProperties();
			if (properties != null) {
				writeLog = Boolean.parseBoolean(properties.getProperty(
						"RunLog", "false"));
				filePath = properties.getProperty("LogFilePath", "Log");
				fileName = properties.getProperty("LogFileName", "log");
				fileExtendName = properties.getProperty("LogFileExtendName",
						"syslog");
				logFileLimitSize = Double.parseDouble(properties.getProperty(
						"LogLimitSize", "20"));

				File logFilePath = new File(filePath);
				if (!logFilePath.exists()) {
					logFilePath.mkdir();
				}

				// fullFileName = filePath + "\\" + fileName + fileIndex++ + "."
				// + fileExtendName;

				while (true) {
					fullFileName = filePath + "/" + fileName + fileIndex++
							+ "." + fileExtendName;
					File logFile = new File(fullFileName);
					if (!logFile.exists()) {
						logFile.createNewFile();
						break;
					}

				}

				// fullFileName = filePath + "\\" + fileName + fileIndex++ + "."
				// + fileExtendName;

				FileWriter fw = new FileWriter(fullFileName);
				Date currDate = new Date(System.currentTimeMillis());
				fw.write(dateFormat.format(currDate));
				fw.write("\t\t Successfully open log file£¡File path£º");
				fw.write(filePath);
				fw.write("\n");
				fw.close();
			} else {
				throw new IOException(
						"Config file should not be loaded! Please confirm the config file under the correct location.");
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	@Override
	public void run() {
		if (writeLog) {
			isRunning = true;

			while (isRunning) {
				WriteLog();
			}
		}

	}

	public void AddLogItem(Object object) {
		if (writeLog) {
			synchronized (logItemList) {
				logItemList.add(object);
			}
		}

	}

	private void WriteLog() {
		if (logItemList.size() != 0) {
			Object currItem = null;
			synchronized (logItemList) {
				currItem = logItemList.get(0);
			}

			FileWriter fw = null;
			try {
				fw = new FileWriter(fullFileName, true);
				Date currDate = new Date(System.currentTimeMillis());
				fw.write(dateFormat.format(currDate));
				if (currItem instanceof String) {
					String itemValue = (String) currItem;
					fw.write("\t\t information£º");
					fw.write(itemValue);
				} else {
					fw.write("\t\t Unsupported Log£º");
					fw.write(currItem.toString());
				}

				fw.write("\r\n");

				FileInputStream fis = null;
				try {
					fis = new FileInputStream(fullFileName);
					if (fis.available() >= logFileLimitSize * 1024 * 1024) {
						fullFileName = filePath + "/" + fileName + fileIndex++
								+ "." + fileExtendName;
					}
					fis.close();
				} catch (IOException e) {
					e.printStackTrace();
				} finally {
					if (fis != null) {
						try {
							fis.close();
						} catch (IOException e) {
							e.printStackTrace();
						}
					}
				}
			} catch (IOException e) {
				e.printStackTrace();
			} finally {
				if (fw != null) {
					try {
						fw.close();
					} catch (IOException e) {
						e.printStackTrace();
					}
				}
			}
			synchronized (logItemList) {
				logItemList.remove(currItem);
			}
		} else {
			try {
				Thread.sleep(10);
			} catch (InterruptedException e) {
				// e.printStackTrace();
				LogWriter.writer.AddLogItem(e.getStackTrace());
			}
		}
	}

	private static LogWriter writer = new LogWriter();

	public static LogWriter getWriter() {
		return writer;
	}
}
