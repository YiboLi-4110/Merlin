
using UnityEngine;
using UnityEngine.UI;

public class GameManagerBehavior : MonoBehaviour
{
    public Sprite[] sprites= new Sprite[4];
    public static GameManagerBehavior gm = null; 
    public int[] InitNum = new int[4];
    //public Type[] types = new Type[5];
    public static GameObject[] Boxes = new GameObject[5];

    public string[] Spell_inBoxes_withName = new string[5];

    public bool[] Spell_inBoxes_isBullet = new bool[5]; 
    public bool[] Spell_inBoxes_isComponent = new bool[5]; 

    //public bool[] BoxStatus = new bool[5];
    //public Image[] Images = new Image[5];
    // Start is called before the first frame update

    // control the bag
    public GameObject BagCanvas;
    public GameObject PauseCanvas;
    public bool IsMoving = true;

    public Boxselected Selected_box;

    // the number of the enemies in the map
    public int NumberOfEnemy;

    public int BasicElementNum = 4;

    public int SpellNum = 5;

    public GameObject GetButton = null;

    public GameObject SetButton = null;

    public GameObject LowImage = null;
    public GameObject InstruCanvas;
// 用于引用按钮组件
    public Button toggleButton;
// 用于记录Panel的当前显示状态
    private bool isPanelVisible = false;
    public GameObject MonsterCanvas;
// 用于引用按钮组件
    public Button monsterButton;
// 用于记录Panel的当前显示状态
    private bool isMonsterVisible = false;

    void Awake()
    {
        IsMoving = true;
        gm = this;
        sprites[0] = Resources.Load<Sprite>("Textures/" + "Ignis");
        if (sprites[0]!=null)
        Debug.Log("success 0");
        sprites[1] = Resources.Load<Sprite>("Textures/" + "Aqua");
        Debug.Log("success 1");
        sprites[2] = Resources.Load<Sprite>("Textures/" + "Terra");
        Debug.Log("success 2");
        sprites[3] = Resources.Load<Sprite>("Textures/" + "Aer");
        Debug.Log("success 3");
        
        NumberOfEnemy = TheGlobalManager.TGM.GetdDifficult();

        Setthoseboxes();
        SetTwoButtons();
    }

    void Start()
    {
        Selected_box = Boxes[0].GetComponent<Boxselected>();
        for (int i=0; i<4; i++)
        {
            InitNum[i] = UnityEngine.Random.Range(1,9);
        }
        //Setfiveboxes();
        /*for (int i=0;i<5;i++)
        {
            BoxStatus[i] = false;
        }*/

        if(!TheGlobalManager.TGM.IsSafe())
        {
            GameObject TempEnemy;
                    // 生成敌人
            int climate = TheGlobalManager.TGM.Climate();
            if(climate == 1)
            {
                for (int i=0; i<(NumberOfEnemy-1); i++)
                {
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/WaterMonster"));
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/AerMonster"));
                }
                TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Boss_0"));
                TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Boss_0"));
            }
            else if(climate == 2)
            {
                for (int i=0; i<NumberOfEnemy; i++)
                {
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/FireMonster"));
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/LittleMonster_1"));
                }
                //TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Boss_0"));
            }
            else if(climate == 3)
            {
                for (int i=0; i<(NumberOfEnemy-1); i++)
                {
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/RockMonster"));
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/LittleMonster_0"));
                }
                TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Boss_1"));
                TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Boss_1"));
                
            }
            else
            {
                for (int i=0; i<NumberOfEnemy; i++)
                {
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/LittleMonster_0"));
                    TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/LittleMonster_1"));
                }
                //TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Boss_0"));
            }
            
            //TempEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Boss_1"));

        }
        else
            NumberOfEnemy = 0;
        toggleButton.onClick.AddListener(TogglePanel);
        monsterButton.onClick.AddListener(MonsterPanel);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            BagCanvas.SetActive(!BagCanvas.gameObject.activeSelf);
            //Setfiveboxes();
            TestBulletManager.TBM.sequence_set(Boxes, 5);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseCanvas.SetActive(!PauseCanvas.gameObject.activeSelf);
        }

        if (BagCanvas.activeSelf==true || PauseCanvas.activeSelf==true || InstruCanvas.activeSelf==true || MonsterCanvas.activeSelf==true) 
            IsMoving = false;
        else IsMoving = true;
    }

    void TogglePanel()
    {
        // 切换Panel的显示状态
        isPanelVisible = !isPanelVisible;
        InstruCanvas.SetActive(isPanelVisible);
//Setfiveboxes();
        //IsMoving = !IsMoving;
        if (isPanelVisible)
        {
            Debug.Log("PanelCanSeeEEEEE");
        }
        // 根据新的状态设置Panel的激活状态
        //panel.SetActive(isPanelVisible);
    }
    void MonsterPanel()
    {
        // 切换Panel的显示状态
        isMonsterVisible = !isMonsterVisible;
        MonsterCanvas.SetActive(isMonsterVisible);
//Setfiveboxes();
        //IsMoving = !IsMoving;
        if (isMonsterVisible)
        {
            Debug.Log("PanelCanSeeEEEEE");
        }
        // 根据新的状态设置Panel的激活状态
        //panel.SetActive(isPanelVisible);
    }
    void Setthoseboxes()
    {
        if(BagCanvas == null)
        {
            Debug.Log("Cant find! BagCanvas is null");
            return;
        }

        for(int i = 0; i < SpellNum; i++)
        {
            Transform getted_box = BagCanvas.transform.Find("Panel/AnOBject/box" + i);
            if(getted_box == null)
                Debug.Log("Panel/AnOBject/box" + i + " is null");
            Boxes[i] = getted_box.gameObject;
        }
    }

    void SetTwoButtons()
    {
        if(BagCanvas == null)
        {
            Debug.Log("Cant find! BagCanvas is null");
            return;
        }
        Transform getted_getbutton = BagCanvas.transform.Find("Panel/GetButton");
        Transform getted_setbutton = BagCanvas.transform.Find("Panel/SetButton");
        Transform image_low = BagCanvas.transform.Find("Panel/ImageLow");
        if(getted_getbutton == null || getted_setbutton == null)
        {
            Debug.Log("GetButton/Setbutton");
        }
        if(image_low == null)
        {
            Debug.Log("ImageLow is null");
        }
        GetButton = getted_getbutton.gameObject;
        SetButton = getted_setbutton.gameObject;
        LowImage = image_low.gameObject;
    }


    public void ClearTheBox(Boxselected box)
    {
        box.IsBulllet = false;
        box.IsComponent = false;
        box.SpellType = null;
    }
}
