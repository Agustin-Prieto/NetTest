/*
Teniendo en cuenta la librería ICache, que fue escrita e implementada por otro equipo y utiliza una cache del tipo Key Value,
tomar la clase CajaRepository y modificar los métodos AddAsync, GetAllAsync, GetAllBySucursalAsync y GetOneAsync para que utilicen cache.

Datos:
    * Existen en la empresa 20 sucursales
    * Como mucho hay 100 cajas en la base

Restricción:    
	* Solo es posible utilizar 1 key (IMPORTANTE)
	
Aclaración:
	* No realizar una implementación de ICache, otro equipo la esta brindando
*/

public interface ICache
{
    Task AddAsync<T>(string key, T obj, int? durationInMinutes);
    Task<T> GetOrDefaultAsync<T>(string? key);
    Task RemoveAsync(string key);
}

public class CajaRepository
{
    private readonly DataContext _db;
    private readonly ICache _cache;

    public CajaRepository(DataContext db, ICache cache)
    {
        _db = db ?? throw new ArgumentNullException(nameof(DataContext);
        _cache = cache;
    }

    public async Task AddAsync(Entities.Caja caja)
    {
        var guid = new Guid.NewGuid();
        var key = guid.ToString("N");

        await _cache.AddAsync(key, caja);
    }

    public async Task<IEnumerable<Entities.Caja>> GetAllAsync()
    {
        return await _cache.GetOrDefaultAsync().ToListAsync();
    }

    public async Task<IEnumerable<Entities.Caja>> GetAllBySucursalAsync(int sucursalId)
    {
        return await _cache.GetOrDefaultAsync()
            .Where(x => x.SucursalId == sucursalId)
            .ToListAsync()
    }

    public async Task<Entities.Caja> GetOneAsync(Guid id)
    {
        return await _cache.GetOrDefaultAsync(id.ToString("N"));
    }
}