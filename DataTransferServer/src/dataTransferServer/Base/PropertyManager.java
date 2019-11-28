package dataTransferServer.Base;

import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.util.Properties;

public class PropertyManager {
	private static Properties properties = LoadProperties();

	public static Properties getProperties() {
		return properties;
	}

	private static Properties LoadProperties() {
		try {
			// InputStream inputStream =
			// PropertyManager.class.getClassLoader().getResourceAsStream("config.properties");
			FileReader fileReader = new FileReader("config.properties");
			// if (fileReader != null)
			// {
			Properties properties = new Properties();
			// properties.
			properties.load(fileReader);
			fileReader.close();
			return properties;
			// }
			// else
			// {
			// throw new
			// IOException("Config file should not be loaded! Please confirm the config file under the correct location.");
			// }
		} catch (FileNotFoundException e) {
			e.printStackTrace();
			return null;
		} catch (IOException e) {
			e.printStackTrace();
			return null;
		}
	}

}
