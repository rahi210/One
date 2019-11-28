package dataTransferServer.Entity;

/*
 * 缺陷分类条目表
 */
public class ClassificationItemEntity {

	private String itemId;
	private String schemeId;
	private int id;
	private String name;
	private String description;
	private String color;
	private int priority;
	private String hotkey;
	private String isAcceptable;
	private String type;
	private String userID;

	public void setItemId(String itemId) {
		this.itemId = itemId;
	}

	public String getItemId() {
		return itemId;
	}

	public void setSchemeId(String schemeId) {
		this.schemeId = schemeId;
	}

	public String getSchemeId() {
		return schemeId;
	}

	public void setId(int id) {
		this.id = id;
	}

	public int getId() {
		return id;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getName() {
		return name;
	}

	public void setDescription(String description) {
		this.description = description;
	}

	public String getDescription() {
		return description;
	}

	public void setColor(String color) {
		this.color = color;
	}

	public String getColor() {
		return color;
	}

	public void setPriority(int priority) {
		this.priority = priority;
	}

	public int getPriority() {
		return priority;
	}

	public void setHotkey(String hotkey) {
		this.hotkey = hotkey;
	}

	public String getHotkey() {
		return hotkey;
	}

	public void setIsAcceptable(String isAcceptable) {
		this.isAcceptable = isAcceptable;
	}

	public String getIsAcceptable() {
		return isAcceptable;
	}

	public void setType(String type) {
		this.type = type;
	}

	public String getType() {
		return type;
	}

	public void setUserID(String userID) {
		this.userID = userID;
	}

	public String getUserID() {
		return userID;
	}
}
