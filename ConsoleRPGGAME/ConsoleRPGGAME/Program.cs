using Game.Inven;
using Game.Player;
using Game.Scene;
using Game.Shop;
using Game.Map;
using Game.GameManager;

namespace Program
{
    internal class CProgram
    {
        static void Main()
        {
            CShop shop = new CShop();
            CInven Inven = new CInven();
            CPlayer player = new CPlayer("까비", 100, 15, Inven);

            SceneManager sceneManager = new SceneManager();

            // 씬 추가 
            sceneManager.AddScene("MainMenu", new MainMenuScene());
            sceneManager.AddScene("Game", new GameScene());
            sceneManager.AddScene("character", new CharacterScene(player));
            sceneManager.AddScene("inventory", new InventoryScene(player));
            sceneManager.AddScene("shop", new ShopScene(player));

            // 기본 씬 설정
            sceneManager.ChangeScene("MainMenu");

            // 맵 
            Console.OutputEncoding = System.Text.Encoding.UTF8; // 특수문자 사용 명령어
            Console.CursorVisible = false;  // 커서 숨기기

            CGameManager game = new CGameManager();
            game.Initialize();
            game.playerMove();


        }
    }
}
