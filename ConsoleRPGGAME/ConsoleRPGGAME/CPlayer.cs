using Game.Inventory;
using Game.NPC;
using Game.Item;
using System.Net.Http.Headers;
using System;
using Game.Util;


namespace Game.Player
{
    public class CPlayer
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Hp { get; set; }
        public int Atk { get; set; }
        public int Vital { get; set; }
        public int Exp { get; set; }
        public int Gold { get; set; }

        public Tool EquipTool { get; set; }
        public Cloth EquipCloth { get; set; }
        public CInventory Inventory { get; set; }

        public CPlayer(string name,int level, int hp, int atk, int def, int exp, int gold, CInventory inven)
        {
            Name = name;
            Level = level;
            Hp = hp;
            Atk = atk;
            Vital = def;
            Exp = exp;
            Gold = gold;
            Inventory = inven;
        }
        

        public void ShowStatus()
        {
            Console.SetCursorPosition(85, 7);
            Console.WriteLine($"\t[{Name}]   [{CurrentMode}]");
            Console.SetCursorPosition(85, 8);
            Console.WriteLine($"[레벨] {Level}");
            Console.SetCursorPosition(85, 9);
            Console.WriteLine($"[체력] {Hp}");
            Console.SetCursorPosition(85, 10);
            Console.WriteLine($"[활력] {Vital}");        
            Console.SetCursorPosition(85, 11);
            Console.WriteLine($"[경험치] {Exp}");
            Console.SetCursorPosition(85, 12);
            Console.WriteLine($"[소지금] {Gold} GOLD");                                  
            
            if (EquipTool != null)
            {
                Helper.ClearFromLine(13);
                Console.SetCursorPosition(85, 13);
                Console.WriteLine($"[장착중인 도구] {EquipTool.name ?? "이름 없음"}");
                Console.SetCursorPosition(85, 14);
                Console.WriteLine($"[효과] {EquipTool.info ?? "정보없음"}");
            }          
            if (EquipCloth != null)
            {
                Console.SetCursorPosition(85, 15);
                Console.WriteLine($"[장착중인 옷] {EquipCloth.name ?? "이름 없음"}");
            }         

        }

        // 장비 장착
        public void EquipItem(CItem item)
        {
            if (item.category == ItemCategory.Tool)
            {
                // 이전 장비가 있을 경우 인벤토리로 돌려보냄
                if (EquipTool != null)
                {
                    Inventory.AddItem(EquipTool);                   
                }
                // 새 장비 장착
                EquipTool = item as Tool;
                Inventory.RemoveItem(EquipTool);

                Console.WriteLine($"[장착 완료] {item.name}을(를) 장착했습니다!");
            }
            else if (item.category == ItemCategory.Cloth)
            {
                if (EquipCloth != null)
                {
                    Inventory.AddItem(EquipCloth);
                }
                EquipCloth = item as Cloth;
                Inventory.RemoveItem(EquipCloth);

                Console.WriteLine($"[장착 완료] {item.name}을(를) 장착했습니다!");
            }
                   
        }

        // 경험치 & 레벨업
        public void GetExp(int amount)
        {
            Exp += amount;
            if (Exp >= 100)
            {
                Level++;
                Exp -= 100;
                Console.WriteLine($"{Name}의 레벨이 {Level}이 되었습니다");
                Hp += 15;
                Atk += 3;                
            }
        }

        public enum ActivityMode
        { 
            백수모드,
            낚시모드,
            벌목모드,
            채집모드,
            채광모드
        }

        public ActivityMode CurrentMode { get; set; }
    }
}
