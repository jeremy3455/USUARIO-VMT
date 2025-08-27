using System.Text.RegularExpressions;

namespace Application.Utilities
{
    public class Validador
    {
        public static (bool esValido, string mensaje) ValidarUsername(string username)
        {
            if (username.Length < 8 || username.Length > 20)
            {
                return (false, "El nombre de usuario debe tener entre 8 y 20 caracteres");
            }
            if (Regex.IsMatch(username, @"[^a-zA-Z0-9]"))
            {
                return (false, "El nombre de usuario no puede contener signos");
            }

            if (!Regex.IsMatch(username, @"[A-Z]"))
            {
                return (false, "El nombre de usuario debe contener al menos una letra mayúscula");
            }
            if (!Regex.IsMatch(username, @"\d"))
            {
                return (false, "El nombre de usuario debe contener al menos un número");
            }

            return (true, "Válido");
        }

        public static (bool esValida, string mensaje) ValidarPassword(string password)
        {
            if (password.Length < 8)
            {
                return (false, "La contraseña debe tener al menos 8 caracteres");
            }
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return (false, "La contraseña debe contener al menos una letra mayúscula");
            }
            if (password.Contains(" "))
            {
                return (false, "La contraseña no puede contener espacios");
            }
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            {
                return (false, "La contraseña debe contener al menos un signo");
            }

            return (true, "Válida");
        }
    }
}
