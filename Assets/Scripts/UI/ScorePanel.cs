using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum TeamEnum
{
	SSGRenders = 0,
	KIATigers,
	SamsungLions,
	HanwhaEagles,
	DoosanBears,
	LGTweens,
	NCDinos,
	KTWiz,
	LotteGiants,
	KiwoomHeros
}

public class ScorePanel : MonoBehaviour
{
	[SerializeField]
	private TeamScore[] _teamScore;

	[SerializeField]
	private Sprite[] _sprits;

	private void Start()
	{
		for(int i = 0; i< _teamScore.Length; i++)
		{
			TeamChange(_teamScore[i].ThisTeam, i);
		}
	}

	public void TeamChange(TeamEnum teamSelect, int teamIndex)
	{
		_teamScore[teamIndex].ChnageTeam(_sprits[(int)teamSelect], teamSelect);
	}
}
