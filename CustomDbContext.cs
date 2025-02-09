using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

public class CustomDbContext : DbContext
{
    public override int SaveChanges()
    {
        BeforeSave();
        var result = base.SaveChanges();
        AfterSave();
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        BeforeSave();
        var result = await base.SaveChangesAsync(cancellationToken);
        AfterSave();
        return result;
    }

    private void BeforeSave()
    {
        // Add your custom logic here
    }

    private void AfterSave()
    {
        // Add your custom logic here
    }
}
