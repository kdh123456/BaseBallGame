using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CountEnum
{
	Strike,
	Ball,
	Out,
	Reset,
	OutReset
}

public class CountPanel : MonoBehaviour
{
	private Image[] _countImage;

	[SerializeField]
	private Color _resetColor;
	[SerializeField]
	public Color _countColor;

	private int _count = 0;

	public CountEnum countenum;

	private void Awake()
	{
		GameManager.Instance.onChangeCount += CountUp;
		GameManager.Instance.onChangeCount += ResetImage;
		GameManager.Instance.onChangeCount += OutResetImage;
		_countImage = GetComponentsInChildren<Image>();
	}

	private void Start()
	{
		ResetImage(CountEnum.Reset);
	}

	private void ResetImage(CountEnum reset)
	{
		if (reset != CountEnum.Reset)
			return;

		if (countenum == CountEnum.Out)
			return;

		foreach (var item in _countImage) 
		{
			item.color = _resetColor;
		}
		_count = 0;
	}

	private void OutResetImage(CountEnum reset)
	{
		if (reset != CountEnum.OutReset)
			return;

		foreach (var item in _countImage)
		{
			item.color = _resetColor;
		}
		_count = 0;
	}

	public void CountUp(CountEnum enums)
	{
		if (enums != countenum)
			return;

		if (_count >= _countImage.Length)
			return;

		_countImage[_count++].color = _countColor;
	}
}
