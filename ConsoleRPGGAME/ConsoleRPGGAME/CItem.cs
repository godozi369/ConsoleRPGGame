using Game.NPC;

namespace Game.Item
{
    public enum ItemCategory { Weapon, Armor, Potion, Fragment }
    public class CItem
    {
        public string type { get; }
        public string name { get; }
        public int abil { get; }
        public int price { get; }
        public string info { get; }
        public ItemCategory category { get; }

        public CItem(string type, string name, int abil, int price, string info, ItemCategory category)
        {
            this.type = type;
            this.name = name;
            this.abil = abil;
            this.price = price;
            this.category = category;
            this.info = info;
        }

        public virtual void ShowInfo() { }

        private List<CItem> itemList = new List<CItem>();
        public CItem()
        {
            itemList.Add(new Weapon("무기", "녹슨 단검", "공격력", 10, 100));
            itemList.Add(new Weapon("무기", "쪼개진 망치", "공격력", 15, 150));
            itemList.Add(new Weapon("무기", "나무 활", "공격력", 12, 120));
            itemList.Add(new Armor("방어구", "가죽 아머", "방어력", 3, 90));
            itemList.Add(new Armor("방어구", "천 아머", "방어력", 2, 60));
            itemList.Add(new Armor("방어구", "금이 간 강철 아머", "방어력", 6, 180));
            itemList.Add(new Potion("포션", "냄새나는 포션", "체력 회복", 10, 10));
            itemList.Add(new Potion("포션", "탁한 포션", "체력 회복", 30, 30));
            itemList.Add(new Potion("포션", "바카스", "응가누", 3, 15));
            itemList.Add(new Fragment("파편", "늑대 영혼 파편", "늑대의 영혼이 담겨있다", 1, 1));
            itemList.Add(new Fragment("파편", "웨어울프 영혼 파편", "늑대의 영혼이 담겨있다", 12, 12));
            itemList.Add(new Fragment("파편", "원숭이 영혼 파편", "원숭이의 영혼이 담겨있다", 1, 1));
            itemList.Add(new Fragment("파편", "몽키킹 영혼 파편", "원숭이의 영혼이 담겨있다", 15, 15));
        }
    }

    class Weapon : CItem
    {
        public Weapon(string type, string name, string info, int abil, int price) : base(type, name, abil, price, info, ItemCategory.Weapon) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}]{name} | {info} : {abil} | 가격 : {price} ");
        }
    }

    class Armor : CItem
    {
        public Armor(string type, string name, string info, int abil, int price) : base(type, name, abil, price, info, ItemCategory.Armor) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}]{name} | {info} : {abil} | 가격 : {price} ");
        }
    }

    class Potion : CItem
    {
        public Potion(string type, string name, string info, int abil, int price) : base(type, name, abil, price, info, ItemCategory.Potion) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}]{name} | {info} : {abil} | 가격 : {price}");
        }
    }  
    class Fragment : CItem
    {
        public Fragment(string type, string name, string info, int abil, int price) : base(type, name, abil, price, info, ItemCategory.Fragment) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}]{name} | {info} | 영혼흡수율 : +{abil} 가격 : {price}");
        }
    }
}
