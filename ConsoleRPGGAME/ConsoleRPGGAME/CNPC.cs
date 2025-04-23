using Game.Item;
using Game.Player;
using Game.Inventory;
using Game.GameManager;
using Game.Util;

namespace Game.NPC
{
    public interface INpcInteract
    {
        public void Interact(CPlayer player);
    }
    public abstract class CNPC : INpcInteract
    {
        public string Name { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public CNPC(string name, int x, int y)
        {
            Name = name;
            X = x;
            Y = y;
        }
        public abstract void Interact(CPlayer player);              
    }
    // 1. 행상인npc
    public class NPC1 : CNPC
    {
        private CShop shop;
        public NPC1(string name, int x, int y, CShop shop) : base(name, x, y) { this.shop = shop; } 
        public override void Interact(CPlayer player)
        {
            // 널 오류 방어
            if (shop == null)
            {
                Console.WriteLine("[에러] 상점 객체가 null입니다");
                return;
            }

            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"[{Name}] 안녕하신가! 모험가여! 혹시 필요한건 없는가?");
            Console.WriteLine($"[{player.Name}] 필요한게있는지 구경해보겠습니다(Y)");
            Console.WriteLine($"[{player.Name}] 다음에 오겠습니다 ㅎㅎ(N)");
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {                   
                    case ConsoleKey.Y: shop.ShowShop(player); return;
                    case ConsoleKey.N: Console.WriteLine($"[{Name}] 다음은 없을지도 몰라! "); return;
                    default: Console.WriteLine($"[{Name}] 뭐하는겐가! 대답을 하라구!"); break;                    
                }
            }            
        }
    }
    public class CShop
    {
        private List<CItem> ShopList = new List<CItem>();
        public CShop()
        {
            ShopList.Add(new Weapon("무기", "녹슨 단검", "공격력", 10, 100));
            ShopList.Add(new Weapon("무기", "쪼개진 망치", "공격력", 15, 150));
            ShopList.Add(new Weapon("무기", "나무 활", "공격력", 12, 120));
            ShopList.Add(new Armor("방어구", "가죽 아머", "방어력", 3, 90));
            ShopList.Add(new Armor("방어구", "천 아머", "방어력", 2, 60));
            ShopList.Add(new Armor("방어구", "금이 간 강철 아머", "방어력", 6, 180));
            ShopList.Add(new Potion("포션", "냄새나는 포션", "체력 회복", 10, 10));
            ShopList.Add(new Potion("포션", "탁한 포션", "체력 회복", 30, 30));
            ShopList.Add(new Potion("포션", "바카스", "응가누", 3, 15));            
        }
        
        public void ShowCategory(ItemCategory category, CPlayer player)
        {
            Helper.ClearFromLine(15);
            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"\t[{category}]\n");

            List<CItem> selectList = new List<CItem>();
            for (int i = 0; i < ShopList.Count; i++)
            {
                if (ShopList[i].category == category)
                {
                    selectList.Add(ShopList[i]);
                }
            }

            for (int i = 0; i < selectList.Count; i++)
            {
                Console.Write($"{i + 1}.");
                selectList[i].ShowInfo();
            }

            Console.WriteLine("\n0. 이전으로 돌아가기");           
            Console.Write($"\n구매할 아이템 번호를 입력하세요 : ");
            

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0)
                {
                    Console.WriteLine("이전으로 돌아갑니다");
                    Console.ReadKey(true);
                    Helper.ClearFromLine(15);
                    Console.SetCursorPosition(0, 15);
                    return;
                }
                if (choice >= 1 && choice <= ShopList.Count)
                {
                    var selectItem = ShopList[choice - 1];
                    if (player.Gold >= selectItem.price)
                    {
                        player.Gold -= selectItem.price;
                        player.Inventory.AddItem(selectItem);
                        Console.WriteLine($"[{player.Name}]가 {selectItem.name}을(를) 구매했습니다!");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 번호입니다");
                }
            }
            else
            {
                Console.WriteLine("숫자만 입력해주세요");
            }
            Console.WriteLine("잠시후 창이 사라집니다");
            Thread.Sleep(3000);
            Helper.ClearFromLine(15);
            Console.SetCursorPosition(0, 15);
        }
        public void ShowShop(CPlayer player)
        {           
            while (true)
            {
                Helper.ClearFromLine(15);
                Console.SetCursorPosition(0, 15);
                Console.WriteLine("============상 점===========\n");
                Console.WriteLine("\t1. 무기\n");
                Console.WriteLine("\t2. 방어구\n");
                Console.WriteLine("\t3. 포션\n");
                Console.WriteLine("\t4. 판매\n");
                Console.Write("카테고리 번호를 입력하세요 : ");

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1: ShowCategory(ItemCategory.Weapon, player); return;
                    case ConsoleKey.D2: ShowCategory(ItemCategory.Armor, player); return;
                    case ConsoleKey.D3: ShowCategory(ItemCategory.Potion, player); return;
                    case ConsoleKey.D4: 
                    default: Console.WriteLine("뭐하는짓이야!"); break;
                }           
            }
        }
    }
}
