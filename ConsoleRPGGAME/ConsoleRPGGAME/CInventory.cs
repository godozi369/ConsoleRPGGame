using System.Diagnostics;
using System.Numerics;
using System.Xml.Linq;
using Game.Item;
using Game.Player;
using Game.Util;

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
            Console.SetCursorPosition(0, 15);
            Console.WriteLine("      [인벤토리]");         
            int itemCount = 0;
            foreach (var item in InvenList)
            {
                itemCount++;
                Console.WriteLine($"[{itemCount}].[{item.type}] {item.name} | {item.info} : {item.abil} | 갯수 : {item.quantity} | 가격 : {item.price} ");
            }
            return itemCount;
        }

        public void AddItem(CItem item)
        {
            bool found = false;
            
            for (int i = 0; i < InvenList.Count; i++)
            {
                if (InvenList[i].name == item.name && InvenList[i].category == item.category)
                {
                    InvenList[i].quantity += 1;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                item.quantity = 1;
                InvenList.Add(item);
            }
        }      
        public void UseItem(int index, CPlayer player)
        {
            if (index >= 1 && index <= InvenList.Count)
            {
                CItem item = InvenList[index - 1];

                Console.SetCursorPosition(0, 15);
                if (item.category == ItemCategory.Potion)
                {
                    player.Hp += item.abil;
                    Console.WriteLine($"{player.Name}의 체력이 {item.abil}만큼 회복되었습니다!");
                }
                else
                {
                    player.EquipItem(item);
                    Console.WriteLine($"{item.name}을 장착했습니다!");
                }
                item.quantity--;
                if (item.quantity <= 0)
                {
                    InvenList.RemoveAt(index-1);
                }
            }
            else
            {
                Console.WriteLine("해당 번호의 아이템이 없습니다.");
            }
            Thread.Sleep(1000);
            Helper.ClearFromLine(15);
        }
        public void RemoveItem(CItem item)
        {
            if (item.quantity > 1)
            {
                item.quantity--;
            }
            else
            {
                InvenList.Remove(item);
            }
        }
        public CItem GetItemByIndex(int index)
        {
            if (index > 0 && index <= InvenList.Count)
            {
                return InvenList[index - 1];
            }
            else { return null; }
        }
    }
}
