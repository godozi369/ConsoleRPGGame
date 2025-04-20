using Game.Monster;
using Game.Player;

namespace Game.Battle
{
    public class CBattle
    {
        public void StartBattle(CPlayer player, CMonster monster)
        {
            Console.Clear();
            Console.WriteLine("================전투 시작================");
            Console.WriteLine($"\t{player.Name} VS {monster.Name}");
            while (player.Hp > 0 && monster.Hp > 0)
            {
                Console.WriteLine("------정보------");
                player.ShowStatus();
                monster.ShowStatus();

                Console.WriteLine("[1] 공격 [2] 아이템 사용 [3] 도망");
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.D1)
                {
                    int playerDamage = player.Atk + (player.EquipWeapon?.abil ?? 0);
                    monster.Hp -= playerDamage;
                    Console.WriteLine($"{player.Name}가 {monster.Name}에게 {playerDamage}만큼 피해를 입혔습니다");

                    if (monster.Hp < 0)
                    {
                        Console.WriteLine($"{monster.Name}을 처치했습니다");
                        break;
                        // 전리품 메서드 추가
                    }
                }
                // 수정 필요 
                else if (key.Key == ConsoleKey.D3)
                {
                    Console.WriteLine("아이템을 사용한다");
                }
                else if (key.Key == ConsoleKey.D3)
                {
                    Console.WriteLine("도망치기 위해 어쩔수없이 한대 맞아버렸다!");
                    Console.WriteLine($"{player.Name}의 체력({player.Hp} - {monster.Atk} = {player.Hp - monster.Atk})");
                    player.Hp -= monster.Atk;
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                }
                Console.WriteLine("계속하시려면 아무키나 누르세요");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}
