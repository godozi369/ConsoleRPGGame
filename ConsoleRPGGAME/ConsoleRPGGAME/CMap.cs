﻿using Game.NPC;

namespace Game.Map
{
    public class CMap
    {
        public enum TileType
        {
            Empty, 
            River, 
            portal  
        }
        const char CIRCLE = '\u25cf'; // 유니코드 문자로 출력하기 위한 상수 
        public TileType[,] _tile; // 2차원 배열로 맵의 각 타일 저장
        public int _size; // 맵의 크기
        public string Name { get; set; } = "이름 없는 맵"; 
        public void SetName(string name)
        {
            Name = name;
        }
        public void Initialize(int size)
        {
            _tile = new TileType[size, size]; // 맵의 크기 size x size 만큼 2차원 배열 생성
            _size = size; // 맵 크기 설정
            for (int y = 0; y < _size; y++)  
            {
                for (int x = 0; x < _size; x++) 
                {
                    // 맵의 테두리 벽으로 만들기
                    if (x == 0 || x == _size - 1 || y == 0 || y == _size - 1)
                    {
                        _tile[y, x] = TileType.River; // 벽타일 설정
                    }
                    else
                    {
                        _tile[y, x] = TileType.Empty; // 내부공간은 빈공간 설정
                    }
                }
            }
        }
        
        // 맵을 화면에 출력하는 메서드
        public void Render(int playerX, int playerY)
        {   
            ConsoleColor prevColor = Console.ForegroundColor; 
            for (int y = 0; y < _size; y++) 
            {
                Console.SetCursorPosition(_size*3, y + 1);
                for (int x = 0; x < _size; x++) 
                {
                    if (x == playerX && y == playerY)
                        Console.ForegroundColor = ConsoleColor.White; // 캐릭터 위치 색                 
                    else if (NPCList.Any(npc => npc.X == x && npc.Y == y))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow; // NPC 위치 색                      
                    }
                    else
                        Console.ForegroundColor = GetTileColor(_tile[y, x]); // 현재 타일의 색을 설정

                    Console.Write(CIRCLE); // 문자 출력 
                }
                Console.WriteLine();
            }
            Console.ForegroundColor = prevColor; // 이전 색상으로 복원
        }     
        // 타일의 종류에 따라 콘솔 글자색을 반환하는 메서드
        public ConsoleColor GetTileColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.River:
                    return ConsoleColor.DarkBlue; 
                case TileType.portal:
                    return ConsoleColor.Cyan;
                default:
                    return ConsoleColor.Green;

            }
        }

        // 포탈 좌표 설정 
        public void SetPortal(int x, int y)
        {
            if (_tile[y, x] == TileType.Empty)
                _tile[y, x] = TileType.portal;
        }

        // NPC 배치 
        public List<CNPC> NPCList = new();
        public void AddNPC(CNPC npc)
        {
            NPCList.Add(npc);
        }

        // 플레이어와 NPC와 거리 계산
        public CNPC GetNearByNPC(int playerX, int playerY)
        {
            foreach (var npc in NPCList)
            {
                if (Math.Abs(npc.X - playerX) + Math.Abs(npc.Y - playerY) == 1)
                    return npc;
            }
            return null;
        }       
    }
}
