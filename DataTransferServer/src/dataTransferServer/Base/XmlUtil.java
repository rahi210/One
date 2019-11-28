package dataTransferServer.Base;

import java.io.File;
import java.io.InputStream;
import java.util.List;

import org.dom4j.Attribute;
import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;
import org.dom4j.ElementHandler;
import org.dom4j.ElementPath;
import org.dom4j.io.SAXReader;

public class XmlUtil {

	/**
	 * �����ַ�������document����
	 * 
	 * @param xmlStr
	 * @return
	 * @throws DocumentException
	 */
	public static Document getDocument(String xmlStr) throws DocumentException {
		return DocumentHelper.parseText(xmlStr);
	}

	/**
	 * ����Xml�ļ�����Document����
	 * 
	 * @param file
	 *            xml�ļ�·��
	 * @return Document����
	 * @throws DocumentException
	 */
	public static Document getDocument(File file) throws DocumentException {
		SAXReader xmlReader = new SAXReader();

		/*ElementHandler addHandler = new XmlUtil().new DieElementHandler(); 
		xmlReader.addHandler("/SMEEResults/DieLayout", addHandler);*/

		return xmlReader.read(file);

	}

	/**
	 * ��������������Document����
	 * 
	 * @param is
	 *            ������
	 * @return Document����
	 * @throws DocumentException
	 */
	public static Document getDocument(InputStream is) throws DocumentException {
		SAXReader xmlReader = new SAXReader();
		return xmlReader.read(is);
	}

	/**
	 * ����Document�õ������
	 * 
	 * @param doc
	 *            DocumentĿ¼
	 * @return �����
	 */
	public static Element getRoot(Document doc) {
		return doc.getRootElement();
	}

	/**
	 * ȡ����ǰ����µ������ӽ��
	 * 
	 * @param root
	 *            ��ǰ���
	 * @return һ��Element
	 */
	@SuppressWarnings("unchecked")
	public static List<Element> getElements(Element root) {
		return root.elements();
	}

	/**
	 * ����Ԫ�����Ʒ���һ��Element
	 * 
	 * @param root
	 *            ��ǰ���
	 * @param name
	 *            Ҫ���ص�Ԫ������
	 * @return һ��Element
	 */
	@SuppressWarnings("unchecked")
	public static List<Element> getElementsByName(Element root, String name) {
		return root.elements(name);
	}

	/**
	 * ����Ԫ�����Ʒ���һ��Ԫ��(����ж��Ԫ�صĻ���ֻ���ص�һ��)
	 * 
	 * @param root
	 *            ��ǰ���
	 * @param name
	 *            Ҫ���ص�Ԫ������
	 * @return һ��ElementԪ��
	 */
	public static Element getElementByName(Element root, String name) {
		return root.element(name);
	}

	/**
	 * ���ݵ�ǰԪ��,���ظ�Ԫ�ص���������
	 * 
	 * @param root
	 *            ��ǰ���
	 * @return ��ǰ������������
	 */
	@SuppressWarnings("unchecked")
	public static List<Attribute> getAttributes(Element root) {
		return root.attributes();
	}

	/**
	 * ������������,���ص�ǰԪ�ص�ĳ������
	 * 
	 * @param root
	 *            ��ǰ���
	 * @return ��ǰ����һ������
	 */
	public static Attribute getAttributeByName(Element root, String name) {
		return root.attribute(name);
	}

	/**
	 * ���ؽڵ��Ӧ��value
	 * 
	 * @param elt
	 * @return
	 * @throws Exception
	 */
	public static String getElementValue(Element elt) throws Exception {
		return elt.getText().trim();
	}

	@SuppressWarnings("unchecked")
	public static String getValueByOne(Element root, String name) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {

			if (oneChild.getName().equals(name)
					&& oneChild.getTextTrim().length() > 0) {
				return oneChild.getTextTrim();
			}
		}

