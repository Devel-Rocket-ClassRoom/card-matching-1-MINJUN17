using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class GameManager
{
    CardDeck deck = new CardDeck();
    private Board _board;
    private bool _isFirstsCard;
    private bool IsMaching;
    private int _firstNum;
    private int _secondNum;
    private int _cardTotalNum;
    private int _difficultyNum;
    private int _skinNum;
    private int _rowNum;
    private int _columnNum;
    private int _tryCount;
    private int _machCount;
    private int _maxTime;
    private int _currentTime;
    private int _maxTry;
    private DateTime _startTime;
    private bool _retry = true;
    private bool _gameSet = false;
    private int _wrongAnswerCount;
    private string _inputNumber;
    private int _timeLine;

    public void Run()
    {
        while (_retry)
        {
            Reset();
            Mode mode = ChoiceMode();
            DifficultyLevel level = ChoiceDifficulty(mode);
            Console.Clear();
            deck.CreateNumberSkin(_cardTotalNum);
            deck.CardShuffle();
            CardSkinType _skin = ChoiceSkin();
            Console.Clear();
            _board = new Board(_cardTotalNum);
            _board.OpenCard(deck, _skin, _cardTotalNum);
            Preview(level);
            _board.CloseBoard(_cardTotalNum);
            Console.Clear();
            _startTime = DateTime.Now;
            while (!_gameSet)
            {
                _currentTime = (int)(DateTime.Now - _startTime).TotalSeconds;
                _board.ShowBoard(deck);
                PrintProgress(mode);
                SelectNumber(mode);
                Console.Clear();
                _board.OpenCard(deck, _skin, _firstNum);
                _board.ShowBoard(deck);
                PrintProgress(mode);
                SelectNumber(mode);
                _board.OpenCard(deck, _skin, _secondNum);
                Console.Clear();
                MachingBoardChange(_firstNum, _secondNum);
                if (!IsMaching)
                {
                    _wrongAnswerCount++;
                    _board.CloseCard(_firstNum);
                    _board.CloseCard(_secondNum);
                }
                IsMaching = false;
                Thread.Sleep(1000);
                Console.Clear();
                GameOverCheck(mode);
            }
        }
        Console.WriteLine("게임을 종료합니다.");
    }
    public Mode ChoiceMode()
    {
        Console.WriteLine("1. 클래식: 횟수 제한");
        Console.WriteLine("2. 타임어택: 시간 제한(횟수 무제한)");
        Console.WriteLine("3. 서바이벌: 연속 세번 틀릴시 게임 오버");
        Console.Write("\n모드를 선택하세요: ");
        bool input = int.TryParse(Console.ReadLine(), out _skinNum);
        while (true)
        {
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
                switch ((Mode)_skinNum)
                {
                    case Mode.TimeAttack:
                        Console.Clear();
                        return Mode.TimeAttack;
                    case Mode.Survivor:
                        Console.Clear();
                        return Mode.Survivor;
                    default:
                        Console.Clear();
                        return Mode.Classic;
                }
            }
        }
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
    public void Preview(DifficultyLevel level)
    {
        int seconds;
        if (level == DifficultyLevel.Hard)
        {
            seconds = 2;
            for (int i = seconds; i > 0; i--)
            {
                Console.WriteLine($"미리보기 시간: {i}초");
                _board.ShowBoard(deck);
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        else if (level == DifficultyLevel.Normal)
        {
            seconds = 3;
            for (int i = seconds; i > 0; i--)
            {
                Console.WriteLine($"미리보기 시간: {i}초");
                _board.ShowBoard(deck);
                Thread.Sleep(1000);
                Console.Clear();
            }
        }
        else if (level == DifficultyLevel.Easy)
        {
            seconds = 5;
            for (int i = seconds; i > 0; i--)
            {
                Console.WriteLine($"미리보기 시간: {i}초");
                _board.ShowBoard(deck);
                Thread.Sleep(1000);
                Console.Clear();
            }
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
        _currentTime = 0;
        _gameSet = false;
        _wrongAnswerCount = 0;
    }
    public DifficultyLevel ChoiceDifficulty(Mode mode)
    {
        while (true)
        {
            if (mode == Mode.Classic)
            {
                Console.WriteLine("1. Hard(4X6): 30번 안에 찾으세요! 미리보기: 2초");
                Console.WriteLine("2. Normal(4X4): 20번 안에 찾으세요! 미리보기: 3초");
                Console.WriteLine("3. Easy(2X4): 10번 안에 찾으세요! 미리보기: 5초");
            }
            else if(mode == Mode.TimeAttack)
            {
                Console.WriteLine("1. Hard(4X6): 120초 안에 찾으세요! 미리보기: 2초");
                Console.WriteLine("2. Normal(4X4): 90초 안에 찾으세요! 미리보기: 3초");
                Console.WriteLine("3. Easy(2X4): 60초 안에 찾으세요! 미리보기: 5초");
            }
            else if(mode == Mode.Survivor)
            {
                Console.WriteLine("1. Hard(4X6): 3번 연속 틀리면 게임 오버! 미리보기: 2초");
                Console.WriteLine("2. Normal(4X4): 3번 연속 틀리면 게임 오버! 미리보기: 3초");
                Console.WriteLine("3. Easy(2X4): 3번 연속 틀리면 게임 오버! 미리보기: 5초");
            }
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
                switch ((DifficultyLevel)_difficultyNum)
                {
                    case DifficultyLevel.Hard:
                        _maxTry = 30;
                        _maxTime = 120;
                        _cardTotalNum = 24;
                        return DifficultyLevel.Hard;
                    case DifficultyLevel.Normal:
                        _maxTry = 20;
                        _maxTime = 90;
                        _cardTotalNum = 16;
                        return DifficultyLevel.Normal;
                    case DifficultyLevel.Easy:
                        _maxTry = 10;
                        _maxTime = 60;
                        _cardTotalNum = 8;
                        return DifficultyLevel.Easy;
                    default:
                        _maxTry = 30;
                        _maxTime = 120;
                        _cardTotalNum = 24;
                        return DifficultyLevel.Hard;
                }
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
            _wrongAnswerCount = 0;
            Console.WriteLine("\n짝을 찾았습니다!");
        }
        else
        {
            Console.WriteLine("\n짝이 맞지 않습니다!");
        }
    }
    
    public void PrintProgress(Mode mode)
    {
        if(mode == Mode.Classic)
        {
            Console.WriteLine($"\n시도 횟수: {_tryCount}/{_maxTry} | 찾은 쌍: {_machCount}/{_board.Boards.Length / 2}");
        }
        else if(mode == Mode.TimeAttack)
        {
            _currentTime = (int)(DateTime.Now - _startTime).TotalSeconds;
            _timeLine = Console.CursorTop;   // 시간 출력 위치 기억
        }
        else if(mode == Mode.Survivor)
        {
            Console.WriteLine($"\n현재 연속 오답: {_wrongAnswerCount} | 찾은 쌍: {_machCount}/{_board.Boards.Length / 2}");
        }
    }
    public void SelectNumber(Mode mode)
    {
        bool input1 = false;
        bool input2 = false;
        while (true)
        {
            if (_isFirstsCard)
            {
                Console.Write($"\n첫 번째 카드를 선택하세요 (행 열):");
            }
            else
            {
                Console.Write($"\n두 번째 카드를 선택하세요 (행 열):");
            }
            string input = Input(mode);
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
    public void GameOverCheck(Mode mode)
    {
        switch (mode)
        {
            case Mode.Classic:
                if (_tryCount >= _maxTry || _machCount == _cardTotalNum / 2) Retry(mode);
                break;
            case Mode.TimeAttack:
                if (_currentTime > _maxTime || _machCount == _cardTotalNum / 2) Retry(mode);
                break;
            case Mode.Survivor:
                if (_wrongAnswerCount >= 3 || _machCount == _cardTotalNum / 2) Retry(mode);
                break;
        }
    }
    public string Input(Mode mode) // 타임어택 실시간 반영을 위해 Console.ReadLine() 대신 사용
    {
        _inputNumber = string.Empty;
        while (true)
        {
            if(_currentTime > _maxTime) // 시간 지나면 게임오버
            {
                GameOverCheck(mode);
                return string.Empty;
            }
            _currentTime = (int)(DateTime.Now - _startTime).TotalSeconds; // 실시간 시간 계산식
            if(mode == Mode.TimeAttack)
            {
                int cursorHorizon = Console.CursorLeft; 
                int cursorVertical = Console.CursorTop; // 밑줄에서 커서 위치가 이동해서 입력시 불편하기 때문에 커서위치 기억
                Console.SetCursorPosition(0, _timeLine);// 시간출력되는 위치로 이동
                Console.Write($"경과 시간: {_currentTime}/{_maxTime} | 찾은 쌍: {_machCount}/{_board.Boards.Length / 2}   "); // 시간 출력

                Console.SetCursorPosition(cursorHorizon, cursorVertical); // 기억한 커서로 이동
            }
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    return _inputNumber;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if(_inputNumber.Length > 0)
                    {
                        _inputNumber = _inputNumber.Substring(0, _inputNumber.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    _inputNumber += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }

            Thread.Sleep(10); // 안쓰면 cpu 과부화
        }
    }
    public void Retry(Mode mode)
    {
        Console.Clear();
        _gameSet = true;
        if (mode == Mode.Classic && _tryCount >= _maxTry)
        {
            Console.WriteLine($"=== 시도 횟수 초과! ===\r\n총 시도 횟수: {_tryCount}");
        }
        else if(mode == Mode.Classic && _tryCount < _maxTry)
        {
            Console.WriteLine($"=== 게임 클리어! ===\r\n총 시도 횟수: {_tryCount}");
        }
        else if(mode == Mode.TimeAttack && _currentTime > _maxTime)
        {
            Console.WriteLine($"=== 제한 시간 초과! ===\r\n찾은 쌍: {_machCount}/{_cardTotalNum / 2}");
        }
        else if(mode == Mode.TimeAttack && _currentTime <= _maxTime)
        {
            Console.WriteLine($"=== 게임 클리어! ===\r\n소요 시간: {_currentTime}/{_maxTime}");
        }
        else if(mode == Mode.Survivor && _wrongAnswerCount >= 3)
        {
            Console.WriteLine($"=== 3연속 오답 게임 오버! ===\r\n찾은 쌍: {_machCount}/{_cardTotalNum / 2}");
        }
        else if(mode == Mode.Survivor && _wrongAnswerCount < 3)
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
