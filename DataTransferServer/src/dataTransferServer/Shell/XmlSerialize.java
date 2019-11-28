package dataTransferServer.Shell;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Properties;

import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;

import dataTransferServer.Base.*;
import dataTransferServer.DataAccess.OracleHelper;
import dataTransferServer.Entity.*;

//序列化
public class XmlSerialize {

	private Element rootElement;
	private WaferResultEntity waferResultEntity;
	private WaferEntity waferEntity;

	public XmlSerialize(String filepath, String fileName, String destFilePath)
			throws DocumentException, SQLException, IOException,
			InterruptedException {

		waferResultEntity = new WaferResultEntity();
		waferEntity = new WaferEntity();

		// UnZipXML(filepath, fileName, destFilePath);
		LoadXml(filepath, fileName, destFilePath);
	}

	private void UnZipXML(String filePath, String fileName, String destFilePath)
			throws DocumentException, SQLException, IOException,
			InterruptedException {

		ArrayList<File> fileList = FileUtil.GetFiles(filePath, ".zip");

		for (File f : fileList) {
			// 解压
			String msg = String.format(
					"%s,File path:%s,File is extracting.........", Utils
							.GetCurrDateTime(), f.getPath());
			LogWriter.getWriter().AddLogItem(msg);
			System.out.println(msg);

			ZipUtil.unzip(f.getPath());

			// 转换
			LoadXml(f.getParent(), fileName, destFilePath);

			Thread.sleep(1000);
		}
	}

