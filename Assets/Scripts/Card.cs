﻿using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject title;
    public GameObject description;
	public GameObject cost;
	public GameObject attack;
	public GameObject health;

	public void Awake()
	{
		title = transform.Find ("Title").gameObject;
		description = transform.Find ("Description").gameObject;
		cost = transform.Find ("Cost").gameObject;
		attack = transform.Find ("Attack").gameObject;
		health = transform.Find ("Health").gameObject;
	}

    public void SetTitle(string title)
    {
        this.title.GetComponent<Text>().text = title;
    }

    public void SetDescription(string description)
    {
        this.description.GetComponent<Text>().text = description;
    }

	public void SetCost(string cost)
	{
		this.cost.GetComponent<Text> ().text = cost;
	}

	public void SetAttack(string attack)
	{
		this.attack.GetComponent<Text> ().text = attack;
	}

	public void SetHealth(string health)
	{
		this.health.GetComponent<Text> ().text = health;
	}
}