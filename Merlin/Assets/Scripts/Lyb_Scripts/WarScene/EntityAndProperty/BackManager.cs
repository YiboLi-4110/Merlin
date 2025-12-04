// 修改！！！！！！

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackManager : MonoBehaviour
{
    public Entity FatherObject;
    private SpriteRenderer BackGround;
    public Vector3 offset = new Vector3(0,0.6f,0.1f);
    Color BackGroundColor;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = FatherObject.transform.position + offset;
        BackGround = gameObject.GetComponent<SpriteRenderer>();
        BackGround.sprite = null;
        BackGroundColor = BackGround.color;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (FatherObject.Status.CurrentSituation.Item1)
        {
            case EntityProperty.situation.Nothing:
                {
                    BackGround.sprite = null;
                }
                break;
            case EntityProperty.situation.Burn:
                {
                    BackGround.sprite = Resources.Load<Sprite>("Textures/burn_situation");
                }
                break;
            case EntityProperty.situation.Frozen:
                {
                    BackGround.sprite = Resources.Load<Sprite>("Textures/frozen_situation");
                }
                break;
            case EntityProperty.situation.Toxic:
                {
                    BackGround.sprite = Resources.Load<Sprite>("Textures/toxic_situation");
                }
                break;
            default:
             break;
        }

        BackGroundColor.a = 0.5f;
        BackGround.color = BackGroundColor;

    }
}
