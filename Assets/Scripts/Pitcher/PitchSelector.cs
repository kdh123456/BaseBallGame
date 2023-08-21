using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchSelector : MonoBehaviour
{
    private PitchType _type;

    public PitchType Type => _type;

	public void TypeSelect(PitchType type)
    {
        _type = type;
    }
}
