package dataTransferServer.Entity;

import java.util.List;

/*
 * 缺陷分类信息表
 */
public class ClassificationSchemeEntity {

	private String schemeId;
	private String computerName;
	private String name;

	private List<ClassificationItemEntity> listClassificationItemEntity;

	public void setSchemeId(String schemeId) {
		this.schemeId = schemeId;
	}

	public String getSchemeId() {
		return schemeId;
	}

	public void setComputerName(String computerName) {
		this.computerName = computerName;
	}

	public String getComputerName() {
		return computerName;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getName() {
		return name;
	}

	public void setListClassificationItemEntity(
			List<ClassificationItemEntity> listClassificationItemEntity) {
		this.listClassificationItemEntity = listClassificationItemEntity;
	}

	public List<ClassificationItemEntity> getListClassificationItemEntity() {
		return listClassificationItemEntity;
	}
}
