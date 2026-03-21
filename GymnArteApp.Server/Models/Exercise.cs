namespace GymnArteApp.Server.Models
{
    public class Exercise
    {
        //id; nome; descricao; exercisetype; dtcriacao; dtmodificacao; usermodificacao nullable;
        public int ExerciseId { get; set; }
        public string Name { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public ExerciseType ExerciseType { get; set; } = null!;
    }
}
