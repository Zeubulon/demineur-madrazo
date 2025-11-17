using System;

namespace demineur_madrazo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool theEnd = false;
            int nRow = 0;
            int nColumn = 0;
            int difficulty = 0;
            string difficultyWord = null;
            int nbMine = 0;
            int[,] grid = null;

            //Boucle de jeu
            while (!theEnd)
            {
                //Affiche le titre
                Title();

                //Affiche les condition du tableau
                Console.SetCursorPosition(3, 4);
                Console.WriteLine("Merci d'entrer le nombre de ligne et de colonne de votre plateau de jeux");
                Console.SetCursorPosition(3, 5);
                Console.WriteLine("en sachant qu'au minimum on a un plateau de 6 lignes x 6 colonnes !");
                Console.SetCursorPosition(3, 6);
                Console.WriteLine("et au maximum un plateau de 30 lignes x 30 colonnes !");
                Console.WriteLine("----------------------------------------------------------------------------");

                //Récolte la taille du tableau voulu par l'utilisateur
                Console.SetCursorPosition(0, 9);
                nRow = Numbers("ligne");
                nColumn = Numbers("colonne");

                //Séléction de la dificulté
                difficulty = Difficulty();

                //Changement de fenêtre pour la fenêtre de jeu
                Console.Clear();
                Title();

                //Calcul du Nombre de mine
                nbMine = MineCalcul(nRow, nColumn, difficulty);

                //affichage des statistique de la partie
                Console.Write("\nA vous de jouer !! Mode : ");


                //Définition de la difficulté de jeu en string (+Couleur du texte)
                difficultyWord = DifficultyWord(difficulty);

                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.WriteLine(difficultyWord);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(nbMine);
                Console.ResetColor();
                Console.WriteLine(" mines se cachent dans le jeu !");

                //Affichage du tableau
                grid = Grid(nRow, nColumn);
                grid = GridBackend(grid, nRow, nColumn, nbMine);

                //Clear de la console + Jeu devient jouable
                Game(nRow, nColumn, grid, nbMine);

                //Proposition de recommencer
                Console.WriteLine("\nSi vous voulez relancer une partie, appuyer sur la touch R");
                ConsoleKeyInfo rematch = Console.ReadKey(true);
                if (rematch.Key == ConsoleKey.R) Console.Clear();
                else { theEnd = true; }
            }
            Console.WriteLine("Finish");

        }
        /// <summary>
        /// Affiche le titre
        /// </summary>
        static void Title()
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  Démineur simplifié (Esteban Madrazo)                     ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════════════════╝");
        }
        /// <summary>
        /// Vérifie que les nombres donné par l'utilisateur son belle est bien des nombres et se situe entre 6 et 30.
        /// </summary>
        /// <param name="w">Mot dans la phrase : Nombre de ...</param>
        /// <returns></returns>
        static int Numbers(string w)
        {
            bool valueOk = false;
            bool rangeOk = false;
            int n = 0;

            while (valueOk != true && rangeOk != true)
            {
                Console.Write($"Nombre de {w} : ");
                valueOk = int.TryParse(Console.ReadLine(), out n);
                if (n >= 6 && n <= 30)
                {
                    rangeOk = true;
                }
                else
                {
                    Console.WriteLine("Votre valeur dois être un nombre qui se situe entre 6 et 30 ! Veuillez ressayer.");
                    valueOk = false;
                }
            }
            return n;

        }
        /// <summary>
        /// Vérifie que les nombres donné par l'utilisateur son belle est bien des nombres et se situe entre 1 et 3.
        /// </summary>
        /// <returns></returns>
        static int Difficulty()
        {
            int difficulty = 0;
            bool valueOk = false;
            bool rangeOk = false;

            Console.WriteLine("\n\tMerci d'entrer la difficulté pour votre jeu\n\ten sachant que :" +
                "\n\t\t1 -- > niveau facile\n\t\t2 --> niveau moyen\n\t\t3 --> niveau difficile");
            Console.WriteLine("----------------------------------------------------------------------------");

            while (valueOk != true && rangeOk != true)
            {
                Console.Write("Votre difficulté : ");
                valueOk = int.TryParse(Console.ReadLine(), out difficulty);
                if (difficulty >= 1 && difficulty <= 3)
                {
                    rangeOk = true;
                }
                else
                {
                    Console.WriteLine("Votre valeur dois être un nombre qui se situe entre 1 et 3 ! Veuillez ressayer.");
                    valueOk = false;
                }
            }

            return difficulty;
        }
        /// <summary>
        /// Définition de la difficulté de jeu en string (+Couleur du texte)
        /// </summary>
        /// <param name="d">Difficulté défini par l'utilisateur</param>
        /// <returns></returns>
        static string DifficultyWord(int d)
        {
            string difficultyWord = "Undefined";

            if (d == 1)
            {
                difficultyWord = "Facile";
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (d == 2)
            {
                difficultyWord = "Moyen";
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else
            {
                difficultyWord = "Difficile";
                Console.ForegroundColor = ConsoleColor.Red;
            }

            return difficultyWord;
        }
        /// <summary>
        /// Calcul la quantité de mine à mettre dans le jeu
        /// </summary>
        /// <param name="nRow">Nombre de ligne</param>
        /// <param name="nCol">Nombre de colonnes</param>
        /// <param name="d">Difficulté défini par l'utilisateur</param>
        /// <returns></returns>
        static int MineCalcul(int nRow, int nCol, int d)
        {
            double minePercent = 0;

            if (d == 1)
            {
                minePercent = 0.10;
            }
            else if (d == 2)
            {
                minePercent = 0.25;
            }
            else
            {
                minePercent = 0.40;
            }

            double surface = (nRow / 2 + 1) * (nCol / 2 + 1);

            double doubleMine = surface * minePercent;
            int nMine = (int)doubleMine;

            return nMine;
        }
        /// <summary>
        /// Affichage du tableau + création de la variable Tableau
        /// </summary>
        /// <param name="nRow">Nombre de lignes</param>
        /// <param name="nColumn">Nombre de colonnes</param>
        /// <returns></returns>
        static int[,] Grid(int nRow, int nColumn)
        {
            int marginTop = 8;
            int marginLeft = 4;
            int n = nRow;

            int[,] grid = new int[nRow, nColumn];

            LineGrid(nRow, "╔", "╦", "╗", marginTop, marginLeft, true);

            marginTop += 2;
            while (nColumn > 1)
            {
                while (n > 1)
                {
                    LineGrid(nRow, "╠", "╬", "╣", marginTop, marginLeft, true);
                    n--;
                }
                n = nRow;
                marginTop += 2;
                nColumn--;
            }

            LineGrid(nRow, "╚", "╩", "╝", marginTop, marginLeft, false);

            marginTop = 8;
            marginLeft = marginLeft + (nRow * 3) + nRow + 2;

            Console.SetCursorPosition(marginLeft, marginTop);
            Console.WriteLine("Consignes :");
            Console.CursorLeft = marginLeft;
            Console.WriteLine("-----------");
            Console.CursorLeft = marginLeft += 3;
            Console.WriteLine("- Pour se déplacer dans le jeu utiliser les touches flèchées");
            Console.CursorLeft = marginLeft;
            Console.WriteLine("- Pour explorer une case la touche enter");
            Console.CursorLeft = marginLeft;
            Console.WriteLine("- Pour définir une case en tant que mine (flag) la touche Espace");
            Console.CursorLeft = marginLeft;
            Console.WriteLine("- La touche Enter sur un flag enlève le flag");
            Console.CursorLeft = marginLeft;
            Console.WriteLine("- Pour quitter la touche Esc\n");

            Console.CursorLeft = marginLeft -= 3;
            Console.WriteLine("La partie est gagnée :");
            Console.CursorLeft = marginLeft += 3;
            Console.WriteLine("- Une fois que toutes les cases ont été explorées");
            Console.CursorLeft = marginLeft;
            Console.WriteLine("- Que toutes les mines n'ont pas été explosées");

            Console.SetCursorPosition(6, 9);

            return grid;
        }
        /// <summary>
        /// Aide à créer le tableau avec la fonction Grid
        /// </summary>
        /// <param name="nRow">Nombres de lignes</param>
        /// <param name="c1">Caractère à gauche</param>
        /// <param name="c2">Caractères du milieu</param>
        /// <param name="c3">Caractères toutes à droite</param>
        /// <param name="marginTop">Marge depuis le haut</param>
        /// <param name="marginLeft">Marge depuis la gauche</param>
        /// <param name="t">Indiqué si c'est la dernière ligne ou pas</param>
        static void LineGrid(int nRow, string c1, string c2, string c3, int marginTop, int marginLeft, bool t)
        {
            int n = nRow;

            Console.SetCursorPosition(marginLeft, marginTop);
            Console.Write($"{c1}═══");
            while (n > 1)
            {
                Console.Write($"{c2}═══");
                n--;
            }
            Console.WriteLine(c3);
            if (t)
            {
                n = nRow;
                marginTop++;

                Console.SetCursorPosition(marginLeft, marginTop);

                Console.Write("║   ");
                while (n > 1)
                {
                    Console.Write("║   ");
                    n--;
                }
                Console.Write("║");
            }
        }
        /// <summary>
        /// Clear de la console + Jeu devient jouable
        /// </summary>
        /// <param name="nb1">Nombres de lignes</param>
        /// <param name="nb2">Nombre de colonnes</param>
        /// <param name="nb3">Tableau des mines</param>
        /// <param name="nbMine">Nombre de mines</param>
        static void Game(int nb1, int nb2, int[,] nb3, int nbMine)
        {
            bool isFinished = false;
            bool isWon = false;
            int col = 0;
            int row = 0;
            int remainingMine = nbMine;

            while (!isFinished)
            {
                int mineCount = 0;
                int emptyCount = 0;

                ConsoleKeyInfo touch = Console.ReadKey(true);
                switch (touch.Key)
                {
                    case ConsoleKey.RightArrow:
                        row++;
                        break;

                    case ConsoleKey.LeftArrow:
                        row--;
                        break;

                    case ConsoleKey.UpArrow:
                        col--;
                        break;

                    case ConsoleKey.DownArrow:
                        col++;
                        break;

                    case ConsoleKey.Enter:
                        if (nb3[row, col] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X");
                            Console.ResetColor();
                            remainingMine--;
                            nb3[row, col] = 4;
                            Console.SetCursorPosition(0, nb1 * 2 + 10);
                            Console.Write($"Il reste encore {remainingMine} mine(s) cachée(s)");
                        }
                        else if (nb3[row, col] == 5 || nb3[row, col] == 6)
                        {
                            Console.Write(" ");
                            if (nb3[row, col] == 5)
                                nb3[row, col] = 0;
                            else
                                nb3[row, col] = 1;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("O");
                            Console.ResetColor();
                            nb3[row, col] = 3;
                        }
                        break;

                    case ConsoleKey.Spacebar:
                        if (nb3[row, col] == 0)
                        {
                            Console.Write("F");
                            nb3[row, col] = 5;
                        }
                        else if (nb3[row, col] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("F");
                            Console.ResetColor();
                            nb3[row, col] = 6;
                        }
                        break;

                    default:
                        break;

                }

                for (int i = 0; i < nb3.GetLength(0); i++)
                {
                    for (int j = 0; j < nb3.GetLength(1); j++)
                    {
                        switch (nb3[i, j])
                        {
                            case 4:
                                mineCount++;
                                break;

                            case 0:
                                emptyCount++;
                                break;

                            case 5:
                                emptyCount++;
                                break;

                            default:
                                break;
                        }
                    }
                }

                if (touch.Key == ConsoleKey.Escape || emptyCount == 0 || remainingMine == 0)
                {
                    if (mineCount != nbMine)
                    {
                        isWon = true;
                    }

                    isFinished = true;
                }

                if (col == nb2)
                {
                    col = 0;
                }
                else if (row == nb1)
                {
                    row = 0;
                }
                else if (col < 0)
                {
                    col = nb2 - 1;
                }
                else if (row < 0)
                {
                    row = nb1 - 1;
                }

                int marginLeft = 6 + 4 * row;
                int marginTop = 9 + 2 * col;
                Console.SetCursorPosition(marginLeft, marginTop);
            }
            Console.SetCursorPosition(0, nb1 * 2 + 12);
            Console.WriteLine("C'est la fin !\n");
            if (isWon)
            {
                Console.WriteLine("C'est Gagné !");
            }
            else
            {
                Console.WriteLine("C'est Perdu !");
            }
        }
        /// <summary>
        /// Rajoute les mines dans la variable tableau
        /// </summary>
        /// <param name="grid">La variable tableau</param>
        /// <param name="nRow">Nombre de lignes</param>
        /// <param name="nColumn">Nombres de colonnes</param>
        /// <param name="nbMine">Nombre de mines</param>
        /// <returns></returns>
        static int[,] GridBackend(int[,] grid, int nRow, int nColumn, int nbMine)
        {
            int mineRow;
            int mineCol;

            Random mRandom = new Random();

            while (nbMine > 0)
            {
                mineRow = mRandom.Next(nRow);
                mineCol = mRandom.Next(nColumn);

                while (grid[mineRow, mineCol] == 1)
                {
                    mineRow = mRandom.Next(nRow);
                    mineCol = mRandom.Next(nColumn);
                }

                grid[mineRow, mineCol] = 1;

                nbMine--;
            }
            return grid;
        }
    }
}