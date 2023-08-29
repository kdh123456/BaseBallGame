using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoModeOnOff : MonoBehaviour
{
    private Button _button;

	[SerializeField]
	private Batter _batter;

	bool isOn = true;
	void Start()
    {
		_button = GetComponent<Button>();
		_button.onClick.AddListener(ButtonOnOff);
	}

	private void ButtonOnOff()
	{
		if(isOn)
			_batter.BatAutoModeOn();
		else
			_batter.BatAutoModeOff();
		isOn = !isOn;
	}
}
