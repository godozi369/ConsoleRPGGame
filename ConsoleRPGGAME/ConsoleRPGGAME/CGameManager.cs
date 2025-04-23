using Game.Battle;
using Game.Map;
using Game.Monster;
using Game.NPC;
using Game.Player;

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
            map1_1.AddNPC(new NPC1("늙은 상인", 10, 5, shop));
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
            if (currentMap._tile[nextY, nextX] == CMap.TileType.Wall)
                return;
            // NPC 벽 설정
            if (currentMap.NPCList.Any(npc => npc.X == nextX && npc.Y == nextY))
                return;

            _playerX = nextX;
            _playerY = nextY;

            // 포탈 타일 위에 잇으면 맵 이동 
            if (currentMap._tile[_playerY, _playerX] == CMap.TileType.portal)
            {

                _currentStage = (_currentStage + 1) % _stages.Count;
                _playerX = 1;
                _playerY = 1;
            }
            
            Random rand = new Random();
            if (rand.Next(0, 100) < 20)
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

    }
}
