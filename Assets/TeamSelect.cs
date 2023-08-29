using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamSelect : MonoBehaviour
{
	[SerializeField]
	Button _upButton;
	[SerializeField]
	Button _downButton;

	[SerializeField]
	private TeamEnum _teamEnum;

	[SerializeField]
	private bool _isFirst;

	private StartGamePanel _panel;
	private Image _image;

	void Start()
	{
		_panel = GetComponentInParent<StartGamePanel>();
		_image = GetComponentInParent<Image>();

		_upButton.onClick.AddListener(Up);
		_downButton.onClick.AddListener(Down);

		_image.sprite = _panel.sprites[(int)_teamEnum];
	}

	private void Up()
	{
		_teamEnum = (TeamEnum)(((int)_teamEnum + 1) % (int)TeamEnum.End);
		_image.sprite = _panel.sprites[(int)_teamEnum];
	}

	private void Down()
	{
		if (_teamEnum == 0)
			_teamEnum = (TeamEnum)((int)TeamEnum.End - 1);
		else
			_teamEnum = (TeamEnum)(int)_teamEnum-1;

		Debug.Log(_teamEnum);
		_image.sprite = _panel.sprites[(int)_teamEnum];
	}

	public void SetTeam()
	{
		GameManager.Instance.ChangeTeam(_teamEnum, _isFirst);
	}
}
