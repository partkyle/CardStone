using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public enum Turn
    {
        Player,
        Opponent
    };

    Turn currentTurn = Turn.Player;

    public class CardPlaceholder
    {
        public string title;
        public string description;
        public string cost = "0";
        public string attack = "0";
        public string health = "0";
    }

    private List<CardPlaceholder> cards;

    public GameObject cardPrefab;
    public GameObject minionPrefab;

    public GameObject playerHand;
    public GameObject playerTabletop;
    public GameObject opponnentHand;
    public GameObject opponnentTabletop;

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

        Turn[] turns = new Turn[] { Turn.Player, Turn.Opponent };
        foreach (Turn turn in turns)
        {
            Hand handObject;
            if (turn == Turn.Player)
            {
                handObject = opponnentHand.GetComponent<Hand>();
            }
            else
            {
                handObject = playerHand.GetComponent<Hand>();
            }
            if (handObject != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    DrawCardForPlayer(turn);
                }
            }
            else
            {
                Debug.LogWarning("Not a hand object");
            }
        }


        ResetTurnState();
    }

    private void DrawCardForPlayer(Turn turn)
    {
        //handObject.Draw(cards[Random.Range(0, cards.Count - 1)]);
        CardPlaceholder card = RandomCard();
        Debug.Log("creating card " + card.title);
        CreateCardObject(card, turn);
    }

    public CardPlaceholder RandomCard()
    {
        CardPlaceholder card = cards[Random.Range(0, cards.Count)];
        return card;
    }

    public void CreateCardObject(CardPlaceholder cardToCreate, Turn player = Turn.Player)
    {
        GameObject card = Instantiate(cardPrefab);
        Card c = card.GetComponent<Card>();

        c.referenceCard = cardToCreate;
        c.InitCard();

        Transform destination;
        if (player == Turn.Player)
        {
            destination = playerHand.transform;
            card.tag = "Player";
        }
        else
        {
            destination = opponnentHand.transform;
            card.tag = "Opponent";
        }
        card.transform.SetParent(destination);

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
        cards.Add(new CardPlaceholder { title = "Boulderfist Ogre", cost = "6", attack = "6", health = "7" });
        cards.Add(new CardPlaceholder { title = "Chillwind Yeti", cost = "4", attack = "4", health = "5" });
        cards.Add(new CardPlaceholder { title = "Yogg Saron, Hope's End", description = "Battlecry: OH SHIT!", cost = "10", attack = "7", health = "5" });
        cards.Add(new CardPlaceholder { title = "N'zoth", description = "Battlecry: One more time, with feeling.", cost = "10", attack = "5", health = "7" });
    }

    public void DropCard(GameObject dropZone, GameObject card, int siblingIndex)
    {
        GameObject minion = Instantiate<GameObject>(minionPrefab);
        Card minionRef = minion.GetComponent<Card>();
        minionRef.referenceCard = card.GetComponent<Card>().referenceCard;
        minionRef.InitCard();

        minion.transform.SetParent(dropZone.transform);
        minion.transform.SetSiblingIndex(siblingIndex);

        // FIXME:
        // not sure why this is needed. This seems to work when the draw card button is clicked.
        // perhaps it has something to do with the board not being completly loaded, and the size is
        // off there?
        minion.transform.localScale = Vector3.one;

        Destroy(card);
    }

    public void SwitchTurn()
    {
        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Opponent;
        }
        else
        {
            currentTurn = Turn.Player;
        }

        ResetTurnState();
    }

    private void ResetTurnState()
    {
        Debug.Log("setting up turn for " + currentTurn);

        if (currentTurn == Turn.Player)
        {
            //opponnentHand.GetComponent<Draggable>().enabled = false;
            opponnentTabletop.GetComponent<DropZone>().enabled = false;
            //playerHand.GetComponent<Draggable>().enabled = true;
            playerTabletop.GetComponent<DropZone>().enabled = true;
        }
        else
        {
            //opponnentHand.GetComponent<Draggable>().enabled = true;
            opponnentTabletop.GetComponent<DropZone>().enabled = true;
            //playerHand.GetComponent<Draggable>().enabled = false;
            playerTabletop.GetComponent<DropZone>().enabled = false;
        }

        DrawCardForPlayer(currentTurn);
    }
}
