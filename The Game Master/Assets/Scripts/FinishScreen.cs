using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
    public Button reset;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = reset.GetComponent<Button>();
        btn.onClick.AddListener(Reload);
    }

    // Update is called once per frame
    void Reload()
    {
        SceneManager.LoadScene("Level1");
    }
}
