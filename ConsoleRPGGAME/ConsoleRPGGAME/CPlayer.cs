using Game.Inventory;
using Game.NPC;
using Game.Item;

namespace Game.Player
{
    public class CPlayer
    {
        public string Name { get; }
        public int Hp { get; }
        public int Atk { get; }
        public int Gold { get; }

        public CItem EquipItem { get; set; }
        public CInventory Inventory { get; set; }

        public CPlayer(string name, int hp, int atk, int gold, CInventory inven)
        {
            Name = name;
            Hp = hp;
            Atk = atk;
            Gold = gold;
            Inventory = new CInventory();
        }
        

        public void ShowStatus()
        {

            if (EquipItem != null)
            {
                Console.WriteLine($"\n이름: {Name}, 공격력: {Atk}+{EquipItem.abil}, 골드: {Gold}");
                Console.WriteLine($"장착중:{EquipItem.name} (공격력 + {EquipItem.abil})");
            }
            else
            {
                Console.WriteLine($"\n이름: {Name}, 공격력: {Atk}, 골드: {Gold}");
                Console.WriteLine("장착중인 아이템이 없습니다.\n");
            }
        }
        public void GetItem()
        {

        }
    }
}
