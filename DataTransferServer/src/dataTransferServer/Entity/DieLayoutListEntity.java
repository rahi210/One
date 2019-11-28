package dataTransferServer.Entity;

import java.io.Serializable;

/*
 * ¾§Æ¬Die±í
 */
public class DieLayoutListEntity implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private String id;
	private String layoutId;
	private int dieAddressX;
	private int dieAddressY;
	private String disposition;

	private String inspClassifiID;
	private String reviewClassifcationID;
	private String autoDieClassifierClassificationID;
	private String processSentinelDieClassificationID;
	private String isInspectable;
	private String isEdgeDie;

	public void setId(String id) {
		this.id = id;
	}

	public String getId() {
		return id;
	}

	public void setLayoutId(String layoutId) {
		this.layoutId = layoutId;
	}

	public String getLayoutId() {
		return layoutId;
	}

	public void setDieAddressX(int dieAddressX) {
		this.dieAddressX = dieAddressX;
	}

	public int getDieAddressX() {
		return dieAddressX;
	}

	public void setDieAddressY(int dieAddressY) {
		this.dieAddressY = dieAddressY;
	}

	public int getDieAddressY() {
		return dieAddressY;
	}

	public void setDisposition(String disposition) {
		this.disposition = disposition;
	}

	public String getDisposition() {
		return disposition;
	}

	public void setInspClassifiID(String inspClassifiID) {
		this.inspClassifiID = inspClassifiID;
	}

	public String getInspClassifiID() {
		return inspClassifiID;
	}

	public void setReviewClassifcationID(String reviewClassifcationID) {
		this.reviewClassifcationID = reviewClassifcationID;
	}

	public String getReviewClassifcationID() {
		return reviewClassifcationID;
	}

	public void setAutoDieClassifierClassificationID(
			String autoDieClassifierClassificationID) {
		this.autoDieClassifierClassificationID = autoDieClassifierClassificationID;
	}

	public String getAutoDieClassifierClassificationID() {
		return autoDieClassifierClassificationID;
	}

	public void setProcessSentinelDieClassificationID(
			String processSentinelDieClassificationID) {
		this.processSentinelDieClassificationID = processSentinelDieClassificationID;
	}

	public String getProcessSentinelDieClassificationID() {
		return processSentinelDieClassificationID;
	}

	public void setIsInspectable(String isInspectable) {
		this.isInspectable = isInspectable;
	}

	public String getIsInspectable() {
		return isInspectable;
	}

	public void setIsEdgeDie(String isEdgeDie) {
		this.isEdgeDie = isEdgeDie;
	}

	public String getIsEdgeDie() {
		return isEdgeDie;
	}
}
