package dataTransferServer.Entity;

import java.util.List;

/*
 * 晶片检测通过信息表
 */
public class InspectionPassEntity {
	private int passId;
	private String inspId;
	private String orientation;
	private String inspectedSurface;
	private String inspectionType;
	private double defectDensity;
	private double defectiveArea;
	private double defectRatio;
	private double randomDefectDensity;
	private int defectiveDie;

	private List<InspectedDieListEntity> listInspectedDieListEntity;
	private List<DefectListEntity> listDefectListEntity;

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

	public void setOrientation(String orientation) {
		this.orientation = orientation;
	}

	public String getOrientation() {
		return orientation;
	}

	public void setInspectedSurface(String inspectedSurface) {
		this.inspectedSurface = inspectedSurface;
	}

	public String getInspectedSurface() {
		return inspectedSurface;
	}

	public void setInspectionType(String inspectionType) {
		this.inspectionType = inspectionType;
	}

	public String getInspectionType() {
		return inspectionType;
	}

	public void setDefectDensity(double defectDensity) {
		this.defectDensity = defectDensity;
	}

	public double getDefectDensity() {
		return defectDensity;
	}

	public void setDefectiveArea(double defectiveArea) {
		this.defectiveArea = defectiveArea;
	}

	public double getDefectiveArea() {
		return defectiveArea;
	}

	public void setDefectRatio(double defectRatio) {
		this.defectRatio = defectRatio;
	}

	public double getDefectRatio() {
		return defectRatio;
	}

	public void setRandomDefectDensity(double randomDefectDensity) {
		this.randomDefectDensity = randomDefectDensity;
	}

	public double getRandomDefectDensity() {
		return randomDefectDensity;
	}

	public void setDefectiveDie(int defectiveDie) {
		this.defectiveDie = defectiveDie;
	}

	public int getDefectiveDie() {
		return defectiveDie;
	}

	public void setListInspectedDieListEntity(
			List<InspectedDieListEntity> listInspectedDieListEntity) {
		this.listInspectedDieListEntity = listInspectedDieListEntity;
	}

	public List<InspectedDieListEntity> getListInspectedDieListEntity() {
		return listInspectedDieListEntity;
	}

	public void setListDefectListEntity(
			List<DefectListEntity> listDefectListEntity) {
		this.listDefectListEntity = listDefectListEntity;
	}

	public List<DefectListEntity> getListDefectListEntity() {
		return listDefectListEntity;
	}

}
