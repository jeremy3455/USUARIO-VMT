namespace Domain.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string RolName { get; set; } = string.Empty;
        public ICollection<RolOpciones> RolOpciones { get; set; } = new List<RolOpciones>();
        public ICollection<RolUsuarios> RolUsuarios { get; set; } = new List<RolUsuarios>();
    }
}