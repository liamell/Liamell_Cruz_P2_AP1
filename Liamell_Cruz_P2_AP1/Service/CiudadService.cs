using Liamell_Cruz_P2_AP1.DAL;
using Liamell_Cruz_P2_AP1.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Liamell_Cruz_P2_AP1.Service;

public class CiudadService(IDbContextFactory<Contexto> DbFactory)
{

    public async Task<bool> Guardar(Ciudades ciudad)
    {
        if (!await Existe(ciudad.CiudadId))
            return await Insertar(ciudad);
        else
            return await Modificar(ciudad);
    }

    private async Task<bool> Existe(int ciudadId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ciudades.AnyAsync(c => c.CiudadId == ciudadId);
    }

    private async Task<bool> Insertar(Ciudades ciudad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Ciudades.Add(ciudad);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Ciudades ciudad)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Entry(ciudad).State = EntityState.Modified;
        return await contexto.SaveChangesAsync() > 0;
    }

  
    public async Task<bool> Eliminar(int ciudadId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var ciudad = await contexto.Ciudades.FindAsync(ciudadId);
        if (ciudad == null)
            return false;

        contexto.Ciudades.Remove(ciudad);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Ciudades?> Buscar(int ciudadId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync(); 
        return await contexto.Ciudades.FindAsync(ciudadId);
    }

    public async Task<List<Ciudades>> Listar(Expression<Func<Ciudades, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Ciudades
            .Where(criterio)
            .ToListAsync();
    }
}
    
