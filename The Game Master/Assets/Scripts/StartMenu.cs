using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public Button play;
    void Start()
    {
        play.onClick.AddListener(PlayClick);
    }

    void PlayClick()
    {
        SceneManager.LoadScene("Level1");
    }
}