	private void LoadXml(String filePath, String fileName, String destFilePath)
			throws DocumentException, SQLException, IOException {

		int cnt = 0;
		boolean isFinish_Lot = false;
		String lotName = Utils.LastLotName;

		LogWriter.getWriter().AddLogItem("start get files....");
		ArrayList<File> fileList = FileUtil.GetFiles(filePath, fileName);
		LogWriter.getWriter().AddLogItem(
				"end get files, count=" + fileList.size());

		if (lotName != null && lotName.length() > 0) {
			for (File f : fileList) {
				if (f.getPath().contains(lotName) == true) {
					ArrayList<File> logList = FileUtil.GetFiles(f.getParent(),
							"log.txt");

					if (logList.size() <= 0) {
						isFinish_Lot = true;
						break;
					}
				}
			}
		}

		// 查找目录下WaferResults.xml文件
		for (File f : fileList) {
			boolean rs = false;
			String msg1 = "";
			String msg2 = "";
			Date currentDate1 = new Date();

			ArrayList<File> logList = FileUtil.GetFiles(f.getParent(),
					"log.txt");

			if (logList.size() > 0) {
				// LogWriter.getWriter().AddLogItem(String.format("%s,文件路径:%s,数据已经被处理.........",
				// Utils
				// .GetCurrDateTime(), f.getPath()));
				cnt++;
				continue;
			}

			if (isFinish_Lot == true && f.getPath().contains(lotName) == false) {
				cnt++;
				continue;
			}

			/*
			 * boolean status = OracleHelper.GetDataArchiveStatus();
			 * 
			 * if (status == true) { LogWriter.getWriter().AddLogItem(
			 * "Data is archiving, please wait a moment......"); System.out
			 * .println("Data is archiving, please wait a moment......");
			 * return; }
			 */

			System.out.println(f.getParent());
			// Date currentDate1 = new Date();
			try {
				msg1 = String.format(
						"%s,File path:%s,Data start converting.........", Utils
								.GetCurrDateTime(), f.getPath());
				LogWriter.getWriter().AddLogItem(msg1);
				System.out.println(msg1);

				/*
				 * FileWriter fileWriter = new FileWriter(f.getParent() +
				 * File.separator + "log.txt", true); fileWriter.write(msg1);
				 * fileWriter.close();
				 */

				// XML数据序列化
				xmlSerializeToEntity(f);

				String msg11 = String.format("%s,Data serialization.........",
						Utils.GetCurrDateTime());
				LogWriter.getWriter().AddLogItem(msg11);

				/*-------------------------------------------------------------
				 * 映射网络驱动器
				 * ------------------------------------------------------------
				 */

				// 复制文件到指定目录
				if (!destFilePath.endsWith(File.separator)) {
					rs = FileUtil.CopyDirectory(f.getParent(), destFilePath
							+ File.separator
							+ waferEntity.getWaferResultEntity().getResultId(),
							true);
				} else {
					rs = FileUtil.CopyDirectory(f.getParent(), destFilePath
							+ waferEntity.getWaferResultEntity().getResultId(),
							true);
				}

				String msg12 = String.format("%s,Copy XML File.........", Utils
						.GetCurrDateTime());
				LogWriter.getWriter().AddLogItem(msg12);

				if (rs == true) {
					// 执行数据插入
					rs = OracleHelper.InsertEntityToDataTable(waferEntity);

					String msg13 = String.format("%s,Insert Database.........",
							Utils.GetCurrDateTime());
					LogWriter.getWriter().AddLogItem(msg13);
				} else {
					msg2 = String
							.format(
									"%s,File path:%s,Failed to copy the directory: create a directory failure",
									Utils.GetCurrDateTime(), f.getPath());

					LogWriter.getWriter().AddLogItem(msg2);
					System.out.println(msg2);
					continue;
				}
			} catch (SQLException e) {
				e.printStackTrace();
				StringBuffer sb = new StringBuffer();
				StackTraceElement[] stackArray = e.getStackTrace();
				for (int i = 0; i < stackArray.length; i++) {
					StackTraceElement element = stackArray[i];
					sb.append(element.toString() + "\n");
				}

				LogWriter.getWriter().AddLogItem(sb.toString());

				Utils.IsReImport = true;
			} catch (Exception e) {
				e.printStackTrace();
				StringBuffer sb = new StringBuffer();
				StackTraceElement[] stackArray = e.getStackTrace();
				for (int i = 0; i < stackArray.length; i++) {
					StackTraceElement element = stackArray[i];
					sb.append(element.toString() + "\n");
				}

				LogWriter.getWriter().AddLogItem(sb.toString());

				Utils.IsReImport = true;
			}

			if (rs == true) {
				msg2 = String
						.format(
								"%s,File path:%s,Data import successfully,A total of :%s seconds",
								Utils.GetCurrDateTime(),
								f.getPath(),
								(new Date().getTime() - currentDate1.getTime()) / 1000);

				if (Utils.IsWriteLog) {
					FileWriter fileWriter1 = new FileWriter(f.getParent()
							+ File.separator + "log.txt", true);
					fileWriter1.write(msg1);
					fileWriter1.write("\r\n");
					fileWriter1.write(msg2);
					fileWriter1.close();
				}
			} else {
				if (Utils.IsReImport) {

					Utils.IsReImport = false;
					++Utils.ReImportNum;

					msg2 = String
							.format(
									"%s,File path:%s,Data import failed,A total of :%s seconds,ReImportNum:%s",
									Utils.GetCurrDateTime(), f.getPath(),
									(new Date().getTime() - currentDate1
											.getTime()) / 1000,
									Utils.ReImportNum);

					if (Utils.ReImportNum == 5) {

						Utils.ReImportNum = 0;

						Utils.ResetSystem(true); // 重启系统

						FileWriter fileWriter1 = new FileWriter(f.getParent()
								+ File.separator + "log.txt", true);
						fileWriter1.write(msg1);
						fileWriter1.write("\r\n");
						fileWriter1.write(msg2);
						fileWriter1.close();
					}
				} else {
					msg2 = String
							.format(
									"%s,File path:%s,Data import failed,A total of :%s seconds",
									Utils.GetCurrDateTime(), f.getPath(),
									(new Date().getTime() - currentDate1
											.getTime()) / 1000);

					FileWriter fileWriter1 = new FileWriter(f.getParent()
							+ File.separator + "log.txt", true);
					fileWriter1.write(msg1);
					fileWriter1.write("\r\n");
					fileWriter1.write(msg2);
					fileWriter1.close();
				}
			}

			LogWriter.getWriter().AddLogItem(msg2);
			System.out.println(msg2);
		}

		System.out.println(String.format(
				"%s,The total conversion of %s documents", Utils
						.GetCurrDateTime(), fileList.size() - cnt));
		LogWriter.getWriter().AddLogItem(
				String.format("%s,The total conversion of %s documents", Utils
						.GetCurrDateTime(), fileList.size() - cnt));
	}

	private void xmlSerializeToEntity(File f) throws DocumentException,
			SQLException {
		System.out.println("File path：" + f.getPath());

		Document doc = XmlUtil.getDocument(f);
		rootElement = XmlUtil.getRoot(doc);

		GetWaferResult(rootElement);

		GetIdentification(rootElement);

		GetClassificationScheme(rootElement);

		InspectionInfoEntity inspectionInfoEntity = GetInspectionInfo(rootElement);

		// 判断批次的Layout信息是否已经存在，若存在就不解析
		boolean isExist = OracleHelper.IsExistsLotLayout(Utils.LastLayer,
				Utils.LastLotName);

		if (!isExist) {
			GetDieLayout(rootElement);

		} else {
			waferResultEntity.setDieLayoutID(Utils.LotId);
			waferResultEntity.setNumDIE(Utils.LotDieNum);
		}

		double numDefect = inspectionInfoEntity.getDefectiveDie();
		double numDie = waferResultEntity.getNumDIE();
		double sfield = 0;
		if (numDie > 0) {
			sfield = Math.round((numDie - numDefect) * 10000 / numDie) / 100.0;

			if (sfield == 100 && numDefect > 0) {
				sfield = 99.99;
			}
		}

		inspectionInfoEntity.setInspectedDie(waferResultEntity.getNumDIE());
		waferResultEntity.setSFIELD(sfield);
		waferResultEntity.setNUMDEFECT((long) numDefect);

		doc = null;
	}

