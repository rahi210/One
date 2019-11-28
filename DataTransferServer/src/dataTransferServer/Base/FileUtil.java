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
		// �ڲ���
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

		// ���޸���������
		File[] files = (File[]) fileList.toArray(new File[fileList.size()]);

		Arrays.sort(files, new Comparator<File>() {
			public int compare(File f1, File f2) {
				long diff = f1.lastModified() - f2.lastModified();
				if (diff > 0)
					return 1;
				else if (diff == 0)
					return 0;
				else
					return -1;// ��� if ���޸�Ϊ ����-1 ͬʱ�˴��޸�Ϊ���� 1 ����ͻ��ǵݼ�
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
				// �ݹ��ˡ�
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
	 * ���Ƶ����ļ�
	 * 
	 * @param srcFileName
	 *            �����Ƶ��ļ���
	 * @param descFileName
	 *            Ŀ���ļ���
	 * @param overlay
	 *            ���Ŀ���ļ����ڣ��Ƿ񸲸�
	 * @return ������Ƴɹ�����true�����򷵻�false
	 */
	public static boolean CopyFile(String srcFileName, String destFileName,
			boolean overlay) {
		File srcFile = new File(srcFileName);

		// �ж�Դ�ļ��Ƿ����
		if (!srcFile.exists()) {
			MESSAGE = "Source file��" + srcFileName + "does not exist";
			System.out.println(MESSAGE);
			return false;
		} else if (!srcFile.isFile()) {
			MESSAGE = "Copy directory failed��Source file��" + srcFileName
					+ " is not a file��";
			System.out.println(MESSAGE);
			return false;
		}

		// �ж�Ŀ���ļ��Ƿ����
		File destFile = new File(destFileName);
		if (destFile.exists()) {
			// ���Ŀ���ļ����ڲ�������
			if (overlay) {
				// ɾ���Ѿ����ڵ�Ŀ���ļ�������Ŀ���ļ���Ŀ¼���ǵ����ļ�
				new File(destFileName).delete();
			}
		} else {
			// ���Ŀ���ļ�����Ŀ¼�����ڣ��򴴽�Ŀ¼
			if (!destFile.getParentFile().exists()) {
				// Ŀ���ļ�����Ŀ¼������
				if (!destFile.getParentFile().mkdirs()) {
					// �����ļ�ʧ�ܣ�����Ŀ���ļ�����Ŀ¼ʧ��
					return false;
				}
			}
		}

		// �����ļ�
		int byteread = 0; // ��ȡ���ֽ���
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
	 * ��������Ŀ¼������
	 * 
	 * @param srcDirName
	 *            ������Ŀ¼��Ŀ¼��
	 * @param destDirName
	 *            Ŀ��Ŀ¼��
	 * @param overlay
	 *            ���Ŀ��Ŀ¼���ڣ��Ƿ񸲸�
	 * @return ������Ƴɹ�����true�����򷵻�false
	 */
	public static boolean CopyDirectory(String srcDirName, String destDirName,
			boolean overlay) {
		// �ж�ԴĿ¼�Ƿ����
		File srcDir = new File(srcDirName);
		if (!srcDir.exists()) {
			MESSAGE = "Copy directory failed��source directory" + srcDirName
					+ " does not exist";

			LogWriter.getWriter().AddLogItem(MESSAGE);
			System.out.println(MESSAGE);
			return false;
		} else if (!srcDir.isDirectory()) {
			MESSAGE = "Copy directory failed��" + srcDirName
					+ " is not directory";
			LogWriter.getWriter().AddLogItem(MESSAGE);
			System.out.println(MESSAGE);
			return false;
		}

		// ���Ŀ��Ŀ¼���������ļ��ָ�����β��������ļ��ָ���
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
		// ���Ŀ���ļ��д���
		if (destDir.exists()) {
			// �����������ɾ���Ѵ��ڵ�Ŀ��Ŀ¼
			if (overlay) {
				new File(destDirName).delete();
			} else {
				MESSAGE = "Copy directory failed��directory" + destDirName
						+ " already exists";
				LogWriter.getWriter().AddLogItem(MESSAGE);
				System.out.println(MESSAGE);
				return false;
			}
		} else {
			// ����Ŀ��Ŀ¼
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
			// �����ļ�
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
			// �����ļ�
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
