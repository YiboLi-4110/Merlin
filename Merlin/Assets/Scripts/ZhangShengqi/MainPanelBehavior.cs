using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MainPanelBehavior : MonoBehaviour
{
    //public Magicscript magicscript;
    public ElementPanelBehavior[] Targetpanels;
    public Image[] Upperimages = new Image[4];
    //public Image[] Lowerimages = new Image[4];
    private TextMeshProUGUI textComponent; // 存储 Text 组件的引用
    //public Button button;
    //private bool myBoolVariable;
    //public Image eleshow;
    // Start is called before the first frame update
    private int[] InitNum = new int[4];
    private int[] CurrNum = new int[4];
    public Button[] buttons;


    void Start()
    {
        for (int i=0; i<4; i++)
        {
            Upperimages[i].sprite = GameManagerBehavior.gm.sprites[i];
            //InitNum[i] = UnityEngine.Random.Range(1,9);
            InitNum[i] = RandomDataBehavior.savedData[i];
            CurrNum[i] = InitNum[i];
            textComponent = Upperimages[i].GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = InitNum[i].ToString();
            GameObject LowerimageElement = GameManagerBehavior.gm.LowImage.transform.Find("Element"+i).gameObject;
            textComponent = LowerimageElement.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = 0.ToString();
            //textComponent = Lowerimages[i].GetComponentInChildren<TextMeshProUGUI>();
            //textComponent.text = magicscript.ElementInfo[i].ToString();
        }   
        if (Targetpanels.Length != buttons.Length)
        {
            Debug.LogError("Panels and buttons count must be the same!");
            return;
        }

        // 为每个按钮添加点击事件
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 为了避免闭包问题
            buttons[i].onClick.AddListener(() => ShowPanel(index));
        }

        // 初始时隐藏所有面板
        HideAllPanels();
                                 
    }
    
    // Update is called once per frame
    void Update()
    {
        //for (int i=0;i<4;i++)
        //{
            //Upperimages[i].sprite = GameManagerBehavior.sprites[i];
        //}
        Updatenum();
        Setnum();
    }
    void Updatenum()
    {
        for (int i=0; i<4; i++)
        {
            //InitNum[i] = Random.Range(1,9);
            textComponent = Upperimages[i].GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = InitNum[i].ToString();
            //textComponent = Lowerimages[i].GetComponentInChildren<TextMeshProUGUI>();
            //textComponent.text = magicscript.ElementInfo[i].ToString();
        }    
    }

    void Setnum()
    {
            int[] temp = new int[4];
            for (int i=0;i<4;i++)
            {
                temp[i] = InitNum[i];
            }
        for (int j=0;j<Targetpanels.Length;j++)
        {
        if (Targetpanels[j].HasBeenCreate)
        {
            
            bool flag = true;
            for (int i=0;i<4;i++)
            {
                //temp[i] = InitNum[i];
                if (InitNum[i]-Targetpanels[j].magicscript.ElementInfo[i]>=0)
                {
                    InitNum[i]-=Targetpanels[j].magicscript.ElementInfo[i];
                }
                else
                {
                    flag = false;
                    Targetpanels[j].HasBeenCreate = false;
                    Targetpanels[j].SetButton.interactable = false;
                    Debug.Log("Hey@@@@@@@");
                }
                    
            }
            if (flag)
                Updatenum();
        }
        }
        for (int i=0;i<4;i++)
{
    InitNum[i] = temp[i];
} 
    }
    private void ShowPanel(int index)
    {
        HideAllPanels();
        Targetpanels[index].gameObject.SetActive(true);

        Targetpanels[index].GetButton.onClick.RemoveAllListeners();
        Targetpanels[index].SetButton.onClick.RemoveAllListeners();

        Targetpanels[index].GetButton.onClick.AddListener(Targetpanels[index].OnButtonClick);
        Targetpanels[index].SetButton.onClick.AddListener(Targetpanels[index].OnSetButtonClick);

        if(Targetpanels[index].HasBeenCreate)
        {
            Debug.Log("Setbutton interactable true");
            Targetpanels[index].SetButton.interactable = true;
        }
        else
        {
            Targetpanels[index].SetButton.interactable = false;
            Debug.Log("Setbutton interactable false");
        }
        for (int i=0; i<GameManagerBehavior.gm.BasicElementNum; i++)
        {
            GameObject LowerimageElement = GameManagerBehavior.gm.LowImage.transform.Find("Element"+i).gameObject;
            textComponent = LowerimageElement.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = Targetpanels[index].magicscript.ElementInfo[i].ToString();
            //eleshow.sprite = magicscript.sprite;
            //LowerimageElement.GetComponent<Image>().sprite = GameManagerBehavior.gm.sprites[i];
            Debug.Log("seccesful gettext");
        }  

    }

    // 隐藏所有面板
    private void HideAllPanels()
    {
        foreach (ElementPanelBehavior panel in Targetpanels)
        {
            panel.gameObject.SetActive(false);
        }
    }
}
