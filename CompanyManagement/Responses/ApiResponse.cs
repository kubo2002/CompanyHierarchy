namespace CompanyManagement.Api.Responses
{
    /// <summary>
    /// Jednotna "obalka" pre API odpovede.
    /// Sluzi na standardizaciu odpovedi vratenych z REST API.
    /// </summary>
    /// <typeparam name="T">
    /// Typ dat, ktore API vracia (DTO, kolekcia, alebo null pri chybe).
    /// </typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indikuje, ci bola poziadavka spracovana uspesne.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Sprava urcena pre klienta (informacna alebo chybova).
        /// </summary>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Vlastne data odpovede.
        /// Pri neuspesnej odpovedi moze byt null.
        /// </summary>
        public T? Data { get; init; }

        /// <summary>
        /// Vytvori uspesnu API odpoved s datami a spravou.
        /// </summary>
        /// <param name="data">Data, ktore sa vratia klientovi.</param>
        /// <param name="message">Volitelna informacna sprava.</param>
        /// <returns>Uspesna API odpoved.</returns>
        public static ApiResponse<T> Ok(T data, string message) => new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };

        /// <summary>
        /// Vytvori neuspesnu API odpoved s datami a spravou.
        /// </summary>
        /// <param name="data">Data, ktore sa vratia klientovi.</param>
        /// <param name="message">Volitelna informacna sprava.</param>
        /// <returns>neuspesna API odpoved.</returns>
        public static ApiResponse<T> Fail(string message) => new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}
