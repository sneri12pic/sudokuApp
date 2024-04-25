using System;
using System.Reflection.Metadata.Ecma335;
namespace sudokuApp
{
    public enum SudokuDifficulty
    {
        Easy,
        Medium,
        Hard,
        Custom
    }

    public class SudokuGenerator
    {
        private static Random random = new Random();

        public static char[][] GenerateSudoku(SudokuDifficulty difficulty)
        {
            char[][] board = new char[9][];
            for (int i = 0; i < 9; i++)
            {
                board[i] = new char[9];
            }

            FillBoardRandomly(board, difficulty);
            RemoveNumbers(board, difficulty);

            return board;
        }

        private static void FillBoardRandomly(char[][] board, SudokuDifficulty difficulty)
        {

            // Generate a random permutation of numbers from 1 to 9
            int[] nums = GenerateRandomPermutation();

            // Fill the board with numbers from the permutation
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    board[i][j] = (char)(nums[(i * 3 + i / 3 + j) % 9] + '0');
                }
            }
        }
        private static int GetCellsToRemoveForDifficulty(SudokuDifficulty difficulty)
        {
            // Define the number of initial numbers based on difficulty
            switch (difficulty)
            {
                case SudokuDifficulty.Easy:
                    return 15; // Easy: 15 blanks
                case SudokuDifficulty.Medium:
                    return 25; // Medium: 25 blanks
                case SudokuDifficulty.Hard:
                    return 40; // Hard: 40 blanks
                case SudokuDifficulty.Custom:
                    Console.WriteLine("Enter the number of initial numbers for the custom difficulty:");
                    int numCustom;
                    while (!int.TryParse(Console.ReadLine(), out numCustom) || numCustom < 0 || numCustom > 81)
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 0 and 81.");
                    }
                    return numCustom;
                default:
                    throw new ArgumentException("Invalid difficulty level");
            }
        }

        private static int[] GenerateRandomPermutation()
        {
            // Generate a random permutation of numbers from 1 to 9
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = nums.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int temp = nums[i];
                nums[i] = nums[j];
                nums[j] = temp;
            }
            return nums;
        }

        private static void RemoveNumbers(char[][] board, SudokuDifficulty difficulty)
        {
            int cellsToRemove = GetCellsToRemoveForDifficulty(difficulty);

            // Track the number of cells removed
            int cellsRemoved = 0;

            // Create a list to store the coordinates of all cells
            List<(int, int)> cellCoordinates = new List<(int, int)>();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cellCoordinates.Add((i, j));
                }
            }

            // Shuffle the list of cell coordinates
            Shuffle(cellCoordinates);

            // Remove numbers from the board randomly
            foreach ((int i, int j) in cellCoordinates)
            {
                if (cellsRemoved < cellsToRemove)
                {
                    board[i][j] = '.';
                    cellsRemoved++;
                }
                else
                {
                    break;
                }
            }
        }

        // Fisher - Yates shuffle algorithm to shuffle a list
        private static void Shuffle<T>(IList<T> list)
        {
            Random random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
