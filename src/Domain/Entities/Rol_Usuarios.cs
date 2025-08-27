namespace Domain.Entities
{
    public class RolUsuarios
    {
     
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;
    
    }
}