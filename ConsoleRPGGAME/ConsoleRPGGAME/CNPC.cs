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

    public class NPC1 : CNPC
    {
        private CShop shop;
        public NPC1(int x, int y) : base("무인도 한달 선배", x, y)
        {
            this.shop = ShopData.Shop1;
        }

        public override void Interact(CPlayer player)
        {
            if (shop == null)
            {
                Console.WriteLine("[에러] 상점 객체가 null입니다");
                return;
            }

            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"[{Name}] 안녕하신가! 자네 도구가 좀 필요해보이는군!");
            Console.WriteLine($"[{player.Name}] 필요한게있는지 구경해보겠습니다(Y)");
            Console.WriteLine($"[{player.Name}] 다음에 오겠습니다 ㅎㅎ(N)");
            
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y: shop.ShowShop(player); return;
                    case ConsoleKey.N: Console.WriteLine($"[{Name}] 다음은 없을지도 몰라! ");
                        Thread.Sleep(1000);
                        Helper.ClearFromLine(15);
                        return;
                    default: 
                        Console.WriteLine($"[{Name}] 여기서 시간은 생명이라네!!!"); break;                
                }
            }
        }
    }

    public class NPC2 : CNPC
    {
        private CShop shop;
        public NPC2(int x, int y) : base("무인도 중수", x, y)
        {
            this.shop = ShopData.Shop2;
        }

        public override void Interact(CPlayer player)
        {
            if (shop == null)
            {
                Console.WriteLine("[에러] 상점 객체가 null입니다");
                return;
            }

            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"[{Name}] 안녕하신가! 혹시 필요한건 없는가?");
            Console.WriteLine($"[{player.Name}] 필요한게있는지 구경해보겠습니다(Y)");
            Console.WriteLine($"[{player.Name}] 다음에 오겠습니다 ㅎㅎ(N)");

            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y: shop.ShowShop(player); return;
                    case ConsoleKey.N: Console.WriteLine($"[{Name}] 다음은 없을지도 몰라! ");
                        Thread.Sleep(1000);
                        Helper.ClearFromLine(15);
                        return;
                    default: Console.WriteLine($"[{Name}] 여기서 시간은 생명이라네!!!"); break;
                }
            }
        }

    }

    public class NPC3 : CNPC
    {
        private CShop shop;
        public NPC3(int x, int y) : base("무인도 장인", x, y)
        {
            this.shop = ShopData.Shop3;
        }

        public override void Interact(CPlayer player)
        {
            if (shop == null)
            {
                Console.WriteLine("[에러] 상점 객체가 null입니다");
                return;
            }

            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"[{Name}] 이곳 최고의 것들은 전부 나에게 있네");
            Console.WriteLine($"[{player.Name}] 오 한번 둘러보겠습니다(Y)");
            Console.WriteLine($"[{player.Name}] 다음에 오겠습니다 ㅎㅎ(N)");
            
            while (true)
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y: shop.ShowShop(player); return;
                    case ConsoleKey.N: Console.WriteLine($"[{Name}] 더 성장해서 오게");
                        Thread.Sleep(1000);
                        Helper.ClearFromLine(15);
                        return;
                    default: Console.WriteLine($"[{Name}] 시간은 생명이야!"); break;
                }
            }
            
        }
    }

    public class ShopData
    {
         public static CShop Shop1 => new CShop(new List<CItem>
         {
             new Tool("도구", "허름한 낚시대", "느림", 15, 100, 0),
             new Tool("도구", "금간 도끼", "느림", 15, 100, 0),
             new Tool("도구", "녹슨 곡괭이", "느림", 12, 120, 0),
             new Tool("도구", "낡은 가위", "느림", 12, 120, 0),
             new Cloth("옷", "가죽 자켓", "활력", 3, 30, 0),
             new Potion("포션", "냄새나는 포션", "체력 회복", 10, 10, 0)
         });

         public static CShop Shop2 => new CShop(new List<CItem>
         {
             new Tool("도구", "평범한 낚시대", "일반", 30, 300, 0),
             new Tool("도구", "평범한 곡괭이", "일반", 24, 240, 0),
             new Tool("도구", "평범한 도끼", "일반", 30, 300, 0),
             new Tool("도구", "평범한 가위", "일반", 24, 240, 0),
             new Cloth("옷", "평범한 작업복", "활력", 6, 60, 0),
             new Potion("포션", "탁한 포션", "체력 회복", 30, 30, 0)
         });

         public static CShop Shop3 => new CShop(new List<CItem>
         {
             new Tool("도구", "고급 낚시대", "빠름", 60, 600, 0),
             new Tool("도구", "고급 곡괭이", "빠름", 48, 480, 0),
             new Tool("도구", "고급 도끼", "빠름", 60, 600, 0),
             new Tool("도구", "고급 가위", "빠름", 48, 480, 0),
             new Cloth("옷", "고급 작업복", "활력", 18, 180, 0),
             new Potion("포션", "바카스", "응가누", 3, 15, 0),
         });
    }
    public class CShop 
    {
        private List<CItem> ShopList;
        public CShop(List<CItem> items)
        {
            ShopList = items;         
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
                    Thread.Sleep(1000);
                    Helper.ClearFromLine(15);
                    Console.SetCursorPosition(0, 15);
                    return;
                }
                if (choice >= 1 && choice <= selectList.Count)
                {
                    var selectItem = selectList[choice - 1];
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
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("숫자만 입력해주세요");
            }
            Console.WriteLine("아무키나 누르면 창이 사라집니다");
            Console.ReadKey();
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
                Console.WriteLine("\t1. 도구\n");
                Console.WriteLine("\t2. 옷\n");
                Console.WriteLine("\t3. 잡화\n");
                Console.WriteLine("\t4. 판매\n");
                Console.Write("카테고리 번호를 입력하세요 : ");

                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1: ShowCategory(ItemCategory.Tool, player); return;
                    case ConsoleKey.D2: ShowCategory(ItemCategory.Cloth, player); return;
                    case ConsoleKey.D3: ShowCategory(ItemCategory.Potion, player); return;
                    case ConsoleKey.D4: ShowSellMenu(player); return;
                    default: Console.WriteLine("뭐하는짓이야!"); break;
                }           
            }
        }
        public void ShowSellMenu(CPlayer player)
        {
            Helper.ClearFromLine(15);
            Console.SetCursorPosition(0, 15);
            Console.WriteLine("======== [판매 메뉴] ========");

            int count = player.Inventory.ShowInventory(); // 인벤토리 표시
            if (count == 0)
            {
                Console.WriteLine("판매할 아이템이 없습니다.");
                Thread.Sleep(1500);
                return;
            }

            Console.WriteLine("\n판매할 아이템 번호를 입력하세요 (0 : 나가기)");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0) return;

                var item = player.Inventory.GetItemByIndex(choice);
                if (item != null)
                {
                    int sellPrice = item.price / 2;

                    // 수량이 2 이상이면 -1만
                    if (item.quantity > 1)
                    {
                        item.quantity--;
                        Console.WriteLine($"[판매] {item.name} 1개를 {sellPrice}G에 판매했습니다. (남은 수량: {item.quantity})");
                    }
                    else
                    {
                        player.Inventory.RemoveItem(item);
                        Console.WriteLine($"[판매] {item.name}을 {sellPrice}G에 판매했습니다. (전부 판매됨)");
                    }

                    player.Gold += sellPrice;
                    Console.WriteLine($"[소지금] 현재 GOLD: {player.Gold}");
                }
                else
                {
                    Console.WriteLine("해당 번호의 아이템이 없습니다.");
                }
            }
            else
            {
                Console.WriteLine("숫자만 입력하세요.");
            }

            Thread.Sleep(2000);
            Helper.ClearFromLine(15);
        }
    }
}
