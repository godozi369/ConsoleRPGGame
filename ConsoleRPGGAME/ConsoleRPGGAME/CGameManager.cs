using Game.Battle;
using Game.Inventory;
using Game.Map;
using Game.Monster;
using Game.NPC;
using Game.Player;
using Game.Item;
using Game.Util;

namespace Game.GameManager
{
    public class CGameManager
    {       
        List<CMap> _stages = new List<CMap>();
        int _currentStage = 0;
        int _playerX = 1, _playerY = 1;
        private CPlayer _player;
        public int PlayerX => _playerX;
        public int PlayerY => _playerY;
        public CPlayer Player => _player;
        public void SetPlayer(CPlayer player) => _player = player;      
        public CMap GetCurrentMap() => _stages[_currentStage];
        public void Initialize()
        {
            CShop shop = new CShop();

            // 스테이지 1 
            CMap map1_1 = new CMap();
            map1_1.Initialize(12);
            map1_1.SetPortal(10, 10);
            map1_1.SetName("울창한 숲 1");
            _stages.Add(map1_1);

            // 스테이지 2
            CMap map1_2 = new CMap();
            map1_2.Initialize(12);
            map1_1.AddNPC(new NPC1("늙은 상인", 10, 5, shop));
            map1_2.SetPortal(1, 1);
            map1_2.SetPortal(10, 10);
            map1_2.SetName("울창한 숲 2"); 
            _stages.Add(map1_2);

            // 스테이지 3 
            CMap map1_3 = new CMap();
            map1_3.Initialize(12);
            map1_3.SetPortal(1, 1);
            map1_3.SetPortal(10, 10);
            map1_3.SetName("울창한 숲 3"); 
            _stages.Add(map1_3);
        }
        public void RenderMap()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(42, 0);
            Console.WriteLine($"[{_stages[_currentStage].Name}]");
            Console.ResetColor();

            _stages[_currentStage].Render(_playerX, _playerY);
        }


        public void playerAction(ConsoleKey key)
        {                        
            CMap currentMap = _stages[_currentStage];
                        
            int nextX = _playerX, nextY = _playerY;

            switch (key)
            {
                case ConsoleKey.LeftArrow: nextX--; break;
                case ConsoleKey.RightArrow: nextX++; break;
                case ConsoleKey.UpArrow: nextY--; break;
                case ConsoleKey.DownArrow: nextY++; break;
                default: return;
            }

            // 벽 충돌 방지
            if (currentMap._tile[nextY, nextX] == CMap.TileType.River)
                return;
            // NPC 벽 설정
            if (currentMap.NPCList.Any(npc => npc.X == nextX && npc.Y == nextY))
                return;

            _playerX = nextX;
            _playerY = nextY;

            // 포탈 타일 위에 잇으면 맵 이동 
            if (currentMap._tile[_playerY, _playerX] == CMap.TileType.portal)
            {
                // 다음 포탈
                if (_playerX == currentMap._size - 2 && _playerY == currentMap._size - 2)
                {
                    _currentStage = (_currentStage + 1) % _stages.Count;
                }

                // 이전 포탈
                else if (_playerX == 1 && _playerY == 1)
                {
                    _currentStage = (_currentStage - 1 + _stages.Count) % _stages.Count;
                }

                _playerX = 1;
                _playerY = 1;
            }
            
            // 몬스터 랜덤 생성 
            Random rand = new Random();
            if (rand.Next(0, 100) < 0)
            {
                CMonster monster;
                if (_currentStage == 0)
                {
                    monster = MonsterFactory.CreateMonkey();
                }
                else if (_currentStage == 1)
                {
                    monster = MonsterFactory.CreateWolf();
                }
                else return;

                CBattle battle = new CBattle();
                battle.StartBattle(_player, monster); 
            }
        }

        // 강가 상호작용
        public bool IsNearRiver(CMap map, int x, int y)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && ny >= 0 && nx < map._size && ny < map._size)
                    {
                        if (map._tile[ny, nx] == CMap.TileType.River) 
                            return true;
                    }
                }
            }
            return false;
        }


        public void TryFishing(CPlayer player)
        {
            Console.SetCursorPosition(0, 15);
            Console.WriteLine("낚시를 시도합니다...");
            Thread.Sleep(1000);

            Random rand = new Random();
            if (rand.Next(0, 100) < 50)
            {
                CItem fish = null;

                switch (_currentStage)
                {
                    case 0:
                        fish = new Fragment("낚시", "못생긴 붕어", "체력 회복", 10, 10, 1);
                        break;
                    case 1:
                        fish = new Fragment("낚시", "썩은 숭어", "체력 회복", 20, 20, 1);
                        break;
                    case 2:
                        fish = new Fragment("낚시", "냄새나는 메기", "체력회복", 30, 30, 1);
                        break;
                    default:
                        fish = new Fragment("낚시", "통조림 통", "썩은내가 난다", 0, 0, 1);
                        break;
                }

                player.Inventory.AddItem(fish);
                Console.WriteLine($"[낚시 성공!] {fish.name}을 잡았습니다!!!");
            }

            else
            {
                Console.WriteLine("아무것도 잡히지 않았습니다 ㅠ");
                Console.WriteLine($"스트레스로 체력 일부를 잃었습니다");
                player.Hp -
            }



            Thread.Sleep(1000);
            Helper.ClearFromLine(15);
        }
    }
}
