namespace GymnArteApp.Server.Models
{
    public class ExerciseType
    {
        //id; nome; descricao; dtcriacao;
        public int ExerciseTypeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public User? UpdatedUser { get; set; }
    }
}
