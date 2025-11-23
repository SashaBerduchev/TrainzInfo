namespace TrainzInfoWebGW.DTO
{
    public class UserDto
    {
        public string Id { get; set; }           // Ідентифікатор користувача
        public string UserName { get; set; }     // Логін / ім'я користувача
        public string Email { get; set; }        // Email
        public string Role { get; set; }         // Роль користувача (Admin, Superadmin, Publisher)
        public bool IsAuthenticated { get; set; } = false; // Чи авторизований
    }
}
