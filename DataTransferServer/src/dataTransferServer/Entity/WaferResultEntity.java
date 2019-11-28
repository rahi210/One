package dataTransferServer.Entity;

/*
 * 晶片检测结果主表
 */
public class WaferResultEntity {

	private String resultId;
	private long startTime;
	private long completionTime;
	private long reviewStartTime;
	private long reviewCompletionTime;
	private long lotStartTime;
	private long lotCompletionTime;
	private String disposition;
	private String masterToolName;
	private String masterSoftwareVersion;
	private String masterToolComputerName;
	private String masterToolToHostLinkState;
	private String moduleName;
	private String computerName;
	private String softwareVersion;
	private String primarySurface;

	private String identificationID;
	private String classificationInfoID;
	private String dieLayoutID;

	private String isChecked;
	private long checkedDate;
	private String checkedBy;
	private long createdDate;

	private double sFIELD = 0.0;
	private long nUMDEFECT = 0;
	private long numDIE =0;

	private IdentificationEntity identificationEntity;
	private ClassificationSchemeEntity classificationSchemeEntity;
	private DieLayoutEntity dieLayoutEntity;

	public String getResultId() {
		return resultId;
	}

	public void setResultId(String resultId) {
		this.resultId = resultId;
	}

	public void setStartTime(long startTime) {
		this.startTime = startTime;
	}

	public long getStartTime() {
		return startTime;
	}

	public void setCompletionTime(long completionTime) {
		this.completionTime = completionTime;
	}

	public long getCompletionTime() {
		return completionTime;
	}

	public void setReviewStartTime(long reviewStartTime) {
		this.reviewStartTime = reviewStartTime;
	}

	public long getReviewStartTime() {
		return reviewStartTime;
	}

	public void setReviewCompletionTime(long reviewCompletionTime) {
		this.reviewCompletionTime = reviewCompletionTime;
	}

	public long getReviewCompletionTime() {
		return reviewCompletionTime;
	}

	public void setLotStartTime(long lotStartTime) {
		this.lotStartTime = lotStartTime;
	}

	public long getLotStartTime() {
		return lotStartTime;
	}

	public void setLotCompletionTime(long lotCompletionTime) {
		this.lotCompletionTime = lotCompletionTime;
	}

	public long getLotCompletionTime() {
		return lotCompletionTime;
	}

	public void setDisposition(String disposition) {
		this.disposition = disposition;
	}

	public String getDisposition() {
		return disposition;
	}

	public void setMasterToolName(String masterToolName) {
		this.masterToolName = masterToolName;
	}

	public String getMasterToolName() {
		return masterToolName;
	}

	public void setMasterSoftwareVersion(String masterSoftwareVersion) {
		this.masterSoftwareVersion = masterSoftwareVersion;
	}

	public String getMasterSoftwareVersion() {
		return masterSoftwareVersion;
	}

	public void setMasterToolComputerName(String masterToolComputerName) {
		this.masterToolComputerName = masterToolComputerName;
	}

	public String getMasterToolComputerName() {
		return masterToolComputerName;
	}

	public void setMasterToolToHostLinkState(String masterToolToHostLinkState) {
		this.masterToolToHostLinkState = masterToolToHostLinkState;
	}

	public String getMasterToolToHostLinkState() {
		return masterToolToHostLinkState;
	}

	public void setModuleName(String moduleName) {
		this.moduleName = moduleName;
	}

	public String getModuleName() {
		return moduleName;
	}

	public void setComputerName(String computerName) {
		this.computerName = computerName;
	}

	public String getComputerName() {
		return computerName;
	}

	public void setSoftwareVersion(String softwareVersion) {
		this.softwareVersion = softwareVersion;
	}

	public String getSoftwareVersion() {
		return softwareVersion;
	}

	public void setPrimarySurface(String primarySurface) {
		this.primarySurface = primarySurface;
	}

	public String getPrimarySurface() {
		return primarySurface;
	}

	public void setIdentificationID(String identificationID) {
		this.identificationID = identificationID;
	}

	public String getIdentificationID() {
		return identificationID;
	}

	public void setClassificationInfoID(String classificationInfoID) {
		this.classificationInfoID = classificationInfoID;
	}

	public String getClassificationInfoID() {
		return classificationInfoID;
	}

	public void setDieLayoutID(String dieLayoutID) {
		this.dieLayoutID = dieLayoutID;
	}

	public String getDieLayoutID() {
		return dieLayoutID;
	}

	public void setIsChecked(String isChecked) {
		this.isChecked = isChecked;
	}

	public String getIsChecked() {
		return isChecked;
	}

	public void setCheckedDate(long checkedDate) {
		this.checkedDate = checkedDate;
	}

	public long getCheckedDate() {
		return checkedDate;
	}

	public void setCheckedBy(String checkedBy) {
		this.checkedBy = checkedBy;
	}

	public String getCheckedBy() {
		return checkedBy;
	}

	public void setCreatedDate(long createdDate) {
		this.createdDate = createdDate;
	}

	public long getCreatedDate() {
		return createdDate;
	}

	public void setIdentificationEntity(
			IdentificationEntity identificationEntity) {
		this.identificationEntity = identificationEntity;
	}

	public IdentificationEntity getIdentificationEntity() {
		return identificationEntity;
	}

	public void setClassificationSchemeEntity(
			ClassificationSchemeEntity classificationSchemeEntity) {
		this.classificationSchemeEntity = classificationSchemeEntity;
	}

	public ClassificationSchemeEntity getClassificationSchemeEntity() {
		return classificationSchemeEntity;
	}

	public void setDieLayoutEntity(DieLayoutEntity dieLayoutEntity) {
		this.dieLayoutEntity = dieLayoutEntity;
	}

	public DieLayoutEntity getDieLayoutEntity() {
		return dieLayoutEntity;
	}

	public void setSFIELD(double sFIELD) {
		this.sFIELD = sFIELD;
	}

	public double getSFIELD() {
		return sFIELD;
	}

	public void setNUMDEFECT(long nUMDEFECT) {
		this.nUMDEFECT = nUMDEFECT;
	}

	public long getNUMDEFECT() {
		return nUMDEFECT;
	}

	public void setNumDIE(long numDIE) {
		this.numDIE = numDIE;
	}

	public long getNumDIE() {
		return numDIE;
	}
}
