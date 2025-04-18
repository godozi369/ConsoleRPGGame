using Game.Inven;
using Game.Shop;
using Game.Item;

namespace Game.Player
{
    public class CPlayer
    {
        public string Name { get; set; }
        public int Gold { get; set; }
        public int Atk { get; set; }

        private CInven EquipItem { get; set; }
        private CInven Inventory { get; set; }

        public CPlayer(string name, int gold, int atk, CInven inven)
        {
            Name = name;
            Gold = gold;
            Atk = atk;
            Inventory = new CInven();
        }

        public void ShowStatus()
        {

            if (EquipItem != null)
            {
                Console.WriteLine($"\n이름: {Name}, 공격력: {Atk}+{EquipItem.Abil}, 골드: {Gold}");
                Console.WriteLine($"장착중:{EquipItem.Name} (공격력 + {Abil})");
            }
            else
            {
                Console.WriteLine($"\n이름: {Name}, 공격력: {Atk}, 골드: {Gold}");
                Console.WriteLine("장착중인 아이템이 없습니다.\n");
            }
        }

    }
}
