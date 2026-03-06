using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class GameManager
{
    CardDeck deck = new CardDeck();
    private string[] board;
    private int cardTotalNum;
    private int num1_1 = 0;
    private int num1_2 = 0;
    private int num2_1 = 0;
    private int num2_2 = 0;
    private int tryCount = 0;
    private int machCount = 0;
    private int maxTry;
    private bool retry = true;

    public void Run()
    {
        while (retry)
        {
            Reset();
            ChoiceDifficulty();
            board = new string[cardTotalNum];
            BoardSetting();
            deck.CreateDeck(cardTotalNum);
            deck.CardShuffle();
            while (true)
            {
                BoardShow();
                SelectFirstNumber();
                Console.Clear();
                OpenBoardShowFirst(num1_1, num1_2);
                SelectSecondNumber();
                Console.Clear();
                OpenBoardShowSecond(num1_1, num1_2, num2_1, num2_2);
                MachingBoardChange(num1_1, num1_2, num2_1, num2_2);
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
            Console.WriteLine("1. Hard: 30번 안에 찾으세요!");
            Console.WriteLine("2. Normal: 20번 안에 찾으세요!");
            Console.WriteLine("3. Easy: 10번 안에 찾으세요!");
            Console.Write("난이도를 선택하세요: ");
            int num;
            bool input = int.TryParse(Console.ReadLine(), out num);
            if (!input)
            {
                Console.WriteLine("숫자를 입력해주세요!");
            }
            else if (num <= 0 || num > 3)
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else
            {
                maxTry -= num * 10;
                cardTotalNum -= (num) * 8;
                Console.WriteLine($"{maxTry}번 안에 찾으세요!");
                Thread.Sleep(2000);
                Console.Clear();
                break;
            }
        }
    }
    public void MachingBoardChange(int n1, int n2, int n3, int n4)
    {
        int num1 = ((n1 - 1) * 4) + n2 - 1;
        int num2 = ((n3 - 1) * 4) + n4 - 1;
        if (deck.Card[num1] == deck.Card[num2])
        {
            board[num1] = $"  {deck.Card[num1].ToString()}";
            board[num2] = $"  {deck.Card[num2].ToString()}";
            machCount++;
            Console.WriteLine("\n짝을 찾았습니다!");
        }
        else
        {
            Console.WriteLine("\n짝이 맞지 않습니다!");
        }
    }
    public void SelectFirstNumber()
    {
        while (true)
        {
            Console.WriteLine($"\n시도 횟수: {tryCount}/{maxTry} | 찾은 쌍: {machCount}/{board.Length/2}");
            Console.Write("\n첫 번째 카드를 선택하세요 (행 열):");
            string input = Console.ReadLine();
            string[] number = input.Split(' ');
            bool input1 = int.TryParse(number[0], out num1_1);
            bool input2 = int.TryParse(number[1], out num1_2);
            int num = ((num1_1 - 1) * 4) + num1_2 - 1;
            if (!input1 || !input2)
            {
                Console.WriteLine("숫자를 입력해주세요!");
            }
            else if (num1_1 > 8 || num1_2 > 8 || num1_1 <= 0 || num1_2 <= 0)
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else if (board[num] != " **")
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else
            {
                break;
            }
        }


    }
    public void SelectSecondNumber()
    {
        while (true)
        {
            Console.WriteLine($"\n시도 횟수: {tryCount}/{maxTry} | 찾은 쌍: {machCount}/{board.Length / 2}");
            Console.Write("\n두 번째 카드를 선택하세요 (행 열):");
            string input = Console.ReadLine();
            string[] number = input.Split(' ');
            int num = ((num1_1 - 1) * 4) + num1_2 - 1;
            bool input1 = int.TryParse(number[0], out num2_1);
            bool input2 = int.TryParse(number[1], out num2_2);
            if (!input1 || !input2)
            {
                Console.WriteLine("숫자를 입력해주세요!");
            }
            else if (num2_1 > 8 || num2_2 > 8 || num2_1 <= 0 || num2_2 <= 0)
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else if(num2_1 == num1_1 && num2_2 == num1_2)
            {
                Console.WriteLine("새로운 숫자를 골라주세요!");
            }
            else if (board[num] != " **")
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else
            {
                tryCount++;
                break;
            }
        }

    }
    public void BoardSetting()
    {
        for(int i = 0; i < board.Length; i++)
        {
            board[i] = " **";
        }
    }
    public void BoardShow()
    {
        Console.WriteLine("    1열2열3열4열");
        int k = 0;
        for (int i = 0; i < cardTotalNum / 4; i++)
        {
            Console.Write($"{i + 1}행");
            for (int j = 0; j < 4; j++)
            {
                Console.Write(board[k]);
                k++;
            }
            Console.WriteLine();
        }
    }
    public void OpenBoardShowFirst(int n1, int n2)
    {
        int openNumber = ((n1 - 1) * 4) + n2 - 1;
        int machOpenNum = 0;
        Console.WriteLine("    1열2열3열4열");
        for (int i = 0; i < cardTotalNum / 4; i++)
        {
            Console.Write($"{i + 1}행");
            for (int j = 0; j < 4; j++)
            {
                if (openNumber == machOpenNum)
                {
                    Console.Write($"[{deck.Card[machOpenNum]}]");
                }
                else
                {
                    Console.Write($"{board[machOpenNum]}");
                }
                machOpenNum++;
            }
            Console.WriteLine();
        }
    }
   
    public void OpenBoardShowSecond(int n1, int n2, int n3, int n4)
    {
        int openNumber1 = ((n1 - 1) * 4) + n2 - 1;
        int openNumber2 = ((n3 - 1) * 4) + n4 - 1;
        int machOpenNum = 0;
        Console.WriteLine("    1열2열3열4열");
        for (int i = 0; i < cardTotalNum / 4; i++)
        {
            Console.Write($"{i + 1}행");
            for (int j = 0; j < 4; j++)
            {
                if (openNumber1 == machOpenNum || openNumber2 == machOpenNum)
                {
                    Console.Write($"[{deck.Card[machOpenNum]}]");
                }
                else
                {
                    Console.Write($"{board[machOpenNum]}");
                }
                machOpenNum++;
            }
            Console.WriteLine();
        }
    }
}
