using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PitchType
{
	FourSeamFastBall,
	TwoSeamFastBall,
	CurveBall,
	SliderBall,
}

public class PitchSelect : MonoBehaviour
{
	[SerializeField]
	private PitchType _type;

	private PitchSelector selector;

	private Text _text;

	private void Start()
	{
		selector = GetComponentInParent<PitchSelector>();
		_text = GetComponentInChildren<Text>();
		_text.text = _type.ToString();

		GetComponent<Button>().onClick.AddListener(SelectType);
	}

	public void SelectType()
	{
		selector.TypeSelect(_type);
	}
}