		return null;
	}

	@SuppressWarnings("unchecked")
	public static String getValueByTwo(Element root, String name) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			for (Element twoChild : (List<Element>) oneChild.elements()) {

				if (twoChild.getName().equals(name)
						&& twoChild.getTextTrim().length() > 0) {
					return twoChild.getTextTrim();
				}
			}
		}

		return null;
	}

	@SuppressWarnings("unchecked")
	public static String getValueByThree(Element root, String name) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			for (Element twoChild : (List<Element>) oneChild.elements()) {
				for (Element threeChild : (List<Element>) twoChild.elements()) {
					if (threeChild.getName().equals(name)
							&& threeChild.getTextTrim().length() > 0) {
						return threeChild.getTextTrim();
					}
				}
			}
		}

		return null;
	}

	@SuppressWarnings("unchecked")
	public static String getValueByTwo(Element root, String name,
			String parentName) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			if (oneChild.getName().equals(parentName)) {
				for (Element twoChild : (List<Element>) oneChild.elements()) {

					if (twoChild.getName().equals(name)
							&& twoChild.getTextTrim().length() > 0) {
						return twoChild.getTextTrim();
					}
				}
			}
		}

		return null;

	}

	@SuppressWarnings("unchecked")
	public static String getValueByThree(Element root, String name,
			String parentName) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			for (Element twoChild : (List<Element>) oneChild.elements()) {
				if (twoChild.getName().equals(parentName)) {
					for (Element threeChild : (List<Element>) twoChild
							.elements()) {
						if (threeChild.getName().equals(name)
								&& threeChild.getTextTrim().length() > 0) {
							return threeChild.getTextTrim();
						}
					}
				}
			}
		}

		return null;
	}

	@SuppressWarnings("unchecked")
	public static String getValueByFour(Element root, String name,
			String parentName) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			for (Element twoChild : (List<Element>) oneChild.elements()) {
				for (Element threeChild : (List<Element>) twoChild.elements()) {
					if (threeChild.getName().equals(parentName)) {
						for (Element fourChild : (List<Element>) threeChild
								.elements()) {
							if (fourChild.getName().equals(name)
									&& fourChild.getTextTrim().length() > 0) {
								return fourChild.getTextTrim();
							}
						}
					}
				}
			}
		}

		return null;
	}

	@SuppressWarnings("unchecked")
	public static String getValueByFive(Element root, String name,
			String parentName) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			for (Element twoChild : (List<Element>) oneChild.elements()) {
				for (Element threeChild : (List<Element>) twoChild.elements()) {

					for (Element fourChild : (List<Element>) threeChild
							.elements()) {
						if (fourChild.getName().equals(parentName)) {
							for (Element fiveChild : (List<Element>) fourChild
									.elements()) {
								if (fiveChild.getName().equals(name)
										&& fiveChild.getTextTrim().length() > 0) {
									return fiveChild.getTextTrim();
								}
							}
						}
					}
				}
			}
		}

		return null;
	}

	@SuppressWarnings("unchecked")
	public static Element getElementByTwo(Element root, String name) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			for (Element twoChild : (List<Element>) oneChild.elements()) {

				if (twoChild.getName().equals(name)) {
					return twoChild;
				}
			}
		}

		return null;
	}

	@SuppressWarnings("unchecked")
	public static Element getElementByThree(Element root, String name) {

		List<Element> childs = root.elements();

		for (Element oneChild : childs) {
			for (Element twoChild : (List<Element>) oneChild.elements()) {
				for (Element threeChild : (List<Element>) twoChild.elements()) {
					if (threeChild.getName().equals(name)) {
						return threeChild;
					}
				}
			}
		}

		return null;
	}

	public class DieElementHandler implements ElementHandler {
		public void onStart(ElementPath ep) {
		}

		public void onEnd(ElementPath ep) {
			Element e = ep.getCurrent();
			e.detach();
		}
	}
}
