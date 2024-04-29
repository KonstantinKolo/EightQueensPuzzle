using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EightQuinsProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            EightQueenProblem problem = new EightQueenProblem();
            problem.Solution();
            problem.DisplaySolutions();
        }
    }
    class EightQueenProblem
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
            while(_chessboard[0,7] != 'Q')
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
                // goes back a row and sets the 'Q' as 'u'
                GetRidOfU();
                _currentRow--;
                for (int col = 0; col < 8; col++)
                {
                    if (_chessboard[_currentRow, col] == 'Q')
                    {
                        _chessboard[_currentRow, col] = 'u';
                        break;
                    }
                }

                ResetBoard();

                bool qIsPlaced = AddQueen();

                if (!qIsPlaced)
                {
                    GetRidOfU();
                    _currentRow--;
                    BackTrack();
                }
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
            foreach (char[,] chessboard in _solutionsList)
            {
                Console.WriteLine(new String('~',12));
                for (sbyte i = 0; i < 8; i++)
                {
                    for (sbyte i2 = 0; i2 < 8; i2++)
                    {
                        Console.Write(chessboard[i,i2]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine(new String('~', 12));
            }
        }
    }
}
