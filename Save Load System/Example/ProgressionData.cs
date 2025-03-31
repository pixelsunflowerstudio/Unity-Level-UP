using UnityEngine;

public class ProgressionData : ISaveData
{
	public int MagnetLevel;
	public int ShieldLevel;
	public int DoubleScoreLevel;
	public int InvincibleLevel;

	public void InitData()
	{
		MagnetLevel = 1;
		ShieldLevel = 1;
		DoubleScoreLevel = 1;
		InvincibleLevel = 1;
	}
}