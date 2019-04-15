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

    public class Player
    {
        public string Tag;
        public GameObject hand;
        public GameObject tabletop;

        private List<CardPlaceholder> cards;

        public Player()
        {
            InitCards();
        }

        public void Enable()
        {
            tabletop.GetComponent<DropZone>().enabled = true;
        }

        public void Disable()
        {
            tabletop.GetComponent<DropZone>().enabled = false;
        }

        private void InitCards()
        {
            cards = new List<CardPlaceholder>();
            cards.Add(new CardPlaceholder { title = "Boulderfist Ogre", cost = "6", attack = "6", health = "7" });
            cards.Add(new CardPlaceholder { title = "Chillwind Yeti", cost = "4", attack = "4", health = "5" });
            cards.Add(new CardPlaceholder { title = "Yogg Saron, Hope's End", description = "Battlecry: OH SHIT!", cost = "10", attack = "7", health = "5" });
            cards.Add(new CardPlaceholder { title = "N'zoth", description = "Battlecry: One more time, with feeling.", cost = "10", attack = "5", health = "7" });
        }

        public CardPlaceholder RandomCard()
        {
            CardPlaceholder card = cards[Random.Range(0, cards.Count)];
            return card;
        }

        public CardPlaceholder Draw()
        {
            return RandomCard();
        }
    }

    public Player currentPlayer;
    public Player otherPlayer;

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

        InitGame();

        ResetTurnState();
    }

    public void CreateCardObject(CardPlaceholder cardToCreate, Player player)
    {
        GameObject card = Instantiate(cardPrefab);
        Card c = card.GetComponent<Card>();

        c.referenceCard = cardToCreate;
        c.InitCard();
        c.tag = player.Tag;
        Transform destination = player.hand.transform;
        card.transform.SetParent(destination);

        Debug.Log("created card component " + card);
    }

    private void InitGame()
    {
        currentPlayer = new Player
        {
            Tag = "Player",
            hand = playerHand,
            tabletop = playerTabletop,
        };

        otherPlayer = new Player
        {
            Tag = "Opponent",
            hand = opponnentHand,
            tabletop = opponnentTabletop,
        };

        CreateCardObject(currentPlayer.Draw(), currentPlayer);
        CreateCardObject(currentPlayer.Draw(), currentPlayer);
        CreateCardObject(currentPlayer.Draw(), currentPlayer);

        CreateCardObject(otherPlayer.Draw(), otherPlayer);
        CreateCardObject(otherPlayer.Draw(), otherPlayer);
        CreateCardObject(otherPlayer.Draw(), otherPlayer);
        CreateCardObject(otherPlayer.Draw(), otherPlayer);
    }

    public void DropCard(GameObject dropZone, GameObject card, int siblingIndex)
    {
        GameObject minion = Instantiate(minionPrefab);
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

        card.GetComponent<Card>().GoAway();
    }

    public void SwitchTurn()
    {
        // swap?
        Player tmp = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tmp;

        ResetTurnState();
        // draw a card
        CreateCardObject(currentPlayer.Draw(), currentPlayer);
    }

    private void ResetTurnState()
    {
        currentPlayer.Enable();
        otherPlayer.Disable();
    }

    public class TurnManager
    {
        void Turn()
        {
            Draw();
            WaitForMoves();
            EndTurn();
        }

        void Draw() { }

        void WaitForMoves() { }

        void EndTurn() { }
    }
}
