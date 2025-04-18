using Game.Item;
using Game.Player;
using Game.Shop;

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
    #region 상점씬
    public class ShopScene : Scene
    {
        private CPlayer player;
        private CShop shop;

        public ShopScene(CPlayer player)
        {
            this.player = player;
            this.shop = new CShop();
        }

        public override string Name => "Shop";

        public override void Load(SceneManager manager)
        {
            Console.WriteLine("\n[상점]");
            Console.WriteLine("원하는 카테고리를 고르세요:");
            Console.WriteLine("1. 무기 | 2. 방어구 | 3. 포션 | 0. 메인메뉴");

            var input = Console.ReadLine();
            ItemCategory category;

            switch (input)
            {
                case "1":
                    category = ItemCategory.Weapon;
                    break;
                case "2":
                    category = ItemCategory.Armor;
                    break;
                case "3":
                    category = ItemCategory.Potion;
                    break;
                case "0":
                    manager.ChangeScene("main");
                    return;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    manager.ChangeScene("main");
                    return;
            }

            // 카테고리별 아이템 출력
            var availableItems = shop.GetItemsByCategory(category);

            if (availableItems.Count == 0)
            {
                Console.WriteLine("해당 카테고리에 아이템이 없습니다.");
                manager.ChangeScene("main");
                return;
            }

            for (int i = 0; i < availableItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. ");
                availableItems[i].ShowInfo();
            }

            Console.Write("구매할 아이템 번호를 입력하세요 (0: 취소): ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= availableItems.Count)
            {
                var selectedItem = availableItems[itemIndex - 1];

                if (player.Gold >= selectedItem.price)
                {
                    player.Gold -= selectedItem.price;
                    player.Inventory.AddItem(selectedItem);
                    Console.WriteLine($"'{selectedItem.name}'을(를) 구매했습니다!");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다!");
                }
            }
            else
            {
                Console.WriteLine("구매를 취소했습니다.");
            }

            manager.ChangeScene("main");
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

            manager.ChangeScene("main");
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
