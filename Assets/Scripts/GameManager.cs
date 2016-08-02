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
        // TODO: figure out why the text resize doesn't work.
        card.GetComponent<Card>().SetCardName(cardToCreate.cardName);
        card.GetComponent<Card>().SetDescription(cardToCreate.description);
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
        cards.Add(new CardPlaceholder { cardName = "Ogre" });
        cards.Add(new CardPlaceholder { cardName = "Yogg Saron, Hope's End", description = "Battlecry: OH SHIT!" });
        cards.Add(new CardPlaceholder { cardName = "Danny McBride", description = "Battlecry: pretty much ruin everything" });
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
