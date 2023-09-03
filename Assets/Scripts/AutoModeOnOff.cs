using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AutoModeOnOff : MonoBehaviour
{
    private Button _button;
	private Image _image;

	[SerializeField]
	private Batter _batter;

	[SerializeField]
	private Color _onColor;
	[SerializeField]
	private Color _offColor;

	bool isOn = true;
	void Start()
    {
		_button = GetComponent<Button>();
		_image = GetComponent<Image>();
		_button.onClick.AddListener(ButtonOnOff);
		ButtonOnOff();
	}

	private void ButtonOnOff()
	{
		if(isOn)
		{
			_batter.BatAutoModeOn();
			_image.color = _onColor;
		}
		else
		{
			_batter.BatAutoModeOff();
			_image.color = _offColor;
		}
		isOn = !isOn;
	}
}
