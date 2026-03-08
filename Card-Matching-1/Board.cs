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
    public ConsoleColor[] colors;
    public string close { get;} = " **  ";

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
    //미리보기 용
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
                Boards[selectCardNum] = $" {card.NumberSkin[selectCardNum],2}  ";
            }
        }
        else if (skinType == CardSkinType.English)
        {
            if (selectCardNum == card.NumberSkin.Length)
            {
                CardOpenAll(card.EnglishSkin, card.EnglishSkin.Length);
            }
            else
            {
                Boards[selectCardNum] = $" {card.EnglishSkin[selectCardNum],2}  ";
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
                Boards[selectCardNum] = $" {card.SymbolSkin[selectCardNum],2}  ";
            }
        }
    }
    public void CardOpenAll(string[] skin, int selectCardNum)
    {
        for (int i = 0; i < skin.Length; i++)
        {
            Boards[i] = $" {skin[i],2}  ";
        }
    }
    //public void OpenCard(CardDeck deck, int selectCardNum)
    //{
    //    Boards[selectCardNum] = $"  {deck.NumberSkin[selectCardNum],2}  ";
    //}
    public void CloseCard(int selectCardNum)
    {
        Boards[selectCardNum] = close;
    }

    public void ShowBoard(CardDeck deck)
    {
        CreateColor();
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
                    Console.ForegroundColor = colors[int.Parse(deck.NumberSkin[k])];
                    Console.Write(Boards[k++]);
                    Console.ResetColor();
                }
            }
            Console.WriteLine("\n");
        }
    }
    public void CreateColor()
    {
        ConsoleColor[] allColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
        colors = new ConsoleColor[12];
        for(int i = 1; i <= colors.Length; i++) // 0은 검정색이라 제외
        {
            colors[i-1] = allColors[i];
        }
    }
    
}
