
using Game.Inventory;
using Game.Map;
using Game.NPC;
using Game.Player;
using Game.Item;
using Game.Util;
using static Game.Player.CPlayer;
using System.Numerics;
using Game.Collect;
using System.Runtime.CompilerServices;
using Game.Scene;
using Game.Audio;
using System.Runtime.ConstrainedExecution;

namespace Game.GameManager
{  
    public class CGameManager
    {
        Random rand = new Random();
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
            
            // 스테이지 1 
            CMap map1_1 = new CMap();
            map1_1.AddNPC(new NPC1(10, 5));
            map1_1.Initialize(12);
            map1_1.SetPortal(10, 10);
            map1_1.SetName("무인도 1");
            _stages.Add(map1_1);

            // 스테이지 2
            CMap map1_2 = new CMap();
            map1_2.AddNPC(new NPC2(5, 8));
            map1_2.Initialize(12);           
            map1_2.SetPortal(1, 1);
            map1_2.SetPortal(10, 10);
            map1_2.SetName("무인도 2"); 
            _stages.Add(map1_2);

            // 스테이지 3 
            CMap map1_3 = new CMap();
            map1_3.AddNPC(new NPC3(8, 8));
            map1_3.Initialize(12);
            map1_3.SetPortal(1, 1);
            map1_3.SetPortal(10, 10);
            map1_3.SetName("무인도 3"); 
            _stages.Add(map1_3);
        }
        public void RenderMap()
        {
            // 맵 이름 출력 
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(42, 0);
            Console.WriteLine($"[{_stages[_currentStage].Name}]");
            Console.ResetColor();

            _stages[_currentStage].Render(_playerX, _playerY);
        }


        public void playerAction(ConsoleKey key, SceneManager manager)
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

            // 행동 작용 메서드
            TryFieldInteraction(_player, manager);

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

