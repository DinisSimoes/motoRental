namespace motoRental.Logging
{
    public static class LogMessages
    {
        public const string MotoRegistered = "Moto com placa '{0}' cadastrada com sucesso.";
        public const string MotoAlreadyExists = "Placa '{0}' já registrada.";
        public const string MotoNotFound = "Moto '{0}' não encontrada.";
        public const string MotoHaveRental = "Moto '{0}' Moto possui registros de locação.";

        public const string DeliveryManNotFound = "Entregador não encontrado.";
        public const string DeliveryManhHaventValidCNH_A = "Entregador não encontrado.";
        
        public const string RentalNotFound = "Locação não encontrada.";
        
        public const string CNPJAlreadyExists = "CNPJ '{0}' já registrado.";
        public const string CNHAlreadyExists = "CNH '{0}' já registrada.";

        public static string writelog(string message, string variable)
        {
            return string.Format(message, variable);
        }
    }
}
