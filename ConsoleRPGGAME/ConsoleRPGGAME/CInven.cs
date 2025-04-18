using Game.Item;

namespace Game.Inven
{
    public class CInven
    {
        private Dictionary<int, CItem> invenList = new Dictionary<int, CItem>();
        private int currentIndex = 1;


        public void AddItem(CItem item)
        {
            invenList[currentIndex++] = item;
        }


        public void ShowInventory()
        {
            Console.WriteLine("\n===== [인벤토리 목록] =====");

            if (invenList.Count == 0)
            {
                Console.WriteLine("아이템이 없습니다.");
            }

            foreach (var kvp in invenList)
            {
                Console.Write($"{kvp.Key}. ");
                kvp.Value.ShowInfo();
            }

            Console.WriteLine("===========================");
        }


        public CItem GetItem(int index)
        {
            if (invenList.TryGetValue(index, out var item))
                return item;

            return null;
        }
    }
}
