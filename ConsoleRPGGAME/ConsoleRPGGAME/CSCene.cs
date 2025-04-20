using Game.Item;
using Game.Player;
using Game.NPC;

namespace Game.Scene
{
    public abstract class Scene
    {
        public abstract string Name { get; }

        public abstract void Load(SceneManager manager);
        public abstract void Unload();
    }
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
            Console.WriteLine("\n[캐릭터 정보창]");
            player.ShowStatus();
            Console.WriteLine("아무 키나 누르면 메인 메뉴로...");
            Console.ReadKey();
            manager.ChangeScene("main");
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
            Console.WriteLine("\n===== 메인 메뉴 =====");
            Console.WriteLine("1. 캐릭터 정보창");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("0. 종료");

            while (true)
            {
                Console.Write("선택: ");
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.D1:
                        manager.ChangeScene("Character");
                        return;
                    case ConsoleKey.D2:
                        manager.ChangeScene("Inventory");
                        return;
                    case ConsoleKey.D3:
                        manager.ChangeScene("Shop");
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
        public override void Unload() => Console.WriteLine("메인 메뉴 언로드");
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
            Console.WriteLine("\n[인벤토리]");
            player.Inventory.ShowInventory();

            Console.WriteLine("아이템 번호 입력시 장착 / 아무 키나 누르면 메인 메뉴로");
            if (int.TryParse(Console.ReadLine(), out int num))
            {
                var item = player.Inventory.GetItem(num);
                if (item != null)
                {
                    player.EquipItem = item;
                    Console.WriteLine($"{item.name} 장착 완료!");
                }
            }

            manager.ChangeScene("Main");
        }

        public override void Unload() { }
    }
    #endregion
    #region 씬매니저
    public class SceneManager
    {
        Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
        private Scene currentScene = null;

        public void AddScene(string key, Scene scene)
        {
            if (!scenes.ContainsKey(key))
            {
                scenes[key] = scene;
                Console.WriteLine($"[씬 추가됨: {key}]");
            }
        }
        public void RemoveScene(string key)
        {
            if (scenes.ContainsKey(key))
            {
                if (currentScene?.Name == scenes[key].Name)
                {
                    currentScene.Unload();
                    currentScene = null;
                }
                scenes.Remove(key);
                Console.WriteLine($"[씬 삭제됨: {key}]");
            }
        }

        public void ChangeScene(string key)
        {
            if (!scenes.ContainsKey(key))
            {
                Console.WriteLine($"[에러] {key} 씬 없음!");
                return;
            }

            currentScene?.Unload();

            currentScene = scenes[key];
            currentScene.Load(this);
            Console.WriteLine($"[현재 씬] {currentScene.Name}");
        }

        public string GetCurrentSceneName()
        {
            return currentScene?.Name ?? string.Empty;
        }

    }
    #endregion
}
