using Pokemon_Like.Model;

namespace Pokemon_Like.Functions
{
    public class Init
    {
        ExerciceMonsterContext? _context;
        public Init()
        {
            InitDB();
        }

        public void InitDB()
        {
            _context = new ExerciceMonsterContext();
            _context.Database.EnsureCreated();
            _context.Logins.Add(new Login { Username = "admin", PasswordHash = "admin" });
            _context.SaveChanges();
        }
    }
}
