using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Board
{
    public string[] Boards { get; private set; }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public int BoardNumber { get; private set; }
    public ConsoleColor[] colors { get; private set; }
    public string close { get; } = " **  ";

    public Board(int cardTotalNum)
    {
        Boards = new string[cardTotalNum];
    }
    public void CloseBoard(int cardTotalNum)
    {
        BoardNumber = 0;
        for (int i = 0; i < cardTotalNum; i++)
        {
            Boards[i] = close;
        }
    }
    public void OpenCard(CardDeck card, CardSkinType skinType, int selectCardNum)
    {
        if (skinType == CardSkinType.Basic)
        {
            if (selectCardNum == card.NumberSkin.Length)
            {
                CardOpenAll(card.NumberSkin, card.NumberSkin.Length);
            }
            else
            {
                Boards[selectCardNum] = $"[{card.NumberSkin[selectCardNum],2}] ";
            }
        }
        else if (skinType == CardSkinType.Alphabet)
        {
            if (selectCardNum == card.NumberSkin.Length)
            {
                CardOpenAll(card.AlphabetSkin, card.AlphabetSkin.Length);
            }
            else
            {
                Boards[selectCardNum] = $"[{card.AlphabetSkin[selectCardNum],2}] ";
            }
        }
        else if (skinType == CardSkinType.Symbol)
        {
            if (selectCardNum == card.NumberSkin.Length)
            {
                CardOpenAll(card.SymbolSkin, card.SymbolSkin.Length);
            }
            else
            {
                Boards[selectCardNum] = $"[{card.SymbolSkin[selectCardNum],2}] ";
            }
        }
    }
    public void MatchingCard(CardDeck card, CardSkinType skinType, int selectCardNum1, int selectCardNum2)
    {
        if (skinType == CardSkinType.Basic)
        {
            Boards[selectCardNum1] = $" {card.NumberSkin[selectCardNum1],2}  ";
            Boards[selectCardNum2] = $" {card.NumberSkin[selectCardNum2],2}  ";
        }
        else if (skinType == CardSkinType.Alphabet)
        {
            Boards[selectCardNum1] = $" {card.AlphabetSkin[selectCardNum1],2}  ";
            Boards[selectCardNum2] = $" {card.AlphabetSkin[selectCardNum2],2}  ";
        }
        else if (skinType == CardSkinType.Symbol)
        {
            Boards[selectCardNum1] = $" {card.SymbolSkin[selectCardNum1],2}  ";
            Boards[selectCardNum2] = $" {card.SymbolSkin[selectCardNum2],2}  ";
        }
    }
    public void CardOpenAll(string[] skin, int selectCardNum)
    {
        for (int i = 0; i < skin.Length; i++)
        {
            Boards[i] = $" {skin[i],2}  ";
        }
    }
    public void CloseCard(int selectCardNum)
    {
        Boards[selectCardNum] = close;
    }

    public void ShowBoard(CardDeck deck)
    {
        CreateColor(deck);
        switch (Boards.Length)
        {
            case 8:
                Row = 2;
                Column = 4;
                break;
            case 16:
                Row = 4;
                Column = 4;
                break;
            case 24:
                Row = 4;
                Column = 6;
                break;
        }
        Console.Write("     ");
        for (int i = 1; i <= Column; i++)
        {
            Console.Write($"{i}열  ");
        }
        Console.WriteLine("\n");
        int k = 0;
        for (int i = 1; i <= Row; i++)
        {
            Console.Write($"{i}행 ");
            for (int j = 0; j < Column; j++)
            {
                if (Boards[k] == close)
                {
                    Console.Write(Boards[k++]);
                }
                else
                {
                    Console.ForegroundColor = colors[int.Parse(deck.NumberSkin[k]) - 1];
                    Console.Write(Boards[k++]);
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\n");
        }
    }
    public void CreateColor(CardDeck deck)
    {
        ConsoleColor[] allColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
        colors = new ConsoleColor[deck.NumberSkin.Length / 2];
        for (int i = 1; i <= colors.Length; i++) // 0은 검정색이라 제외
        {
            colors[i - 1] = allColors[i];
        }
    }

}
