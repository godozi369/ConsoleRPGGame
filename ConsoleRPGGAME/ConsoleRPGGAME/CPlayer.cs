using Game.Inventory;
using Game.NPC;
using Game.Item;
using System.Net.Http.Headers;
using System;


namespace Game.Player
{
    public class CPlayer
    {
        public string Name { get; set; }
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Gold { get; set; }

        public Weapon EquipWeapon { get; set; }
        public Armor EquipArmor { get; set; }
        public CInventory Inventory { get; set; }

        public CPlayer(string name, int hp, int atk, int def, int gold, CInventory inven)
        {
            Name = name;
            Hp = hp;
            Atk = atk;
            Def = def;
            Gold = gold;
            Inventory = new CInventory();
        }
        

        public void ShowStatus()
        {
            Console.SetCursorPosition(85, 5);
            Console.WriteLine($"\t\t[{Name}]");
            Console.SetCursorPosition(85, 6);
            Console.WriteLine($"[체력] {Hp}");
            Console.SetCursorPosition(85, 7);
            Console.WriteLine($"[공격력] {Atk}");
            Console.SetCursorPosition(85, 8);
            Console.WriteLine($"[방어력] {Def}");
            Console.SetCursorPosition(85, 9);
            Console.WriteLine($"[소지금] {Gold} GOLD");
            
            int totalAtk = Atk + ( EquipWeapon?.abil ?? 0 );
            Console.SetCursorPosition(85, 10);
            Console.WriteLine($"[공격력] {Atk} + {EquipWeapon?.abil ?? 0} = {totalAtk}");
            
            if (EquipWeapon != null)
            {
                Console.SetCursorPosition(85, 11);
                Console.WriteLine($"[장착중인 무기] {EquipWeapon.name} (+{EquipWeapon.abil}) ");
            }
            if (EquipArmor != null)
            {
                Console.SetCursorPosition(85, 11);
                Console.WriteLine($"[장착중인 방어구] {EquipArmor.name} (+{EquipArmor.abil}) ");
            }
        }
        public void EquipItem(CItem item)
        {
            switch (item.category)
            {
                case ItemCategory.Weapon:
                    EquipWeapon = (Weapon)item;
                    Console.WriteLine($"[{item.type}] {item.name}을 장착했습니다");
                    break;
                case ItemCategory.Armor:
                    EquipArmor = (Armor)item;
                    Console.WriteLine($"[{item.type}] {item.name}을 장착했습니다");
                    break;
                default: Console.WriteLine("장착 불가능한 아이템입니다.");
                    break;
            }
        }
    }
}
