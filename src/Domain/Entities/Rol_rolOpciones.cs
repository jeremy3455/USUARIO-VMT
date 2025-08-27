namespace Domain.Entities
{
    public class RolrolOpciones
    {

        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;
        public int OpcionId { get; set; }
        public RolOpciones RelOpc { get; set; } = null!;
    
    }
}