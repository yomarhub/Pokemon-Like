using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_Like.Model
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
