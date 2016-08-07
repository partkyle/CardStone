using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform oldParent = null;

    private GameObject placeholder = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        // create a placeholder object when dragging and dropping
        placeholder = new GameObject();
        placeholder.name = "Placeholder";
        placeholder.transform.SetParent(this.transform.parent);

        // copy the relevant layout element pieces
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = this.GetComponent<LayoutElement>().flexibleWidth;
        le.flexibleHeight = this.GetComponent<LayoutElement>().flexibleHeight;

        // make the placeholder take the same spot
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        this.oldParent = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");

        this.transform.position = eventData.position;

        Transform comparison = FindNearestDropPoint(eventData);
        if (comparison == null)
        {
            comparison = this.oldParent;
        }

        this.placeholder.transform.SetParent(comparison);

        int newSiblingIndex = comparison.childCount;

        for (int i = 0; i < comparison.childCount; i++)
        {
            if (this.transform.position.x < comparison.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                {
                    newSiblingIndex--;
                }

                break;
            }
        }

        this.placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        int siblingIndex = this.placeholder.transform.GetSiblingIndex();

        Transform newParent = FindNearestDropPoint(eventData);
        if (newParent == null)
        {
            transform.SetParent(oldParent);
            transform.SetSiblingIndex(siblingIndex);
        }
        else
        {
            GameManager.instance.DropCard(newParent.gameObject, gameObject, siblingIndex);
        }
        oldParent = null;
        
        // clear out the placeholder
        Destroy(placeholder);
    }

    private static Transform FindNearestDropPoint(PointerEventData eventData)
    {
        Transform newParent = null;

        // find all raycasts that are below the mouse,
        // get the first draggable and use that as the drop point.
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            DropZone dropzone = result.gameObject.GetComponent<DropZone>();
            if (dropzone != null)
            {
                newParent = result.gameObject.transform;
                break;
            }
        }

        return newParent;
    }
}
