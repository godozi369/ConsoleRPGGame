using Game.Item;
using Game.Player;
using Game.NPC;
using Game.GameManager;
using Game.Map;

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
    public static class ActionMenu
    {
        public static void ShowActions(SceneManager manager)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("======행동======");
            Console.WriteLine("[Q] 캐릭터 정보");
            Console.WriteLine("[W] 인벤토리");
            Console.WriteLine("[Back Space] 종료");

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Q: manager.ChangeScene(SceneType.PlayerStatus); break;
                    case ConsoleKey.W: manager.ChangeScene(SceneType.Inventory); break;
                    case ConsoleKey.Backspace: 
                        Console.WriteLine("게임 종료"); 
                        Environment.Exit(0);
                        break;
                }
            }
        }
        
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

                // 행동 UI 
                Console.SetCursorPosition(0, 0);
                ActionMenu.ShowActions(manager);

                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.DownArrow:
                        gameManager.playerMove(key.Key);
                        break;
                }
            }
        }
        public override void Unload() { }
    }
    #endregion
    #region 캐릭터씬
    public class CharacterScene : Scene
    {
        private CPlayer player;
        public CharacterScene(CPlayer player)
        {
            this.player = player;
        }     

        public override void Load(SceneManager manager)
        {
            Console.WriteLine("      [캐릭터 정보창]");
            player.ShowStatus();
            Console.WriteLine("아무 키나 누르면 메인 메뉴로...");
            Console.ReadKey();
            Console.Clear();
            manager.ChangeScene(SceneType.Action);
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
            Console.WriteLine("      [인벤토리]");
            Console.WriteLine();
            player.Inventory.ShowInventory();

            Console.WriteLine("아이템 번호 입력시 장착 / 아무 키나 누르면 메인 메뉴로..");
            if (int.TryParse(Console.ReadLine(), out int num))
            {
                var item = player.Inventory.GetItemByIndex(num);
                if (item != null)
                {
                    player.EquipItem(item);
                }
                else
                {
                    Console.WriteLine("해당 번호의 아이템이 없습니다");
                }
            }
            Console.Clear() ;
            manager.ChangeScene(SceneType.Action);
        }
        public override void Unload() { }
    }
    #endregion
}
