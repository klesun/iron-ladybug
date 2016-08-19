using UnityEngine;
using System.Collections;

public class HeroStats : MonoBehaviour 
{
	public QuoteBox quoteBox;
	public NpcControl npc;

	public int _strawberryCount = 0;
	public int strawberryCount {
		get { return _strawberryCount; }
		set { if (_strawberryCount != value) {
			_strawberryCount = value;
			quoteBox.ShowStats (this, npc);
		} }
	}
	public int _cockshotCount = 0;
	public int cockshotCount {
		get { return _cockshotCount; }
		set { if (_cockshotCount != value) {
			_cockshotCount = value;
			quoteBox.ShowStats (this, npc);
		} }
	}
	public int _enemyCount = 0;
	public int enemyCount {
		get { return _enemyCount; }
		set { if (_enemyCount != value) {
			_enemyCount = value;
			quoteBox.ShowStats (this, npc);
		} }
	}

	private int _totalStrawberryCount = 0;
	public int totalStrawberryCount { get { return _totalStrawberryCount; } }
	private int _totalCockshotCount = 0;
	public int totalCockshotCount { get { return _totalCockshotCount; } }
	private int _totalEnemyCount = 0;
	public int totalEnemyCount { get { return _totalEnemyCount; } }

	void Start () 
	{
		_totalStrawberryCount += Object.FindObjectsOfType<Strawberry> ().Length;
		_totalCockshotCount += Object.FindObjectsOfType<Cockshot> ().Length;
		_totalEnemyCount += Object.FindObjectsOfType<EnemyLogic> ().Length;
	}
}
