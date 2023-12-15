using AssignmentProblemWithTaboos.Algorithms;

namespace AssignmentProblemWithTaboos
{
    public static class Program
    {
        public static void Main()
        {

            var test = new double[6, 6];
            test[0, 0] = 8;
            test[0, 1] = 8;
            test[0, 2] = 8;
            test[0, 3] = 8;
            test[0, 4] = 4;
            test[0, 5] = 5;

            test[1, 0] = 9;
            test[1, 1] = 7;
            test[1, 2] = 3;
            test[1, 3] = 2;
            test[1, 4] = 6;
            test[1, 5] = 4;

            test[2, 0] = 6;
            test[2, 1] = 3;
            test[2, 2] = 3;
            test[2, 3] = 7;
            test[2, 4] = 4;
            test[2, 5] = 9;
            
            test[3, 0] = 2;
            test[3, 1] = 7;
            test[3, 2] = 7;
            test[3, 3] = 6;
            test[3, 4] = 9;
            test[3, 5] = 1;
            
            test[4, 0] = 4;
            test[4, 1] = 5;
            test[4, 2] = 3;
            test[4, 3] = 9;
            test[4, 4] = 5;
            test[4, 5] = 6;
            
            test[5, 0] = 5;
            test[5, 1] = 3;
            test[5, 2] = 3;
            test[5, 3] = 3;
            test[5, 4] = 2;
            test[5, 5] = 3;


            var taboos = new bool[6, 6];
            taboos[0, 0] = true;
            taboos[0, 1] = true;
            taboos[0, 2] = true;
            taboos[0, 3] = true;
            taboos[0, 4] = true;
            taboos[0, 5] = true;

            taboos[1, 0] = true;
            taboos[1, 1] = true;
            taboos[1, 2] = true;
            taboos[1, 3] = true;
            taboos[1, 4] = true;
            taboos[1, 5] = true;

            taboos[2, 0] = true;
            taboos[2, 1] = true;
            taboos[2, 2] = true;
            taboos[2, 3] = true;
            taboos[2, 4] = true;
            taboos[2, 5] = true;

            taboos[3, 0] = true;
            taboos[3, 1] = true;
            taboos[3, 2] = true;
            taboos[3, 3] = true;
            taboos[3, 4] = true;
            taboos[3, 5] = true;

            taboos[4, 0] = true;
            taboos[4, 1] = true;
            taboos[4, 2] = true;
            taboos[4, 3] = true;
            taboos[4, 4] = true;
            taboos[4, 5] = true;

            taboos[5, 0] = true;
            taboos[5, 1] = true;
            taboos[5, 2] = true;
            taboos[5, 3] = true;
            taboos[5, 4] = true;
            taboos[5, 5] = true;

            var res = HungarianMethod.Execute(test, taboos);

            var test2 = false;
        }
    }
}