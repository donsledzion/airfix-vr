using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitfireController : MonoBehaviour
{
    [SerializeField] float _forwardSpeed = 1f;
    [SerializeField] float _alongRotateSpeed = 10f;
    [SerializeField] float _acrossRotateSpeed = 10f;
    [SerializeField] float _verticalRotateSpeed = 10f;
    [SerializeField] Transform _propeller;
    [SerializeField] float _propellerSpeed = 100f;
    [Space]
    [SerializeField] float _maxSpeed = 50;
    [SerializeField] float _minSpeed = 10;
    [SerializeField] float _accelerationFactor = 5;
    [SerializeField] AudioSource _propellerSound;
    [SerializeField] float _defaultSpeed = 20f;
    [SerializeField] float _defaultPropellerSpeed = 400f;
    [Space]
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _leftGunTransform;
    [SerializeField] Transform _rightGunTransform;
    LeverController _leverController;
    float _fireDelayTime = .08f;
    bool _fireDelay = false;
    bool _leftGunFireDelay = false;
    bool _rightGunFireDelay = false;
    Transform _currentGun;

    private void Start()
    {
        _currentGun = _leftGunTransform;
        _leverController = gameObject.GetComponent<LeverController>();
    }
    void Update()
    {
        transform.Translate(transform.forward * _forwardSpeed * Time.deltaTime, Space.World);
        _propeller.Rotate(_propeller.up, _propellerSpeed * Time.deltaTime, Space.World);
        GameManager.ins.PlaneSpeed = _forwardSpeed;
        HandleInputs();
    }

    private void HandleInputs()
    {

        float primaryTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        float secondaryTrigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

        if (secondaryTrigger > .05f)
            Accelerate(secondaryTrigger);
        else if (primaryTrigger > .05f)
            Accelerate(-primaryTrigger);

        RotateAcross(_leverController.SwingX);
        RotateAlong(-_leverController.SwingZ);
        RotateVertical(_leverController.SwingY);

        if (OVRInput.Get(OVRInput.Button.One))
            Fire(_leftGunTransform);
        if (OVRInput.Get(OVRInput.Button.Three))
            Fire(_rightGunTransform);
    }

    private void Accelerate(float primaryTrigger)
    {
        _forwardSpeed = Mathf.Clamp(_forwardSpeed += primaryTrigger * _accelerationFactor, _minSpeed, _maxSpeed);
        _propellerSound.pitch = _forwardSpeed / _defaultSpeed;
        _propellerSpeed = Mathf.Clamp(_defaultPropellerSpeed * (_forwardSpeed / _defaultSpeed), 100, 2000);
    }

    private void RotateAcross(float y)
    {
        transform.Rotate(transform.right * y * _alongRotateSpeed * Time.deltaTime, Space.World);
    }

    private void RotateAlong(float x)
    {
        transform.Rotate(transform.forward * -x * _alongRotateSpeed * Time.deltaTime, Space.World);
    }

    private void RotateVertical(float y)
    {
        transform.Rotate(transform.up * y * _verticalRotateSpeed * Time.deltaTime, Space.World);
    }

    private void Fire()
    {
        if (!_fireDelay)
        {
            _fireDelay = true;
            Instantiate(_bulletPrefab, _currentGun);
            SwitchGun();
            StartCoroutine("FireDelay");
        }
    }

    private void Fire(Transform gunTransform)
    {
        if (!_leftGunFireDelay && gunTransform == _leftGunTransform)
        {            
            Instantiate(_bulletPrefab, gunTransform);            
            _leftGunFireDelay = true;
            StartCoroutine("LeftGunFireDelay");
        }
        else if (!_rightGunFireDelay && gunTransform == _rightGunTransform)
        {
            Instantiate(_bulletPrefab, gunTransform);
            _rightGunFireDelay = true;
            StartCoroutine("RightGunFireDelay");
        }
    }

    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(_fireDelayTime);
        _fireDelay = false;
    }
    IEnumerator LeftGunFireDelay()
    {
        yield return new WaitForSeconds(_fireDelayTime);
        _leftGunFireDelay = false;
    }

    IEnumerator RightGunFireDelay()
    {
        yield return new WaitForSeconds(_fireDelayTime);
        _rightGunFireDelay = false;
    }

    void SwitchGun()
    {
        if (_currentGun == _leftGunTransform)
            _currentGun = _rightGunTransform;
        else
            _currentGun = _leftGunTransform;
    }

}
