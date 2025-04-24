using Game.Item;
using Game.Player;
using Game.NPC;
using Game.GameManager;
using Game.Map;
using static System.Net.Mime.MediaTypeNames;
using Game.Util;
using Game.Audio;
using static Game.Player.CPlayer;

namespace Game.Scene
{
    public enum SceneType
    {
        Intro,
        Game,
        Inventory,
    }
    public abstract class Scene
    {
        public abstract void Load(SceneManager manager);
        public abstract void Unload();
    }
    #region 씬매니저
    public class SceneManager
    {
        private Dictionary<SceneType, Scene> scenes = new();
        private Scene currentScene;

        public void Register(SceneType type, Scene scene)
        {
            scenes[type] = scene;
        }
        public void ChangeScene(SceneType type)
        {
            if (!scenes.ContainsKey(type))
            {
                Console.WriteLine($"[에러] {type}씬 없음");
                return;
            }

            currentScene?.Unload();
            currentScene = scenes[type];
            currentScene.Load(this);
        }
        public SceneType? Current => currentScene != null
            ? scenes.FirstOrDefault(x => x.Value == currentScene).Key
            : null;
    }
    #endregion
    #region 인트로 씬
    public class IntroScene : Scene
    {
        private CPlayer _player;
        private CGameManager _gameManager;
        public IntroScene(CPlayer player, CGameManager gameManager)
        {
            _player = player;
            _gameManager = gameManager;
        }
        
        public override void Load(SceneManager manager)
        {

            string[] logo = new string[]
            {
                "",
                "",
                "",
                "",
                "███╗   ███╗██╗   ██╗██╗███╗   ██╗██████╗  ██████╗  ",
                "████╗ ████║██║   ██║██║████╗  ██║██╔══██╗██╔═══██╗ ",
                "██╔████╔██║██║   ██║██║██╔██╗ ██║██║  ██║██║   ██║ ",
                "██║╚██╔╝██║██║   ██║██║██║╚██╗██║██║  ██║██║   ██║ ",
                "██║ ╚═╝ ██║╚██████╔╝██║██║ ╚████║██████╔╝╚██████╔╝ ",
                "╚═╝     ╚═╝ ╚═════╝ ╚═╝╚═╝  ╚═══╝╚═════╝  ╚═════╝  "
            };

            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var line in logo)
            {
                Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
                Console.WriteLine(line);
                Thread.Sleep(150);
            }

            Console.ResetColor();
            
            Console.WriteLine("\n\t\t\t\t\t아무 키나 눌러 생존을 시작하세요...");

            Console.ReadKey(true);                      
        }

