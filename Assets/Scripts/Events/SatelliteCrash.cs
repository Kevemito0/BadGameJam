using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SatelliteCrash : MonoBehaviour
{
    [Header("Sesler")]
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip fallingSound;

    [Header("Ekran Kararma")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float waitBeforeReload = 1f;

    private AudioSource _audioSource;
    private bool _crashed = false;

    private GameObject _fadeObj;
    private Material _fadeMat;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.spatialBlend = 0f;
        _audioSource.loop = true;
        // Spawn olunca ses yok — PlayFallingSound() dışarıdan çağrılır
    }

    private void Start()
    {
        CreateFadeOverlay();
    }

    public void PlayFallingSound()
    {
        if (fallingSound != null)
        {
            _audioSource.clip = fallingSound;
            _audioSource.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_crashed) return;
        _crashed = true;

        _audioSource.Stop();
        _audioSource.loop = false;

        if (crashSound != null)
            _audioSource.PlayOneShot(crashSound);

        StartCoroutine(CrashSequence());
    }

    private IEnumerator CrashSequence()
    {
        yield return new WaitForSeconds(0.3f);

        if (_fadeObj != null)
        {
            _fadeObj.SetActive(true);
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                _fadeMat.color = new Color(0f, 0f, 0f, Mathf.Clamp01(elapsed / fadeDuration));
                yield return null;
            }
        }

        yield return new WaitForSeconds(waitBeforeReload);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CreateFadeOverlay()
    {
        _fadeObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _fadeObj.name = "FadeOverlay";
        Destroy(_fadeObj.GetComponent<Collider>());

        _fadeMat = new Material(Shader.Find("Sprites/Default"));
        _fadeMat.color = new Color(0f, 0f, 0f, 0f);
        _fadeObj.GetComponent<Renderer>().material = _fadeMat;

        Camera cam = Camera.main;
        if (cam != null)
        {
            _fadeObj.transform.SetParent(cam.transform);
            _fadeObj.transform.localPosition = new Vector3(0f, 0f, cam.nearClipPlane + 0.01f);
            float h = 2f * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * (cam.nearClipPlane + 0.01f);
            float w = h * cam.aspect;
            _fadeObj.transform.localScale = new Vector3(w, h, 1f);
            _fadeObj.transform.localRotation = Quaternion.identity;
        }

        _fadeObj.SetActive(false);
    }
}