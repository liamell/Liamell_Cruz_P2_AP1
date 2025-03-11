using Liamell_Cruz_P2_AP1.DAL;
using Liamell_Cruz_P2_AP1.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Liamell_Cruz_P2_AP1.Service;

public class EncuestaService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Guardar(Encuesta encuesta)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        if (!await Existe(encuesta.EncuestaId))
            return await Insertar(encuesta);
        else
            return await Modificar(encuesta);
    }

    private async Task<bool> Existe(int EncuestaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Encuesta.AnyAsync(E => E.EncuestaId == EncuestaId);
    }


    private async Task<bool> Insertar(Encuesta encuestas)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        await AfectarCiudad(encuestas.encuestaDetalle.ToArray(), true);
        contexto.Encuesta.Add(encuestas);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task AfectarCiudad(EncuestaDetalle[] detalles, bool resta = true)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        foreach (var detalle in detalles)
        {
            var ciudad = await contexto.Ciudades.SingleOrDefaultAsync(c => c.CiudadId == detalle.CiudadId);
            if (ciudad != null)
            {
                if (resta)
                    ciudad.Monto -= detalle.Monto;
                else
                    ciudad.Monto += detalle.Monto;
            }
        }

        await contexto.SaveChangesAsync();
    }


    private async Task<bool> Modificar(Encuesta encuesta)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var encuestaOriginal = await contexto.Encuesta
            .Include(e => e.encuestaDetalle)
            .FirstOrDefaultAsync(e => e.EncuestaId == encuesta.EncuestaId);

        if (encuestaOriginal == null)
            return false;

        await AfectarCiudad(encuestaOriginal.encuestaDetalle.ToArray(), false);

        foreach (var detalleOriginal in encuestaOriginal.encuestaDetalle)
        {
            if (!encuesta.encuestaDetalle.Any(d => d.DetalleId == detalleOriginal.DetalleId))
            {
                contexto.EncuestaDetalle.Remove(detalleOriginal);
            }
        }

        await AfectarCiudad(encuesta.encuestaDetalle.ToArray(), true);

        contexto.Entry(encuestaOriginal).CurrentValues.SetValues(encuesta);

        foreach (var detalle in encuesta.encuestaDetalle)
        {
            var detalleExistente = encuestaOriginal.encuestaDetalle
                .FirstOrDefault(d => d.DetalleId == detalle.DetalleId);

            if (detalleExistente != null)
            {
                contexto.Entry(detalleExistente).CurrentValues.SetValues(detalle);
            }
            else
            {
                encuestaOriginal.encuestaDetalle.Add(detalle);
            }
        }

        return await contexto.SaveChangesAsync() > 0;
    }




    public async Task<bool> Eliminar(int EncuestaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var encuesta = await contexto.Encuesta
            .Include(e => e.encuestaDetalle)
            .FirstOrDefaultAsync(e => e.EncuestaId == EncuestaId);

        if (encuesta == null)
            return false;

        await AfectarCiudad(encuesta.encuestaDetalle.ToArray(), false); // Revertir cambios en ciudades

        contexto.EncuestaDetalle.RemoveRange(encuesta.encuestaDetalle);
        contexto.Encuesta.Remove(encuesta);

        var cantidad = await contexto.SaveChangesAsync();
        return cantidad > 0;
    }


    public async Task<Encuesta> Buscar(int EncuestaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Encuesta
            .Include(e => e.encuestaDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EncuestaId == EncuestaId);
    }

    public async Task<Encuesta> BuscarConDetalle(int EncuestaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Encuesta
            .Include(e => e.encuestaDetalle)
            .ThenInclude(d => d.Ciudades)
            .FirstOrDefaultAsync(e => e.EncuestaId == EncuestaId);
    }


    public async Task<List<Encuesta>> Listar(Expression<Func<Encuesta, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Encuesta
            .Include(e => e.encuestaDetalle)
            .ThenInclude(d => d.Ciudades)
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();
    }
}