        private bool isFishing = false;
        public void TryFishing(CPlayer player, SceneManager manager)
        {
            if (isFishing) return;         
            isFishing = true;
            try
            {

                Console.SetCursorPosition(0, 15);
                if (player.EquipTool == null || !player.EquipTool.name.Contains("낚시대"))
                {
                    Console.WriteLine("[낚시 실패] 낚시대를 장착해야 낚시할 수 있습니다!");
                    Thread.Sleep(1000);
                    Helper.ClearFromLine(15);
                    return;
                }

                BgmPlayer bgm = new BgmPlayer();
                bgm.Play("MUSIC/fish.mp3");

                var scene = new FishingScene();
                scene.Load(manager);

                Console.SetCursorPosition(0, 15);
                Console.Write("낚시를 시도합니다...");
                int delay = GetDelayByToolName(player.EquipTool.name);

                for (int i = 0; i < delay / 300; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(300);
                }
                Thread.Sleep(delay);
                Console.WriteLine("\n오! 반응이 온다!");
                Thread.Sleep(1000);

                
                if (rand.Next(0, 100) < 50)
                {
                    CItem fish = null;
                    int exp = 0;

                    switch (_currentStage)
                    {
                        case 0:
                            fish = new Fragment("물고기", "못생긴 붕어", "체력 회복", 10, 10, 1);
                            exp = 5;
                            break;
                        case 1:
                            fish = new Fragment("물고기", "썩은 숭어", "체력 회복", 20, 20, 1);
                            exp = 10;
                            break;
                        case 2:
                            fish = new Fragment("물고기", "냄새나는 메기", "체력회복", 30, 30, 1);
                            exp = 15;
                            break;
                        default:
                            fish = new Fragment("물고기", "통조림 통", "썩은내가 난다", 0, 0, 1);
                            exp = 1;
                            break;
                    }

                    player.Inventory.AddItem(fish);
                    Console.WriteLine($"[낚시 성공!] {fish.name}을 잡았습니다!!!");

                    player.GetExp(exp);
                    Console.WriteLine($"[경험치] {exp} EXP를 획득했습니다!");
                }
                else
                {
                    Console.WriteLine("아무것도 잡히지 않았습니다 ㅠ"); 
                }
                Console.WriteLine($"스트레스로 체력 10을 잃었습니다");
                player.Hp -= 10;
                if (player.Hp <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("당신은 체력이 다해 쓰러졌습니다...");
                    Console.WriteLine("게임 오버");
                    Environment.Exit(0);
                }
                Thread.Sleep(1500);
                Console.Clear();
                bgm.Stop();
            
            }
            finally
            {
                isFishing = false;                           
            }
        }
        public void SelectMode(CPlayer player)
        {
            Helper.ClearFromLine(15);
            Console.SetCursorPosition(0, 15);
            Console.Write("[모드를 선택하세요]");
            Console.Write("\n1. 벌목");
            Console.Write("\n2. 채집");
            Console.Write("\n3. 채광");
            Console.Write("\n4. 낚시");
            Console.WriteLine("\n0. 해제");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1": player.CurrentMode = ActivityMode.벌목모드; break;
                case "2": player.CurrentMode = ActivityMode.채집모드; break;
                case "3": player.CurrentMode = ActivityMode.채광모드; break;
                case "4": player.CurrentMode = ActivityMode.낚시모드; break;
                case "0": player.CurrentMode = ActivityMode.백수모드; break;
                default: return;
                
            }
            Console.WriteLine("계속하려면 아무 키나 눌러,,,");
            Console.ReadKey();
            Thread.Sleep(300);           
            Helper.ClearFromLine(15);
        }
        public int GetDelayByToolName(string toolName)
        {
            return toolName switch
            {
                "허름한 낚시대" => rand.Next(2000, 3000),
                "평범한 낚시대" => rand.Next(1500, 2500),
                "고급 낚시대" => rand.Next(1200, 1800),
                "금간 도끼" => rand.Next(2000, 3000),
                "평범한 도끼" => rand.Next(1500, 2500),
                "고급 도끼" => rand.Next(1200, 1800),
                "녹슨 곡괭이" => rand.Next(2000, 3000),
                "평범한 곡괭이" => rand.Next(1500, 2500),
                "고급 곡괭이" => rand.Next(1200, 1800),
                "낡은 가위" => rand.Next(2000, 3000),
                "평범한 가위" => rand.Next(1500, 2500),
                "고급 가위" => rand.Next(1200, 1800),
                _ => rand.Next(1200, 1800)
            };
        }   
        public void TryFieldInteraction(CPlayer player, SceneManager manager)
        {           


            if (player.CurrentMode == ActivityMode.백수모드)
                return;           
          
            if (rand.Next(100) < 15) 
            {
                List<CCollect> mix = null;

                switch (player.CurrentMode)
                {   
                    case ActivityMode.벌목모드:
                        mix = GetTreesByStage(_currentStage);
                        break;
                    case ActivityMode.채집모드:
                        mix = GetHerbsByStage(_currentStage);
                        break;
                    case ActivityMode.채광모드:
                        mix = GetOresByStage(_currentStage);
                        break;
                }
                if (mix != null && mix.Count > 0)
                {
                    var selected = mix[rand.Next(mix.Count)];

                    Console.SetCursorPosition(0, 16);
                    Console.WriteLine($"[{selected.Name}] 발견! {selected.Description}");
                    Console.Write("채집하시겠습니까? (Y/N): ");

                    string input = Console.ReadLine()?.Trim().ToUpper();
                    if (input == "Y")
                    {
                        // 도구 확인
                        if (!IsProperToolEquipped(player))
                        {
                            Console.WriteLine($"[{selected.Name}]를(을) 채집하려면 적절한 도구를 장착하세요");
                            Thread.Sleep(1000);
                            return;
                        }
                        BgmPlayer bgm = new BgmPlayer();

                        // 씬 설정
                        if (player.CurrentMode == ActivityMode.벌목모드)
                        {
                            bgm.Play("MUSIC/logging.mp3");
                            new LoggingScene().Load(null);
                        }
                        else if (player.CurrentMode == ActivityMode.채집모드)
                        {
                            bgm.Play("MUSIC/scissor.mp3");
                            new CollectingScene().Load(null);
                        }
                        else if (player.CurrentMode == ActivityMode.채광모드)
                        {
                            bgm.Play("MUSIC/mining.mp3");
                            new MiningScene().Load(null);
                        }

                            // 도구별 딜레이설정
                            int delay = GetDelayByToolName(player.EquipTool.name);
                        Console.WriteLine($"\n[{selected.Name}] 채집 중(체력 - 10)");
                        for (int i = 0; i < delay / 300; i++)
                        {
                            Console.Write(".");
                            Thread.Sleep(300);
                        }
                        Thread.Sleep(delay);
                        bgm.Stop();

                        player.Hp -= 10;

                    if (player.Hp <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine("당신은 체력이 다해 쓰러졌습니다...");
                        Console.WriteLine("게임 오버");
                        Environment.Exit(0);  
                    }

                        selected.Collect(player);

                        player.GetExp(selected.Exp);
                        Console.WriteLine($"[경험치] {selected.Exp} EXP를 획득했습니다!");
                        Thread.Sleep(1000);
                    }
                    else if (input == "N")
                    {
                        Console.WriteLine("\n 아오 힘들어 안할래,,,");
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Console.WriteLine("난 뭘하고 싶은거지,,,");
                    }

                    Console.Clear();
                }
            }
        }
        private bool IsProperToolEquipped(CPlayer player)
        {
            if (player.EquipTool == null)
                return false;

            switch (player.CurrentMode)
            {
                case ActivityMode.벌목모드:
                    return player.EquipTool.name.Contains("도끼"); 
                case ActivityMode.채광모드:
                    return player.EquipTool.name.Contains("곡괭이");
                case ActivityMode.낚시모드:
                    return player.EquipTool.name.Contains("낚시대");
                case ActivityMode.채집모드:
                    return player.EquipTool.name.Contains("가위"); 
                default:
                    return false;
            }
        }
        private List<CCollect> GetTreesByStage(int stage)
        {
            switch (stage)
            {
                case 0: return new List<CCollect> { new NormalTree(), new NormalTree(), new NormalTree() };
                case 1: return new List<CCollect> { new AppleTree(), new NormalTree(), new NormalTree() };
                case 2: return new List<CCollect> { new ShineTree(), new AppleTree(), new NormalTree() };
                default: return new List<CCollect>();
            }
        }
        private List<CCollect> GetHerbsByStage(int stage)
        {
            switch (stage)
            {
                case 0: return new List<CCollect> { new HealingHerb(), new PoisonHerb(), new PoisonHerb() };
                case 1: return new List<CCollect> { new PoisonHerb(), new PoisonHerb(), new PoisonHerb() };
                case 2: return new List<CCollect> { new HealingHerb(), new MagicHerb(), new PoisonHerb() };
                default: return new List<CCollect>();
            }
        }
        private List<CCollect> GetOresByStage(int stage)
        {
            switch (stage)
            {
                case 0: return new List<CCollect> { new StoneOre(), new StoneOre(), new StoneOre() };
                case 1: return new List<CCollect> { new IronOre(), new StoneOre(), new StoneOre() };
                case 2: return new List<CCollect> { new GoldOre(), new IronOre() , new StoneOre() };
                default: return new List<CCollect>();
            }
        }
    }
}
