using System;

namespace sudokuApp
{
    public class SudokuValidator
    {
        public static bool IsValidMove(char[][] board, int row, int col, char num)
        {
            return IsRowValid(board, row, num) && IsColumnValid(board, col, num) && IsBoxValid(board, row - row % 3, col - col % 3, num);
        }

        public static bool IsSudokuSolved(char[][] board)
        {
            // Check rows
            for (int row = 0; row < 9; row++)
            {
                if (!IsSetValid(board[row]))
                {
                    return false;
                }
            }

            // Check columns
            for (int col = 0; col < 9; col++)
            {
                char[] column = new char[9];
                for (int row = 0; row < 9; row++)
                {
                    column[row] = board[row][col];
                }
                if (!IsSetValid(column))
                {
                    return false;
                }
            }

            // Check 3x3 subgrids
            for (int startRow = 0; startRow < 9; startRow += 3)
            {
                for (int startCol = 0; startCol < 9; startCol += 3)
                {
                    char[] subgrid = new char[9];
                    int index = 0;
                    for (int row = startRow; row < startRow + 3; row++)
                    {
                        for (int col = startCol; col < startCol + 3; col++)
                        {
                            subgrid[index++] = board[row][col];
                        }
                    }
                    if (!IsSetValid(subgrid))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool IsSetValid(char[] set)
        {
            bool[] seen = new bool[9];
            foreach (char c in set)
            {
                if (c != '.' && seen[c - '1'])
                {
                    return false; // Duplicate found
                }
                if (c != '.')
                {
                    seen[c - '1'] = true;
                }
            }
            return true;
        }

        private static bool IsRowValid(char[][] board, int row, char num)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row][col] == num)
                {
                    return false; // Number is already present, row is invalid
                }
            }
            return true; // Row is valid
        }

        private static bool IsColumnValid(char[][] board, int col, char num)
        {
            for (int row = 0; row < 9; row++)
            {
                if (board[row][col] == num)
                {
                    return false; // Number is already present, column is invalid
                }
            }
            return true; // Column is valid
        }

        private static bool IsBoxValid(char[][] board, int startRow, int startCol, char num)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row + startRow][col + startCol] == num)
                    {
                        return false; // Number is already present, box is invalid
                    }
                }
            }
            return true; // Box is valid
        }
        public static bool IsSudokuFullyFilled(char[][] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row][col] == '.')
                    {
                        return false; // If any cell is empty, sudoku is not fully filled
                    }
                }
            }
            return true; // All cells are filled
        }

    }
}
