namespace Domain.Entities
{
    public class Sesion
    {
        public int IdSesion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int UsuarioId { get; set; }
        
        public Usuario Usuario { get; set; } = null!;
    }
}