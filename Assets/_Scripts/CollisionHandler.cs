using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField, Range(0f, 14f)] private float _delayTime;
    [SerializeField] private AudioClip _success;
    [SerializeField] private AudioClip _crash;
    [SerializeField] private ParticleSystem _successParticle;
    [SerializeField] private ParticleSystem _crashParticle;

    private AudioSource _audioSource;
    private bool _isTransitioning;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _isTransitioning = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (_isTransitioning) return;


        string tag = other.gameObject.tag;
        switch (tag)
        {
            case "LaunchPad":
                Debug.Log("Launch Pad'e bastim");
                break;
            case "LandingPad":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartCrashSequence()
    {
        _isTransitioning = true;
        
        _audioSource.Stop();
        _audioSource.PlayOneShot(_crash);

        _crashParticle.Play();
        GetComponent<MovementOfRocket>().enabled = false;
        Invoke(nameof(ReloadScene), _delayTime);
    }
    private void StartSuccessSequence()
    {
        _isTransitioning = true;

        _audioSource.Stop();
        _audioSource.PlayOneShot(_success);

        _successParticle.Play();
        GetComponent<MovementOfRocket>().enabled = false;
        Invoke(nameof(LoadNextLevel), _delayTime);
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        int nextLevelIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextLevelIndex);
    }
}
