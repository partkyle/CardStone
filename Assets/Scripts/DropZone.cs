using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class DropZone : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop to " + this.gameObject.name);
    }
    
}
