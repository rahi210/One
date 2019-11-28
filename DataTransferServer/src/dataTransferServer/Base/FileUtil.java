package dataTransferServer.Base;

import java.io.File;
import java.io.FilenameFilter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Comparator;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

public class FileUtil {

	private static String MESSAGE = "";

	public static ArrayList<File> GetFiles(String dirName) {
		ArrayList<File> fileList = new ArrayList<File>();
		File dir = new File(dirName);

		File[] files = dir.listFiles();
		for (File f : files) {
			fileList.add(f);
		}

		return fileList;
	}

	public static ArrayList<File> GetFiles(String dirName,
			final String fileNameFilter) {
		ArrayList<File> fileList = new ArrayList<File>();

		File dir = new File(dirName);

		// fileNameFilter = ".xml";
		// 内部类
		FilenameFilter searchSuffix = new FilenameFilter() {
			public boolean accept(File dir, String name) {
				return name.contains(fileNameFilter);
			}
		};

		FilenameFilter txtSuffix = new FilenameFilter() {
			public boolean accept(File dir, String name) {
				return name.endsWith("log.txt");
			}
		};

		GetFileToList(dir, searchSuffix,txtSuffix, fileList);

		// 按修改日期排序
		File[] files = (File[]) fileList.toArray(new File[fileList.size()]);

		Arrays.sort(files, new Comparator<File>() {
			public int compare(File f1, File f2) {
				long diff = f1.lastModified() - f2.lastModified();
				if (diff > 0)
					return 1;
				else if (diff == 0)
					return 0;
				else
					return -1;// 如果 if 中修改为 返回-1 同时此处修改为返回 1 排序就会是递减
			}

			public boolean equals(Object obj) {
				return true;
			}
		});

		fileList = new ArrayList<File>(Arrays.asList(files));

		return fileList;
	}

	private static void GetFileToList(File dir, FilenameFilter searchSuffix,FilenameFilter txtSuffix,
			ArrayList<File> al) {

		File[] files = dir.listFiles();

		String[] txtArray = dir.list(txtSuffix);
		
		for (File f : files) {
			if (f.isDirectory()) {
				// 递归了。
				if (!f.getName().equals("Front")) {
					GetFileToList(f, searchSuffix,txtSuffix, al);
				}
			} else {
				if (searchSuffix.accept(dir, f.getName())&&txtArray.length==0) {
					al.add(f);
				}
			}
		}
	}