        public override void Unload() { }
    }
    #endregion
    #region 게임씬
    public class GameScene : Scene
    {
        private CGameManager _gameManager;
        private CPlayer _player;

        public GameScene(CGameManager gameManager, CPlayer player)
        {
            _gameManager = gameManager;
            _player = player;
        }

        public override void Load(SceneManager manager)
        {
            ConsoleKeyInfo key;

            while (true)
            {                     
                // 맵 
                _gameManager.RenderMap();

                // 키 세팅 출력 
                Console.SetCursorPosition(85, 0);
                Console.WriteLine("========== [키 세팅] ==========");
                Console.SetCursorPosition(85, 1);
                Console.WriteLine("    ←↑↓→ : 이동");
                Console.SetCursorPosition(85, 2);
                Console.WriteLine("    Q : 활동모드 변경");
                Console.SetCursorPosition(85, 3);
                Console.WriteLine("    W : 인벤토리");
                Console.SetCursorPosition(85, 4);
                Console.WriteLine("    Spacebar : 상호작용");
                Console.SetCursorPosition(85, 5);
                Console.WriteLine("    BackSpace : 종료");
                Console.SetCursorPosition(85, 6);
                Console.WriteLine("===============================");

                // UI 정보
                Console.SetCursorPosition(65, 1);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write("●");
                Console.ResetColor();
                Console.WriteLine(" : 강");
                Console.SetCursorPosition(65, 3);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("●");
                Console.ResetColor();
                Console.WriteLine(" : 필드");
                Console.SetCursorPosition(65, 5);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("●");
                Console.ResetColor();
                Console.WriteLine(" : 플레이어");
                Console.SetCursorPosition(65, 7);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("●");
                Console.ResetColor();
                Console.WriteLine(" : NPC");
                Console.SetCursorPosition(65, 9);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("●");
                Console.ResetColor();
                Console.WriteLine(" : 포탈");

                // 캐릭터 상태 출력                
                _player.ShowStatus();

                // 행동 UI 
                Console.SetCursorPosition(0, 0);              

                // 키 세팅

                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    // NPC 상호작용 키
                    case ConsoleKey.Spacebar:
                        var currentMap = _gameManager.GetCurrentMap();
                        var npc = currentMap.GetNearByNPC(_gameManager.PlayerX, _gameManager.PlayerY);
                        if (npc != null)
                        {
                            npc.Interact(_gameManager.Player);
                        }
                        else if (_gameManager.IsNearRiver(currentMap, _gameManager.PlayerX, _gameManager.PlayerY) && _gameManager.Player.CurrentMode == ActivityMode.낚시모드)
                        {
                            _gameManager.TryFishing(_gameManager.Player);
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        _gameManager.playerAction(key.Key);
                        break;
                    case ConsoleKey.W:
                        manager.ChangeScene(SceneType.Inventory);
                        break;
                    case ConsoleKey.Q:
                        _gameManager.SelectMode(_gameManager.Player);
                        break;
                    case ConsoleKey.Backspace:
                        Console.WriteLine("게임 종료");
                        Environment.Exit(0);
                        break;
                }
            }
        }
        public override void Unload() { }
    }
    #endregion
    #region 인벤토리씬
    public class InventoryScene : Scene
    {
        private CPlayer player;
        public InventoryScene(CPlayer player, CGameManager gameManager)
        {
            this.player = player;
        }

        public override void Load(SceneManager manager)
        {            
            player.Inventory.ShowInventory();

            Console.WriteLine("아이템 번호 입력시 장착(사용) \n아무 키나 누르면 메인 메뉴로..");
            if (int.TryParse(Console.ReadLine(), out int num))
            {
                var item = player.Inventory.GetItemByIndex(num);
                if (item != null)
                {
                    if (item.category == ItemCategory.Potion)
                    {
                        player.Inventory.UseItem(num);
                        player.Hp += item.abil;
                        Console.WriteLine($"{player.Name}의 체력이 {item.abil}만큼 회복됐습니다. ");
                    }
                    else
                    {
                        player.EquipItem(item);
                    }
                }
                else
                {
                    Console.WriteLine("해당 번호의 아이템이 없습니다");
                }
            }
            Helper.ClearFromLine(15);
            Console.SetCursorPosition(0, 15);
        }
        public override void Unload() { }
    }
    #endregion
    #region 낚시 씬
    public class FishingScene : Scene
    {
        public override void Load(SceneManager manager)
        {
          
            Console.Clear();
            string[] scene = new string[]
            {
            "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
            "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
            "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
            "BBBBBBBBBBBBBBBBBBBBBB|BBBBBBBBBBBBBBBBBBBBBBB",  
            "BBBBBBBBBBBBBBBBBBBBBB|BBBBBBBBBBBBBBBBBBBBBBB",
            "BBBBBBBBBBBBBBBBBBBBBB|BBBBBBBBBBBBBBBBBBBBBBB",
            "BBBBBBBBBBBBBBBBBBBBBB|BBBBBBBBBBBBBBBBBBBBBBB",
            "BBBBBBBBBBBBBBBBBBBBBB|BBBBBBBBBBBBBBBBBBBBBBB",
            "BBBBBBBBBBBBBBBBBBBBBB|BBBBBBBBBBBBBBBBBBBBBBB",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",  
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",  
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",  
            };

            foreach (var line in scene)
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'B': Console.ForegroundColor = ConsoleColor.DarkBlue; Console.Write('■'); break; // 물
                        case 'G': Console.ForegroundColor = ConsoleColor.Green; Console.Write('■'); break; // 땅
                        case '|': Console.ForegroundColor = ConsoleColor.DarkRed; Console.Write('|'); break; // 낚싯줄
                        default: Console.ResetColor(); Console.Write(' '); break;
                    }
                }
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("\n[낚시 중] 물고기를 기다리는 중...");
            Thread.Sleep(2000); // 연출 시간
        }
        public override void Unload() { }
    }
    #endregion
    #region 채광 씬
    public class MiningScene : Scene
    {
        public override void Load(SceneManager manager)
        {
           
            Console.Clear();
            string[] scene = new string[]
            {
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGG######GGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGG###@######GGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGG@@##@@###*#GGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGG##@@@########GGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGG@@@#######@@@@@@GGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG"
            };
            foreach (var line in scene)
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '*':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('★');
                            break;
                        case 'G':
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write('■');
                            break;
                        case '#':
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write('■');
                            break;
                        case '@':
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write('■');
                            break;
                        default:
                            Console.Write(' ');
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            Console.WriteLine("\n[채광 중] 암석을 쿵! 쾅! 캡니다...");
            Thread.Sleep(2000);
        }
        public override void Unload() { }
    }
    #endregion
    #region 벌목 씬
    public class LoggingScene : Scene
    {
        public override void Load(SceneManager manager)
        {
            
            Console.Clear();
            string[] scene = new string[]
            {
            "SSSSSSSSSSSSSSSSSSSSSSWWSSWWWWSWWWWSSSSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSSSWWWWWWWWWWWWWWWWWSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSSWWWWWWWWWWWWWWWWWSSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSWWWWWWWWWWWWWWWWWWWSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSWWWWWWWWWWWWWWWWWWWSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSWWWWWWWWWWWWWWWWWWWSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSSWWWWXXWWWWXXWWWWWSSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSSWWWWXXXWWXXWWWWWWSSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSSSSSSXXXXXXXXSSSSSSSSSSSSSSSSSSSSSSSSSS",
            "SSSSSSSSSSSSSSSSSSSSSSSSXXXXXXXXSSSSSSSSSSSSSSSSSSSSSSSSSS",
            "TTTTTTTTTTTTTTTTTTTTTTTTXXXXXXXXTTTTTTTTTTTTTTTTTTTTTTTTTT",
            "TTTTTTTTTTTTTTTTTTTTTTTTXXXXXXXXTTTTTTTTTTTTTTTTTTTTTTTTTT",
            "TTTTTTTTTTTTTTTTTTTTTTTTXXXXXXXXTTTTTTTTTTTTTTTTTTTTTTTTTT"
            };

            foreach (var line in scene)
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'S': Console.ForegroundColor = ConsoleColor.Blue; break;          
                        case 'T': Console.ForegroundColor = ConsoleColor.Green; break;
                        case 'X': Console.ForegroundColor = ConsoleColor.DarkRed; break;
                        case 'W': Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                        default: Console.ForegroundColor = ConsoleColor.Gray; break;      
                    }
                    Console.Write('■');
                }   
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("\n[벌목 중] 도끼질 쾅쾅! 나무를 패는 중...");
            Thread.Sleep(2000);
        }

        public override void Unload() { }
    }
    #endregion
    #region 채집 씬
    public class CollectingScene : Scene
    {
        public override void Load(SceneManager manager)
        {
           
            Console.Clear();
            string[] scene = new string[]
            {
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGG*GGG*GGGG*GGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGG**GG****G**GGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGG**********GGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGG********GGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG",
            };

            foreach (var line in scene)
            {
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case 'G':
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write('■');
                            break;
                        case '*':
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write('★');
                            break;
                        default:
                            Console.Write(' ');
                            break;
                    }
                }
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("\n[채집 중] 약초를 조심스럽게 채취합니다...");
            Thread.Sleep(2000);
        }

        public override void Unload() { }
    }
    #endregion
}
