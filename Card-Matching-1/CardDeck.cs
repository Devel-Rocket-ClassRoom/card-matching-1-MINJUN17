using System;
using System.Collections.Generic;
using System.Text;

public class CardDeck
{
    public int[] Card { get; set; }
    private int CardNumber = 1;
    Random rand = new Random();
    public void CreateDeck(int num)
    {
        Card = new int[num];
        for(int i = 0; i < Card.Length; i++)
        {
            Card[i] = CardNumber++;
            if(CardNumber > Card.Length/2)
            {
                CardNumber = 1;
            }
        }
    }
    public void CardShuffle()
    {
        for(int i = 0; i < Card.Length; i++)
        {
            int randomIndex = rand.Next(Card.Length);
            int temp = Card[i];
            Card[i] = Card[randomIndex];
            Card[randomIndex] = temp;
        }
    }
}
