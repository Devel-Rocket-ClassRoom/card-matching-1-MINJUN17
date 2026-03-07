using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class Board
{
    private CardDeck deck;
    private bool IsMaching;
    private bool IsSelect;
    public string[] Boards { get; private set; }
    public int Row { get; private set; }
    public int Column { get; private set; }
    public int BoardNumber { get; private set; }

    public Board(int cardTotalNum)
    {
        Boards = new string[cardTotalNum];
    }
    public void CloseBoard(int cardTotalNum)
    {
        BoardNumber = 0;
        for (int i = 0; i < cardTotalNum; i++)
        {
            Boards[i] = " **  ";
        }
    }
    //미리보기 용
    public void OpenAll(CardDeck card, CardSkinType skin)
    {
        if(skin == CardSkinType.Basic)
        {
            PrintOpenAll(card.NumberSkin);
        }
        else if(skin == CardSkinType.English)
        {
            PrintOpenAll(card.EnglishSkin);
        }
        else if (skin == CardSkinType.Symbol)
        {
            PrintOpenAll(card.SymbolSkin);
        }
    }
    public void PrintOpenAll(string[] skin)
    {
        for (int i = 0; i < skin.Length; i++)
        {
            Boards[i] = $" {skin[i],2}  ";
        }
    }
    public void OpenCard(CardDeck deck, int selectCardNum)
    {
        Boards[selectCardNum] = $"  {deck.NumberSkin[selectCardNum].ToString()}  ";
    }
    public void CloseCard(int selectCardNum)
    {
        Boards[selectCardNum] = " **  ";
    }

    public void ShowBoard()
    {
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
                Console.Write(Boards[k++]);
            }
            Console.WriteLine("\n");
        }
    }
}
