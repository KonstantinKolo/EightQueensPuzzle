using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data.SqlTypes;

namespace EightQuinsProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            //using Stopwatch() to measure the speed of using cycle vs recursion
            var timerCycle = new Stopwatch();
            timerCycle.Start();

            EightQueenProblemCycle usingCycles = new EightQueenProblemCycle();
            usingCycles.Solution();
            usingCycles.DisplaySolutions();

            timerCycle.Stop();
            TimeSpan timeTakenCycle = timerCycle.Elapsed;
            Console.WriteLine($"Time taken by using CYCLE: {timeTakenCycle.ToString(@"m\:ss\.ffff")}");



            var timeRecursion = new Stopwatch();
            timeRecursion.Start();

            EightQueenProblemRecursive usingRecursion = new EightQueenProblemRecursive();
            usingRecursion.Solution();
            usingRecursion.DisplaySolution();

            timeRecursion.Stop();
            TimeSpan timeTakenRecursion = timerCycle.Elapsed;
            Console.WriteLine($"Time take by using RECURSION: {timeTakenRecursion.ToString(@"m\:ss\.ffff")}");
        }
    }

    class EightQueenProblemRecursive
    {
        private int _currentRow = 0;
        private int _currentCol = 0;
        private char[,] _chessboard = new char[8, 8];
        private List<char[,]> _solutionsList = new List<char[,]>();

        public EightQueenProblemRecursive()
        {
            initializeChessboard();
        }

        public void Solution()
        {
            AddQueen();
            if (_currentRow < 0)
                return;

            AddQueenAttack();

            _currentRow++;
            if (_currentRow >= 8)
            {
                AddCombination();
            }

            _currentCol = 0;

            Solution();
        }
        private void AddQueen()
        {
            //this will run when there are no free spaces on that row
            if(_currentCol >= 8)
            {
                //get rid of the 'u's on _currentRow
                _currentCol = 0;
  
                RemoveItemFromRow('u', '-');

                _currentRow--;

                //this will run when the program is over
                if (_currentRow < 0)
                {
                    return;
                }

                //get rid of the 'Q' and set her as 'u'
                _currentCol = 0;
                RemoveItemFromRow('Q', 'u');

                RemoveItemFromBoard('-', '*');

                if (_currentRow != 0)
                {
                    ResetBoard();
                }
                else
                {
                    _currentCol = 0;
                }

                AddQueen();
                return;
            }

            if (_chessboard[_currentRow,_currentCol] == '*')
            {
                _chessboard[_currentRow, _currentCol] = 'Q';
            }
            else
            {
                _currentCol++;
                AddQueen();
            }
        }
        private void AddQueenAttack()
        {
            CardinalDirectionAttack();
            LeftRightDAttack();
            RightLeftDAttack();
        }
        private void CardinalDirectionAttack(int k = 0)
        {
            //up-down
            if (_chessboard[_currentRow, k] == '*') 
                _chessboard[_currentRow, k] = '-';

            //left-right
            if (_chessboard[k, _currentCol] == '*') 
                _chessboard[k, _currentCol] = '-';

            k++;
            if (k >= 8)
                return;

            CardinalDirectionAttack(k);
        }
        private void LeftRightDAttack(int k = 0)
        {
            int attackRow = _currentRow - _currentCol + k;
            int attackCol = _currentCol - _currentRow + k;
            if (_currentRow - _currentCol < 0)
            {
                attackRow = 0 + k;
            }
            if (_currentCol - _currentRow < 0)
            {
                attackCol = 0 + k;
            }

            if (_chessboard[attackRow, attackCol] == '*')
                _chessboard[attackRow, attackCol] = '-';

            k++;
            if (attackRow >= 7 || attackCol >= 7)
                return;


            LeftRightDAttack(k);
        }
        private void RightLeftDAttack(int k = 0)
        {
            int attackRow;
            int attackCol = _currentCol + _currentRow - k;

            if (_currentCol + _currentRow < 8)
            {
                attackRow = k;
            }
            else
            {
                attackRow = -(7 - _currentRow - _currentCol - k);
                attackCol = 7 - k;
            }

            if (_chessboard[attackRow, attackCol] == '*')
                _chessboard[attackRow, attackCol] = '-';

            k++;
            if (attackRow >= 7 || attackCol <= 0)
                return;

            RightLeftDAttack(k);
        }
        private void RemoveItemFromRow(char item, char newItem)
        {
            if (_currentCol >= 8)
                return;

            if (_chessboard[_currentRow, _currentCol] == item)
            {
                _chessboard[_currentRow, _currentCol] = newItem;
            }

            _currentCol++;
            RemoveItemFromRow(item, newItem);
        }
        private void RemoveItemFromBoard(char item, char newItem, int row = 0, int col = 0)
        {
            if (_chessboard[row, col] == item)
            {
                _chessboard[row, col] = newItem;
            }

            col++;
            if (col >= 8)
            {
                row++;
                col = 0;
            }
            if (row >= 8)
                return;

            RemoveItemFromBoard(item, newItem, row, col);
        }
        private void AddCombination()
        {
            char[,] outputChessboard = (char[,])_chessboard.Clone();
            _solutionsList.Add(outputChessboard);

            //mark current Q as already used
            _currentRow--;
            _chessboard[_currentRow, _currentCol] = 'u';

            RemoveItemFromBoard('-', '*');
            ResetBoard();
        }
        private void ResetBoard(int row = 0, int col = 0)
        {
            if (_chessboard[row, col] == 'Q') 
            {
                _currentRow = row;
                _currentCol = col;
                AddQueenAttack();

                row++;
                col = -1;
            }


            col++;
            if (col >= 8)
            {
                _currentRow++;
                _currentCol = 0;
                return;
            }

            ResetBoard(row,col);
        }
        private void initializeChessboard(int row = 0, int col = 0)
        {
            _chessboard[row, col] = '*';

            col++;
            if(col >= 8)
            {
                col = 0;
                row++;
                if (row >= 8)
                {
                    return;
                }
            }

            initializeChessboard(row,col);
        }
        public void DisplaySolution()
        {
            // Set a variable to the Desktop path.
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Write the chessboard combinations to a new file named "EightQueensPuzzle.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "RecursiveEightQueensPuzzle.txt")))
            {
                outputFile.WriteLine($"Number of solutions found: {_solutionsList.Count}");
                foreach (char[,] chessboard in _solutionsList)
                {
                    outputFile.WriteLine();
                    for (int i = 0; i < 8; i++)
                    {
                        for (int i2 = 0; i2 < 8; i2++)
                        {
                            outputFile.Write(chessboard[i, i2]);
                        }
                        outputFile.WriteLine();
                    }
                    outputFile.WriteLine();
                }
            }
        }
    }

    class EightQueenProblemCycle
    {
        private int _currentRow = 0;
        private int _currentCol = 0;
        private char[,] _chessboard = new char[8,8];
        private List<char[,]> _solutionsList = new List<char[,]>();

        public void Solution()
        {
            initializeChessboard();

            Placement();
        }
        private void initializeChessboard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    _chessboard[row, col] = '*';
                }
            }
        }
        private void Placement()
        {
            while(_solutionsList.Count < 92)
            {
                while(_currentRow != 8)
                {
                    bool qIsPlaced = AddQueen();

                    if (!qIsPlaced)
                    {
                        _currentRow--;
                        BackTrack();
                        qIsPlaced = true;
                    }
                
                    AddAttack();
                    _currentRow++;
                }

                char[,] outputChessboard = (char[,])_chessboard.Clone();
                _solutionsList.Add(outputChessboard);
                SearchNewSolution();
            }
        }
        private void SearchNewSolution()
        {
            _currentRow--;
            _chessboard[_currentRow, _currentCol] = 'u';
        }
        private bool AddQueen()
        {
            for (int s1 = 0; s1 < 8; s1++)
            {
                if (_chessboard[_currentRow, s1] == '*')
                {
                    _chessboard[_currentRow, s1] = 'Q';
                    _currentCol = s1;
                    return true;
                }
            }

            return false;
        }
        private void AddAttack()
        {
            for (int s1 = 0; s1 < 8; s1++)
            {
                //right-left
                if (_currentCol != s1 && _chessboard[_currentRow,s1] != 'u')  
                {
                    _chessboard[_currentRow, s1] = '-';
                }

                //up-down
                if (_currentRow != s1 && _chessboard[s1,_currentCol] != 'u')
                {
                    _chessboard[s1, _currentCol] = '-';
                }
            }

            int helperCol = _currentCol + 1;
            //down-right 
            if (_currentCol != 7 && _currentRow != 7)
            {
                for (int s1 = _currentRow + 1; s1 < 8; s1++)
                {
                    if(_chessboard[s1,helperCol] != 'u')
                    {
                        _chessboard[s1, helperCol] = '-';
                    }
                    helperCol++;

                    if (helperCol == 8)
                        break;
                }
            }

            //down-left
            if (_currentCol != 0 && _currentRow != 7)
            {
                helperCol = _currentCol - 1;
                for (int s1 = _currentRow + 1; s1 < 8; s1++)
                {
                    if (_chessboard[s1, helperCol] != 'u')
                    {
                        _chessboard[s1, helperCol] = '-';
                    }
                    helperCol--;

                    if (helperCol < 0)
                        break;
                }
            }

            //up-right
            if (_currentCol != 7 && _currentRow != 0)
            {
                helperCol = _currentCol + 1;
                for (int s1 = _currentRow - 1; s1 >= 0; s1--)
                {
                    if(_chessboard[s1,helperCol] != 'u')
                    {
                        _chessboard[s1, helperCol] = '-';
                    }
                    helperCol++;

                    if (helperCol == 8)
                        break;
                }
            }

            //up-left
            if (_currentCol != 0 && _currentRow != 0)
            {
                helperCol = _currentCol - 1;
                for (int s1 = _currentRow - 1; s1 >= 0; s1--)
                {
                    if (_chessboard[s1, helperCol] != 'u')
                    {
                        _chessboard[s1, helperCol] = '-';
                    }
                    helperCol--;

                    if (helperCol < 0)
                        break;
                }
            }
;        }
        private void BackTrack()
        {
            //mark the queen spot as unusable
            for (int s1 = 0; s1 < 8; s1++)
            {
                if(_chessboard[_currentRow,s1] == 'Q')
                {
                    _chessboard[_currentRow, s1] = 'u';
                    break;
                }
            }

            ResetBoard();

            // Checks if there are any free spots for a 'Q' to place
            bool hasPlacedQueen = AddQueen();

            if (!hasPlacedQueen)
            {
                // goes back a row and clearn the current row
                GetRidOfU();
                _currentRow--;
                BackTrack();
            }
        }
        private void GetRidOfU()
        {
            for (int col = 0; col < 8; col++)
            {
                for (int row = _currentRow; row < 8; row++)
                {
                    if(_chessboard[row,col] == 'u') 
                    {
                        _chessboard[row, col] = '*';
                    }
                }
            }
        }
        private void ResetBoard()
        {
            ResetRow();

            for(int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    bool queenDetected = CheckForQueen(col, row);

                    if (!queenDetected && _chessboard[row,col] != 'u')
                    {
                        _chessboard[row, col] = '*';
                    }
                }
            }
        }
        private void ResetRow()
        {
            for (int col = 0; col < 8; col++)
            {
                if(_chessboard[_currentRow,col] != 'Q' && _chessboard[_currentRow,col] != 'u')
                {
                     _chessboard[_currentRow, col] = '-';
                }
            }
        }
        private bool CheckForQueen(int col,int row)
        {
            //check up-down that row
            for (int s2 = 0; s2 < 8; s2++)
            {
                if (_chessboard[s2, col] == 'Q')
                {
                    return true;
                }
            }

            //check right-left
            for (int s2 = 0; s2 < 8; s2++)
            {
                if (_chessboard[row, s2] == 'Q')
                {
                    return true;
                }
            }

            int rowHelper;

            //check diagnoally left-up that row
            if (row > 0 && col > 0)
            {
                rowHelper = row - 1;
                for (int s2 = col - 1; s2 >= 0; s2--)
                {
                    if (_chessboard[rowHelper,s2] == 'Q')
                    {
                        return true;
                    }

                    rowHelper--;

                    if (rowHelper < 0)
                        break;
                }
            }

            //check diagnolly right-up that row
            if (row > 0 && col != 7)
            {
                rowHelper = row - 1;
                for (int s2 = col + 1; s2 < 8; s2++)
                {
                    if (_chessboard[rowHelper, s2] == 'Q')
                    {
                        return true;
                    }

                    rowHelper--;

                    if (rowHelper < 0)
                        break;
                }
            }

            //check diagnolly left-down that row
            if (row != 7 && col != 0)
            {
                rowHelper = row + 1;
                for (int s2 = col - 1; s2 >= 0; s2--)
                {
                    if (_chessboard[rowHelper, s2] == 'Q')
                    {
                        return true;
                    }

                    rowHelper++;

                    if (rowHelper > 7)
                        break;
                }
            }

            //check diagnolly right-down that row
            if (row != 7 && col != 7)
            {
                rowHelper = row + 1;
                for (int s2 = col + 1; s2 < 8; s2++)
                {
                    if (_chessboard[rowHelper, s2] == 'Q')
                    {
                        return true;
                    }

                    rowHelper++;

                    if (rowHelper > 7)
                        break;
                }
            }

            return false;
        }
        public void DisplaySolutions()
        {
            // Set a variable to the Desktop path.
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Write the chessboard combinations to a new file named "EightQueensPuzzle.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "CycleEightQueensPuzzle.txt")))
            {
                outputFile.WriteLine($"Number of solutions found: {_solutionsList.Count}");
                foreach (char[,] chessboard in _solutionsList)
                {
                    outputFile.WriteLine();
                    for (int i = 0; i < 8; i++)
                    {
                        for (int i2 = 0; i2 < 8; i2++)
                        {
                            outputFile.Write(chessboard[i, i2]);
                        }
                        outputFile.WriteLine();
                    }
                    outputFile.WriteLine();
                }
            }
        }
    }
}
