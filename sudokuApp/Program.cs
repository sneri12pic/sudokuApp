using System;
using System.Reflection.Emit;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

namespace sudokuApp
{
    class Program
    {
        static int cursorRow = 0;
        static int cursorCol = 0;
        static char[][] sudokuBoard;
        static MoveStack moveStack;
        static MoveStack redoStack;

        static void Main(string[] args)
        {
            moveStack = new MoveStack();
            redoStack = new MoveStack();

            ShowMenu();

            while (true)
            {
                Console.Clear();
                PrintSudokuBoard();

                PrintUndoRedoMessage();

                // Read user input
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                HandleInput(keyInfo);
            }
        }
        static void PrintSudokuBoard()
        {
            for (int row = 0; row < 9; row++)
            {
                PrintRow(row);
                Console.WriteLine();
            }
        }
        static void ShowMenu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Choose the difficulty level:");
            //  Difficulties
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Easy");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("2. Medium");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. Hard");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("4. Custom");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter your choice (1, 2, 3 or 4): ");

            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (choice)
            {
                case '1':
                    sudokuBoard = SudokuGenerator.GenerateSudoku(SudokuDifficulty.Easy);
                    break;
                case '2':
                    sudokuBoard = SudokuGenerator.GenerateSudoku(SudokuDifficulty.Medium);
                    break;
                case '3':
                    sudokuBoard = SudokuGenerator.GenerateSudoku(SudokuDifficulty.Hard);
                    break;
                case '4':
                    sudokuBoard = SudokuGenerator.GenerateSudoku(SudokuDifficulty.Custom);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Exiting...");
                    return;
            }
        }
        static void PrintRow(int row)
        {
            for (int col = 0; col < 9; col++)
            {
                // Check if the cell is the selected cell
                if (row == cursorRow && col == cursorCol)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                // Check if the cell contains '.' and is modified by the user
                else if (sudokuBoard[row][col] == '.' && IsUserInput(row, col))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    // Reset color to default if not selected and not modified by the user
                    Console.ResetColor();
                }

                // Print cell value
                Console.Write(sudokuBoard[row][col] + " ");
            }

            // Reset console color after printing each row
            Console.ForegroundColor = ConsoleColor.White;
        }



        static bool IsUserInput(int row, int col)
        {
            /*Console.ForegroundColor = ConsoleColor.Cyan;*/
            return sudokuBoard[row][col] == '.';
        }

        static bool IsMoveStackEmpty()
        {
            return moveStack.IsEmpty();
        }

        static void PushMove(int row, int col, char value)
        {
            moveStack.Push(row, col, value);
        }

        static SudokuMove PopMove()
        {
            return moveStack.Pop();
        }
        static void PrintUndoRedoMessage()
        {
            // Print the message near the Sudoku grid
            Console.SetCursorPosition(0, 11); // Adjust the position as needed
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press 'Z' for undo");
            Console.WriteLine("Press 'X' for redo");
            Console.ResetColor();
        }

        static void HandleInput(ConsoleKeyInfo keyInfo)
        {
            SudokuValidator validator = new SudokuValidator();
            // Check if Escape key is pressed
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Console.Clear();
                moveStack.Clear();
                // Go back to the menu
                ShowMenu();
                return;
            }
            // Create a copy of the current Sudoku board before making any changes
            char[][] originalBoard = new char[9][];
            for (int i = 0; i < 9; i++)
            {
                originalBoard[i] = new char[9];
                Array.Copy(sudokuBoard[i], originalBoard[i], 9);
            }
            // Move cursor by using arrow keys
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    cursorRow = Math.Max(0, cursorRow - 1);
                    break;
                case ConsoleKey.DownArrow:
                    cursorRow = Math.Min(8, cursorRow + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    cursorCol = Math.Max(0, cursorCol - 1);
                    break;
                case ConsoleKey.RightArrow:
                    cursorCol = Math.Min(8, cursorCol + 1);
                    break;
                // Undo
                case ConsoleKey.Z:
                    if (!moveStack.IsEmpty())
                    {
                        SudokuMove undoneMove = moveStack.Pop();
                        moveStack.PushRedo(undoneMove.Row, undoneMove.Col, sudokuBoard[undoneMove.Row][undoneMove.Col]);
                        sudokuBoard[undoneMove.Row][undoneMove.Col] = '.';
                        // Undo the move in the board
                    }
                    break;
                // Redo
                case ConsoleKey.X:
                    if (!moveStack.IsRedoStackEmpty())
                    {
                        SudokuMove redoneMove = moveStack.PopRedo();
                        sudokuBoard[redoneMove.Row][redoneMove.Col] = redoneMove.Value;
                        moveStack.Push(redoneMove.Row, redoneMove.Col, redoneMove.Value);
                        // Redo the move in the board
                    }
                    break;
                case ConsoleKey.D1:
                case ConsoleKey.D2:
                case ConsoleKey.D3:
                case ConsoleKey.D4:
                case ConsoleKey.D5:
                case ConsoleKey.D6:
                case ConsoleKey.D7:
                case ConsoleKey.D8:
                case ConsoleKey.D9:
                    char num = keyInfo.KeyChar;
                    // Check if the cell is empty and is valid
                    if (sudokuBoard[cursorRow][cursorCol] == '.' && SudokuValidator.IsValidMove(sudokuBoard, cursorRow, cursorCol, num))
                    {
                        // If the cell is empty and is valid, update the board
                        sudokuBoard[cursorRow][cursorCol] = num;

                        // Push the move onto the stack
                        PushMove(cursorRow, cursorCol, num);

                        // Check for victory
                        if (SudokuValidator.IsSudokuFullyFilled(sudokuBoard) && SudokuValidator.IsSudokuSolved(sudokuBoard))
                        {
                            // Display the moves
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine();
                            Console.WriteLine("|---------------|");
                            Console.WriteLine("|    Victory!   |");
                            Console.WriteLine("|---------------|");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Moves:");
                            while (!IsMoveStackEmpty())
                            {
                                SudokuMove move = PopMove();
                                Console.WriteLine($"Row: {move.Row + 1}, Col: {move.Col + 1}, Value: {move.Value}");
                            }
                            Console.WriteLine("Press any key to exit.");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                    }
                    break;
            }
        }
    }
}
