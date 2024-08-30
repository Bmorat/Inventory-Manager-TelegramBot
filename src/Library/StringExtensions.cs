namespace Library
{
    /// <summary>
    /// Contiene un método para capitalizar strings
    /// </summary>
    public static class StringExtensions //Clase de extension para strings
    {
        /// <summary>
        /// Convierte la primera letra de la cadena en mayúscula.
        /// </summary>
        /// <param name="input">La cadena de entrada.</param>
        /// <returns>Una nueva cadena con la primera letra en mayúscula.</returns>
        /// <remarks>
        /// Este método devuelve la cadena original si está vacía o es nula.
        /// </remarks>
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}