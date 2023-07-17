using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOfRocket : MonoBehaviour
{
    #region Private Fields
    private Transform _transform;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _engine;
    [SerializeField] private float _forceMagnitude;
    [SerializeField] private float _rotationMagnitude;


    [SerializeField] private ParticleSystem _engineParticles;
    [SerializeField] private ParticleSystem _leftThrusterParticles;
    [SerializeField] private ParticleSystem _rightThrusterParticles;
    #endregion


    void Awake()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        OnSpacePressed();
        ApplyRotation();
    }


    private void OnSpacePressed()
    {
        ManageAudioAndParticleProcess();
        ApplyThrust();
    }

    private void ManageAudioAndParticleProcess()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _audioSource.PlayOneShot(_engine);
            PlayParticleEffect(_engineParticles);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _audioSource.Stop();
            _engineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddRelativeForce(Vector3.up * _forceMagnitude * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void ApplyRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Rotate(_rotationMagnitude);
            PlayParticleEffect(_rightThrusterParticles);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            Rotate(-1 * _rotationMagnitude);
            PlayParticleEffect(_leftThrusterParticles);
        }
        else
        {
            StopParticleEffects();
        }
    }

    private void Rotate(float magnitude)
    {
        _rb.freezeRotation = true;
        _transform.Rotate(Vector3.forward * magnitude * Time.deltaTime);
        _rb.freezeRotation = false;

    }
    private void PlayParticleEffect(ParticleSystem particle)
    {
        if (!particle.isPlaying) particle.Play();
    }
    private void StopParticleEffects()
    {
        _rightThrusterParticles.Stop();
        _leftThrusterParticles.Stop();
    }


}
