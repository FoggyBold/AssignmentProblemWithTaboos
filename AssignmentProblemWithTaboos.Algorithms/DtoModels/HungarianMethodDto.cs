namespace AssignmentProblemWithTaboos.Algorithms.DtoModels
{
    public sealed record HungarianMethodDto
    {
        public double Weight { get; init; }
        public List<Appointment> Appointments { get; init; }
    }
}
