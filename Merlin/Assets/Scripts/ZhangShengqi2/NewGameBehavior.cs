
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NewGameBehavior : MonoBehaviour
{
    public Button button1;
    public Button button2;
    //public Button button_continue;
    // Start is called before the first frame update
    void Start()
    {
        button1 = transform.Find("Panel/Button1").GetComponent<Button>();
        button2 = transform.Find("Panel/Button2").GetComponent<Button>();

        button1.onClick.AddListener(OnNewButtonClicked);
        button2.onClick.AddListener(OnLoadButtonClicked);
    }

    // Update is called once per frame
    void OnNewButtonClicked()
    {
        SceneManager.LoadScene("InputScene");
    }

    void OnLoadButtonClicked()
    {
        if(TheGlobalManager.TGM.TrytoLoad())
        {
            SceneManager.LoadScene("WarScene");
        }
        else
        {
            Debug.Log("Cant load the scene");
        }
    }
}
