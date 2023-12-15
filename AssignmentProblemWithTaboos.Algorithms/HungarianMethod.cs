using AssignmentProblemWithTaboos.Algorithms.DtoModels;

namespace AssignmentProblemWithTaboos.Algorithms
{
    public static class HungarianMethod
    {
        public static List<List<HungarianMethodDto>> Execute(double[,] matrix, bool[,] taboos)
        {
            //Проверка на совместимось
            var compatibilityCheck = CompatibilityCheck(matrix, taboos);
            if (!compatibilityCheck)
                throw new ArgumentException("Задача не совместна!");

            //Применение запретов
            ApplicationTaboos(matrix, taboos);
            //Редукция по строкам
            ReductionByLine(matrix);
            //Редукция по столбцам
            ReductionByColumn(matrix);
            var matching = FindingMaxMatchingInBipartiteGraphAlgorithm.Execute(matrix);
            while (matching.Max(item => item.Count) != matrix.GetLength(0)) 
            {
                var maxMatching = matching.First(m => m.Count == matching.Max(item => item.Count));
                if (maxMatching.Count != matrix.GetLength(0))
                {
                    ApplyHungarianMethod(matrix, maxMatching);
                }

                matching = FindingMaxMatchingInBipartiteGraphAlgorithm.Execute(matrix);
            }
            
            List<List<HungarianMethodDto>> result = matching
                .Where(item => item.Count == matrix.GetLength(0))
                .Select(item => item.Select(i => new HungarianMethodDto
                    {
                        Row = i.Start,
                        Column = i.End
                    }).ToList())
                .ToList();

            return result;
        }

        private static void ApplyHungarianMethod(double[,] matrix, List<StepDto> matchings)
        {
            List<int> selectedRows = new();
            List<int> selectedColumns = new();

            var selectedRow = -1;
            var selectedColumn = -1;
            for (var i = 0; i < matrix.GetLength(0) && selectedRow == -1; i++)
            {
                for (var j = 0; j < matrix.GetLength(1) && selectedRow == -1; j++)
                {
                    if(matrix[i, j] == 0 && !matchings.Any(item => item.Start == i && item.End == j))
                    {
                        selectedRow = i;
                        selectedColumn = j;
                    }
                }
            }

            if (selectedRow == -1 && selectedColumn == -1)
                return;

            selectedRows.Add(selectedRow);

            while (selectedColumn != -1)
            {
                selectedColumns.Add(selectedColumn);

                selectedRow = -1;

                for (var i = 0; i < matrix.GetLength(0) && selectedRow == -1; i++)
                {
                    if (matrix[i, selectedColumn] == 0 && matchings.Any(item => item.Start == i && item.End == selectedColumn))
                    {
                        selectedRow = i;
                    }
                }

                selectedColumn = -1;

                if (selectedRow != -1)
                {
                    selectedRows.Add(selectedRow);

                    for (var j = 0; j < matrix.GetLength(0) && selectedColumn == -1; j++)
                    {
                        if (matrix[selectedRow, j] == 0 && !matchings.Any(item => item.Start == selectedRow && item.End == j) && !selectedColumns.Contains(j))
                        {
                            selectedColumn = j;
                        }
                    }
                }
            }

            List<double> notSelectedItems = new();
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                if (selectedRows.Contains(i))
                {
                    for (var j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (!selectedColumns.Contains(j))
                        {
                            notSelectedItems.Add(matrix[i, j]);
                        }
                    }
                }
            }

            double minInNotSelectedItems = notSelectedItems.Min();

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if(!(selectedRows.Contains(i) && !selectedColumns.Contains(j) || !selectedRows.Contains(i) && selectedColumns.Contains(j)))
                    {
                        if(selectedRows.Contains(i) && selectedColumns.Contains(j))
                        {
                            matrix[i, j] += minInNotSelectedItems;
                        }
                        else
                        {
                            matrix[i, j] -= minInNotSelectedItems;
                        }
                    }
                }
            }
        }

        private static List<int> SearchLinesWithoutIndependentZeros(double[,] matrix, List<StepDto> matchings)
        {
            List<int> res = new();

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                bool hasIndependentZeros = false;
                for (var j = 0; j < matrix.GetLength(1) && !hasIndependentZeros; j++)
                {
                    if(matchings.Any(item => item.Start == i && item.End == j))
                        hasIndependentZeros = true;
                }
            }

            return res;
        }

        private static void ReductionByLine(double[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                var min = FindMinOnLine(matrix, i);
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] -= min;
                }
            }
        }

        private static void ReductionByColumn(double[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(1); i++)
            {
                var min = FindMinOnColumn(matrix, i);
                for (var j = 0; j < matrix.GetLength(0); j++)
                {
                    matrix[j, i] -= min;
                }
            }
        }

        private static double FindMinOnLine(double[,] matrix, int lineIndex)
        {
            var min = matrix[lineIndex, 0];
            for (var i = 1; i < matrix.GetLength(1); i++)
            {
                min = min > matrix[lineIndex, i] ? matrix[lineIndex, i] : min;
            }

            return min;
        }

        private static double FindMinOnColumn(double[,] matrix, int columnIndex)
        {
            var min = matrix[0, columnIndex];
            for (var i = 1; i < matrix.GetLength(0); i++)
            {
                min = min > matrix[i, columnIndex] ? matrix[i, columnIndex] : min;
            }

            return min;
        }

        private static double FindMaxInMatrix(double[,] matrix)
        {
            var max = matrix[0, 0];
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    max = max < matrix[i, j] ? matrix[j, i] : max;
                }
            }

            return max;
        }

        private static void ApplicationTaboos(double[,] matrix, bool[,] taboos)
        {
            var M = FindMaxInMatrix(matrix) * Math.Max(matrix.GetLength(0), matrix.GetLength(1)) + 1;

            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (!taboos[i, j])
                        matrix[i, j] = M;
                }
            }
        }

        private static bool CompatibilityCheck(double[,] matrix, bool[,] taboos)
        {
            double[,] taboosMatrix = new double[taboos.GetLength(0), taboos.GetLength(1)];
            for (var i = 0; i < taboosMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < taboosMatrix.GetLength(1); j++)
                {
                    taboosMatrix[1, j] = taboos[i, j] ? 0 : 1;
                }
            }

            var matching = FindingMaxMatchingInBipartiteGraphAlgorithm.Execute(taboosMatrix);

            return matching.Any(item => item.Count >= matrix.GetLength(1));
        }
    }
}