	@SuppressWarnings("unused")
	private boolean UploadImg(String filePath, String ftpPath)
			throws FileNotFoundException {

		boolean isUpload = false;
		Properties properties = PropertyManager.getProperties();

		String ip = properties.getProperty("FtpServer");
		int port = Integer.parseInt(properties.getProperty("FtpPort"));
		String user = properties.getProperty("FtpUser");
		String pwd = properties.getProperty("FtpPwd");

		ArrayList<File> imgList = FileUtil.GetFiles(filePath + File.separator
				+ "Front");

		for (File img : imgList) {
			FileInputStream in = new FileInputStream(img);
			// "/WaferResults/front"
			isUpload = FtpUtil.uploadFile(ip, port, user, pwd, ftpPath, img
					.getName(), in);

			if (!isUpload) {
				break;
			}
		}

		return isUpload;
	}

	private WaferResultEntity GetWaferResult(Element element) {

		Element equipment = XmlUtil.getElementByName(element, "Equipment");
		Element moduleList = XmlUtil.getElementByName(equipment, "ModuleList");
		Element module = XmlUtil.getElementByName(moduleList, "Module");

		moduleList.getParent().getName();

		WaferResultEntity model = new WaferResultEntity();

		model.setResultId(Utils.NewGuid());

		model.setStartTime(Utils.DataTimeFormat(element
				.elementText("StartTime")));
		model.setCompletionTime(Utils.DataTimeFormat(element
				.elementText("CompletionTime")));
		model.setReviewStartTime(Utils.DataTimeFormat(element
				.elementText("ReviewStartTime")));
		model.setReviewCompletionTime(Utils.DataTimeFormat(element
				.elementText("ReviewCompletionTime")));
		model.setLotStartTime(Utils.DataTimeFormat(element
				.elementText("LotStartTime")));
		model.setLotCompletionTime(Utils.DataTimeFormat(element
				.elementText("LotCompletionTime")));

		model.setDisposition(element.elementText("Disposition"));

		model.setMasterToolName(equipment.elementText("MasterToolName"));
		model.setMasterSoftwareVersion(equipment
				.elementText("MasterSoftwareVersion"));
		model.setMasterToolComputerName(equipment
				.elementText("MasterToolComputerName"));
		model.setMasterToolToHostLinkState(equipment
				.elementText("MasterToolToHostLinkState"));
		model.setModuleName(module.elementText("Name"));
		model.setComputerName(module.elementText("ComputerName"));
		model.setSoftwareVersion(module.elementText("SoftwareVersion"));
		model.setPrimarySurface(module.elementText("PrimarySurface"));

		model.setIdentificationID(Utils.NewGuid());
		model.setClassificationInfoID(Utils.NewGuid());
		model.setDieLayoutID(Utils.NewGuid());

		model.setCreatedDate(Utils.GetCurrDate());

		// System.out.println(model.getResultId());
		// System.out.println(model.getStartTime());
		// System.out.println(model.getCompletionTime());
		// System.out.println(model.getDisposition());
		// System.out.println(model.getMasterToolName());
		//		
		// System.out.println(model.getModuleName());
		// System.out.println(model.getComputerName());
		//		
		// System.out.println(model.getCreatedDate());

		waferResultEntity = model;
		waferEntity.setWaferResultEntity(model);

		return model;
	}

