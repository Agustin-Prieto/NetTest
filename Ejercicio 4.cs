/*
A partir de las clases CajaRepository y SucursalRepository, crear la clase BaseRepository<T> 
que unifique los métodos GetAllAsync y GetOneAsync
Crear un abstract BaseEntity que defina la property Id y luego modificar las entities Caja y Sucursal para que hereden de BaseEntity 
Aclaración: Se deben respetar la interfaces. 
*/

namespace Domain.Entities
{
    public abstract class BaseEntity<T>
    {
        public abstract T Id { get; }
    }

    public class Caja : BaseEntity
    {
        public override Guid Id { get; }
        public int SucursalId { get; }
        public string Descripcion { get; }
        public int TipoCajaId { get; }

        public Caja(Guid id, int sucursalId, string descripcion, int tipoCajaId)
        {
            Id = id;
            SucursalId = sucursalId;   
            Descripcion = descripcion;
            TipoCajaId = tipoCajaId;
        }
    }

    public class Sucursal : BaseEntity
    {
        public override int Id { get; }
        public string Direccion { get; }
        public string Telefono { get; }

        public Sucursal(int id, string direccion, string telefono)
        {
            Id = id;
            Direccion = direccion;
            Telefono = telefono;
        }
    }
}

namespace Infrastructure.Data.Repositories
{
	public interface ICajaRepository 
	{
		Task<IEnumerable<Caja>> GetAllAsync();
		Task<Caja> GetOneAsync(Guid id);
	}
	
	public interface ISucursalRepository
	{
		Task<IEnumerable<Sucursal>> GetAllAsync();
		Task<Sucursal> GetOneAsync(int id);
	}
	
    public abstract class BaseRepository<T,TKey>
    {
        public abstract Task<IEnumerable<TValue>> GetAllAsync();
        public abstract Task<TValue> GetOneAsync<TKey>(TKey key);
    }

    public class CajaRepository : BaseRepository<ICajaRepository, Guid>
    {
        private readonly DataContext _db;

        public CajaRepository(DataContext db, ICajaRepository cajaRepository)
            : base(cajaRepository)
            => _db = db;

        public override async Task<IEnumerable<Caja>> GetAllAsync()
            => await _db.Cajas.ToListAsync();

        public override async Task<Caja> GetOneAsync(Guid id)
            => await _db.Cajas.FirstOrDefaultAsync(x => x.Id == id);
    }

    public class SucursalRepository : BaseRepository<ISucursalRepository, int>
    {
        private readonly DataContext _db;

        public CajaRepository(DataContext db, ISucursalRepository sucursalRepository)
            : base(sucursalRepository)
            => _db = db;

        public override async Task<IEnumerable<Sucursal>> GetAllAsync()
            => await _db.Sucursales.ToListAsync();

        public override async Task<Sucursal> GetOneAsync(int id)
            => await _db.Sucursales.FirstOrDefaultAsync(x => x.Id == id);
    }
}