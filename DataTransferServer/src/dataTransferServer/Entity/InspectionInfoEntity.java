package dataTransferServer.Entity;

/*
 * æß∆¨ºÏ≤‚–≈œ¢±Ì
 */
public class InspectionInfoEntity {

	private String inspId;
	private String resultId;
	private String moduleName;
	private long startTime;
	private long completionTime;
	private String device;
	private String layer;
	private String name;
	private long modificationTime;
	private String lastAuthor;
	private String disposition;
	private String classificationSchemeID;

	private double defectDensity;
	private double randomDefectDensity;
	private double defectRatio;
	private double defectiveArea;
	private double defectiveDie;

	private String imagesDirectoryName;
	private String recipe_id;

	private long maskA_Die;
	private long maskB_Die;
	private long maskC_Die;
	private long maskD_Die;
	private long maskE_Die;

	private long maskA_Defect;
	private long maskB_Defect;
	private long maskC_Defect;
	private long maskD_Defect;
	private long maskE_Defect;

	private long inspectedDie;
	
	private String maskArray;

	private InspectionPassEntity inspectionPassEntity;

	public void setInspId(String inspId) {
		this.inspId = inspId;
	}

	public String getInspId() {
		return inspId;
	}

	public void setResultId(String resultId) {
		this.resultId = resultId;
	}

	public String getResultId() {
		return resultId;
	}

	public void setModuleName(String moduleName) {
		this.moduleName = moduleName;
	}

	public String getModuleName() {
		return moduleName;
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

	public void setDevice(String device) {
		this.device = device;
	}

	public String getDevice() {
		return device;
	}

	public void setLayer(String layer) {
		this.layer = layer;
	}

	public String getLayer() {
		return layer;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getName() {
		return name;
	}

	public void setModificationTime(long modificationTime) {
		this.modificationTime = modificationTime;
	}

	public long getModificationTime() {
		return modificationTime;
	}

	public void setLastAuthor(String lastAuthor) {
		this.lastAuthor = lastAuthor;
	}

	public String getLastAuthor() {
		return lastAuthor;
	}

	public void setDisposition(String disposition) {
		this.disposition = disposition;
	}

	public String getDisposition() {
		return disposition;
	}

	public void setClassificationSchemeID(String classificationSchemeID) {
		this.classificationSchemeID = classificationSchemeID;
	}

	public String getClassificationSchemeID() {
		return classificationSchemeID;
	}

	public void setDefectDensity(double defectDensity) {
		this.defectDensity = defectDensity;
	}

	public double getDefectDensity() {
		return defectDensity;
	}

	public void setRandomDefectDensity(double randomDefectDensity) {
		this.randomDefectDensity = randomDefectDensity;
	}

	public double getRandomDefectDensity() {
		return randomDefectDensity;
	}

	public void setDefectRatio(double defectRatio) {
		this.defectRatio = defectRatio;
	}

	public double getDefectRatio() {
		return defectRatio;
	}

	public void setDefectiveArea(double defectiveArea) {
		this.defectiveArea = defectiveArea;
	}

	public double getDefectiveArea() {
		return defectiveArea;
	}

	public void setImagesDirectoryName(String imagesDirectoryName) {
		this.imagesDirectoryName = imagesDirectoryName;
	}

	public String getImagesDirectoryName() {
		return imagesDirectoryName;
	}

	public void setDefectiveDie(double defectiveDie) {
		this.defectiveDie = defectiveDie;
	}

	public double getDefectiveDie() {
		return defectiveDie;
	}

	public void setInspectionPassEntity(
			InspectionPassEntity inspectionPassEntity) {
		this.inspectionPassEntity = inspectionPassEntity;
	}

	public InspectionPassEntity getInspectionPassEntity() {
		return inspectionPassEntity;
	}

	public void setRecipe_id(String recipe_id) {
		this.recipe_id = recipe_id;
	}

	public String getRecipe_id() {
		return recipe_id;
	}

	public void setMaskA_Die(long maskA_Die) {
		this.maskA_Die = maskA_Die;
	}

	public long getMaskA_Die() {
		return maskA_Die;
	}

	public void setMaskB_Die(long maskB_Die) {
		this.maskB_Die = maskB_Die;
	}

	public long getMaskB_Die() {
		return maskB_Die;
	}

	public void setMaskC_Die(long maskC_Die) {
		this.maskC_Die = maskC_Die;
	}

	public long getMaskC_Die() {
		return maskC_Die;
	}

	public void setMaskD_Die(long maskD_Die) {
		this.maskD_Die = maskD_Die;
	}

	public long getMaskD_Die() {
		return maskD_Die;
	}

	public void setMaskE_Die(long maskE_Die) {
		this.maskE_Die = maskE_Die;
	}

	public long getMaskE_Die() {
		return maskE_Die;
	}

	public void setMaskA_Defect(long maskA_Defect) {
		this.maskA_Defect = maskA_Defect;
	}

	public long getMaskA_Defect() {
		return maskA_Defect;
	}

	public void setMaskB_Defect(long maskB_Defect) {
		this.maskB_Defect = maskB_Defect;
	}

	public long getMaskB_Defect() {
		return maskB_Defect;
	}

	public void setMaskC_Defect(long maskC_Defect) {
		this.maskC_Defect = maskC_Defect;
	}

	public long getMaskC_Defect() {
		return maskC_Defect;
	}

	public void setMaskD_Defect(long maskD_Defect) {
		this.maskD_Defect = maskD_Defect;
	}

	public long getMaskD_Defect() {
		return maskD_Defect;
	}

	public void setMaskE_Defect(long maskE_Defect) {
		this.maskE_Defect = maskE_Defect;
	}

	public long getMaskE_Defect() {
		return maskE_Defect;
	}

	public void setInspectedDie(long inspectedDie) {
		this.inspectedDie = inspectedDie;
	}

	public long getInspectedDie() {
		return inspectedDie;
	}

	public void setMaskArray(String maskArray) {
		this.maskArray = maskArray;
	}

	public String getMaskArray() {
		return maskArray;
	}
}