	// 检测设备信息
	private IdentificationEntity GetIdentification(Element element) {
		IdentificationEntity model = new IdentificationEntity();

		Element identification = XmlUtil.getElementByName(element,
				"Identification");
		Element carrier = XmlUtil.getElementByName(identification, "Carrier");
		Element processProgram = XmlUtil.getElementByName(identification,
				"ProcessProgram");
		Element substrate = XmlUtil.getElementByName(identification,
				"Substrate");

		model.setIdentificationId(waferResultEntity.getIdentificationID());

		model.setLot(identification.elementText("Lot"));
		model.setOperator(identification.elementText("Operator"));

		if (carrier != null) {
			// <Carrier>
			model.setCarrier_ID(carrier.elementText("ID"));
			model.setCarrier_Station(carrier.elementText("Station"));
		}

		if (processProgram != null) {
			// <ProcessProgram>
			model.setDevice(processProgram.elementText("Device"));
			model.setLayer(processProgram.elementText("Layer"));
			model.setPPName(processProgram.elementText("Name"));
			model.setModificationTime(Utils.DataTimeFormat(processProgram
					.elementText("ModificationTime")));
			model.setLastAuthor(processProgram.elementText("LastAuthor"));
		}

		if (substrate != null) {
			// <Substrate>
			model.setSubstrate_ID(substrate.elementText("ID"));
			model.setSubstrate_Number(substrate.elementText("Number"));
			model.setSubstrate_DiameterMM(substrate.elementText("DiameterMM"));
			model.setSubstrate_Slot(substrate.elementText("Slot"));
			model.setSubstrate_Type(substrate.elementText("Type"));
			model.setSubstrate_FiducialType(substrate
					.elementText("FiducialType"));
			model.setSubstrate_NotchLocation(substrate
					.elementText("NotchLocation"));
		}

		// System.out.println(model.getIdentificationId());
		//
		// System.out.println(model.getLot());
		// System.out.println(model.getOperator());
		//
		// System.out.println(model.getCarrier_ID());
		// System.out.println(model.getCarrier_Station());
		//
		// System.out.println(model.getDevice());
		// System.out.println(model.getLayer());
		// System.out.println(model.getPPName());
		// System.out.println(model.getModificationTime());
		// System.out.println(model.getLastAuthor());
		//
		// System.out.println(model.getSubstrate_ID());
		// System.out.println(model.getSubstrate_Number());
		// System.out.println(model.getSubstrate_DiameterMM());
		// System.out.println(model.getSubstrate_Slot());
		// System.out.println(model.getSubstrate_Type());
		// System.out.println(model.getSubstrate_FiducialType());

		waferResultEntity.setIdentificationEntity(model);
		waferEntity.setIdentificationEntity(model);
		Utils.LastLotName = model.getLot();

		return model;
	}

	// 缺陷分类信息表
	@SuppressWarnings("unchecked")
	private ClassificationSchemeEntity GetClassificationScheme(Element element) {

		ClassificationSchemeEntity model = new ClassificationSchemeEntity();

		Element classificationInformation = XmlUtil.getElementByName(element,
				"ClassificationInformation");
		Element classificationSchemeList = XmlUtil.getElementByName(
				classificationInformation, "ClassificationSchemeList");
		Element classificationScheme = XmlUtil.getElementByName(
				classificationSchemeList, "ClassificationScheme");

		model.setSchemeId(waferResultEntity.getClassificationInfoID());

		model.setComputerName(waferResultEntity.getComputerName());
		model.setName(classificationScheme.elementText("ID"));

		// System.out.println(model.getSchemeId());
		//
		// System.out.println(model.getComputerName());
		// System.out.println(model.getName());

		waferResultEntity.setClassificationSchemeEntity(model);

		Element cassificationList = XmlUtil.getElementByName(
				classificationScheme, "ClassificationList");

		List<ClassificationItemEntity> listClassificationItemEntity = new ArrayList();

		// <ClassificationList>
		List<Element> classificationElements = cassificationList.elements();
		for (Element child : classificationElements) {
			// <Classification>
			ClassificationItemEntity classificationModel = new ClassificationItemEntity();

			classificationModel.setItemId(Utils.NewGuid());
			classificationModel.setSchemeId(waferResultEntity
					.getClassificationInfoID());

			classificationModel
					.setId(Integer.parseInt(child.elementText("ID")));
			classificationModel.setName(child.elementText("Name"));
			classificationModel
					.setDescription(child.elementText("Description"));
			classificationModel.setColor(child.elementText("Color"));
			classificationModel.setPriority(Integer.parseInt(child
					.elementText("Priority")));
			classificationModel.setHotkey(child.elementText("Hotkey"));
			classificationModel.setIsAcceptable(child
					.elementText("IsAcceptable"));

			listClassificationItemEntity.add(classificationModel);

			// System.out.println(classificationModel.getSchemeId());
			//
			// System.out.println(classificationModel.getId());
			// System.out.println(classificationModel.getName());
			// System.out.println(classificationModel.getDescription());
			// System.out.println(classificationModel.getColor());
			// System.out.println(classificationModel.getPriority());
			// System.out.println(classificationModel.getHotkey());
			// System.out.println(classificationModel.getIsAcceptable());
		}

		model.setListClassificationItemEntity(listClassificationItemEntity);

		waferEntity.setClassificationSchemeEntity(model);
		waferEntity.setClassificationItemEntity(listClassificationItemEntity);

		return model;
	}

