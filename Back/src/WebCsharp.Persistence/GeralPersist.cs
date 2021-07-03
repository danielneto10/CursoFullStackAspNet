using System.Threading.Tasks;
using WebCsharp.Persistence.Contexto;
using WebCsharp.Persistence.Contratos;

namespace WebCsharp.Persistence
{
    public class GeralPersist : IGeralPersist
    {
        private readonly WebCsharpContext context;
        public GeralPersist(WebCsharpContext context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityArray) where T : class
        {
            context.RemoveRange(entityArray);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}