package dataTransferServer.Entity;

import java.util.List;

/*
 * ¾§Æ¬Layout±í
 */
public class DieLayoutEntity {

	private String layoutId;
	private String dieAddressRange;
	private String anchorDie;
	private String size;
	private String pitch;
	private String dieAddress;
	private String sWCSCoordinates;
	private String dieAddressIncrement;
	private int columns;
	private int rows;

	private List<DieLayoutListEntity> dieLayoutListEntity;

	public void setLayoutId(String layoutId) {
		this.layoutId = layoutId;
	}

	public String getLayoutId() {
		return layoutId;
	}

	public void setDieAddressRange(String dieAddressRange) {
		this.dieAddressRange = dieAddressRange;
	}

	public String getDieAddressRange() {
		return dieAddressRange;
	}

	public void setAnchorDie(String anchorDie) {
		this.anchorDie = anchorDie;
	}

	public String getAnchorDie() {
		return anchorDie;
	}

	public void setSize(String size) {
		this.size = size;
	}

	public String getSize() {
		return size;
	}

	public void setPitch(String pitch) {
		this.pitch = pitch;
	}

	public String getPitch() {
		return pitch;
	}

	public void setDieAddress(String dieAddress) {
		this.dieAddress = dieAddress;
	}

	public String getDieAddress() {
		return dieAddress;
	}

	public void setSWCSCoordinates(String sWCSCoordinates) {
		this.sWCSCoordinates = sWCSCoordinates;
	}

	public String getSWCSCoordinates() {
		return sWCSCoordinates;
	}

	public void setDieAddressIncrement(String dieAddressIncrement) {
		this.dieAddressIncrement = dieAddressIncrement;
	}

	public String getDieAddressIncrement() {
		return dieAddressIncrement;
	}

	public void setColumns(int columns) {
		this.columns = columns;
	}

	public int getColumns() {
		return columns;
	}

	public void setRows(int rows) {
		this.rows = rows;
	}

	public int getRows() {
		return rows;
	}

	public void setDieLayoutListEntity(
			List<DieLayoutListEntity> dieLayoutListEntity) {
		this.dieLayoutListEntity = dieLayoutListEntity;
	}

	public List<DieLayoutListEntity> getDieLayoutListEntity() {
		return dieLayoutListEntity;
	}

}
