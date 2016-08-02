using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject cardNameObject;
    public GameObject descriptionObject;

    public void SetCardName(string cardName)
    {
        cardNameObject.GetComponent<Text>().text = cardName;
    }

    public void SetDescription(string description)
    {
        descriptionObject.GetComponent<Text>().text = description;
    }
}