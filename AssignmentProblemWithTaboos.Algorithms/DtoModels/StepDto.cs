namespace AssignmentProblemWithTaboos.Algorithms.DtoModels
{
    public sealed record StepDto
    {
        public int Start { get; init; }
        public int End { get; init; }
        public bool Direction { get; init; }
    }
}
