namespace DAL.Abstract.Entitys.Authentication
{
    public class User : EntityBase
    {
        public string Login { get; set; }
        public string Password { get; set; }

        //public int? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
