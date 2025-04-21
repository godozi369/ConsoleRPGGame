using Game.Item;
using Game.Player;
using Game.NPC;

namespace Game.Scene
{
    public enum SceneType
    {
        MainMenu,
        Map,
        Inventory,
        Battle,
        NPC,
        PlayerStatus
    }

    public abstract class Scene
    {
        public abstract string Name { get; }

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

        public string GetCurrentSceneName()
        {
            return currentScene?.Name ?? "None";
        }
    }
    #endregion
    #region 게임씬
    public class GameScene : Scene
    {
        public override string Name => "Game";
        public override void Load(SceneManager manager) => Console.WriteLine("게임씬 로드");
        public override void Unload() => Console.WriteLine("게임씬 언로드");
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

        public override string Name => "Character";

        public override void Load(SceneManager manager)
        {
            Console.WriteLine("      [캐릭터 정보창]");
            player.ShowStatus();
            Console.WriteLine("아무 키나 누르면 메인 메뉴로...");
            Console.ReadKey();
            Console.Clear();
            manager.ChangeScene(SceneType.MainMenu);
        }
        public override void Unload()
        {
            Console.WriteLine("[캐릭터 씬 종료]");
        }
    }

    #endregion
    #region 메인메뉴씬
    public class MainMenuScene : Scene
    {
        public override string Name => "MainMenu";
        public override void Load(SceneManager manager)
        {
            Console.WriteLine("===== 메인 메뉴 =====");
            Console.WriteLine("[1] 캐릭터 정보창");
            Console.WriteLine("[2] 인벤토리");          
            Console.WriteLine("[0] 종료");

            while (true)
            {
                Console.Write("선택: ");
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        manager.ChangeScene(SceneType.PlayerStatus);
                        return;
                    case ConsoleKey.D2:
                        Console.Clear();
                        manager.ChangeScene(SceneType.Inventory);
                        return;
                    case ConsoleKey.D3:
                        manager.ChangeScene(SceneType.NPC);
                        return;

                    case ConsoleKey.D0:
                        Console.WriteLine("게임 종료");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력");
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

        public override string Name => "Inventory";

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
            manager.ChangeScene(SceneType.MainMenu);
        }
        public override void Unload() { }
    }
    #endregion
}
