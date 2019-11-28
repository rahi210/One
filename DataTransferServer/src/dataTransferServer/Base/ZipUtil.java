package dataTransferServer.Base;

import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

public final class ZipUtil {

	private ZipUtil() {
	}

	/**
	 * ��ѹ�ļ�
	 * 
	 * @param filePath
	 *            ѹ���ļ�·��
	 */
	public static void unzip(String filePath) {
		File source = new File(filePath);
		if (source.exists()) {
			ZipInputStream zis = null;
			BufferedOutputStream bos = null;
			try {
				zis = new ZipInputStream(new FileInputStream(source));
				ZipEntry entry = null;
				while ((entry = zis.getNextEntry()) != null
						&& !entry.isDirectory()) {
					File target = new File(source.getParent() + File.separator
							+ source.getName().split("\\.")[0], entry.getName());
					if (!target.getParentFile().exists()) {
						// �����ļ���Ŀ¼
						target.getParentFile().mkdirs();
					}
					// д���ļ�
					bos = new BufferedOutputStream(new FileOutputStream(target));
					int read = 0;
					byte[] buffer = new byte[1024 * 10];
					while ((read = zis.read(buffer, 0, buffer.length)) != -1) {
						bos.write(buffer, 0, read);
					}
					bos.flush();
					bos.close();
				}
				zis.closeEntry();
				zis.close();
				//source.delete();
			} catch (IOException e) {
				throw new RuntimeException(e);
			}
		}
	}
}