using Game.Shop;

namespace Game.Item
{
    public enum ItemCategory { Weapon, Armor, Potion }
    public class CItem
    {
        public string name { get; set; }
        public int abil { get; set; }
        public int price { get; set; }
        public string info { get; set; }
        public ItemCategory category { get; set; }

        public CItem(string name, int abil, int price, string info, ItemCategory category)
        {
            this.name = name;
            this.abil = abil;
            this.price = price;
            this.category = category;
            this.info = info;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"{name} , {info} : {abil} , 가격 : {price}");
        }

        private List<CItem> itemList = new List<CItem>();
        public CItem()
        {
            itemList.Add(new Weapon("녹슨 단검", "공격력", 10, 100));
            itemList.Add(new Weapon("쪼개진 망치", "공격력", 15, 150));
            itemList.Add(new Weapon("나무 활", "공격력", 12, 120));
            itemList.Add(new Armor("가죽 아머", "방어력", 3, 90));
            itemList.Add(new Armor("천 아머", "방어력", 2, 60));
            itemList.Add(new Armor("금이 간 강철 아머", "방어력", 6, 180));
            itemList.Add(new Potion("냄새나는 포션", "체력 회복", 10, 10));
            itemList.Add(new Potion("탁한 포션", "체력 회복", 30, 30));
            itemList.Add(new Potion("바카스", "응가누", 3, 15));
        }
    }

    class Weapon : CItem
    {
        public Weapon(string name, string info, int abil, int price) : base(name, abil, price, info, ItemCategory.Weapon) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[무기]{name} , {info} : {abil} , 가격 : {price} ");
        }
    }

    class Armor : CItem
    {
        public Armor(string name, string info, int abil, int price) : base(name, abil, price, info, ItemCategory.Armor) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[방어구]{name} , {info} : {abil} , 가격 : {price} ");
        }
    }

    class Potion : CItem
    {
        public Potion(string name, string info, int abil, int price) : base(name, abil, price, info, ItemCategory.Potion) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[포션]{name} , {info} : {abil} , 가격 : {price}");
        }
    }
    class Normal : CItem
    {
        public Normal(string name, string info, int abil, int price) : base(name, abil, price, info, ItemCategory.Potion) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[기타]{name} , {info} , 가격 : {price}");
        }
    }
}