	// 晶片Layout表
	@SuppressWarnings("unchecked")
	private DieLayoutEntity GetDieLayout(Element element) {

		DieLayoutEntity modelDieLayout = new DieLayoutEntity();
		List<DieLayoutListEntity> listDieLayoutListEntity = new ArrayList();

		modelDieLayout.setLayoutId(waferResultEntity.getDieLayoutID());

		Element dieLayout = XmlUtil.getElementByName(element, "DieLayout");

		List<Element> eltChilds = dieLayout.elements();

		for (Element child : eltChilds) {
			// <DieAddressRange>
			if (child.getName().equals("DieAddressRange")) {
				Element minElement = XmlUtil.getElementByName(child, "Min");
				Element maxElement = XmlUtil.getElementByName(child, "Max");

				if (minElement != null && maxElement != null) {
					modelDieLayout.setDieAddressRange(String.format(
							"%s,%s|%s,%s", minElement.elementText("x"),
							minElement.elementText("y"), maxElement
									.elementText("x"), maxElement
									.elementText("y")));
				}

				// System.out.println(modelDieLayout.getDieAddressRange());
			}
			// <AnchorDie>
			else if (child.getName().equals("AnchorDie")) {
				Element dieAddress = XmlUtil.getElementByName(child,
						"DieAddress");
				Element sWCSCoordinates = XmlUtil.getElementByName(child,
						"SWCSCoordinates");

				if (dieAddress != null && sWCSCoordinates != null) {
					modelDieLayout.setAnchorDie(String.format("%s,%s|%s,%s",
							dieAddress.elementText("x"), dieAddress
									.elementText("y"), sWCSCoordinates
									.elementText("x"), sWCSCoordinates
									.elementText("y")));
				}

				// System.out.println(modelDieLayout.getAnchorDie());
			}
			// <DieGridList count="1">
			else if (child.getName().equals("DieGridList")) {
				// <DieGrid>
				// Element dieGrid = XmlUtil.getElementByName(child, "DieGrid");
				// Element dieCharacteristics =
				// XmlUtil.getElementByName(dieGrid, "DieCharacteristics");

				Element size = XmlUtil.getElementByThree(child, "Size");
				if (size != null) {
					modelDieLayout.setSize(String.format("%s,%s", size
							.elementText("x"), size.elementText("y")));

					// System.out.println(modelDieLayout.getSize());
				}

				Element pitch = XmlUtil.getElementByThree(child, "Pitch");
				if (pitch != null) {
					modelDieLayout.setPitch(String.format("%s,%s", pitch
							.elementText("x"), pitch.elementText("y")));

					// System.out.println(modelDieLayout.getPitch());
				}

				Element dieAddress = XmlUtil.getElementByThree(child,
						"DieAddress");
				if (dieAddress != null) {
					modelDieLayout.setDieAddress(String.format("%s,%s",
							dieAddress.elementText("x"), dieAddress
									.elementText("y")));

					// System.out.println(modelDieLayout.getDieAddress());
				}

				Element sWCSCoordinates = XmlUtil.getElementByThree(child,
						"SWCSCoordinates");
				if (sWCSCoordinates != null) {
					modelDieLayout.setSWCSCoordinates(String.format("%s,%s",
							sWCSCoordinates.elementText("x"), sWCSCoordinates
									.elementText("y")));

					// System.out.println(modelDieLayout.getSWCSCoordinates());
				}

				Element dieAddressIncrement = XmlUtil.getElementByTwo(child,
						"DieAddressIncrement");
				if (dieAddressIncrement != null) {
					modelDieLayout.setDieAddressIncrement(String.format(
							"%s,%s", dieAddressIncrement.elementText("x"),
							dieAddressIncrement.elementText("y")));

					// System.out.println(modelDieLayout.getDieAddressIncrement());
				}

				modelDieLayout.setColumns(Integer.parseInt(XmlUtil
						.getValueByTwo(child, "Columns")));
				modelDieLayout.setRows(Integer.parseInt(XmlUtil.getValueByTwo(
						child, "Rows")));

				// System.out.println(modelDieLayout.getColumns());
				// System.out.println(modelDieLayout.getRows());
			} else if (child.getName().equals("DieList")) {

				for (Element die : (List<Element>) child.elements()) {
					DieLayoutListEntity dieLayoutListEntity = new DieLayoutListEntity();

					dieLayoutListEntity.setId(Utils.NewGuid());
					dieLayoutListEntity.setLayoutId(modelDieLayout
							.getLayoutId());

					Element dieAddress = XmlUtil.getElementByName(die,
							"DieAddress");
					dieLayoutListEntity.setDieAddressX(Integer
							.parseInt(dieAddress.elementText("x")));
					dieLayoutListEntity.setDieAddressY(Integer
							.parseInt(dieAddress.elementText("y")));

					dieLayoutListEntity.setDisposition(die
							.elementText("Disposition"));
					dieLayoutListEntity.setInspClassifiID(die
							.elementText("InspectionClassificationID"));
					dieLayoutListEntity.setReviewClassifcationID(die
							.elementText("ReviewClassifcationID"));
					dieLayoutListEntity
							.setAutoDieClassifierClassificationID(die
									.elementText("AutoDieClassifierClassificationID"));
					dieLayoutListEntity
							.setProcessSentinelDieClassificationID(die
									.elementText("ProcessSentinelDieClassificationID"));
					dieLayoutListEntity.setIsInspectable(die
							.elementText("IsInspectable"));
					dieLayoutListEntity.setIsEdgeDie(die
							.elementText("IsEdgeDie"));

					// die num
					if (!dieLayoutListEntity.getDisposition().trim()
							.toLowerCase().equals("notexist")
							&& !dieLayoutListEntity.getDisposition().trim()
									.toLowerCase().equals("notprocess")) {
						long numDie = waferResultEntity.getNumDIE();
						long numDefect = waferResultEntity.getNUMDEFECT();

						if (!dieLayoutListEntity.getInspClassifiID()
								.equals("0")) {
							waferResultEntity.setNUMDEFECT(++numDefect);
						}

						waferResultEntity.setNumDIE(++numDie);
					}

					// Disposition=notexist or ClassifiID<>0
					if (dieLayoutListEntity.getDisposition().trim()
							.toLowerCase().equals("notexist")
							|| dieLayoutListEntity.getDisposition().trim()
									.toLowerCase().equals("notprocess")
							|| !dieLayoutListEntity.getInspClassifiID().equals(
									"0")) {
						listDieLayoutListEntity.add(dieLayoutListEntity);
					}
					// System.out.println(dieLayoutListEntity.getId());
					// System.out.println(dieLayoutListEntity.getLayoutId());
					//
					// System.out.println(dieLayoutListEntity.getDieAddressX());
					// System.out.println(dieLayoutListEntity.getDieAddressY());
					// System.out.println(dieLayoutListEntity.getDisposition());
					// System.out.println(dieLayoutListEntity.getInspClassifiID());
				}

				// System.out.println(listDieLayoutListEntity.size());
			}

		}

		double numDefect = waferResultEntity.getNUMDEFECT();
		double numDie = waferResultEntity.getNumDIE();
		double sfield = Math.round((numDie - numDefect) * 10000 / numDie) / 100.0;

		waferResultEntity.setSFIELD(sfield);

		// modelDieLayout.setLot(waferEntity.getIdentificationEntity().getLot());
		// modelDieLayout.setDieNum(waferResultEntity.getNumDIE());

		waferResultEntity.setDieLayoutEntity(modelDieLayout);
		modelDieLayout.setDieLayoutListEntity(listDieLayoutListEntity);

		waferEntity.setDieLayoutEntity(modelDieLayout);
		waferEntity.setDieLayoutListEntity(listDieLayoutListEntity);

		return modelDieLayout;
	}

