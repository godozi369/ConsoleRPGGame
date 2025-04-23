using Game.Item;
using System.Runtime.InteropServices;
using Game.Player;

namespace Game.Collect
{
    public abstract class CCollect
    {
        public virtual string Name { get; }
        public virtual string Description { get; }

        public abstract void Collect(CPlayer player) { }
    }  
    public abstract class TreeCollect : CCollect
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override void Collect(CPlayer player); 
    }

    public class NormalTree : TreeCollect
    {
        public NormalTree()
        {
            Name = "평범한 나무";
            Description = "울창한 숲에 어울리는 나무";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine("[평범한 나무] 나무 조각을 얻었습니다!");
            player.Inventory.AddItem(new Fragment("재료", "나무 조각", "기본 제작 재료", 1, 5,0));
        }
    }
    public class AppleTree : TreeCollect
    {
        public AppleTree()
        {
            Name = "사과나무";
            Description = "빨간 사과가 열려있다";
        }
        public override void Collect(CPlayer player)
        {
            Random rand = new Random();
            if (rand.Next(100) < 80)
            {
                Console.WriteLine($"[{Name}] 사과를 획득했습니다!");
                player.Inventory.AddItem(new Potion("소비아이템", "사과", "HP 10 회복", 10, 10, 0));
            }
            else
            {
                Console.WriteLine($"[{Name}] 아쉽게도 아무것도 얻지 못했습니다.");
            }
        }
    }
    public class ShineTree : TreeCollect
    {
        public ShineTree()
        {
            Name = "희귀 나무";
            Description = "잎사귀가 찬란하게 빛난다";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine("[희귀 나무] 빛나는 잎을 획득했습니다!");
            player.Inventory.AddItem(new Potion("아이템", "빛나는 잎", "HP 333 회복", 333, 333, 0));
        }
    }
    public abstract class HerbCollect : CCollect
    {
        public string Name { get; set;}
        public string Description { get; set; }
        public override void Collect(CPlayer player);
    }
    public class HealingHerb : HerbCollect
    {
        public HealingHerb()
        {
            Name = "회복초";
            Description = "자연 치유력이 깃든 약초.";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine("[회복초] HP 15 회복!");
            player.Inventory.AddItem(new Potion("약초", "회복초", "HP 15 회복", 15, 15, 0));
        }
    }

    public class PoisonHerb : HerbCollect
    {
        public PoisonHerb()
        {
            Name = "독초";
            Description = "어딘가 위험해보이는 약초";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine("[독초] 상태이상: 중독 (확률적)");
            player.Inventory.AddItem(new Fragment("약초", "독초", "조심해야 할 독성 약초", -10, 15, 0));
        }
    }
    public class MagicHerb : HerbCollect
    {
        public MagicHerb()
        {
            Name = "마법초";
            Description = "신비한 힘이 느껴지는 약초";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine("[마법초] 상태이상: 각성");
            player.Inventory.AddItem(new Fragment("약초", "마법초", "신비한 힘이 느껴지는 약초", 33, 33, 0));
        }
    }

    public abstract class MineralCollect : CCollect
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public override void Collect(CPlayer player);
    }
    public class StoneOre : MineralCollect
    {
        public StoneOre()
        {
            Name = "돌덩이";
            Description = "단단해보이는 돌덩이";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine($"{Name}");
            player.Inventory.AddItem(new Fragment("광석", "돌덩이", Description, 1, 15, 0));
        }
    }
    public class IronOre : MineralCollect
    {
        public IronOre()
        {
            Name = "철광석";
            Description = "은은한 빛이 돈다";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine($"{Name}");
            player.Inventory.AddItem(new Fragment("광석", "철광석", Description, 3, 30, 0));
        }
    }
    public class GoldOre : MineralCollect
    {
        public GoldOre()
        {
            Name = "금광석";
            Description = "금빛이 강렬하다";
        }
        public override void Collect(CPlayer player)
        {
            Console.WriteLine($"{Name}");
            player.Inventory.AddItem(new Fragment("광석", "금광석", Description, 9, 90, 0));
        }
    }

}
