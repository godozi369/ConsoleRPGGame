using Game.Item;
using Game.Monster;
using Game.Player;
using Game.Util;

namespace Game.Battle
{
    public class CBattle
    {
        public void StartBattle(CPlayer player, CMonster monster)
        {
            int turn = 1;         
            Helper.ClearFromLine(15);
            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"야생의 [{monster.Name}]를(을) 만났다 !!!");
            Thread.Sleep(1000);
           
            while (player.Hp > 0 && monster.Hp > 0)
            {
                Helper.ClearFromLine(16);
                Console.SetCursorPosition(0, 16);              
                Console.WriteLine($"============{turn}번째 턴============ ");
                turn++;
                Console.WriteLine($"\t[{player.Name}] VS [{monster.Name}]");
                monster.ShowStatus();
                Console.WriteLine("[1] 공격 [2] 아이템 사용 [3] 도망");
                Console.Write("[입력] : ");
                var key = Console.ReadLine();

                if ( key == "1" )
                {
                    Helper.ClearFromLine(21);
                    Console.SetCursorPosition(0, 21);
                    int playerDamage = player.Atk + (player.EquipWeapon?.abil ?? 0);
                    monster.Hp -= playerDamage;
                    player.Hp -= monster.Atk;
                    Console.WriteLine($"[{player.Name}]의 공격!!!");
                    Thread.Sleep(1000);
                    Console.WriteLine($"{player.Name}가 {monster.Name}에게 {playerDamage}만큼 피해를 입혔습니다!");
                    Thread.Sleep(1000);
                    Console.WriteLine($"{monster.Name}의 공격!!!");
                    Thread.Sleep(1000);
                    Console.WriteLine($"{monster.Name}가 {player.Name}에게 {monster.Atk}만큼 피해를 입혔습니다!");
                    Thread.Sleep(2000);

                    if (monster.Hp <= 0)
                    {
                        Console.WriteLine($"[{monster.Name}]을 처치했습니다");

                        // 드랍 아이템 확률 설정
                        Random rand = new Random();
                        float dropChance = rand.Next(0, 101);

                        CItem drop = null;

                        if (monster.Name == "원숭이" && dropChance < 50)
                        {
                            drop = new Fragment("파편", "원숭이 영혼 파편", "원숭이의 영혼이 담긴 파편입니다", 3, 1);
                        }
                        else if (monster.Name == "늑대" && dropChance < 30)
                        {
                            drop = new Fragment("파편", "늑대 영혼 파편", "늑대의 영혼이 담긴 파편", 3, 1);   
                        }

                        if (drop != null)
                        {
                            player.Inventory.AddItem(drop);
                            Console.WriteLine($"[드랍] [{drop.name}] 아이템을 획득했습니다!");
                        }
                        else
                        {
                            Console.WriteLine("아무 아이템도 드랍되지 않았습니다.");
                        }                      
                                          
                        player.Gold += monster.Gold;
                        Console.WriteLine($"[골드] {monster.Gold} GOLD를 획득했습니다!");
                     
                        // 경험치 획득
                        player.GetExp(monster.Exp);
                        Console.WriteLine($"[경험치] {monster.Exp} 경험치를 획득했습니다!");
                    }
                    else if (player.Hp <= 0)
                    {
                        Console.WriteLine($"{player.Name}이 사망했습니다.");                       
                    }
                }
                // 수정 필요 
                else if ( key == "2")
                {
                    Console.WriteLine("아이템을 사용한다");
                }
                else if ( key == "3")
                {
                    Console.WriteLine("도망치기 위해 어쩔수없이 한대 맞아버렸다!");
                    Console.WriteLine($"{player.Name}의 체력({player.Hp} - {monster.Atk} = {player.Hp - monster.Atk})");
                    player.Hp -= monster.Atk;
                    break;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    turn--;
                }
            }
            Console.WriteLine("아무키나 입력하면 게임으로...");
            Console.ReadKey();
            Helper.ClearFromLine(15);
            
        }
    }
}
