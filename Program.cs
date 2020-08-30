using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrint
{
    class Program
    {
        static void Main(string[] args)
        {
            bool hrapokracuje = true;
            while(hrapokracuje)
            {
                switch (HlavniNabidka.ZobrazHlavniNabidku())
                {
                    case 0:
                        //Nová hra
                        Console.Clear();
                        HerniSvet hra = new HerniSvet();
                        hra.AktualizujZobrazeni();
                        while (hra.Stav == HerniSvet.StavHry.Probiha)
                        {
                            hra.ZpracovaniPohybu();
                            hra.ZkontrolujPolicko();
                            hra.AktualizujZobrazeni();
                        }
                        switch (hra.Stav)
                        {
                            case HerniSvet.StavHry.Ukonceni:
                                Console.Clear();
                                Console.WriteLine("Hra ukončena");
                                break;
                            case HerniSvet.StavHry.Vyhra:
                                Console.Clear();
                                Console.WriteLine("Vyhrál jsi!");
                                break;
                            case HerniSvet.StavHry.Prohra:
                                Console.Clear();
                                Console.WriteLine("Prohrál jsi!");
                                break;
                        }
                        Console.ReadKey();
                        break;
                    case 1:
                        //Instrukce
                        Console.Clear();
                        Console.Write("Instrukce\nOvládání:\n");
                        Console.Write("\"Šipky\"");
                        Console.Write("\nCíl hry: Sesbírej všechny předměty a dojdi k východu");
                        Console.ReadKey();
                        break;
                    case 2:
                        //Konec
                        hrapokracuje = false;
                        break;
                }
            }
        }
    }
    class HlavniNabidka
    {
        public static int ZobrazHlavniNabidku()
        {
            //funkce, která zobrazuje hlavní nabídku a posílá zpátky hodnotu podle vybraného pole
            string[] polozkyNabidky = new string[3];
            polozkyNabidky[0] = "Nová hra";
            polozkyNabidky[1] = "Instrukce";
            polozkyNabidky[2] = "Konec";
            int zvolenaPolozka = 0;
            bool vyberdokoncen = false;
            Console.Clear();
            while(!vyberdokoncen)
            {
                Console.SetCursorPosition(0, 3);
                for (int I = 0; I < polozkyNabidky.Length; I++)
                {
                    if (zvolenaPolozka == I)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    Console.WriteLine(polozkyNabidky[I]);
                    Console.ResetColor();
                }
                ConsoleKeyInfo stisknutaKlavesa = Console.ReadKey(true);
                switch(stisknutaKlavesa.Key)
                {
                    case ConsoleKey.UpArrow:
                        zvolenaPolozka--;
                        if (zvolenaPolozka < 0)
                        {
                            zvolenaPolozka = polozkyNabidky.Length - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        zvolenaPolozka++;
                        if (zvolenaPolozka > polozkyNabidky.Length - 1)
                        {
                            zvolenaPolozka = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        vyberdokoncen = true;
                        break;
                    default:
                        break;
                }
            }
            return zvolenaPolozka;
        }
    }
    class HerniSvet
    {
        int[,] Mapa;
        const int MapaSirka = 24;
        const int MapaVyska = 24;
        const int Prekazka = 3;
        const int Predmet = 2;
        const int Vychod = 1;
        int ZbyvajiciPredmety;
        public enum StavHry {Probiha,Vyhra,Prohra,Ukonceni }
        public StavHry Stav = StavHry.Probiha;

        private int x = 1, y = 1;
        int predchoziX = 1, predchoziY = 1;

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if (value >= 1 && value < MapaSirka - 1)
                {
                    if (Mapa[value, y] == Prekazka)
                    {
                        return;
                    }
                    Mapa[x, y] = Prekazka;
                    x = value;
                }
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value >= 1 && value < MapaVyska - 1)
                {
                    if (Mapa[x, value] == Prekazka)
                    {
                        return;
                    }
                    Mapa[x, y] = Prekazka;
                    y = value;
                }
            }
        }

        public HerniSvet()
        {
            Mapa = new int [MapaSirka, MapaVyska];
            for (int z = 0; z < MapaVyska; z++)
            {
                //v podstatě - všechny proměnné na levo nebo pravo v rohu jsou nyní 3
                Mapa[0, z] = Prekazka;
                Mapa[MapaSirka - 1, z] = Prekazka;                
            }
            for (int v = 0; v < MapaSirka; v++)
            {
                Mapa[v, 0] = Prekazka;
                Mapa[v, MapaVyska - 1] = Prekazka;
            }
            Mapa[2, 2] = Prekazka;
            Mapa[3, 10] = Prekazka;
            Mapa[20, 5] = Prekazka;
            Mapa[15, 20] = Prekazka;

            Mapa[10, 7] = Predmet;
            Mapa[21, 22] = Predmet;
            Mapa[4, 2] = Predmet;

            Mapa[13, 19] = Vychod;
            for (int J = 1; J < 10; J++)
            {
                Mapa[J, 8] = Prekazka;
            }
            ZobrazHerniSvet();
        }
        private void ZobrazHerniSvet()
        {
            for (int Y = 0; Y < MapaVyska; Y++)
            {
                for (int X = 0; X < MapaSirka; X++)
                {
                    switch(Mapa[X, Y])
                    {
                        case Prekazka:
                            Console.SetCursorPosition(X, Y);
                            Console.Write('#');
                            break;
                        case Predmet:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.SetCursorPosition(X, Y);
                            Console.Write('*');
                            Console.ResetColor();
                            ZbyvajiciPredmety++;
                            break;
                        case Vychod:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.SetCursorPosition(X, Y);
                            Console.Write('@');
                            Console.ResetColor();
                            break;
                    }
                }
            }
        }
        public void ZpracovaniPohybu()
        {
            //převádí stisk tlačítek na pohyb
            ConsoleKeyInfo StisknutaKlavesa = Console.ReadKey(true);
            switch(StisknutaKlavesa.Key)
            {
                case ConsoleKey.DownArrow:
                    Y++;
                    break;
                case ConsoleKey.UpArrow:
                    Y--;
                    break;
                case ConsoleKey.LeftArrow:
                    X--;
                    break;
                case ConsoleKey.RightArrow:
                    X++;
                    break;
                case ConsoleKey.Escape:
                    Stav = StavHry.Ukonceni;
                    break;
            }
        }
        public void AktualizujZobrazeni()
        {
            Console.SetCursorPosition(predchoziX, predchoziY);
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(' ');
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write('X');
            Console.ResetColor();
            predchoziX = X;
            predchoziY = Y;
            Console.SetCursorPosition(MapaSirka + 2, 0);
            Console.WriteLine("Předmětů zbývá sebrat: " + ZbyvajiciPredmety);
        }
        public void ZkontrolujPolicko()
        {
            if (Mapa[X, Y] == Predmet)
            {
                Mapa[X, Y] = 0;
                ZbyvajiciPredmety--;
            }
            else if (Mapa[X, Y] == Vychod && ZbyvajiciPredmety > 0)
            {
                Stav = StavHry.Prohra;
            }
            else if (Mapa[X, Y] == Vychod && ZbyvajiciPredmety == 0)
            {
                Stav = StavHry.Vyhra;
            }
            if (Mapa[X - 1, Y] == Prekazka && Mapa[X, Y - 1] == Prekazka && Mapa[X, Y + 1] == Prekazka && Mapa[X + 1, Y] == Prekazka)
            {
                Stav = StavHry.Prohra;
            }
        }
    }
}
