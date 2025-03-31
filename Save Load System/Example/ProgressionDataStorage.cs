public class ProgressionDataStorage : DataStorage<ProgressionData>
{
	protected override bool _manualSave => false;
	protected override StorageType _storageType => StorageType.ProgressionData;
}