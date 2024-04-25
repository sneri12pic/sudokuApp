using System;
using System.Collections.Generic;

namespace sudokuApp
{
    public class SudokuMove
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public char Value { get; set; }

        public SudokuMove(int row, int col, char value)
        {
            Row = row;
            Col = col;
            Value = value;
        }
    }

    public class MoveStack
    {
        private Stack<SudokuMove> stack;
        public Stack<SudokuMove> redoStack;

        public MoveStack()
        {
            stack = new Stack<SudokuMove>();
            redoStack = new Stack<SudokuMove>();
        }

        public void Push(int row, int col, char value)
        {
            stack.Push(new SudokuMove(row, col, value));
            // Clear redo stack when a new move is pushed
            redoStack.Clear();
        }

        public SudokuMove Pop()
        {
            if (stack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty");
            }
            return stack.Pop();
        }

        public void PushRedo(int row, int col, char value)
        {
            redoStack.Push(new SudokuMove(row, col, value));
        }

        public SudokuMove PopRedo()
        {
            if (redoStack.Count == 0)
            {
                throw new InvalidOperationException("Redo stack is empty");
            }
            return redoStack.Pop();
        }

        public bool IsEmpty()
        {
            return stack.Count == 0;
        }

        public bool IsRedoStackEmpty()
        {
            return redoStack.Count == 0;
        }
        public void Clear()
        {
            stack.Clear();
        }

    }
}
