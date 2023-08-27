using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterState : State
{
    [SerializeField]
    private GameObject _pitchObj;

    [SerializeField]
    private GameObject _batObject;

	private Batter _batter;

	private GameObject _ballObject => GameManager.Instance.BallObject;

	private bool _isBat = false;

	private void Start()
	{
		GameManager.Instance.onStateChange += BatReset;
		_batter = GetComponent<Batter>();
	}

	public override bool IsStateOn()
	{
		if(GameManager.Instance.State == BattingState.Pitching)
		{
			if (Vector3.Distance(_ballObject.transform.position,
				this.transform.position) < 8 && !_isBat)
			{
				return true;
			}
		}

		return false;
	}

	public override void StateOn()
	{
		Debug.Log("Bat");

		Vector3 vec = _pitchObj.transform.position;

		float x =  Random.Range(vec.x - 0.2f, vec.x + 0.2f);
		float y =  Random.Range(vec.y - 0.2f, vec.y + 0.2f);

		_batObject.transform.position = new Vector3(x, y, vec.z);

		_batter.Bat();

		_isBat = true;
	}

	private void BatReset(BattingState state) => _isBat = false;
}
