using Game.Item;

namespace Game.Inventory
{
    public class CInventory
    {
        private List<CItem> InvenList;
        // 장착중인 무기 
        private CItem EquipWeapon;
        // 장착중인 방어구 
        private CItem EquipArmor;

        public CInventory()
        {
            EquipWeapon = new CItem();
            EquipArmor = new CItem();

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


    }
}
