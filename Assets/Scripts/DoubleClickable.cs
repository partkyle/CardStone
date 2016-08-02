using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DoubleClickable : MonoBehaviour, IPointerClickHandler
{
    public GameObject prefab;
    public Transform desiredParent;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.CreateCardObject(GameManager.instance.RandomCard());
   }
}
