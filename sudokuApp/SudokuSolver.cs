using System;

namespace sudokuApp
{
    public class SudokuSolver
    {
        public void SolveSudoku(char[][] board)
        {
            if (board == null || board.Length == 0)
                return;

            Solve(board);
        }

        private bool Solve(char[][] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row][col] == '.')
                    {
                        for (char num = '1'; num <= '9'; num++)
                        {
                            if (IsValid(board, row, col, num))
                            {
                                board[row][col] = num;

                                if (Solve(board)) // Recursively solve
                                    return true;

                                board[row][col] = '.'; // Backtrack
                            }
                        }

                        return false; // No valid number found, backtrack
                    }
                }
            }

            return true; // Successfully solved
        }

        public bool IsValid(char[][] board, int row, int col, char num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[row][i] == num || board[i][col] == num ||
                    board[3 * (row / 3) + i / 3][3 * (col / 3) + i % 3] == num)
                    return false;
            }
            return true;
        }
    }
}
