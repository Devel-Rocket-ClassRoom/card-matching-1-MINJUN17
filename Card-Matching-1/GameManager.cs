using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class GameManager
{
    CardDeck deck = new CardDeck();
    private Board _board;
    private bool _isFirstsCard = true;
    private bool IsMaching = false;
    private int _firstNum;
    private int _secondNum;
    private int _cardTotalNum;
    private int _difficultyNum;
    private int _skinNum = 0;
    private int _rowNum = 0;
    private int _columnNum = 0;
    private int _tryCount = 0;
    private int _machCount = 0;
    private int _maxTry;
    private bool _retry = true;

    public void Run()
    {
        while (_retry)
        {
            Reset();
            ChoiceDifficulty();
            Console.Clear();
            deck.CreateNumberSkin(_cardTotalNum);
            deck.CardShuffle();
            CardSkinType _skin = ChoiceSkin();
            Console.Clear();
            _board = new Board(_cardTotalNum);
            _board.OpenCard(deck, _skin, _cardTotalNum);
            Preview(_difficultyNum);
            _board.CloseBoard(_cardTotalNum);
            Console.Clear();
            while (true)
            {
                _board.ShowBoard(deck);
                SelectNumber();
                Console.Clear();
                _board.OpenCard(deck, _skin, _firstNum);
                _board.ShowBoard(deck);
                SelectNumber();
                _board.OpenCard(deck, _skin, _secondNum);
                Console.Clear();
                MachingBoardChange(_firstNum, _secondNum);
                if (!IsMaching)
                {
                    _board.CloseCard(_firstNum);
                    _board.CloseCard(_secondNum);
                }
                IsMaching = false;
                Thread.Sleep(2000);
                Console.Clear();
                if (_tryCount >= _maxTry || _machCount == _cardTotalNum / 2)
                {
                    Retry();
                    break;
                }
            }
        }
        Console.WriteLine("게임을 종료합니다.");
    }
    public CardSkinType ChoiceSkin()
    {
        while (true)
        {
            Console.WriteLine("1. 숫자 스킨");
            Console.WriteLine("2. 알파벳 스킨");
            Console.WriteLine("3. 기호 스킨");
            Console.Write("\n스킨을 선택하세요: ");
            bool input = int.TryParse(Console.ReadLine(), out _skinNum);
            if (!input)
            {
                Console.WriteLine("숫자를 입력해주세요!");
            }
            else if (_skinNum <= 0 || _skinNum > 3)
            {
                Console.WriteLine("올바른 숫자가 아닙니다!");
            }
            else
            {
                switch ((CardSkinType)_skinNum)
                {
                    case CardSkinType.Alphabet:
                        deck.CreatAlphabetSkin(_cardTotalNum);
                        return CardSkinType.Alphabet;
                    case CardSkinType.Symbol:
                        deck.CreateSymbolSkin(_cardTotalNum);
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
        _board.ShowBoard(deck);
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
        _isFirstsCard = true;
        IsMaching = false;
        _skinNum = 0;
        _rowNum = 0;
        _columnNum = 0;
        _tryCount = 0;
        _machCount = 0;
        _retry = true;
    }
    public void ChoiceDifficulty()
    {
        _maxTry = 40;
        _cardTotalNum = 32;
        while (true)
        {
            Console.WriteLine("1. Hard(4X6): 30번 안에 찾으세요! 미리보기: 2초");
            Console.WriteLine("2. Normal(4X4): 20번 안에 찾으세요! 미리보기: 3초");
            Console.WriteLine("3. Easy(2X4): 10번 안에 찾으세요! 미리보기: 5초");
            Console.Write("\n난이도를 선택하세요: ");
            bool input = int.TryParse(Console.ReadLine(), out _difficultyNum);
            if (!input)
            {
                Console.WriteLine("\n숫자를 입력해주세요!");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else if (_difficultyNum <= 0 || _difficultyNum > 3)
            {
                Console.WriteLine("\n올바른 숫자가 아닙니다! 다시 입력해주세요!");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else
            {
                _maxTry -= _difficultyNum * 10;
                _cardTotalNum -= (_difficultyNum) * 8;
                Console.WriteLine($"{_maxTry}번 안에 찾으세요!");
                Console.Clear();
                break;
            }
        }
    }
    public void MachingBoardChange(int firstNum, int secondNum)
    {
        _board.ShowBoard(deck);
        if (deck.NumberSkin[firstNum] == deck.NumberSkin[secondNum])
        {
            IsMaching = true;
            _machCount++;
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
            Console.WriteLine($"\n시도 횟수: {_tryCount}/{_maxTry} | 찾은 쌍: {_machCount}/{_board.Boards.Length / 2}");
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
            if (number.Length < 2)
            {
                Console.Clear();
                _board.ShowBoard(deck);
                Console.WriteLine("숫자 두개를 띄어쓰기로 구분해서 입력해주세요!");
                continue;
            }
            input1 = int.TryParse(number[0], out _rowNum);
            input2 = int.TryParse(number[1], out _columnNum);
            if (!input1 || !input2)
            {
                Console.Clear();
                _board.ShowBoard(deck);
                Console.WriteLine("숫자를 입력해주세요!");
                continue;
            }
            else if (_rowNum > _board.Row || _columnNum > _board.Column || _rowNum <= 0 || _columnNum <= 0)
            {
                Console.Clear();
                _board.ShowBoard(deck);
                Console.WriteLine("올바른 숫자가 아닙니다!");
                continue;
            }
            else
            {
                if (_isFirstsCard)
                {
                    _firstNum = ((_rowNum - 1) * _board.Column) + _columnNum - 1;
                    if (_board.Boards[_firstNum] != _board.close)
                    {
                        Console.Clear();
                        _board.ShowBoard(deck);
                        Console.WriteLine("새로운 숫자를 입력해주세요!");
                        continue;
                    }
                }
                else
                {
                    _secondNum = ((_rowNum - 1) * _board.Column) + _columnNum - 1;
                    if (_board.Boards[_secondNum] != _board.close)
                    {
                        Console.Clear();
                        _board.ShowBoard(deck);
                        Console.WriteLine("새로운 숫자를 입력해주세요!");
                        continue;
                    }
                }
                if (!_isFirstsCard) { _tryCount++; }
                _isFirstsCard = !_isFirstsCard;
                break;
            }
        }
    }
    public void Retry()
    {
        if (_tryCount >= _maxTry)
        {
            Console.WriteLine($"=== 시도 횟수 초과! ===\r\n총 시도 횟수: {_tryCount}");
        }
        else
        {
            Console.WriteLine($"=== 게임 클리어! ===\r\n총 시도 횟수: {_tryCount}");
        }
        while (true)
        {
            Console.Write("\n다시 하시겠습니까(y,n): ");
            string input = Console.ReadLine().ToUpper();
            if (input != "Y" && input != "N")
            {
                Console.WriteLine("\n다시 입력해 주세요!!");
                continue;
            }
            if (input == "N")
            {
                _retry = false;
            }
            Console.Clear();
            break;
        }
    }
}
