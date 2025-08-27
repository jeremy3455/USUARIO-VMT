namespace Application.Utilities
{
    public static class EmailGenerator
    {
        public static string GenerateEmail(string nombres, string apellidos, List<string> existingEmails)
        {
            string primerNombre = nombres.Split(' ')[0].ToLower();
            string primerApellido = apellidos.Split(' ')[0].ToLower();
            
            string baseEmail = $"{primerNombre[0]}{primerApellido}";
            string finalEmail = baseEmail;
            
            int counter = 1;
            while (existingEmails.Contains($"{finalEmail}@mail.com"))
            {
                finalEmail = $"{baseEmail}{counter}";
                counter++;
            }
            
            return $"{finalEmail}@mail.com";
        }
    }
}