using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] Transform _lever;
    [SerializeField] Vector3 _maxSwing = new Vector3(20f, 20f, 20f);
    [SerializeField] Vector3 _swing = new Vector3();
    [SerializeField] Vector3 _swingSpeed = Vector3.one;

    [SerializeField] Vector3 _inputs;
    
    public float SwingX { get { return _lever.localEulerAngles.x > 180f ? -(Mathf.Abs(360f - _lever.localEulerAngles.x)) : _lever.localEulerAngles.x ; } }
    public float SwingY { get { return _lever.localEulerAngles.y > 180f ? -(Mathf.Abs(360f - _lever.localEulerAngles.y)) : _lever.localEulerAngles.y; } }
    public float SwingZ { get { return _lever.localEulerAngles.z > 180f ? -(Mathf.Abs(360f - _lever.localEulerAngles.z)) : _lever.localEulerAngles.z ; } }

    Vector2 primaryThumbstick;
    Vector2 secondaryThumbstick;
    void Update()
    {
        primaryThumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        secondaryThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        _inputs = new Vector3(secondaryThumbstick.y,primaryThumbstick.x,-secondaryThumbstick.x);
        _lever.localEulerAngles = MapSwing(_inputs);
    }

    Vector3 MapSwing(Vector3 inputs)
    {
        float angleX = _lever.localEulerAngles.x > 180f ? -(Mathf.Abs(360f - _lever.localEulerAngles.x)) : _lever.localEulerAngles.x;        
        float outSwingX = Mathf.Clamp(angleX + (inputs.x - angleX / _maxSwing.x) * Time.deltaTime * _swingSpeed.x, -_maxSwing.x,_maxSwing.x);
        
        float angleY = _lever.localEulerAngles.y > 180f ? -(Mathf.Abs(360f - _lever.localEulerAngles.y)) : _lever.localEulerAngles.y;        
        float outSwingY = Mathf.Clamp(angleY + (inputs.y - angleY / _maxSwing.y) * Time.deltaTime * _swingSpeed.y, -_maxSwing.y,_maxSwing.y);
        
        float angleZ = _lever.localEulerAngles.z > 180f ? -(Mathf.Abs(360f - _lever.localEulerAngles.z)) : _lever.localEulerAngles.z;        
        float outSwingZ = Mathf.Clamp(angleZ + (inputs.z - angleZ / _maxSwing.z) * Time.deltaTime * _swingSpeed.z, -_maxSwing.z,_maxSwing.z);
        return new Vector3(outSwingX,outSwingY, outSwingZ);
    }
}
