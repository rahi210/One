package dataTransferServer.Entity;

/*
 * 晶片检测Die信息列表
 */
public class InspectedDieListEntity {

	private String inspectedDieId;
	private int passId;
	private String inspId;
	private String dieAddress;
	private String classificationID;
	private String disposition;

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

	public void setDieAddress(String dieAddress) {
		this.dieAddress = dieAddress;
	}

	public String getDieAddress() {
		return dieAddress;
	}

	public void setClassificationID(String classificationID) {
		this.classificationID = classificationID;
	}

	public String getClassificationID() {
		return classificationID;
	}

	public void setDisposition(String disposition) {
		this.disposition = disposition;
	}

	public String getDisposition() {
		return disposition;
	}

	public void setInspectedDieId(String inspectedDieId) {
		this.inspectedDieId = inspectedDieId;
	}

	public String getInspectedDieId() {
		return inspectedDieId;
	}
}
