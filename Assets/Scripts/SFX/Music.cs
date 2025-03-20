using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public static Music instance { get; private set; }
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _musicSlider;
    private AudioSource _music;
    void Awake()
    {
        float volume = PlayerPrefs.GetFloat("Music");
        _musicSlider.value = volume;
        _music = GetComponent<AudioSource>();
        if(insance == null)
            instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(transform.gameObject);
    }
    private void Start()
    {
        ChangeVolume();
    }
    public void ChangeVolume()
    {
        float volume = _musicSlider.value;
        _mixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("Music", volume);
    }
}
