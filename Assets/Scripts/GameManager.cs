using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public class CardPlaceholder
    {
        public string cardName;
        public string description;
		public string cost = "0";
		public string attack ="0";
		public string health = "0";
    }

    public List<CardPlaceholder> cards;
    public GameObject hand;
    public GameObject cardPrefab;

    public void Awake()
    {
        Debug.Log("GameManager::Awake");

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitCards();

        Hand handObject = hand.GetComponent<Hand>();
        if (handObject != null)
        {
            for (int i = 0; i < 3; i++)
            {
                //handObject.Draw(cards[Random.Range(0, cards.Count - 1)]);
                CardPlaceholder card = RandomCard();
                Debug.Log("creating card " + card.cardName);
                CreateCardObject(card);
            }
        }
        else
        {
            Debug.LogWarning("Not a hand object");
        }
    }

    public CardPlaceholder RandomCard()
    {
        CardPlaceholder card = cards[Random.Range(0, cards.Count)];
        return card;
    }

    public void CreateCardObject(CardPlaceholder cardToCreate)
    {
        GameObject card = Instantiate(cardPrefab);
		Card c = card.GetComponent<Card> ();
        c.SetTitle(cardToCreate.cardName);
        c.SetDescription(cardToCreate.description);
		c.SetCost (cardToCreate.cost);
		c.SetAttack (cardToCreate.attack);
		c.SetHealth (cardToCreate.health);
        card.transform.SetParent(hand.transform);

        // FIXME:
        // not sure why this is needed. This seems to work when the draw card button is clicked.
        // perhaps it has something to do with the board not being completly loaded, and the size is
        // off there?
        card.transform.localScale = Vector3.one;

        Debug.Log("created card component " + card);
    }

    private void InitCards()
    {
        cards = new List<CardPlaceholder>();
		cards.Add(new CardPlaceholder { cardName = "Boulderfist Ogre", cost = "6", attack = "6", health = "7"});
		cards.Add(new CardPlaceholder { cardName = "Chillwind Yeti", cost = "4", attack = "4", health = "5" });
		cards.Add(new CardPlaceholder { cardName = "Yogg Saron, Hope's End", description = "Battlecry: OH SHIT!", cost = "10", attack = "7", health = "5" });
		cards.Add(new CardPlaceholder { cardName = "N'zoth", description = "Battlecry: One more time, with feeling.", cost = "10", attack = "5", health = "7" });
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
