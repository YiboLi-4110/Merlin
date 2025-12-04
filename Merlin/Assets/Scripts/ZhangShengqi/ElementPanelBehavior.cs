using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ElementPanelBehavior : MonoBehaviour
{
    public Magicscript magicscript;
    private TextMeshProUGUI textComponent; // 存储 Text 组件的引用
    public TextMeshProUGUI text1; 
    public TextMeshProUGUI text2; 
    public Button GetButton = null;

    public Button SetButton = null;

    public bool HasBeenCreate = false;
    public Image eleshow = null;



    void Awake()
    {
        text1 = transform.Find("ImageText/NameText").GetComponent<TextMeshProUGUI>();
        text2 = transform.Find("ImageText/FuncText").GetComponent<TextMeshProUGUI>();
        eleshow = transform.Find("ImageText/Elementshow").GetComponent<Image>();
        Debug.Log("Element setbutton");
        GetButton = GameManagerBehavior.gm.GetButton.GetComponent<Button>();
        SetButton = GameManagerBehavior.gm.SetButton.GetComponent<Button>();
        SetButton.interactable = false;

        for (int i=0; i<GameManagerBehavior.gm.BasicElementNum; i++)
        {
            GameObject LowerimageElement = GameManagerBehavior.gm.LowImage.transform.Find("Element"+i).gameObject;
            textComponent = LowerimageElement.GetComponentInChildren<TextMeshProUGUI>();
            textComponent.text = magicscript.ElementInfo[i].ToString();
            eleshow.sprite = magicscript.sprite;
            LowerimageElement.GetComponent<Image>().sprite = GameManagerBehavior.gm.sprites[i];
            Debug.Log("seccesful gettext");
        }  
        text1.text = magicscript.NameInfo;
        text2.text = magicscript.FuncInfo;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        Setcolor();
    }

    public void OnButtonClick()
    {
        HasBeenCreate = true;
        SetButton.interactable = true;
        //Debug.Log("OnClick here");
        // 将 bool 变量设为 true
        //myBoolVariable = !myBoolVariable;
        //Debug.Log("按钮已点击，myBoolVariable 变为: " + myBoolVariable);

        //int index = GameManagerBehavior.gm.Getnum();

        //if (myBoolVariable)  
            //CreateChildImage(GameManagerBehavior.gm.Selected_box);
        //else
        //    DestroyChildImage(GameManagerBehavior.gm.Selected_box);
        
    }


    public void OnSetButtonClick()
    {
        DestroyChildImage(GameManagerBehavior.gm.Selected_box);
        CreateChildImage(GameManagerBehavior.gm.Selected_box);
    }



    private Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            // 递归查找子对象的子对象
            Transform foundChild = FindChildByName(child, name);
            if (foundChild != null)
                return foundChild;
        }

        return null;    
    }


    private void DestroyChildImage(Boxselected SelectedBox)
    {
            Transform childTransform = FindChildByName(SelectedBox.transform, "ChildImage");
            if(childTransform == null) return;
            GameObject.Destroy(childTransform.gameObject);  
            GameManagerBehavior.gm.ClearTheBox(SelectedBox);
    }


    private void CreateChildImage(Boxselected SelectedBox)
    {
        // 创建一个新的 GameObject 作为子对象
        GameObject childImageObject = new GameObject("ChildImage");
        // 设置子对象的父对象为传入的父 GameObject
        childImageObject.transform.SetParent(SelectedBox.transform, false);

        // 为子对象添加 Image 组件
        Image childImage = childImageObject.AddComponent<Image>();
        if (childImage == null)
        {
            Debug.LogError("无法为子对象添加 Image 组件。");
            return;
        }
        // 调用设置 Sprite 的方法
        SetSpriteForChildImage(childImage, SelectedBox);
    }



    private void SetSpriteForChildImage(Image childImage, Boxselected SelectedBox)
    {
        // 为子 Image 的 sprite 属性赋值
        childImage.sprite = magicscript.sprite;
        RectTransform childRectTransform = childImage.GetComponent<RectTransform>();
        childRectTransform.sizeDelta *= 0.2f;

        GameManagerBehavior.gm.ClearTheBox(SelectedBox);
        if(magicscript.IsBullet)
        {
            SelectedBox.IsBulllet = true;
            Type spelltype = Type.GetType(magicscript.TypeName);
            if(spelltype == null)
                Debug.Log("Cant understand the type name");
            SelectedBox.SpellType = spelltype;
        }
        if(magicscript.IsComponent)
        {
            SelectedBox.IsComponent = true;
            Type spelltype = Type.GetType(magicscript.TypeName);
            if(spelltype == null)
                Debug.Log("Cant understand the type name");
            SelectedBox.SpellType = spelltype;
        }
        
    }



    void Setcolor()
    {
        if (HasBeenCreate)
        {
            Color mcolor = eleshow.color;
            mcolor.a = 1.0f;
            eleshow.color = mcolor;
            //Debug.Log("按钮已点击，myBoolVariable 变为: " + myBoolVariable);
        }
        else
        {
            Color mcolor = eleshow.color;
            mcolor.a = 10f/255f;
            eleshow.color = mcolor;
        }
    }
}
