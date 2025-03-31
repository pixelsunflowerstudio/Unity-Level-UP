public class UserDataStorage : DataStorage<UserData>
{
	protected override bool _manualSave => false;
	protected override StorageType _storageType => StorageType.UserData;
}