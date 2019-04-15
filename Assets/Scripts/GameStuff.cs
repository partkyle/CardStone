using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this may come in handy for a later attempt with
// enums that have methods on them
//
// https://stackoverflow.com/questions/469287/c-sharp-vs-java-enum-for-those-new-to-c
//
// I'm not sure if the GameEvent concept works for all cases

public class GameStuff : MonoBehaviour
{
    public abstract class GameEvent
    {
        // another interesting concept would be to return events here.
        // That could be an interesting way of changing state
        // throughout the game loop
        // For example, a play event would trigger some sort of unity play event
        // and notify those listeners,
        // and then it could return a GameEvent for summon
        // PlayCard could translate to PlayMinion and PlaySpell,
        // though these concepts might not work out because play is changed based on
        // it being targeted or not.
        //
        // though maybe not.
        //
        // Playing a BattleCry minion could be broken into a Effect or TargetedEffect and a Summon.
        // All of these are different phases.
        public abstract void Process(GameState state);

        // A class for wrapping up multiple events of the same type
        public class MultiEvent : GameEvent
        {
            List<GameEvent> events;

            public MultiEvent(params GameEvent[] events)

            {
                this.events = new List<GameEvent>(events);
            }

            public override void Process(GameState state)
            {
                foreach (GameEvent e in events)
                {

                    e.Process(state);
                }
            }
        }

        public class PlayMinion : GameEvent
        {
            Hand hand;
            Card card;

            public PlayMinion(Hand hand, Card card)
            {
                this.hand = hand;
                this.card = card;
            }

            public override void Process(GameState state)
            {
                hand.Remove(card);
            }
        }

        public class SummonMinion : GameEvent
        {
            Minion minion;
            Board board;
            int position;


            public SummonMinion(Minion minion, Board board, int position)
            {
                this.minion = minion;
                this.board = board;
                this.position = position;
            }

            public override void Process(GameState state)
            {
                board.Summon(minion, position);
            }
        }

        public class AttackMinion : GameEvent
        {
            public Minion attacker;
            public Minion defender;

            public AttackMinion(Minion attacker, Minion defender)
            {
                this.attacker = attacker;
                this.defender = defender;
            }

            public override void Process(GameState state)
            {
                defender.TakeDamage(attacker.Attack);
                attacker.TakeDamage(defender.Attack);
            }

        }
    }

    public class GameState
    {
        public Player Player { get; }
        public Player Opponent { get; }
    }

    public class Player
    {
        public Board Board { get; private set; }
        public Deck Deck { get; private set; }
        public Hand Hand { get; private set; }

        public Player(Board board, Deck deck, Hand hand)
        {
            Board = board;
            Deck = deck;
            Hand = hand;
        }

        public void Draw()
        {
            Card c = Deck.Draw();
            Hand.Add(c);
        }

        public GameEvent PlayCard(Card c)
        {
            return new GameEvent.PlayMinion(Hand, c);
        }
    }

    public class Board
    {
        public static int MAX_MINIONS;

        public List<Minion> Minions { get; } = new List<Minion>();

        public bool CanSummon()
        {
            return Minions.Count >= MAX_MINIONS;
        }

        public void Summon(Minion minion, int position)
        {
            Minions.Insert(position, minion);
        }
    }

    public class Card
    {
        System.Guid id;

        public override bool Equals(object obj)
        {
            Card c = (Card)obj;
            return id.Equals(c.id);
        }
    }

    public class Deck
    {
        public List<Card> Cards { get; private set; }

        public Deck(List<Card> cards)
        {
            Cards = cards;
        }

        public Card Draw()
        {
            Card card = Cards[0];
            Cards.RemoveAt(0);

            return card;
        }
    }

    public class Hand
    {
        public List<Card> Cards { get; } = new List<Card>();

        public void Add(Card c)
        {
            Cards.Add(c);
        }

        public void Remove(Card c)
        {
            Cards.Remove(c);
        }
    }

    public class Minion
    {
        public int Attack { get; }
        public int MaxHealth { get; }
        public int Damage { get; private set; }

        public void TakeDamage(int damage)
        {
            Damage -= damage;
        }
    }
}