using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Batter : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private Bat bat;

	[SerializeField]
	private Bat _virtualBat;

	[SerializeField]
	private GameObject _batPos;

	[SerializeField]
	private GameObject _ballPos;
	private bool _autoMode = false; 

	Vector3 vec;
	Quaternion rotation;

	private void Start()
	{
		vec = transform.position;
		rotation = transform.rotation;

		GameManager.Instance.onStateChange += Run;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && GameManager.Instance.State == BattingState.Pitching)
		{
			this.transform.position = vec;
			this.transform.rotation = rotation;
			_animator.SetBool("Batting", true);
			GameManager.Instance.ChangeState(BattingState.Bat);
		}
	}

	private void Batting()
	{
		_virtualBat.gameObject.SetActive(true);
		Vector3 pos = Vector3.zero;
		if(_autoMode)
		{
			Vector3 vec = _ballPos.transform.position;

			float x = Random.Range(vec.x - 0.1f, vec.x + 0.1f);
			float y = Random.Range(vec.y - 0.1f, vec.y + 0.1f);

			pos = new Vector3(x, y, vec.z) + Vector3.right;
		}
		else
		{
			pos = _batPos.transform.position + Vector3.right;
		}
		_virtualBat.transform.position = pos;
		_virtualBat.vBodys = bat.bodys;
	}

	public void Bat()
	{
		this.transform.position = vec;
		this.transform.rotation = rotation;
		_animator.SetBool("Batting", true);
		GameManager.Instance.ChangeState(BattingState.Bat);
	}

	public void BatAutoModeOn() => _autoMode = true;
	public void BatAutoModeOff() => _autoMode = false;

	private void Run(BattingState state)
	{ 
		if(state == BattingState.Batting)
		{
			GameObject obj = Instantiate(GameManager.Instance.RunnerObject);
			obj.transform.position = this.transform.position;
			Runner runner = obj.GetComponent<Runner>();
			runner.RunBase();
			RunnerManager.Instance.AddRunner(runner);
			this.gameObject.SetActive(false);
		}
	}
}
