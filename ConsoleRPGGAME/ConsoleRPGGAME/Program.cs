using System.Text;

using Game.Inventory;
using Game.Player;
using Game.Scene;
using Game.NPC;
using Game.Map;
using Game.GameManager;


namespace Program
{
    internal class CProgram
    {
        static void Main()
        {            
            Console.OutputEncoding = System.Text.Encoding.UTF8; // 특수문자 사용 명령어
            Console.CursorVisible = false;  // 커서 숨기기

            CInventory Inven = new CInventory();
            CPlayer player = new CPlayer("까비", 300, 15, 0, 150, Inven);

            CGameManager gameManager = new CGameManager();
            Console.SetCursorPosition(20, 0);
            gameManager.Initialize();   // 맵 생성

            CShop shop = new CShop();
            CNPC npc = new NPC1("지나가는 행상인", shop);
            // npc.Interact(player); // npc상호작용 test

            Inven.ShowInventory();
          
            // 씬 추가  ( 삭제 ) 
            SceneManager sceneManager = new SceneManager();
            sceneManager.Register(SceneType.Game, new GameScene(gameManager, player));
            sceneManager.Register(SceneType.Inventory, new InventoryScene(player));
            
            
            // 기본 씬 설정
            sceneManager.ChangeScene(SceneType.Game);



        }
    }
}
