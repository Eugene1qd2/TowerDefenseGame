using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowCost : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject info;
    public void OnPointerEnter(PointerEventData eventData)
    {

        info.active = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        info.active = false;
    }

}
