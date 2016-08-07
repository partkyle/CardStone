using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject title;
    public GameObject description;
    public GameObject cost;
    public GameObject attack;
    public GameObject health;

    public GameManager.CardPlaceholder referenceCard;

    public void Awake()
    {
        title = transform.Find("Title").gameObject;
        description = transform.Find("Description").gameObject;
        cost = transform.Find("Cost").gameObject;
        attack = transform.Find("Attack").gameObject;
        health = transform.Find("Health").gameObject;
    }

    public void InitCard()
    {
        SetTitle(this.referenceCard.title);
        SetDescription(this.referenceCard.description);
        SetCost(this.referenceCard.cost);
        SetAttack(this.referenceCard.attack);
        SetHealth(this.referenceCard.health);
    }

    private void SetTitle(string title)
    {
        this.title.GetComponent<Text>().text = title;
    }

    private void SetDescription(string description)
    {
        this.description.GetComponent<Text>().text = description;
    }

    private void SetCost(string cost)
    {
        this.cost.GetComponent<Text>().text = cost;
    }

    private void SetAttack(string attack)
    {
        this.attack.GetComponent<Text>().text = attack;
    }

    private void SetHealth(string health)
    {
        this.health.GetComponent<Text>().text = health;
    }
}