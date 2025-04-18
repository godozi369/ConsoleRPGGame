using Game.Item;


namespace Game.Shop
{
    public class CShop
    {
        private List<CItem> ShopList = new List<CItem>();

        public CShop()
        {
            ShopList.Add(new Weapon("녹슨 단검", "공격력", 10, 100));
            ShopList.Add(new Weapon("쪼개진 망치", "공격력", 15, 150));
            ShopList.Add(new Weapon("나무 활", "공격력", 12, 120));
            ShopList.Add(new Armor("가죽 아머", "방어력", 3, 90));
            ShopList.Add(new Armor("천 아머", "방어력", 2, 60));
            ShopList.Add(new Armor("금이 간 강철 아머", "방어력", 6, 180));
            ShopList.Add(new Potion("냄새나는 포션", "체력 회복", 10, 10));
            ShopList.Add(new Potion("탁한 포션", "체력 회복", 30, 30));
        }
        public void ShowCategory(ItemCategory category)
        {
            Console.WriteLine($"\n\t[{category}]");

            foreach (var item in ShopList)
            {
                if (item.category == category)
                    item.ShowInfo();
            }
        }
        public void ShowMenu()
        {
            Console.WriteLine("============상 점===========\n");
            Console.WriteLine("\t1. 무기\n");
            Console.WriteLine("\t2. 방어구\n");
            Console.WriteLine("\t3. 포션\n");
            Console.Write("카테고리 번호를 입력하세요 : ");

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                switch (input)
                {
                    case 1: ShowCategory(ItemCategory.Weapon); break;
                    case 2: ShowCategory(ItemCategory.Armor); break;
                    case 3: ShowCategory(ItemCategory.Potion); break;
                    default: Console.WriteLine("잘못입력하셨습니다"); break;
                }
            }

        }
        public List<CItem> GetItemsByCategory(ItemCategory category)
        {
            return ShopList.Where(item => item.category == category).ToList();
        }
    }
}
