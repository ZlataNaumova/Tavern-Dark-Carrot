using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerDeck;
    [SerializeField] private PlayingCardView cardPrefab;

    private List<PlayingCard> playerCards = new List<PlayingCard>();
    private List<PlayingCard> enemyCards = new List<PlayingCard>();
    private List<PlayingCard> playerActionDeck = new List<PlayingCard>();
    private List<PlayingCard> enemyActionDeck = new List<PlayingCard>();

    private int enemyStrength = 0;
    private int playerStrength = 0;

    private CardGameState currentCardGameState;

    private System.Random random = new System.Random();

    private void OnEnable()
    {
        TavernEventsManager.DefendersToCards += GeneratePlayersCards;

        UpdateCardGameState(CardGameState.start);
    }

    private void OnDisable()
    {
        TavernEventsManager.DefendersToCards -= GeneratePlayersCards;
    }
    private void AddCardToDeck(VisitorAI defender)
    {
        var cardView = Instantiate(cardPrefab, playerDeck.transform);
        cardView.Render(defender);
    }

    private void GeneratePlayersCards(List<VisitorAI> defenders)
    {
        foreach (VisitorAI d in defenders)
        {
            AddCardToDeck(d);
        }
        //foreach (VisitorAI d in defenders)
        //{
        //    playerCards.Add(new PlayingCard(d.Strenght, d.DefenderType));
        //}
        //GenerateEnemyDeck();
    }
    private void GenerateEnemyDeck()
    {
        foreach (PlayingCard c in playerCards)
        {
            enemyCards.Add(new PlayingCard(c.strength - 1, c.cardType));
        }
    }

    private void CardGameStarts()
    {
        if(playerCards.Count > 0)
        {
            
            UpdateCardGameState(CardGameState.enemyTurn);
        }
        

    }

    private void EnemyTurn()
    {
        PlayingCard attackCard = enemyCards.Find(c => c.isUsed == false);
        enemyStrength += attackCard.strength;
        attackCard.isUsed = true;
        enemyActionDeck.Add(attackCard);
        enemyCards.Remove(attackCard);
        UpdateCardGameState(CardGameState.playerTurn);
    }

    private void PlayerTurn()
    {
        PlayingCard attackCard = ClickOnAttackCard();
        playerStrength += attackCard.strength;
        attackCard.isUsed = true;
        playerActionDeck.Add(attackCard);
        playerCards.Remove(attackCard);
        UpdateCardGameState(CardGameState.fight);
    }

    private PlayingCard ClickOnAttackCard()
    {
        throw new NotImplementedException();
    }

    private void CardFight()
    {
        foreach (PlayingCard p in enemyActionDeck)
        {
            playerStrength -= p.strength;
            if (playerStrength <= 0)
            {
                enemyActionDeck.Remove(p);
            }
            else
            {
                p.strength -= playerStrength;
            }
        }
        foreach (PlayingCard p in playerActionDeck)
        {
            enemyStrength -= p.strength;
            if (enemyStrength <= 0)
            {
                playerActionDeck.Remove(p);
            }
            else
            {
                p.strength -= enemyStrength;
            }
        }
        if(playerActionDeck.Count <= 0 || playerCards.Count <= 0)
        {
            EnemyWins();
        }
        if (enemyActionDeck.Count <= 0 || enemyCards.Count <= 0)
        {
            PlayerWins();
        }

        UpdateCardGameState(CardGameState.enemyTurn);
    }

    private void PlayerWins()
    {
        UpdateCardGameState(CardGameState.gameOver);
    }

    private void EnemyWins()
    {
        UpdateCardGameState(CardGameState.gameOver);
    }

    private void CardGameOver()
    {

    }

    private void UpdateCardGameState(CardGameState newState)
    {
        currentCardGameState = newState;
        switch (newState)
        {
            case CardGameState.start:
                CardGameStarts();
                break;
            case CardGameState.enemyTurn:
                EnemyTurn();
                break;
            case CardGameState.playerTurn:
                PlayerTurn();
                break;
            case CardGameState.fight:
                CardFight();
                break;
            case CardGameState.gameOver:
                CardGameOver();
                break;
            default:
                break;
        }

    }

}

public enum CardGameState{
    start,
    enemyTurn,
    playerTurn,
    fight,
    gameOver
}
