namespace Api_para_login.Dados
{
    public class Data
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Valido { get; set; }
        public string? ConfirmToken { get; set; }
    }
}