	/**
	 * 复制单个文件
	 * 
	 * @param srcFileName
	 *            待复制的文件名
	 * @param descFileName
	 *            目标文件名
	 * @param overlay
	 *            如果目标文件存在，是否覆盖
	 * @return 如果复制成功返回true，否则返回false
	 */
	public static boolean CopyFile(String srcFileName, String destFileName,
			boolean overlay) {
		File srcFile = new File(srcFileName);

		// 判断源文件是否存在
		if (!srcFile.exists()) {
			MESSAGE = "Source file：" + srcFileName + "does not exist";
			System.out.println(MESSAGE);
			return false;
		} else if (!srcFile.isFile()) {
			MESSAGE = "Copy directory failed，Source file：" + srcFileName
					+ " is not a file！";
			System.out.println(MESSAGE);
			return false;
		}

		// 判断目标文件是否存在
		File destFile = new File(destFileName);
		if (destFile.exists()) {
			// 如果目标文件存在并允许覆盖
			if (overlay) {
				// 删除已经存在的目标文件，无论目标文件是目录还是单个文件
				new File(destFileName).delete();
			}
		} else {
			// 如果目标文件所在目录不存在，则创建目录
			if (!destFile.getParentFile().exists()) {
				// 目标文件所在目录不存在
				if (!destFile.getParentFile().mkdirs()) {
					// 复制文件失败：创建目标文件所在目录失败
					return false;
				}
			}
		}

		// 复制文件
		int byteread = 0; // 读取的字节数
		InputStream in = null;
		OutputStream out = null;

		try {
			in = new FileInputStream(srcFile);
			out = new FileOutputStream(destFile);
			byte[] buffer = new byte[1024];

			while ((byteread = in.read(buffer)) != -1) {
				out.write(buffer, 0, byteread);
			}
			return true;
		} catch (FileNotFoundException e) {
			return false;
		} catch (IOException e) {
			return false;
		} finally {
			try {
				if (out != null)
					out.close();
				if (in != null)
					in.close();
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
	}

	/**
	 * 复制整个目录的内容
	 * 
	 * @param srcDirName
	 *            待复制目录的目录名
	 * @param destDirName
	 *            目标目录名
	 * @param overlay
	 *            如果目标目录存在，是否覆盖
	 * @return 如果复制成功返回true，否则返回false
	 */
	public static boolean CopyDirectory(String srcDirName, String destDirName,
			boolean overlay) {
		// 判断源目录是否存在
		File srcDir = new File(srcDirName);
		if (!srcDir.exists()) {
			MESSAGE = "Copy directory failed：source directory" + srcDirName
					+ " does not exist";

			LogWriter.getWriter().AddLogItem(MESSAGE);
			System.out.println(MESSAGE);
			return false;
		} else if (!srcDir.isDirectory()) {
			MESSAGE = "Copy directory failed：" + srcDirName
					+ " is not directory";
			LogWriter.getWriter().AddLogItem(MESSAGE);
			System.out.println(MESSAGE);
			return false;
		}

		// 如果目标目录名不是以文件分隔符结尾，则加上文件分隔符
		// if (!destDirName.endsWith(File.separator)) {
		// destDirName = destDirName + File.separator + srcDir.getName()
		// + File.separator;
		// } else {
		// destDirName = destDirName + srcDir.getName() + File.separator;
		// }
		if (!destDirName.endsWith(File.separator)) {
			destDirName = destDirName + File.separator;
		}

		File destDir = new File(destDirName);
		// 如果目标文件夹存在
		if (destDir.exists()) {
			// 如果允许覆盖则删除已存在的目标目录
			if (overlay) {
				new File(destDirName).delete();
			} else {
				MESSAGE = "Copy directory failed：directory" + destDirName
						+ " already exists";
				LogWriter.getWriter().AddLogItem(MESSAGE);
				System.out.println(MESSAGE);
				return false;
			}
		} else {
			// 创建目的目录
			System.out
					.println("Directory does not exist, ready to create.......");
			if (!destDir.mkdirs()) {
				System.out
						.println("Failed to copy the directory: create a directory failure");
				return false;
			}
		}

		boolean flag = true;
		File[] files = srcDir.listFiles();
		for (int i = 0; i < files.length; i++) {
			// 复制文件
			if (files[i].isFile()) {
				flag = CopyFile(files[i].getAbsolutePath(), destDirName
						+ files[i].getName(), overlay);
				if (!flag)
					break;
			} else if (files[i].isDirectory()) {
				// flag = CopyDirectory(files[i].getAbsolutePath(), destDirName
				// + files[i].getName(), overlay);
				// flag = CopyDirectory(files[i].getAbsolutePath(), destDirName,
				// overlay);
				flag = CopyFiles(files[i].getAbsolutePath(), destDirName,
						overlay);
				if (!flag)
					break;
			}
		}
		if (!flag) {
			MESSAGE = "Failed to copy files from " + srcDirName + " to "
					+ destDirName;
			LogWriter.getWriter().AddLogItem(MESSAGE);
			System.out.println(MESSAGE);
			return false;
		} else {
			return true;
		}
	}

	public static boolean CopyFiles(String srcDirName, String destDirName,
			boolean overlay) {
		boolean flag = true;
		File srcDir = new File(srcDirName);

		File[] files = srcDir.listFiles();
		for (int i = 0; i < files.length; i++) {
			// 复制文件
			if (files[i].isFile()) {
				flag = CopyFile(files[i].getAbsolutePath(), destDirName
						+ files[i].getName(), overlay);
				if (!flag)
					break;
			} else if (files[i].isDirectory()) {
				// flag = CopyDirectory(files[i].getAbsolutePath(), destDirName
				// + files[i].getName(), overlay);
				flag = CopyFiles(files[i].getAbsolutePath(), destDirName,
						overlay);
				if (!flag)
					break;
			}
		}

		return flag;
	}

}
