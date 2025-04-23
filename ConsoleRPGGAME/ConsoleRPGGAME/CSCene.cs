using Game.Item;
using Game.Player;
using Game.NPC;
using Game.GameManager;
using Game.Map;
using static System.Net.Mime.MediaTypeNames;
using Game.Util;

namespace Game.Scene
{
    public enum SceneType
    {
        Action,
        Game,
        Inventory,
        Battle,
        NPC,
        PlayerStatus
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
    #region 게임씬
    public class GameScene : Scene
    {
        private CGameManager gameManager;
        private CPlayer player;

        public GameScene(CGameManager game, CPlayer player)
        {
            gameManager = game;
            this.player = player;
        }

        public override void Load(SceneManager manager)
        {
            ConsoleKeyInfo key;

            while (true)
            {
                               
                // 맵 
                gameManager.RenderMap();

                // 키 세팅 출력 
                Console.SetCursorPosition(85, 0);
                Console.WriteLine("========== [키 세팅] ==========");
                Console.SetCursorPosition(85, 1);
                Console.WriteLine("    ←↑↓→ : 이동");
                Console.SetCursorPosition(85, 2);
                Console.WriteLine("    W : 인벤토리");
                Console.SetCursorPosition(85, 3);
                Console.WriteLine("    Spacebar : 상호작용");
                Console.SetCursorPosition(85, 4);
                Console.WriteLine("    BackSpace : 종료");
                Console.SetCursorPosition(85, 5);
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
                player.ShowStatus();

                // 행동 UI 
                Console.SetCursorPosition(0, 0);              

                // 키 세팅

                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    // NPC 상호작용 키
                    case ConsoleKey.Spacebar:
                        var currentMap = gameManager.GetCurrentMap();
                        var npc = currentMap.GetNearByNPC(gameManager.PlayerX, gameManager.PlayerY);
                        if (npc != null)
                        {
                            npc.Interact(gameManager.Player);
                        }
                        else if (gameManager.IsNearRiver(currentMap, gameManager.PlayerX, gameManager.PlayerY))
                        {
                            gameManager.TryFishing(gameManager.Player);
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        gameManager.playerAction(key.Key);
                        break;
                    case ConsoleKey.W:
                        manager.ChangeScene(SceneType.Inventory);
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
        public InventoryScene(CPlayer player)
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
}
