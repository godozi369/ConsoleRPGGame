

namespace Game.Monster
{
    public enum MonsterType { 일반, 보스};
    public class CMonster
    {
        public string Name { get; }
        public MonsterType Type { get; }
        public int Hp { get; set; }
        public int Atk { get; }
        public CMonster(string name, MonsterType type, int hp, int atk)
        {
            Name = name;
            Type = type;
            Hp = hp;
            Atk = atk;
        }
        public void ShowStatus()
        {
            Console.WriteLine($"[{Type}][{Name}] 체력 : {Hp} 공격력 : {Atk}");
        }
    }
    
    public static class MonsterFactory
    {
        public static CMonster CreateMonkey()
        {
            return new CMonster("원숭이", MonsterType.일반, 30, 6);
        }
        public static CMonster CreateMonkeyKing()
        {
            return new CMonster("시저", MonsterType.보스, 150, 15);
        }
        public static CMonster CreateWolf()
        {
            return new CMonster("늑대", MonsterType.일반, 45, 9);
        }
        public static CMonster CreateWareWolf()
        {
            return new CMonster("늑대인간", MonsterType.보스, 300, 30);
        }
        public static CMonster CreateGolem()
        {
            return new CMonster("골렘", MonsterType.일반, 90, 30);
        }
        public static CMonster CreateBigGolem()
        {
            return new CMonster("거대한 골렘", MonsterType.보스, 666, 66);
        }
    }
}
