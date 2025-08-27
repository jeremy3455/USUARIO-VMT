namespace Application.DTOs
{
    public class LoginRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UsuarioCreateRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int PersonaId { get; set; }
    }

    public class UsuarioUpdateRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Status { get; set; } = "ACTIVE";
    }
    public class PersonaCreateRequest
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }

    public class PersonaUpdateRequest
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
}
