using Game.NPC;

namespace Game.Item
{
    public enum ItemCategory { Tool, Cloth, Potion, Fragment }
    public class CItem
    {
        public string type { get; }
        public string name { get; }
        public int abil { get; }
        public int price { get; }
        public string info { get; }
        public int quantity { get; set; }
        public ItemCategory category { get; }

        public CItem(string type, string name, int abil, int price, string info, ItemCategory category, int quantity)
        {
            this.type = type;
            this.name = name;
            this.abil = abil;
            this.price = price;
            this.category = category;
            this.info = info;
            this.quantity = quantity;
        }
  
        public virtual void ShowInfo() { }

        private List<CItem> itemList = new List<CItem>();
        public CItem()
        {
            itemList.Add(new Tool("도구", "허름한 낚시대", "느림", 15, 100, 0));
            itemList.Add(new Tool("도구", "평범한 낚시대", "일반", 30, 300, 0));
            itemList.Add(new Tool("도구", "고급 낚시대", "빠름", 60, 600, 0));
            itemList.Add(new Tool("도구", "금간 도끼", "느림", 15, 100, 0));
            itemList.Add(new Tool("도구", "평범한 도끼", "일반", 30, 300, 0));
            itemList.Add(new Tool("도구", "고급 도끼", "빠름", 60, 600, 0));
            itemList.Add(new Tool("도구", "녹슨 곡괭이", "느림", 12, 120, 0));
            itemList.Add(new Tool("도구", "평범한 곡괭이", "일반", 24, 240, 0));
            itemList.Add(new Tool("도구", "고급 곡괭이", "빠름", 48, 480, 0));
            itemList.Add(new Tool("도구", "낡은 가위", "느림", 12, 120, 0));
            itemList.Add(new Tool("도구", "평범한 가위", "일반", 24, 240, 0));
            itemList.Add(new Tool("도구", "고급 가위", "빠름", 48, 480, 0));
            itemList.Add(new Cloth("옷", "가죽 자켓", "활력", 3, 30, 0));
            itemList.Add(new Cloth("옷", "평범한 작업복", "활력", 6, 60, 0));
            itemList.Add(new Cloth("옷", "고급 작업복", "활력", 18, 180, 0));
            itemList.Add(new Potion("포션", "냄새나는 포션", "체력 회복", 10, 10, 0));
            itemList.Add(new Potion("포션", "탁한 포션", "체력 회복", 30, 30, 0));
            itemList.Add(new Potion("포션", "바카스", "응가누", 3, 15, 0));
            itemList.Add(new Potion("약초", "회복초", "HP 15 회복", 15, 15, 0));
            itemList.Add(new Potion("약초", "독초", "조심해야 할 독성 약초", -10, 15, 0));
            itemList.Add(new Potion("약초", "마법초", "신비한 힘이 느껴지는 약초", 33, 33, 0));
            itemList.Add(new Potion("물고기", "못생긴 붕어", "체력 회복", 10, 10, 0));
            itemList.Add(new Potion("물고기", "썩은 숭어", "체력 회복", 20, 20, 0));
            itemList.Add(new Potion("물고기", "냄새나는 메기", "체력 회복", 30, 30, 0));
            itemList.Add(new Potion("물고기", "통조림 통", "썩은내가 난다", 0, 0, 0));
            itemList.Add(new Fragment("재료", "나무 조각", "기본 제작 재료", 1, 5, 0));
            itemList.Add(new Fragment("재료", "사과", "HP 10 회복", 10, 10, 0));
            itemList.Add(new Fragment("아이템", "빛나는 잎", "HP 333 회복", 333, 333, 0));
            itemList.Add(new Fragment("광석", "돌덩이", "단단해보이는 돌덩이", 1, 15, 0));
            itemList.Add(new Fragment("광석", "철광석", "은은한 빛이 돈다", 3, 30, 0));
            itemList.Add(new Fragment("광석", "금광석", "금빛이 강렬하다", 9, 90, 0));
        }
    }

    public class Tool : CItem
    {
        public Tool(string type, string name, string info, int abil, int price, int quantity) : base(type, name, abil, price, info, ItemCategory.Tool, quantity) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}] {name} | {info} : {abil} | 가격 : {price} ");
        }      
    }

    public class Cloth : CItem
    {
        public Cloth(string type, string name, string info, int abil, int price, int quantity) : base(type, name, abil, price, info, ItemCategory.Cloth, quantity) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}] {name} | {info} : {abil} | 가격 : {price} ");
        }
    }

    class Potion : CItem
    {
        public Potion(string type, string name, string info, int abil, int price, int quantity) : base(type, name, abil, price, info, ItemCategory.Potion, quantity) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}] {name} | {info} : {abil} | 가격 : {price}");
        }
    }
    class Fragment : CItem
    {
        public Fragment(string type, string name, string info, int abil, int price, int quantity) : base(type, name, abil, price, info, ItemCategory.Fragment, quantity) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"[{type}] {name} | {info} | {abil} 가격 : {price}");
        }
    
    }
    
}
     