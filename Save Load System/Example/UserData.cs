using UnityEngine.Device;

public class UserData : ISaveData
{
	public bool MuteMusic;
	public bool MuteSound;
	public string CurrentLanguage;

	public void InitData()
	{
		MuteMusic = false;
		MuteSound = false;
		CurrentLanguage = "EN";
	}
}