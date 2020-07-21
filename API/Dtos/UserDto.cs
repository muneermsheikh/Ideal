namespace API.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public int EmployeeId {get; set; }
        public string UserType {get; set; }
    }
}