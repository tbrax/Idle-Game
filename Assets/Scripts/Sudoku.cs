using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;


public class SudokuCell
{
    List<int> position;
    int answer;
    bool solved;
    List<int> possibleAnswers;
    List<int> validNums;

    public SudokuCell(List<int> pos)
    {
        position = pos;
        answer = 0;
        solved = false;
        possibleAnswers = new List<int>();
        validNums = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            possibleAnswers.Add(i + 1);
            validNums.Add(i + 1);
        }
    }


    public void remove(int num)
    {
        if (possibleAnswers.Contains(num) && solved == false)
        {
            possibleAnswers.Remove(num);
            if (possibleAnswers.Count == 1)
            {
                answer = possibleAnswers[0];
                solved = true;
            }
        }
        if (possibleAnswers.Contains(num) && solved ==true)
        {
            answer = 0;
        }


    }

    public List<int> popList(int num0,int num1)
    {
        List<int> t = new List<int>();
        for (int i = num0; i <= num1; i++)
        {
            t.Add(i);
        }
        return t;
    }

    public bool solvedMethod()
    {
        return solved;
    }

    public List<int> checkPosition()
    {
        return position;
    }

    public List<int> returnPossible()
    {
        return possibleAnswers;
    }

    public int lenOfPossible()
    {
        return possibleAnswers.Count;
    }

    public int getAnswer()
    {
        return answer;
    }

    public bool methodSolved()
    {
        return solved;

    }

    public int returnSolved()
    {
        if (solved)
        {
            return possibleAnswers[0];
        }
        else
        {
            return 0;
        }

    }

    public void removeAnswer(int num)
    {
        if (solved == false && possibleAnswers.Contains(num))
        {
            possibleAnswers.Remove(num);
            if (possibleAnswers.Count == 1)
            {
                answer = possibleAnswers[0];
                solved = true;
            }
        }
        if (solved == true && possibleAnswers.Contains(num))
        {
            answer = 0;
        }
     }



    public void setAnswer(int num)
    {
        if (solved == false && possibleAnswers.Contains(num))
        {
            answer = num;
            solved = true;
            possibleAnswers = new List<int>();
            possibleAnswers.Add(num);

        }
    }

    public void reset()
    {
        solved = false;
        answer = 0;
        possibleAnswers.Clear();
        possibleAnswers = popList(1, 10);
    }

    public void copyAnswer(int a, bool s, List<int> p, List<int> v)
    {
        answer = a;
        solved = s;
        List<int> tp = new List<int>();
        List<int> tv = new List<int>();

        foreach (int i in possibleAnswers)
        {
            tp.Add(i);
        }

        foreach (int i in validNums)
        {
            tv.Add(i);
        }
        possibleAnswers = tp;
        validNums = tv;
    }

    public SudokuCell deepCopy()
    {
        SudokuCell c = new SudokuCell(position);
        //c.copyAnswer(answer,solved,possibleAnswers,validNums);
        return c;
    }

    public void setSame(SudokuCell c)
    {
        answer = c.getAnswer();
        possibleAnswers = c.returnPossible();

        position = c.checkPosition();
        solved = c.methodSolved();


    }


}



public class Sudoku
{
    GameObject sudokuUI;

    List<int> correct = new List<int>();
    List<int> saved = new List<int>();

    List<GameObject> text = new List<GameObject>();

    //List<SudokuCell> sudoku;

    List<int> possibleAnswers;

    string level;
    //int guesses = 0;
    int counter;
    List<List<int>> gridToSolveChange;
    List<List<int>> gridToSolveAns;
    List<List<int>> gridToSolve;
    int selectedX = -1;
    int selectedY = -1;

    public Sudoku(GameObject ui)
    {
        
        sudokuUI = ui;
        setupButtons();

        possibleAnswers = new List<int>();

        possibleAnswers = popList(1, 9);

    }

    public List<int> popList(int num0, int num1)
    {
        List<int> t = new List<int>();
        for (int i = num0; i <= num1; i++)
        {
            t.Add(i);
        }
        return t;
    }

    public List<GameObject> getChildren(GameObject g)
    {
        List<GameObject> childrens = new List<GameObject>();

        foreach (Transform child in g.transform)
        {
            if (!childrens.Contains(child.gameObject))
            {
                childrens.Add(child.gameObject);
            }
        }
        return childrens;
    }

