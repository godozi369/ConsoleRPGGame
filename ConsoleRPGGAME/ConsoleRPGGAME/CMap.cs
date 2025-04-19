

namespace Game.Map
{
    class CMap
    {
        public enum TileType
        {
            Empty, 
            Wall, 
            portal  
        }
        const char CIRCLE = '\u25cf'; // 유니코드 문자로 출력하기 위한 상수 
        public TileType[,] _tile; // 2차원 배열로 맵의 각 타일 저장
        public int _size; // 맵의 크기

        public void Initialize(int size)
        {
            _tile = new TileType[size, size]; // 맵의 크기 size x size 만큼 2차원 배열 생성
            _size = size; // 맵 크기 설정
            for (int y = 0; y < _size; y++)  
            {
                for (int x = 0; x < _size; x++) 
                {
                    // 맵의 테두리 벽으로 만들기
                    if (x == 0 || x == _size - 1 || y == 0 || y == _size - 1)
                    {
                        _tile[y, x] = TileType.Wall; // 벽타일 설정
                    }
                    else
                    {
                        _tile[y, x] = TileType.Empty; // 내부공간은 빈공간 설정
                    }
                }
            }
        }
        
        // 맵을 화면에 출력하는 메서드
        public void Render(int playerX, int playerY)
        {   
            ConsoleColor prevColor = Console.ForegroundColor; 
            for (int y = 0; y < _size; y++) 
            {
                Console.SetCursorPosition(_size*2, y);
                for (int x = 0; x < _size; x++) 
                {
                    if (x == playerX && y == playerY)
                        Console.ForegroundColor = ConsoleColor.Red; // 캐릭터 위치 색
                    else
                        Console.ForegroundColor = GetTileColor(_tile[y, x]); // 현재 타일의 색을 설정

                    Console.Write(CIRCLE); // 문자 출력 
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prevColor; // 이전 색상으로 복원
        }

        //타일의 종류에 따라 콘솔 글자색을 반환하는 메서드
        public ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Cyan;
                case TileType.Wall:
                    return ConsoleColor.DarkCyan;
                case TileType.portal:
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.Cyan;

            }
        }

        // 포탈 좌표 설정 
        public void SetPortal(int x, int y)
        {
            if (_tile[y, x] == TileType.Empty)
                _tile[y, x] = TileType.portal;
        }
    }
}
