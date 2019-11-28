package dataTransferServer.Entity;

/*
 * 检测设备信息表
 */
public class IdentificationEntity {
	private String identificationId;

	private String lot;
	private String operator;
	private String carrier_ID;
	private String carrier_Station;
	private String device;
	private String layer;
	private String pPName;
	private long modificationTime;
	private String lastAuthor;

	private String substrate_ID;
	private String substrate_Number;
	private String substrate_DiameterMM;
	private String substrate_Slot;
	private String substrate_Type;
	private String substrate_FiducialType;
	private String substrate_NotchLocation;

	public void setIdentificationId(String identificationId) {
		this.identificationId = identificationId;
	}

	public String getIdentificationId() {
		return identificationId;
	}

	public void setLot(String lot) {
		this.lot = lot;
	}

	public String getLot() {
		return lot;
	}

	public void setOperator(String operator) {
		this.operator = operator;
	}

	public String getOperator() {
		return operator;
	}

	public void setCarrier_ID(String carrier_ID) {
		this.carrier_ID = carrier_ID;
	}

	public String getCarrier_ID() {
		return carrier_ID;
	}

	public void setCarrier_Station(String carrier_Station) {
		this.carrier_Station = carrier_Station;
	}

	public String getCarrier_Station() {
		return carrier_Station;
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

	public void setPPName(String pPName) {
		this.pPName = pPName;
	}

	public String getPPName() {
		return pPName;
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

	public void setSubstrate_ID(String substrate_ID) {
		this.substrate_ID = substrate_ID;
	}

	public String getSubstrate_ID() {
		return substrate_ID;
	}

	public void setSubstrate_Number(String substrate_Number) {
		this.substrate_Number = substrate_Number;
	}

	public String getSubstrate_Number() {
		return substrate_Number;
	}

	public void setSubstrate_DiameterMM(String substrate_DiameterMM) {
		this.substrate_DiameterMM = substrate_DiameterMM;
	}

	public String getSubstrate_DiameterMM() {
		return substrate_DiameterMM;
	}

	public void setSubstrate_Slot(String substrate_Slot) {
		this.substrate_Slot = substrate_Slot;
	}

	public String getSubstrate_Slot() {
		return substrate_Slot;
	}

	public void setSubstrate_Type(String substrate_Type) {
		this.substrate_Type = substrate_Type;
	}

	public String getSubstrate_Type() {
		return substrate_Type;
	}

	public void setSubstrate_FiducialType(String substrate_FiducialType) {
		this.substrate_FiducialType = substrate_FiducialType;
	}

	public String getSubstrate_FiducialType() {
		return substrate_FiducialType;
	}

	public void setSubstrate_NotchLocation(String substrate_NotchLocation) {
		this.substrate_NotchLocation = substrate_NotchLocation;
	}

	public String getSubstrate_NotchLocation() {
		return substrate_NotchLocation;
	}
}