	// 晶片检测信息表
	@SuppressWarnings("unchecked")
	private InspectionInfoEntity GetInspectionInfo(Element element) {
		InspectionInfoEntity model = new InspectionInfoEntity();

		model.setInspId(Utils.NewGuid());
		model.setResultId(waferResultEntity.getResultId());

		Element inspectionInformationList = XmlUtil.getElementByName(element,
				"InspectionInformationList");

		List<Element> listElement = inspectionInformationList.elements();
		for (Element child : listElement) {

			model.setModuleName(child.elementText("ModuleName"));
			model.setStartTime(Utils.DataTimeFormat(child
					.elementText("StartTime")));
			model.setCompletionTime(Utils.DataTimeFormat(child
					.elementText("CompletionTime")));

			waferEntity.getIdentificationEntity().setDevice(
					XmlUtil.getValueByTwo(child, "Device"));
			waferEntity.getIdentificationEntity().setLayer(
					XmlUtil.getValueByTwo(child, "Layer"));

			model.setDevice(XmlUtil.getValueByTwo(child, "Device"));
			model.setLayer(XmlUtil.getValueByTwo(child, "Layer"));
			model.setName(XmlUtil.getValueByTwo(child, "Name"));
			// recipe_id 20170518
			model.setRecipe_id(XmlUtil.getValueByTwo(child, "recipe_id"));
			model.setModificationTime(Utils.DataTimeFormat(XmlUtil
					.getValueByTwo(child, "ModificationTime")));
			model.setLastAuthor(XmlUtil.getValueByTwo(child, "LastAuthor"));

			model.setDisposition(child.elementText("Disposition"));
			model.setClassificationSchemeID(child
					.elementText("ClassificationSchemeID"));
			model.setDefectDensity(Double.parseDouble(child
					.elementText("DefectDensity")));
			model.setRandomDefectDensity(Double.parseDouble(child
					.elementText("RandomDefectDensity")));
			model.setDefectRatio(Double.parseDouble(child
					.elementText("DefectRatio")));
			model.setDefectiveArea(Double.parseDouble(child
					.elementText("DefectiveArea")));
			model.setDefectiveDie(Double.parseDouble(child
					.elementText("DefectiveDie")));

			model.setImagesDirectoryName(child
					.elementText("ImagesDirectoryName"));

			// System.out.println(model.getModuleName());
			// System.out.println(model.getStartTime());
			// System.out.println(model.getCompletionTime());
			// System.out.println(model.getDevice());
			// System.out.println(model.getLayer());
			// System.out.println(model.getName());
			// System.out.println(model.getModificationTime());
			// System.out.println(model.getLastAuthor());
			// System.out.println(model.getDisposition());
			// System.out.println(model.getClassificationSchemeID());
			// System.out.println(model.getDefectDensity());
			// System.out.println(model.getRandomDefectDensity());
			// System.out.println(model.getRandomDefectDensity());
			// System.out.println(model.getDefectRatio());
			// System.out.println(model.getDefectiveArea());
			// System.out.println(model.getDefectiveDie());
			// System.out.println(model.getImagesDirectoryName());

			// 晶片检测通过信息表
			Element inspectionPassList = XmlUtil.getElementByName(child,
					"InspectionPassList");

			if (inspectionPassList != null) {
				InspectionPassEntity inspectionPassEntity = new InspectionPassEntity();

				model.setInspectionPassEntity(inspectionPassEntity);

				List<InspectedDieListEntity> listInspectedDieListEntity = new ArrayList();
				List<DefectListEntity> listDefectListEntity = new ArrayList();

				inspectionPassEntity
						.setListInspectedDieListEntity(listInspectedDieListEntity);
				inspectionPassEntity
						.setListDefectListEntity(listDefectListEntity);

				inspectionPassEntity.setInspId(model.getInspId());
				inspectionPassEntity.setPassId(Integer.parseInt(XmlUtil
						.getValueByTwo(inspectionPassList, "ID")));

				inspectionPassEntity.setOrientation(String.format("%s,%s",
						XmlUtil.getValueByThree(inspectionPassList,
								"FiducialOrientation"), XmlUtil
								.getValueByThree(inspectionPassList,
										"SurfaceOrientation")));

				inspectionPassEntity.setInspectedSurface(XmlUtil.getValueByTwo(
						inspectionPassList, "InspectedSurface"));
				inspectionPassEntity.setInspectionType(XmlUtil.getValueByTwo(
						inspectionPassList, "InspectionType"));

				inspectionPassEntity.setDefectDensity(Double
						.parseDouble(XmlUtil.getValueByThree(
								inspectionPassList, "DefectDensity")));
				inspectionPassEntity.setDefectiveArea(Double
						.parseDouble(XmlUtil.getValueByThree(
								inspectionPassList, "DefectiveArea")));
				inspectionPassEntity.setDefectRatio(Double.parseDouble(XmlUtil
						.getValueByThree(inspectionPassList, "DefectRatio")));
				inspectionPassEntity.setRandomDefectDensity(Double
						.parseDouble(XmlUtil.getValueByThree(
								inspectionPassList, "RandomDefectDensity")));
				inspectionPassEntity.setDefectiveDie(Integer.parseInt(XmlUtil
						.getValueByThree(inspectionPassList, "DefectiveDie")));

				// System.out.println(inspectionPassEntity.getPassId());
				// System.out.println(inspectionPassEntity.getOrientation());
				// System.out.println(inspectionPassEntity.getInspectedSurface());
				// System.out.println(inspectionPassEntity.getInspectionType());
				// System.out.println(inspectionPassEntity.getDefectDensity());
				// System.out.println(inspectionPassEntity.getDefectiveArea());
				// System.out.println(inspectionPassEntity.getDefectRatio());
				// System.out.println(inspectionPassEntity.getRandomDefectDensity());
				// System.out.println(inspectionPassEntity.getDefectiveDie());

				// 晶片检测Die信息列表
				Element inspectedDieList = XmlUtil.getElementByTwo(
						inspectionPassList, "InspectedDieList");

				if (inspectedDieList != null) {
					long inspecteddie = 0;

					for (Element childInspectedDie : (List<Element>) inspectedDieList
							.elements()) {
						InspectedDieListEntity inspectedDieListEntity = new InspectedDieListEntity();

						inspectedDieListEntity.setInspectedDieId(Utils
								.NewGuid());
						inspectedDieListEntity.setPassId(inspectionPassEntity
								.getPassId());
						inspectedDieListEntity.setInspId(model.getInspId());

						inspectedDieListEntity
								.setDieAddress(String.format("%s,%s", XmlUtil
										.getValueByTwo(childInspectedDie, "x"),
										XmlUtil.getValueByTwo(
												childInspectedDie, "y")));

						inspectedDieListEntity
								.setClassificationID(childInspectedDie
										.elementText("ClassificationID"));
						inspectedDieListEntity.setDisposition(childInspectedDie
								.elementText("Disposition"));

						// System.out.println(inspectedDieListEntity
						// .getDieAddress());
						inspecteddie++;
						listInspectedDieListEntity.add(inspectedDieListEntity);
					}

					// model.setInspectedDie(inspecteddie);
					model.setInspectedDie(waferResultEntity.getNumDIE());
				}

				// Mask类型定义的节点
				Element maskList = XmlUtil.getElementByTwo(inspectionPassList,
						"MaskTypeList");
				String maskArray = "";
				if (maskList != null) {
					for (Element childMskList : (List<Element>) maskList
							.elements()) {
						maskArray += childMskList.getTextTrim();
					}
				}

				model.setMaskArray(maskArray);

				Element defectList = XmlUtil.getElementByTwo(
						inspectionPassList, "DefectList");

				if (defectList != null) {
					for (Element childDefectList : (List<Element>) defectList
							.elements()) {
						DefectListEntity defectListEntity = new DefectListEntity();

						defectListEntity.setId(Integer.parseInt(childDefectList
								.elementText("ID")));
						defectListEntity.setPassId(inspectionPassEntity
								.getPassId());
						defectListEntity.setInspId(model.getInspId());

						defectListEntity.setInspectionType(childDefectList
								.elementText("InspectionType"));

						defectListEntity.setSWCSCoordinates(String.format(
								"%s,%s,%s", XmlUtil
										.getValueByTwo(childDefectList, "x",
												"SWCSCoordinates"), XmlUtil
										.getValueByTwo(childDefectList, "y",
												"SWCSCoordinates"), XmlUtil
										.getValueByTwo(childDefectList, "z",
												"SWCSCoordinates")));

						int icId = Integer.parseInt(childDefectList
								.elementText("InspectionClassificationID"));

						defectListEntity.setClassificationitemId(icId);

						for (ClassificationItemEntity c : waferResultEntity
								.getClassificationSchemeEntity()
								.getListClassificationItemEntity()) {

							if (c.getId() == icId) {
								defectListEntity.setInspClassifiID(c
										.getItemId());
							}
						}

						defectListEntity.setSize(String.format("%s,%s", XmlUtil
								.getValueByTwo(childDefectList, "x", "Size"),
								XmlUtil.getValueByTwo(childDefectList, "y",
										"Size")));

						defectListEntity.setMajorAxisSize(childDefectList
								.elementText("MajorAxisSize"));
						defectListEntity
								.setMajorMinorAxisAspectRatio(childDefectList
										.elementText("MajorMinorAxisAspectRatio"));
						defectListEntity.setArea(childDefectList
								.elementText("Area"));

						defectListEntity.setDieAddress(String.format("%s,%s",
								XmlUtil.getValueByThree(childDefectList, "x",
										"DieAddress"), XmlUtil.getValueByThree(
										childDefectList, "y", "DieAddress")));

						defectListEntity.setImageName(XmlUtil.getValueByThree(
								childDefectList, "Name", "Image"));
						defectListEntity.setStyle(XmlUtil.getValueByThree(
								childDefectList, "Style", "Image"));

						defectListEntity.setPixelSize(String.format("%s,%s",
								XmlUtil.getValueByFour(childDefectList, "x",
										"PixelSize"), XmlUtil.getValueByFour(
										childDefectList, "y", "PixelSize")));

						Element maskType = XmlUtil.getElementByName(
								childDefectList, "MaskType");

						if (maskType != null) {
							defectListEntity.setMaskType(childDefectList
									.elementText("MaskType"));
						}

						// System.out.println(defectListEntity
						// .getId());
						// System.out.println(defectListEntity
						// .getSWCSCoordinates());
						// System.out.println(defectListEntity
						// .getSize());
						// System.out.println(defectListEntity
						// .getMajorAxisSize());
						// System.out.println(defectListEntity
						// .getMajorMinorAxisAspectRatio());
						// System.out.println(defectListEntity
						// .getArea());
						// System.out.println(defectListEntity.getDieAddress());
						// System.out.println(defectListEntity.getImageName());
						// System.out.println(defectListEntity.getStyle());
						// System.out.println(defectListEntity.getPixelSize());

						defectListEntity.setResultId(waferResultEntity
								.getResultId());

						listDefectListEntity.add(defectListEntity);
					}
				}

				waferEntity.setInspectionPassEntity(inspectionPassEntity);
				waferEntity
						.setInspectedDieListEntity(listInspectedDieListEntity);
				waferEntity.setDefectListEntity(listDefectListEntity);
			}
		}

		/*
		 * double numDefect = model.getDefectiveDie(); double numDie =
		 * waferResultEntity.getNumDIE(); double sfield = Math.round((numDie -
		 * numDefect) * 10000 / numDie) / 100.0;
		 * 
		 * waferResultEntity.setSFIELD(sfield);
		 * waferResultEntity.setNUMDEFECT((long) numDefect);
		 */

		waferEntity.setInspectionInfoEntity(model);

		Utils.LastLayer = model.getLayer();

		return model;
	}
}
