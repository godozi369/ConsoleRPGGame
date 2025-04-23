using System.Diagnostics;
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
        public void UseItem(int index)
        {
            if (index >= 1 && index <= InvenList.Count)
            {
                CItem item = InvenList[index - 1];

                item.quantity -= 1;
                Console.WriteLine($"[사용] {item.name}을(를) 사용했습니다! 남은 수량 : {item.quantity}");

                if (item.quantity <= 0)
                {
                    Console.WriteLine($"[삭제] {item.name}이(가) 인벤토리에서 사라졌습니다.");
                    InvenList.RemoveAt(index - 1);
                }
            }
            else
            {
                Console.WriteLine("해당 번호의 아이템이 없습니다.");
            }
        }
        public void RemoveItem(CItem item)
        {
            InvenList.Remove(item);
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
