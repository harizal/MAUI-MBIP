namespace AndroidCompound5.DTOs
{
    public class CardDto
    {
        public CardDto()
        {
            NoKp = "";
            Nama = "";
            Address1 = "";
            Address2 = "";
            Address3 = "";
            Message = "";

        }
        public string NoKp { get; set; }
        public string Nama { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        public bool IsSuccessRead { get; set; }
        public string Message { get; set; }
    }
}