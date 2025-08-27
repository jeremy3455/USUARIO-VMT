namespace Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string SessionActive { get; set; } = "N";
        public int PersonaId { get; set; }
        public string Status { get; set; } = "ACTIVE";
        public int IntentosLogin { get; set; } = 0;
        public DateTime? FechaBloqueo { get; set; }
        public bool IsDeleted { get; set; } = false;


        public Persona Persona { get; set; } = null!;
        
        public ICollection<RolUsuarios> RolesUsuarios { get; set; } = new List<RolUsuarios>();

    }
}