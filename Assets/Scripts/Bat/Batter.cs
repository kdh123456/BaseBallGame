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
		_virtualBat.transform.position = _batPos.transform.position + Vector3.right;
		_virtualBat.vBodys = bat.bodys;
	}

	public void Bat()
	{
		this.transform.position = vec;
		this.transform.rotation = rotation;
		_animator.SetBool("Batting", true);
		GameManager.Instance.ChangeState(BattingState.Bat);
	}

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
