using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitfireBullet : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 20f;
    [SerializeField] float _selfDestroyDistance = 1000f;
    AudioSource _audioSource;
    Transform _planeTransform;
    [SerializeField] ParticleSystem fireParticle;

    private void Awake()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _planeTransform = transform.parent;
        Instantiate(fireParticle,transform.parent);
        transform.parent = null;
    }

    private void Update()
    {
        transform.Translate(transform.forward * _bulletSpeed * Time.deltaTime, Space.World);
        if ((transform.position - _planeTransform.position).magnitude > _selfDestroyDistance)
            Destroy(gameObject);
    }

    public void AssignPlaneTransform(Transform planeTransform)
    {
        _planeTransform = transform;
    }
}
