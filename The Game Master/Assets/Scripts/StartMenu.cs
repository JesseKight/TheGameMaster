using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Button play;
    public Slider music;
    public AudioSource backgroundMusic;
    void Start()
    {
        play.onClick.AddListener(PlayClick);
    }

    private void Update()
    {
        backgroundMusic.volume = music.value;
    }

    void PlayClick()
    {
        SceneManager.LoadScene("Level1");
    }
}
