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
	 * 根据字符串生成document对象
	 * 
	 * @param xmlStr
	 * @return
	 * @throws DocumentException
	 */
	public static Document getDocument(String xmlStr) throws DocumentException {
		return DocumentHelper.parseText(xmlStr);
	}

	/**
	 * 根据Xml文件生成Document对象
	 * 
	 * @param file
	 *            xml文件路径
	 * @return Document对象
	 * @throws DocumentException
	 */
	public static Document getDocument(File file) throws DocumentException {
		SAXReader xmlReader = new SAXReader();

		/*ElementHandler addHandler = new XmlUtil().new DieElementHandler(); 
		xmlReader.addHandler("/SMEEResults/DieLayout", addHandler);*/

		return xmlReader.read(file);

	}

	/**
	 * 根据输入流生成Document对象
	 * 
	 * @param is
	 *            输入流
	 * @return Document对象
	 * @throws DocumentException
	 */
	public static Document getDocument(InputStream is) throws DocumentException {
		SAXReader xmlReader = new SAXReader();
		return xmlReader.read(is);
	}

	/**
	 * 根据Document得到根结点
	 * 
	 * @param doc
	 *            Document目录
	 * @return 根结点
	 */
	public static Element getRoot(Document doc) {
		return doc.getRootElement();
	}

	/**
	 * 取出当前结点下的所有子结点
	 * 
	 * @param root
	 *            当前结点
	 * @return 一组Element
	 */
	@SuppressWarnings("unchecked")
	public static List<Element> getElements(Element root) {
		return root.elements();
	}

	/**
	 * 根据元素名称返回一组Element
	 * 
	 * @param root
	 *            当前结点
	 * @param name
	 *            要返回的元素名称
	 * @return 一组Element
	 */
	@SuppressWarnings("unchecked")
	public static List<Element> getElementsByName(Element root, String name) {
		return root.elements(name);
	}

	/**
	 * 根据元素名称返回一个元素(如果有多个元素的话，只返回第一个)
	 * 
	 * @param root
	 *            当前结点
	 * @param name
	 *            要返回的元素名称
	 * @return 一个Element元素
	 */
	public static Element getElementByName(Element root, String name) {
		return root.element(name);
	}

	/**
	 * 根据当前元素,返回该元素的所有属性
	 * 
	 * @param root
	 *            当前结点
	 * @return 当前结点的所有属性
	 */
	@SuppressWarnings("unchecked")
	public static List<Attribute> getAttributes(Element root) {
		return root.attributes();
	}

	/**
	 * 根据属性名称,返回当前元素的某个属性
	 * 
	 * @param root
	 *            当前结点
	 * @return 当前结点的一个属性
	 */
	public static Attribute getAttributeByName(Element root, String name) {
		return root.attribute(name);
	}

	/**
	 * 返回节点对应的value
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