    List<SudokuCell> makeEmpty()
    {

        List<SudokuCell> s = new List<SudokuCell>();
        for (int i = 0; i<9; i++)
        {
            int k = 0;
            if (i == 0 || i == 1 || i == 2)
            {
                k = 0;
            }
            else if (i == 3 || i == 4 || i == 5)
            {
                k = 4;
            }
            else if (i == 6 || i == 7 || i == 8)
            {
                k = 7;
            }


            for (int j = 0; j < 9; j++)
            {
                int k2 = k;

                if (j == 0 || j == 1 || j == 2)
                {
                    k2 += 0;
                }
                else if (j == 3 || j == 4 || j == 5)
                {
                    k2 += 1;
                }
                else if (j == 6 || j == 7 || j == 8)
                {
                    k2 += 2;
                }
                List<int> li = new List<int>();
                li.Add(i);
                li.Add(j);
                li.Add(k2);
                SudokuCell suc = new SudokuCell(li);
                s.Add(suc);
            }
        }

        return s;
    }

    List<SudokuCell> deepCopy(List<SudokuCell> sudoku)
    {
        List<SudokuCell> s = makeEmpty();

        for (int i = 0; i < sudoku.Count; i++)
        {
            s[i].setSame(sudoku[i]);
        }



            return s;
    }

