using Microsoft.EntityFrameworkCore;

namespace RateReader.Data.Factories.Interfaces
{
    public interface IConetextFactory<out TContext>
        where TContext: DbContext
    {
        TContext Create();
    }
}