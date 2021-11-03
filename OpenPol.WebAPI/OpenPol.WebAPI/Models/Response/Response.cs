namespace OpenPol.WebAPI.Models.Response
{
    public class Response
    {
        public string Mensaje { get; set; }
        public int Success { get; set; }
        public UserResponse Data { get; set; }
    }
}
