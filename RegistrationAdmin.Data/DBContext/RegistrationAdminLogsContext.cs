using Microsoft.EntityFrameworkCore;

namespace RegistrationAdmin.Data.DBContext
{
    public class RegistrationAdminLogsContext : DbContext
    {     
       public RegistrationAdminLogsContext(DbContextOptions<RegistrationAdminLogsContext> options)
          : base(options)
        {
        }
    }
}
