using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boxselected : MonoBehaviour, IPointerClickHandler
{
    public bool IsBulllet = false;
    public bool IsComponent = false;

    public Type SpellType = null;

    private void Awake()
    {
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }
    }
 
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("EventTriggerTest OnClick");
        UnityEngine.UI.Image im = GameManagerBehavior.gm.Selected_box.GetComponent<UnityEngine.UI.Image>();
        UnityEngine.UI.Image im2 = gameObject.GetComponent<UnityEngine.UI.Image>();
        im.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        im2.color = new Color(1.0f, 0.7f, 0.4f, 1.0f);
        GameManagerBehavior.gm.Selected_box = this;
    }

}
