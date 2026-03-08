using System;
using System.Collections.Generic;
using System.Text;

public class CardDeck
{
    public string[] NumberSkin { get; private set; }
    public string[] EnglishSkin {  get; private set; }
    public string[] SymbolSkin { get; private set;  }
    private int CardNumber = 1;
    Random rand = new Random();

    public void CreateNumberSkin(int num)
    {
        NumberSkin = new string[num];
        for(int i = 0; i < NumberSkin.Length; i++)
        {
            NumberSkin[i] = CardNumber++.ToString();
            if(CardNumber > NumberSkin.Length/2)
            {
                CardNumber = 1;
            }
        }
    }
    
    public void CardShuffle()
    {
        for(int i = 0; i < NumberSkin.Length; i++)
        {
            int randomIndex = rand.Next(NumberSkin.Length);
            string temp = NumberSkin[i];
            NumberSkin[i] = NumberSkin[randomIndex];
            NumberSkin[randomIndex] = temp;
        }
    }
    public void CreatEnglishSkin(int num)
    {
        EnglishSkin = new string[num];

        for (int i = 0; i < EnglishSkin.Length; i++)
        {
            int english = int.Parse(NumberSkin[i]) + 64;
            EnglishSkin[i] = ((char)english).ToString();
        }
    }
    public void CreateSymbolSkin(int num)
    {
        SymbolSkin = new string[num];
        int k = 0;
        while(k < num)
        {
            switch (int.Parse(NumberSkin[k]))
            {
                case 1:
                    SymbolSkin[k++] = "★";
                    break;
                case 2:
                    SymbolSkin[k++] = "■";
                    break;
                case 3:
                    SymbolSkin[k++] = "●";
                    break;
                case 4:
                    SymbolSkin[k++] = "▲";
                    break;
                case 5:
                    SymbolSkin[k++] = "♣";
                    break;
                case 6:
                    SymbolSkin[k++] = "♠";
                    break;
                case 7:
                    SymbolSkin[k++] = "♥";
                    break;
                case 8:
                    SymbolSkin[k++] = "◆";
                    break;
                case 9:
                    SymbolSkin[k++] = "◇";
                    break;
                case 10:
                    SymbolSkin[k++] = "○";
                    break;
                case 11:
                    SymbolSkin[k++] = "□";
                    break;
                case 12:
                    SymbolSkin[k++] = "△";
                    break;
            }
        }
        
    }
}
