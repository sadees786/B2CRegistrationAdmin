using Microsoft.EntityFrameworkCore;


namespace RegistrationAdmin.DBContext
{
    public class RegistrationAdminLogsContext : DbContext
    {
     
       public RegistrationAdminLogsContext(DbContextOptions<RegistrationAdminLogsContext> options)
          : base(options)
        {
        }


    }
}
