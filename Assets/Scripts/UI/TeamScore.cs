using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamScore : MonoBehaviour
{
	private int score = 0;

	private TextMeshProUGUI _text;
	private Image _image;

	[SerializeField]
	private TeamEnum _thisTeam;

	public TeamEnum ThisTeam => _thisTeam;

	private void Awake()
	{
		_text = GetComponentInChildren<TextMeshProUGUI>();
		_image = GetComponent<Image>();
	}

	private void Start()
	{
		GameManager.Instance.onAddScore += AddScore;

		
	}

	private void AddScore(TeamEnum teamName, int count)
	{
		if(teamName == _thisTeam)
		{
			score += count;
			_text.text = score.ToString();
		}
	}

	public void ChnageTeam(Sprite sprite, TeamEnum teamName)
	{
		_image.sprite = sprite;
		_thisTeam = teamName;
	}
}
