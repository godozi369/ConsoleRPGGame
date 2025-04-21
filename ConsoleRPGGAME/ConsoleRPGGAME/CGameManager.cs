using Game.Map;

namespace Game.GameManager
{
    public class CGameManager
    {
        List<CMap> _stages = new List<CMap>();
        int _currentStage = 0;
        int _playerX = 1, _playerY = 1;

        public void Initialize()
        {
            // 스테이지 1 
            CMap map1_1 = new CMap();
            map1_1.Initialize(12);
            map1_1.SetPortal(10, 10);
            _stages.Add(map1_1);

            // 스테이지 2
            CMap map1_2 = new CMap();
            map1_2.Initialize(12);
            map1_2.SetPortal(10, 10);
            _stages.Add(map1_2);

            // 스테이지 3 
            CMap map1_3 = new CMap();
            map1_3.Initialize(12);
            map1_3.SetPortal(10, 10);
            _stages.Add(map1_3);
        }
        public void RenderMap()
        {
            _stages[_currentStage].Render(_playerX, _playerY);
        }


        public void playerMove(ConsoleKey key)
        {
            ConsoleKeyInfo cki;
            while (true)
            {              
                CMap currentMap = _stages[_currentStage];
                currentMap.Render(_playerX, _playerY);

                cki = Console.ReadKey(true);

                int nextX = _playerX, nextY = _playerY;
                switch (cki.Key)
                {
                    case ConsoleKey.LeftArrow: nextX--; break;
                    case ConsoleKey.RightArrow: nextX++; break;
                    case ConsoleKey.UpArrow: nextY--; break;
                    case ConsoleKey.DownArrow: nextY++; break;
                    case ConsoleKey.Q: return;
                }

                // 벽 충돌 방지
                if (currentMap._tile[nextY, nextX] == CMap.TileType.Wall)
                    continue;

                _playerX = nextX;
                _playerY = nextY;

                // 포탈 타일 위에 잇으면 맵 이동 
                if (currentMap._tile[_playerY, _playerX] == CMap.TileType.portal)
                {

                    _currentStage = (_currentStage + 1) % _stages.Count;
                    _playerX = 1;
                    _playerY = 1;
                }
            }
        }

    }
}
