package dataTransferServer.Entity;

import java.util.List;

public class WaferEntity {

	private WaferResultEntity waferResultEntity;
	private IdentificationEntity identificationEntity;
	private ClassificationSchemeEntity classificationSchemeEntity;
	private List<ClassificationItemEntity> classificationItemEntity;
	private DieLayoutEntity dieLayoutEntity;
	private List<DieLayoutListEntity> dieLayoutListEntity;
	private InspectionInfoEntity inspectionInfoEntity;
	private InspectionPassEntity inspectionPassEntity;
	private List<InspectedDieListEntity> inspectedDieListEntity;
	private List<DefectListEntity> defectListEntity;

	public void setWaferResultEntity(WaferResultEntity waferResultEntity) {
		this.waferResultEntity = waferResultEntity;
	}

	public WaferResultEntity getWaferResultEntity() {
		return waferResultEntity;
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

	public void setClassificationItemEntity(
			List<ClassificationItemEntity> classificationItemEntity) {
		this.classificationItemEntity = classificationItemEntity;
	}

	public List<ClassificationItemEntity> getClassificationItemEntity() {
		return classificationItemEntity;
	}

	public void setDieLayoutEntity(DieLayoutEntity dieLayoutEntity) {
		this.dieLayoutEntity = dieLayoutEntity;
	}

	public DieLayoutEntity getDieLayoutEntity() {
		return dieLayoutEntity;
	}

	public void setDieLayoutListEntity(
			List<DieLayoutListEntity> dieLayoutListEntity) {
		this.dieLayoutListEntity = dieLayoutListEntity;
	}

	public List<DieLayoutListEntity> getDieLayoutListEntity() {
		return dieLayoutListEntity;
	}

	public void setInspectionInfoEntity(
			InspectionInfoEntity inspectionInfoEntity) {
		this.inspectionInfoEntity = inspectionInfoEntity;
	}

	public InspectionInfoEntity getInspectionInfoEntity() {
		return inspectionInfoEntity;
	}

	public void setInspectionPassEntity(
			InspectionPassEntity inspectionPassEntity) {
		this.inspectionPassEntity = inspectionPassEntity;
	}

	public InspectionPassEntity getInspectionPassEntity() {
		return inspectionPassEntity;
	}

	public void setInspectedDieListEntity(
			List<InspectedDieListEntity> inspectedDieListEntity) {
		this.inspectedDieListEntity = inspectedDieListEntity;
	}

	public List<InspectedDieListEntity> getInspectedDieListEntity() {
		return inspectedDieListEntity;
	}

	public void setDefectListEntity(List<DefectListEntity> defectListEntity) {
		this.defectListEntity = defectListEntity;
	}

	public List<DefectListEntity> getDefectListEntity() {
		return defectListEntity;
	}

}
