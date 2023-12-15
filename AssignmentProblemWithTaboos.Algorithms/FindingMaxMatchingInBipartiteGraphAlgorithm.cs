using AssignmentProblemWithTaboos.Algorithms.DtoModels;

namespace AssignmentProblemWithTaboos.Algorithms
{
    public static class FindingMaxMatchingInBipartiteGraphAlgorithm
    {
        public static List<List<StepDto>> Execute(double[,] matrix)
        {
            PrepareData(matrix, out var t, out var lr);

            List<List<StepDto>> globalResult = new();
            SearchCouples(lr, t, 0, new List<StepDto>(), globalResult);

            return globalResult;
        }

        private static List<StepDto> SearchCouples(sbyte[,] lr, bool[] t, int currPoint, List<StepDto> steps, List<List<StepDto>> result)
        {
            if (currPoint != lr.GetLength(0))
            {
                List<List<StepDto>> calculatedPaths = new();

                for (var j = 0; j < lr.GetLength(1); j++)
                {
                    List<StepDto> calculatedPath = new();
                    if (CheckCouples(CopyLR(lr), CopyT(t), currPoint, j, calculatedPath, true, true))
                    {
                        if (currPoint != 0)
                            calculatedPath.AddRange(steps);

                        calculatedPaths.Add(calculatedPath);
                    }
                }

                foreach (List<StepDto> path in calculatedPaths)
                {
                    sbyte[,] newLR = PrepareLR(lr, path);
                    bool[] newT = PrepareT(t, path);

                    List<StepDto> newPath = SearchCouples(newLR, newT, currPoint + 1, path, result);
                    newPath = newPath.Where(item => newPath.Where(c => c.Start == item.Start && c.End == item.End).Count() == 1).ToList();
                    if (!result.Any(item => newPath.All(np => item.Any(i => i.Start == np.Start && i.End == np.End))) && (!result.Any() || !result.Any(item => item.Count > newPath.Count)))
                        result.Add(newPath);
                }
            }

            return steps;
        }

        private static bool CheckCouples(sbyte[,] lr, bool[] t, int currPointLeft, int currPointRight, List<StepDto> result, bool straightWay, bool isStart, int count = 0)
        {
            if (count == lr.GetLength(0) + lr.GetLength(1))
                return false;

            count++;
            if (straightWay)
            {
                if (isStart)
                {
                    if (lr[currPointLeft, currPointRight] == 1)
                    {
                        if (t[currPointRight])
                        {
                            result.Add(new StepDto
                            {
                                Start = currPointLeft,
                                End = currPointRight,
                                Direction = true
                            });
                            lr[currPointLeft, currPointRight] = -1;
                            t[currPointRight] = false;
                            return true;
                        }
                        else
                        {
                            if (CheckCouples(lr, t, currPointRight, -1, result, false, false, count))
                            {
                                result.Add(new StepDto
                                {
                                    Start = currPointLeft,
                                    End = currPointRight,
                                    Direction = true
                                });
                                lr[currPointLeft, currPointRight] = -1;
                                return true;
                            }
                        }
                    }
                }
                else
                {

                    for (var i = 0; i < lr.GetLength(1); i++)
                    {
                        if (lr[currPointLeft, i] == 1)
                        {
                            if (t[i])
                            {
                                result.Add(new StepDto
                                {
                                    Start = currPointLeft,
                                    End = i,
                                    Direction = true
                                });
                                lr[currPointLeft, i] = -1;
                                t[i] = false;
                                return true;
                            }
                            else
                            {
                                if (CheckCouples(lr, t, i, -1, result, false, false, count))
                                {
                                    result.Add(new StepDto
                                    {
                                        Start = currPointLeft,
                                        End = i,
                                        Direction = true
                                    });
                                    lr[currPointLeft, i] = -1;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < lr.GetLength(0); i++)
                {
                    if (lr[i, currPointLeft] == -1)
                    {
                        if (CheckCouples(lr, t, i, -1, result, true, false, count))
                        {
                            result.Add(new StepDto
                            {
                                Start = i,
                                End = currPointLeft,
                                Direction = false
                            });
                            lr[i, currPointLeft] = 1;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        static private sbyte[,] PrepareLR(sbyte[,] lr, List<StepDto> steps)
        {
            sbyte[,] newLR = CopyLR(lr);

            foreach (var step in steps)
            {
                if (step.Direction)
                {
                    newLR[step.Start, step.End] = -1;
                }
                else
                {
                    newLR[step.Start, step.End] = 1;
                }
            }

            return newLR;
        }

        static private bool[] PrepareT(bool[] t, List<StepDto> steps)
        {
            bool[] newT = new bool[t.Length];
            for (var i = 0; i < t.Length; i++)
            {
                newT[i] = t[i];
            }

            foreach (var step in steps)
            {
                if (step.Direction)
                {
                    newT[step.End] = false;
                }
            }

            return newT;
        }

        static private sbyte[,] CopyLR(sbyte[,] lr)
        {
            sbyte[,] newLR = new sbyte[lr.GetLength(0), lr.GetLength(1)];
            for (var i = 0; i < lr.GetLength(0); i++)
            {
                for (var j = 0; j < lr.GetLength(1); j++)
                {
                    newLR[i, j] = lr[i, j];
                }
            }
            return newLR;
        }

        static private bool[] CopyT(bool[] t)
        {
            bool[] newT = new bool[t.Length];
            for (var i = 0; i < t.Length; i++)
            {
                newT[i] = t[i];
            }
            return newT;
        }

        private static void PrepareData(double[,] matrix, out bool[] t, out sbyte[,] lr)
        {
            t = new bool[matrix.GetLength(1)];
            for (var i = 0; i < t.Length; i++)
            {
                t[i] = true;
            }

            lr = new sbyte[matrix.GetLength(0), matrix.GetLength(1)];
            for (var i = 0; i < lr.GetLength(0); i++)
            {
                for (var j = 0; j < lr.GetLength(1); j++)
                {
                    lr[i, j] = (sbyte)(matrix[i, j] == 0 ? 1 : 0);
                }
            }
        }
    }
}
