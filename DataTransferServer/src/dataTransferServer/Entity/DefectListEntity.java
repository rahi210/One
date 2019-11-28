package dataTransferServer.Entity;

/*
 * 晶片检测缺陷信息列表
 */
public class DefectListEntity {
	private int id;
	private int passId;
	private String inspId;

	private String inspectionType;
	private String sWCSCoordinates;
	private String inspClassifiID;
	private String size;
	private String majorAxisSize;
	private String majorMinorAxisAspectRatio;
	private String area;
	private String dieAddress;
	private String imageName;
	private String style;
	private String pixelSize;
	private String isChecked;
	private int checkedDate;
	private String checkedBy;
	private String modifiedDefect;

	private String resultId;
	
	private int classificationitemId;
	
	private String maskType;

	public void setId(int id) {
		this.id = id;
	}

	public int getId() {
		return id;
	}

	public void setPassId(int passId) {
		this.passId = passId;
	}

	public int getPassId() {
		return passId;
	}

	public void setInspId(String inspId) {
		this.inspId = inspId;
	}

	public String getInspId() {
		return inspId;
	}

	public void setInspectionType(String inspectionType) {
		this.inspectionType = inspectionType;
	}

	public String getInspectionType() {
		return inspectionType;
	}

	public void setSWCSCoordinates(String sWCSCoordinates) {
		this.sWCSCoordinates = sWCSCoordinates;
	}

	public String getSWCSCoordinates() {
		return sWCSCoordinates;
	}

	public void setInspClassifiID(String inspClassifiID) {
		this.inspClassifiID = inspClassifiID;
	}

	public String getInspClassifiID() {
		return inspClassifiID;
	}

	public void setSize(String size) {
		this.size = size;
	}

	public String getSize() {
		return size;
	}

	public void setMajorAxisSize(String majorAxisSize) {
		this.majorAxisSize = majorAxisSize;
	}

	public String getMajorAxisSize() {
		return majorAxisSize;
	}

	public void setMajorMinorAxisAspectRatio(String majorMinorAxisAspectRatio) {
		this.majorMinorAxisAspectRatio = majorMinorAxisAspectRatio;
	}

	public String getMajorMinorAxisAspectRatio() {
		return majorMinorAxisAspectRatio;
	}

	public void setArea(String area) {
		this.area = area;
	}

	public String getArea() {
		return area;
	}

	public void setDieAddress(String dieAddress) {
		this.dieAddress = dieAddress;
	}

	public String getDieAddress() {
		return dieAddress;
	}

	public void setImageName(String imageName) {
		this.imageName = imageName;
	}

	public String getImageName() {
		return imageName;
	}

	public void setStyle(String style) {
		this.style = style;
	}

	public String getStyle() {
		return style;
	}

	public void setPixelSize(String pixelSize) {
		this.pixelSize = pixelSize;
	}

	public String getPixelSize() {
		return pixelSize;
	}

	public void setIsChecked(String isChecked) {
		this.isChecked = isChecked;
	}

	public String getIsChecked() {
		return isChecked;
	}

	public void setCheckedDate(int checkedDate) {
		this.checkedDate = checkedDate;
	}

	public int getCheckedDate() {
		return checkedDate;
	}

	public void setCheckedBy(String checkedBy) {
		this.checkedBy = checkedBy;
	}

	public String getCheckedBy() {
		return checkedBy;
	}

	public void setModifiedDefect(String modifiedDefect) {
		this.modifiedDefect = modifiedDefect;
	}

	public String getModifiedDefect() {
		return modifiedDefect;
	}

	public void setResultId(String resultId) {
		this.resultId = resultId;
	}

	public String getResultId() {
		return resultId;
	}

	public void setClassificationitemId(int classificationitemId) {
		this.classificationitemId = classificationitemId;
	}

	public int getClassificationitemId() {
		return classificationitemId;
	}

	public void setMaskType(String maskType) {
		this.maskType = maskType;
	}

	public String getMaskType() {
		return maskType;
	}
}
