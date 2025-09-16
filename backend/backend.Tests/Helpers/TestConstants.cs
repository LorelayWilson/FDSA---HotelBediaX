namespace backend.Tests.Helpers
{
    /// <summary>
    /// Constantes utilizadas en los tests para evitar números mágicos
    /// </summary>
    public static class TestConstants
    {
        /// <summary>
        /// ID de destino inexistente utilizado en tests de casos negativos
        /// </summary>
        public const int NonExistentDestinationId = 999;
        
        /// <summary>
        /// ID de destino válido utilizado en tests de casos positivos
        /// </summary>
        public const int ValidDestinationId = 1;
        
        /// <summary>
        /// Tiempo de tolerancia para comparaciones de DateTime en tests (5 segundos)
        /// </summary>
        public static readonly TimeSpan DateTimeTolerance = TimeSpan.FromSeconds(5);
    }
}
