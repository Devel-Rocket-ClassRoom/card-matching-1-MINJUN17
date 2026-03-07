using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class GameManager
{
    CardDeck deck = new CardDeck();
    Board board;
    //private string[] board;
    private bool IsMaching = false;
    private int _firstNum;
    private int _secondNum;
    private int cardTotalNum;
    private int difficultyNum;
    private int SkinNum = 0;
    private int num1_1 = 0;
    private int num1_2 = 0;
    private int num2_1 = 0;
    private int num2_2 = 0;
    private int tryCount = 0;
    private int machCount = 0;
    private int maxTry;
    private int cardNumber;
    private bool retry = true;

    public void Run()
    {
        while (retry)
        {
            Reset();
            ChoiceDifficulty();
            Console.Clear(); 
            //board = new string[cardTotalNum];
            deck.CreateDeck(cardTotalNum);
            deck.CardShuffle();
            //BoardSetting();
            CardSkinType skin = ChoiceSkin();
            Console.Clear();
            board = new Board(cardTotalNum);
            board.OpenAll(deck, skin);
            Preview(difficultyNum);
            board.CloseBoard(cardTotalNum);
            Console.Clear();
            while (true)
            {
                //BoardShow();
                board.ShowBoard();
                cardNumber = 1;
                SelectNumber();
                Console.Clear();
                //OpenFirstNumber(num1_1, num1_2);
                board.OpenCard(deck, _firstNum);
                board.ShowBoard();
                SelectNumber();
                board.OpenCard(deck, _secondNum);
                Console.Clear();
                //OpenSecondNumber(num1_1, num1_2, num2_1, num2_2);
                MachingBoardChange(num1_1, num1_2, num2_1, num2_2);
                if (!IsMaching)
                {
                    board.CloseCard(_firstNum);
                    board.CloseCard(_secondNum);
                }
                IsMaching = false;
                Thread.Sleep(2000);
                Console.Clear();
                if(tryCount >= maxTry)
                {
                    Console.WriteLine($"=== 시도 횟수 초과! ===\r\n총 시도 횟수: {tryCount}");
                    Console.Write("다시 하시겠습니까(y,n): ");
                    string input = Console.ReadLine();
                    if (input == "n")
                    {
                        retry = false;
                    }
                    break;
                }
                if (machCount == cardTotalNum / 2)
                {
                    Console.WriteLine($"=== 게임 클리어! ===\r\n총 시도 횟수: {tryCount}");
                    Console.Write("다시 하시겠습니까(y,n): ");
                    string input = Console.ReadLine();
                    if (input == "n")
                    {
                        retry = false;
                    }
                    break;
                }
            }
        }
        
    }
    public CardSkinType ChoiceSkin()
    {
        while (true)
        {
            Console.WriteLine("1. 기본 스킨");
            Console.WriteLine("2. 알파벳 스킨");
            Console.WriteLine("3. 기호 스킨");
            Console.Write("스킨을 선택하세요: ");
            bool input = int.TryParse(Console.ReadLine(), out SkinNum);
            if (!input)
            {
                Console.WriteLine("숫자를 입력해주세요!");
            }
            else if (SkinNum <= 0 || SkinNum > 3)
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else
            {
                switch (SkinNum)
                {
                    case 2:
                        deck.CreatEnglishSkin(cardTotalNum);
                        return CardSkinType.English;
                    case 3:
                        deck.CreateSymbolSkin(cardTotalNum); 
                        return CardSkinType.Symbol;
                    default:
                        return CardSkinType.Basic;
                }
            }
        }
    }
    public void Preview(int num)
    {
        Console.WriteLine("미리보기 !!");
        board.ShowBoard();
        if(num == 1)
        {
            Thread.Sleep(2000);
        }
        else if(num == 2)
        {
            Thread.Sleep(3000);
        }
        else
        {
            Thread.Sleep(5000);
        }
    }
    public void Reset()
    {
        tryCount = 0;
        machCount = 0;
    }
    public void ChoiceDifficulty()
    {
        maxTry = 40;
        cardTotalNum = 32;
        while (true)
        {
            Console.WriteLine("1. Hard(4X6): 30번 안에 찾으세요! 미리보기: 2초");
            Console.WriteLine("2. Normal(4X4): 20번 안에 찾으세요! 미리보기: 3초");
            Console.WriteLine("3. Easy(2X4): 10번 안에 찾으세요! 미리보기: 5초");
            Console.Write("난이도를 선택하세요: ");
            bool input = int.TryParse(Console.ReadLine(), out difficultyNum);
            if (!input)
            {
                Console.WriteLine("숫자를 입력해주세요!");
            }
            else if (difficultyNum <= 0 || difficultyNum > 3)
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else
            {
                maxTry -= difficultyNum * 10;
                cardTotalNum -= (difficultyNum) * 8;
                Console.WriteLine($"{maxTry}번 안에 찾으세요!");
                Console.Clear();
                break;
            }
        }
    }
    public void MachingBoardChange(int n1, int n2, int n3, int n4)
    {
        int num1 = ((n1 - 1) * 4) + n2 - 1;
        int num2 = ((n3 - 1) * 4) + n4 - 1;
        board.ShowBoard();
        if (deck.NumberSkin[num1] == deck.NumberSkin[num2])
        {
            IsMaching = true;
            machCount++;
            Console.WriteLine("\n짝을 찾았습니다!");
        }
        else
        {
            Console.WriteLine("\n짝이 맞지 않습니다!");
        }
    }
    

    public void SelectNumber()
    {
        bool input1 = false;
        bool input2 = false;
        while (true)
        {
            Console.WriteLine($"\n시도 횟수: {tryCount}/{maxTry} | 찾은 쌍: {machCount}/{board.Boards.Length / 2}");
            Console.Write($"\n{cardNumber} 번째 카드를 선택하세요 (행 열):");
            string input = Console.ReadLine();
            string[] number = input.Split(' ');
            if (cardNumber == 1)
            {
                input1 = int.TryParse(number[0], out num1_1);
                input2 = int.TryParse(number[1], out num1_2);
            }
            else
            {
                input1 = int.TryParse(number[0], out num2_1);
                input2 = int.TryParse(number[1], out num2_2);
                if (num2_1 == num1_1 && num2_2 == num1_2)
                {
                    Console.Clear();
                    board.ShowBoard();
                    Console.WriteLine("새로운 숫자를 골라주세요!");
                    continue;
                }
                tryCount++;
            }
            //int num = ((num1_1 - 1) * 4) + num1_2 - 1;
            if (!input1 || !input2)
            {
                Console.WriteLine("숫자를 입력해주세요!");
                continue;
            }
            else if (num1_1 > 8 || num1_2 > 8 || num1_1 <= 0 || num1_2 <= 0)
            {
                Console.WriteLine("1올바른 숫자가 아닙니다!");
                continue;
            }
            //else if (board.Boards[num] != " **  ")
            //{
            //    Console.WriteLine("2올바른 숫자가 아닙니다!");
            //}
            else
            {
                _firstNum = ((num1_1 - 1) * 4) + num1_2 - 1;
                _secondNum = ((num2_1 - 1) * 4) + num2_2 - 1;
                cardNumber++;
                break;
            }
        }
    }
    
    //public void BoardSetting()
    //{
    //    for(int i = 0; i < board.Boards.Length; i++)
    //    {
    //        board.Boards[i] = " **";
    //    }
    //}
    //public void BoardShow()
    //{
    //    Console.WriteLine("    1열2열3열4열");
    //    int k = 0;
    //    for (int i = 0; i < cardTotalNum / 4; i++)
    //    {
    //        Console.Write($"{i + 1}행");
    //        for (int j = 0; j < 4; j++)
    //        {
    //            Console.Write(board.Boards[k]);
    //            k++;
    //        }
    //        Console.WriteLine();
    //    }
    //}
    //public void OpenFirstNumber(int n1, int n2)
    //{
    //    int openNumber = ((n1 - 1) * 4) + n2 - 1;
    //    int machOpenNum = 0;
    //    Console.WriteLine("    1열2열3열4열");
    //    for (int i = 0; i < cardTotalNum / 4; i++)
    //    {
    //        Console.Write($"{i + 1}행");
    //        for (int j = 0; j < 4; j++)
    //        {
    //            if (openNumber == machOpenNum)
    //            {
    //                if(SkinNum == 1)
    //                {
    //                    Console.Write($"[{deck.NumberSkin[machOpenNum]}]");
    //                }
    //                else if(SkinNum == 2)
    //                {
    //                    Console.Write($"[{deck.EnglishSkin[machOpenNum]}]");
    //                }
    //                else if(SkinNum == 3)
    //                {
    //                    Console.Write($"[{deck.SymbolSkin[machOpenNum]}]");
    //                }
    //            }
    //            else
    //            {
    //                Console.Write($"{board.Boards[machOpenNum]}");
    //            }
    //            machOpenNum++;
    //        }
    //        Console.WriteLine();
    //    }
    //}
   
    //public void OpenSecondNumber(int n1, int n2, int n3, int n4)
    //{
    //    int openNumber1 = ((n1 - 1) * 4) + n2 - 1;
    //    int openNumber2 = ((n3 - 1) * 4) + n4 - 1;
    //    int machOpenNum = 0;
    //    Console.WriteLine("    1열2열3열4열");
    //    for (int i = 0; i < cardTotalNum / 4; i++)
    //    {
    //        Console.Write($"{i + 1}행");
    //        for (int j = 0; j < 4; j++)
    //        {
    //            if (openNumber1 == machOpenNum || openNumber2 == machOpenNum)
    //            {
    //                if (SkinNum == 1)
    //                {
    //                    Console.Write($"[{deck.NumberSkin[machOpenNum]}]");
    //                }
    //                else if (SkinNum == 2)
    //                {
    //                    Console.Write($"[{deck.EnglishSkin[machOpenNum]}]");
    //                }
    //                else if (SkinNum == 3)
    //                {
    //                    Console.Write($"[{deck.SymbolSkin[machOpenNum]}]");
    //                }
    //            }
    //            else
    //            {
    //                Console.Write($"{board.Boards[machOpenNum]}");
    //            }
    //            machOpenNum++;
    //        }
    //        Console.WriteLine();
    //    }
    //}
}
