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
        // create an object from the defined prefab and add it to the desiredParent     
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(desiredParent);

        // update the texts of the prefab
        // there must be a better way to do this
        foreach (UnityEngine.UI.Text text in obj.GetComponentsInChildren<UnityEngine.UI.Text>())
        {
            Debug.Log(text);
            if (text.name == "Card Text")
            {
                text.text = "Black Lotus";
            }

            if (text.name == "Card Description")
            {
                text.text = "Win on turn 1. Discard $1,200.";
            }
        }
   }
}
