

namespace Game.Monster
{
    public enum MonsterType { 일반, 보스};
    public class CMonster
    {
        public string Name { get; }
        public MonsterType Type { get; }
        public int Hp { get; set; }
        public int Atk { get; }
        public int Exp { get; }
        public int Gold { get; }       

        public CMonster(string name, MonsterType type, int hp, int atk, int exp, int gold)
        {
            Name = name;
            Type = type;
            Hp = hp;
            Atk = atk;
            Exp = exp;
            Gold = gold;
        }
        public void ShowStatus()
        {
            Console.WriteLine($"[{Type}:{Name}] 체력 : {Hp} 공격력 : {Atk}");
        }
    }
    

    public static class MonsterFactory
    {
        public static CMonster CreateMonkey()
        {
            return new CMonster("원숭이", MonsterType.일반, 30, 6, 3, 10);
        }
        public static CMonster CreateMonkeyKing()
        {
            return new CMonster("시저", MonsterType.보스, 150, 15, 15, 50);
        }
        public static CMonster CreateWolf()
        {
            return new CMonster("늑대", MonsterType.일반, 45, 9, 6, 20);
        }
        public static CMonster CreateWareWolf()
        {
            return new CMonster("늑대인간", MonsterType.보스, 300, 30, 30, 100);
        }
        public static CMonster CreateGolem()
        {
            return new CMonster("골렘", MonsterType.일반, 90, 30, 9, 30);
        }
        public static CMonster CreateBigGolem()
        {
            return new CMonster("거대한 골렘", MonsterType.보스, 666, 66, 66, 150);
        }
    }
}