    public void setText(string s, int i)
    {
        text[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = s;
    }

    bool validSudoku(List<SudokuCell> sudoku)
    {
        foreach (SudokuCell i in sudoku)
        {
            if (i.getAnswer() == 0)
            {
                return false;
            }


        }

        return true;
    }

    List<SudokuCell> sudokuGen()
    {
         List<SudokuCell> sudoku;
        List<int> cells;
        List<int> lowestNum;
        List<SudokuCell> lowest;
        sudoku = makeEmpty();

        while (!validSudoku(sudoku))
        {

            sudoku = makeEmpty();
            cells = popList(0, 80);
            while (cells.Count > 0)
            {
                lowestNum = new List<int>();
                lowest = new List<SudokuCell>();
                foreach (int i in cells)
                {
                    lowestNum.Add(sudoku[i].lenOfPossible());
                }
                int m = lowestNum.Min();

                foreach (int i in cells)
                {
                    if (sudoku[i].lenOfPossible() == m)
                    {
                        lowest.Add(sudoku[i]);
                    }

                }

                SudokuCell choiceElement = lowest[UnityEngine.Random.Range(0, lowest.Count)];
                int choiceIndex = sudoku.IndexOf(choiceElement);
                List<int> position1 = choiceElement.checkPosition();
                cells.Remove(choiceIndex);

                if (choiceElement.solvedMethod() == false)
                {
                    List<int> possibleValues = choiceElement.returnPossible();
                    int finalValue = possibleValues[UnityEngine.Random.Range(0, possibleValues.Count)];
                    choiceElement.setAnswer(finalValue);
                    foreach (int i in cells)
                    {
                        List<int> position2 = sudoku[i].checkPosition();
                        if (position1[0] == position2[0])
                        {
                            sudoku[i].removeAnswer(finalValue);
                        }
                        if (position1[1] == position2[1])
                        {
                            sudoku[i].removeAnswer(finalValue);
                        }
                        if (position1[2] == position2[2])
                        {
                            sudoku[i].removeAnswer(finalValue);
                        }
                    }
                }
                else
                {
                    int finalValue = choiceElement.returnSolved();
                    foreach (int i in cells)
                    {
                        List<int> position2 = sudoku[i].checkPosition();
                        if (position1[0] == position2[0])
                        {
                            sudoku[i].removeAnswer(finalValue);
                        }
                        if (position1[1] == position2[1])
                        {
                            sudoku[i].removeAnswer(finalValue);
                        }
                        if (position1[2] == position2[2])
                        {
                            sudoku[i].removeAnswer(finalValue);
                        }
                    }
                }
            }

        }
        return sudoku;
    }


    bool sudokuChecker(List<SudokuCell> sudoku)
    {
        List<int> position1;
        List<int> position2;

        for (int i = 0; i < sudoku.Count; i++)
        {
            for (int n = 0; n < sudoku.Count; n++)
            {
                if ( i != n)
                {
                    position1 = sudoku[i].checkPosition();
                    position2 = sudoku[n].checkPosition();

                    if (position1[0] == position2[0] || position1[1] == position2[1] || position1[2] == position2[2])
                    {
                        int num1 = sudoku[i].returnSolved();
                        int num2 = sudoku[n].returnSolved();
                        if (num1 == num2)
                        {
                            return false;
                        }
                    }
                }
            }
        }
            return true;
    }



    public List<SudokuCell> perfectSudoku()
    {

        bool result = false;
        List<SudokuCell> sudoku = sudokuGen();
        result = sudokuChecker(sudoku);

        while (result == false)
        {
            sudoku = sudokuGen();
            result = sudokuChecker(sudoku);
        }

        return sudoku;

    }

    int getGridAnswer(List<SudokuCell> sudoku, int row, int col)
    {

        return sudoku[(row * 9 + col)].getAnswer();

    }

    void setGridAnswer(List<SudokuCell> sudoku, int row, int col,int ans)
    {

        sudoku[(row * 9 + col)].setAnswer(ans);

    }


    List<List<int>> getGrid(List<SudokuCell> sudoku)
    {
        List<List<int>> grid = new List<List<int>>();
        for (int i = 0; i < 9; i++)
        {
            List<int> g1 = new List<int>();

            for (int j = 0; j < 9; j++)
            {

                g1.Add(sudoku[(i*9)+j].getAnswer());

            }
            grid.Add(g1);
        }


        return grid;
    }

    bool checkGrid(List<List<int>> grid)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {


                if (grid[row][col] ==0)
                {
                    return false;
                }
            }
        }
        return true;
    }


    bool checkGridWin(List<List<int>> grid)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row][col] == 0)
                {
                    return false;
                }


                if (!(isSafeG(grid, row, col, grid[row][col])))
                {
                    return false;
                }
                
            }
        }
        return true;
    }

    bool isSafeG(List<List<int>> grid, int row, int col, int num)
    {
        for (int x = 0; x <= 8; x++)
            if (x != col && grid[row][x] == num)
            {

                return false;   
            }

        for (int x = 0; x <= 8; x++)
            if (x != row && grid[x][col] == num)
            {

                return false;
            }

        int startRow = row - row % 3,
                startCol = col - col % 3;

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                int ti = i + startRow;
                int tj = j + startCol;
                if (!(ti == row && tj == col) && grid[ti][tj] == num)
                {
                    return false;
                }
            }

        return true;
    }





    bool isSafe(List<List<int>> grid, int row, int col, int num)
    {
        for (int x = 0; x <= 8; x++)
            if (grid[row][x] == num)
                return false;

        for (int x = 0; x <= 8; x++)
            if (grid[x][col] == num)
                return false;

        int startRow = row - row % 3,
                startCol = col - col % 3;

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (grid[i + startRow][j +
                                startCol] == num)
                    return false;

        return true;
    }


    bool solveSudoku(List<List<int>> grid, int row, int col)
    {
        counter += 1;
        if (counter > 100000)
        {
            return false;
        }


        if (row == 9-1 && col == 9)
        {
            return true;
        }

        if (col == 9)
        {
            row++;
            col = 0;
        }

        if (grid[row][col] > 0)
        {
            return solveSudoku(grid, row, col + 1);
        }

        for (int num = 1; num <= 9; num++)
        {

            if (isSafe(grid,row,col,num))
            {

                grid[row][col] = num;
                if (solveSudoku(grid,row,col+1))
                {
                    return true;
                }
                    
            }

            grid[row][col] = 0;

        }




        return false;
    }



    void printSudoku(List<List<int>> sudoku)
    {
        for (int i = 0; i < sudoku.Count; i++)
        {


            for (int j = 0; j < sudoku[i].Count; j++)
            {
                string t = sudoku[i][j].ToString();
                if (t == "0")
                {
                    t = "";
                }


                setText(t, (i*9+j));
            }   
        }
    }





    bool equalChecker(List<SudokuCell> s1, List<SudokuCell> s2)
    {
        for (int i = 0; i < s1.Count;i++)
        {
            if ( s1[i].returnSolved() != s2[i].returnSolved())
            {
                return false;
            }
        }
        return true;

    }



    List<List<int>> copyGrd(List<List<int>> grid)
    {
        List<List<int>> copy = new List<List<int>>();

        for (int i = 0; i< grid.Count; i++)
        {
            List<int> temp = new List<int>();

            for (int j = 0; j < grid[i].Count; j++)
            {
                temp.Add(grid[i][j]);

            }

            copy.Add(temp);
        }



        return copy;
    }


    List<SudokuCell> puzzleGen(List<SudokuCell> sudoku)
    {
        

        bool contin = true;
        int removed = 0;

        List<List<int>> grid = getGrid(sudoku);

        gridToSolveAns = copyGrd(grid);
        //70
        while (contin && removed < 70)
        {
            removed++;
            counter = 0;
            int row = UnityEngine.Random.Range(0, 9);
            int col = UnityEngine.Random.Range(0, 9);


            int guessct = 0;
            while (guessct < 10 && grid[row][col] == 0)
            {
                guessct++;
                row = UnityEngine.Random.Range(0, 9);
                col = UnityEngine.Random.Range(0, 9);
            }



            int backup = grid[row][col];
            grid[row][col] = 0;
            List<List<int>> copyGrid = copyGrd(grid);
            if (solveSudoku(copyGrid, 0, 0))
            {
                
            }
            else
            {
                grid[row][col] = backup;
                contin = false;
            }

        }

        gridToSolve = copyGrd(grid);
        gridToSolveChange = copyGrd(grid);



        return sudoku;
    }


    void resetFont()
    {
        for (int i = 0; i < text.Count; i++)
        {
            setText("0",i);
            text[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
    }

    void fontBold()
    {
        for (int i = 0; i < text.Count; i++)
        {

            int x = i / 9;
            int y = i % 9;
            if (gridToSolveChange[x][y] != 0)
            {
                text[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            }
        }
    }

    public void make()
    {
        List<SudokuCell> p = perfectSudoku();
        List<SudokuCell> s = puzzleGen(p);

        selectedX = -1;
        selectedY = -1;

        resetFont();
        printSudoku(gridToSolveChange);
        resetSquareColor();

        fontBold();

    }
 
    void printSudoku(List<SudokuCell> sudoku)
    {
         for (int i = 0; i < sudoku.Count; i++)
        {
            string t = sudoku[i].getAnswer().ToString();
            setText(t, i);
        }
    }




    void checkWin()
    {
        if (checkGridWin(gridToSolveChange))
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    colorSquare(i, j, "win");

                }
            }
        }
        else
        {

        }
    }

    public void pickNum(int i)
    {

        if (gridToSolve != null && gridToSolve[selectedX][selectedY] == 0)
        {
            if (selectedX > -1 && selectedY > -1 && selectedX < 10 && selectedY < 10)
            {
                gridToSolveChange[selectedX][selectedY] = i;

                printSudoku(gridToSolveChange);


                colorNearErrorAll(selectedX, selectedY);


                checkWin();
            }
        }

    }


    List<List<int>> getNear(int x, int y)
    {
        List<List<int>> l = new List<List<int>>();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                bool add = false;

                if (i == x || j == y)
                {
                    add = true;
                }

                int iRow = i - i % 3;
                int iCol = j - j % 3;

                int xRow = x - x % 3;
                int yCol = y - y % 3;

                if (iRow == xRow && iCol == yCol)
                {
                    add = true;
                }



                if (add)
                {
                    List<int> l2 = new List<int>();
                    l2.Add(i);
                    l2.Add(j);

                    l.Add(l2);
                }

            }
        }
        return l;
    }


    void colorNearErrorAll(int x, int y)
    {
        List<List<int>> near = getNear(x, y);

        foreach (List<int> i in near)
        {

            if (!(i[0] == x && i[1] == y))
            {
                colorSquare(i[0], i[1], "group");

                if (gridToSolveChange[i[0]][i[1]] != 0)
                { 
                    colorNearError(i[0], i[1]);
                }
            }
        }
    }


    void colorNearError(int x, int y)
    {
        if (!(isSafeG(gridToSolveChange, x, y, gridToSolveChange[x][y])))
        {
            colorSquare(x, y, "error");
        }

    }

    void colorSquare(int x, int y, string color)
    {

        Color32 c0 = new Color32();
        c0.a = 255;
        switch (color)
        {
            case "error":
                c0.r = 255;
                c0.g = 94;
                c0.b = 94;
                break;
            case "select":
                c0.r = 66;
                c0.g = 209;
                c0.b = 245;
                break;
            case "group":
                c0.r = 190;
                c0.g = 207;
                c0.b = 212;
                break;
            case "win":
                c0.r = 143;
                c0.g = 255;
                c0.b = 94;
                break;
            default:
                c0.r = 255;
                c0.g = 255;
                c0.b = 255;

                break;
        }

        text[(x * 9) + y].GetComponent<Image>().color = c0;

    }

    void resetSquareColor()
    {
        for (int i = 0; i<9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                colorSquare(i,j, "");

            }
        }
    }

    public void clickSquare(int i, int j)
    {
        if (gridToSolve != null)
        { 
            selectedX = i;
            selectedY = j;
            resetSquareColor();
            colorSquare(selectedX, selectedY, "select");
            colorNearErrorAll(selectedX, selectedY);
        }
    }

    


    void setupButtons()
    {

        List<GameObject> c00 = getChildren(sudokuUI);

        List<GameObject> c0l = getChildren(c00[0]);


        int i = 0;
        foreach (GameObject c0 in c0l)
        {
            List<GameObject> c1l = getChildren(c0);
            
            int j = 0;
            foreach (GameObject c1 in c1l)
            {
                Button bc = c1.GetComponent<Button>();
                int tempInt0 = i;
                int tempInt1 = j;
                bc.onClick.AddListener(delegate { clickSquare(tempInt0, tempInt1); });
                text.Add(c1);
                saved.Add(0);

                j++;
            }

            i++;
        }


    }
        
            
            
}


    //for (int i = 0; i < correct.Count; i++)
    //{
    //Button bc = buildTemp.GetComponent<Button>();
    //int tempInt = i;
    //bc.onClick.AddListener(delegate { openMenuBuild(tempInt); });
    //}

