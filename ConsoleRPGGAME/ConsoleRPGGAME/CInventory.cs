using Game.Item;
using Game.Player;

namespace Game.Inventory
{
    public class CInventory
    {
        
        private List<CItem> InvenList;
        public CInventory()
        {
            InvenList = new List<CItem>();
        }

        public int ShowInventory()
        {
            int itemCount = 0;
            foreach (var item in InvenList)
            {
                itemCount++;
                Console.WriteLine("=========================================");
                Console.WriteLine($"{itemCount}.[{item.type}]{item.name}");
                Console.WriteLine($"[정보] : {item.info}");
                Console.WriteLine($"[능력치] : {item.abil}");
                Console.WriteLine($"[가격] : {item.price}");
                Console.WriteLine("=========================================");
            }
            return itemCount;
        }

        public void AddItem(CItem item)
        {
            InvenList.Add(item);
            Console.WriteLine($"[인벤토리] '{item.name}'이(가) 추가됨!");
        }
        public void SellItem(CItem item, CPlayer player)
        {
            InvenList.Remove(item);
            Console.WriteLine($"[인벤토리] {item.name}을(를) {item.price/2}골드에 판매하였습니다");
            player.Gold += item.price;
            Console.WriteLine($"[소지금] : {player.Gold} + {item.price/2} = {player.Gold + item.price/2}");
        }
    }
}
