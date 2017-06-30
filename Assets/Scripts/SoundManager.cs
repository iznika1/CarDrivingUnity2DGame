using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{


    public bool muted;
    public float volume;
    public Button soundStateButton;
    public Sprite muteButtonSprite;
    public Sprite unmuteButtonSprite;

    public AudioMixerGroup mixerChanel;
    private static string VOLUME_EXPOSED_PARAM = "MyExposedParam";
    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        muted = false;
        if (PlayerPrefs.GetString("SoundState", null) != null)
            muted = PlayerPrefs.GetString("SoundState", null).Equals("Mute") ? muted = true : muted = false;

        GetVolune();
    }

    // Update is called once per frame
    void Update()
    {
        if (muted)
            Mute();
        else
            UnMute();
    }

    private void Mute()
    {
        soundStateButton.image.overrideSprite = muteButtonSprite;
        setVolune(-80f);
        PlayerPrefs.SetString("SoundState", "Mute");
    }

    private void UnMute()
    {
        soundStateButton.image.overrideSprite = unmuteButtonSprite;
        setVolune(-14f);
        PlayerPrefs.SetString("SoundState", "UnMute");
    }


    private void setVolune(float volume)
    {
        mixerChanel.audioMixer.SetFloat(VOLUME_EXPOSED_PARAM, volume);
        PlayerPrefs.SetFloat(VOLUME_EXPOSED_PARAM, volume);
    }

    private void GetVolune()
    {
        mixerChanel.audioMixer.GetFloat("MyExposedParam", out volume);
    }

    public void ChangeSoundState()
    {
        muted = !muted;
    }
}
