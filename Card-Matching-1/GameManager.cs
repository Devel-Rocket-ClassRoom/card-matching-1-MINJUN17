using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class GameManager
{
    CardDeck deck = new CardDeck();
    Board board;
    //private string[] board;
    private bool _isFirstsCard = true;
    private bool IsMaching = false;
    private int _firstNum;
    private int _secondNum;
    private int cardTotalNum;
    private int difficultyNum;
    private int SkinNum = 0;
    private int rowNum = 0;
    private int columnNum = 0;
    private int num2_1 = 0;
    private int num2_2 = 0;
    private int tryCount = 0;
    private int machCount = 0;
    private int maxTry;
    //private int cardNumber;
    private bool retry = true;

    public void Run()
    {
        while (retry)
        {
            Reset();
            ChoiceDifficulty();
            Console.Clear();
            //board = new string[cardTotalNum];
            deck.CreateNumberSkin(cardTotalNum);
            deck.CardShuffle();
            //BoardSetting();
            CardSkinType skin = ChoiceSkin();
            Console.Clear();
            board = new Board(cardTotalNum);
            board.OpenCard(deck, skin, cardTotalNum);
            Preview(difficultyNum);
            board.CloseBoard(cardTotalNum);
            Console.Clear();
            while (true)
            {
                //BoardShow();
                board.ShowBoard(deck);
                SelectNumber();
                Console.Clear();
                //OpenFirstNumber(num1_1, num1_2);
                board.OpenCard(deck, skin, _firstNum);
                board.ShowBoard(deck);
                SelectNumber();
                board.OpenCard(deck, skin, _secondNum);
                Console.Clear();
                //OpenSecondNumber(num1_1, num1_2, num2_1, num2_2);
                MachingBoardChange(_firstNum, _secondNum);
                if (!IsMaching)
                {
                    board.CloseCard(_firstNum);
                    board.CloseCard(_secondNum);
                }
                IsMaching = false;
                Thread.Sleep(2000);
                Console.Clear();
                if (tryCount >= maxTry)
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
        board.ShowBoard(deck);
        if (num == 1)
        {
            Thread.Sleep(2000);
        }
        else if (num == 2)
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
                Console.WriteLine("\n숫자를 입력해주세요!");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else if (difficultyNum <= 0 || difficultyNum > 3)
            {
                Console.WriteLine("\n올바른 숫자가 아닙니다! 다시 입력해주세요!");
                Thread.Sleep(1500);
                Console.Clear();
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
    public void MachingBoardChange(int firstNum, int secondNum)
    {
        board.ShowBoard(deck);
        if (deck.NumberSkin[firstNum] == deck.NumberSkin[secondNum])
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
            if (_isFirstsCard)
            {
                Console.Write($"\n첫 번째 카드를 선택하세요 (행 열):");
            }
            else
            {
                Console.Write($"\n두 번째 카드를 선택하세요 (행 열):");
            }
            string input = Console.ReadLine();
            string[] number = input.Split(' ');
            input1 = int.TryParse(number[0], out rowNum);
            input2 = int.TryParse(number[1], out columnNum);
            if (!input1 || !input2)
            {
                Console.Clear();
                board.ShowBoard(deck);
                Console.WriteLine("숫자를 입력해주세요!");
                continue;
            }
            else if (rowNum > board.Row || columnNum > board.Column || rowNum <= 0 || columnNum <= 0)
            {
                Console.Clear();
                board.ShowBoard(deck);
                Console.WriteLine("올바른 숫자가 아닙니다!");
                continue;
            }
            else
            {
                if (_isFirstsCard)
                {
                    _firstNum = ((rowNum - 1) * 4) + columnNum - 1;
                    if (board.Boards[_firstNum] != board.close)
                    {
                        Console.Clear();
                        board.ShowBoard(deck);
                        Console.WriteLine("새로운 숫자를 입력해주세요!");
                        continue;
                    }
                }
                else
                {
                    _secondNum = ((rowNum - 1) * 4) + columnNum - 1;
                    if (board.Boards[_secondNum] != board.close)
                    {
                        Console.Clear();
                        board.ShowBoard(deck);
                        Console.WriteLine("새로운 숫자를 입력해주세요!");
                        continue;
                    }
                }
                if (!_isFirstsCard) { tryCount++; }
                _isFirstsCard = !_isFirstsCard;
                break;
            }
        }
    }
}
