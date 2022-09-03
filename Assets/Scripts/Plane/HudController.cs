using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudController : MonoBehaviour
{
    [SerializeField] Transform _hudCompass;
    [SerializeField] TextMeshProUGUI _speedGauge;
    void Update()
    {
        _hudCompass.localEulerAngles = new Vector3(_hudCompass.localEulerAngles .x,transform.localEulerAngles.y,_hudCompass.localEulerAngles.z);
        UpdateSpeedGauge();
    }

    void UpdateSpeedGauge()
    {
        _speedGauge.text = (10*(int)GameManager.ins.PlaneSpeed).ToString();
    }
        
}
