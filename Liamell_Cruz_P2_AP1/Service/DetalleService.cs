using Liamell_Cruz_P2_AP1.DAL;
using Liamell_Cruz_P2_AP1.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Liamell_Cruz_P2_AP1.Service;

public class DetalleService(IDbContextFactory<Contexto> DbFactory)
{
    
        public async Task<List<Ciudades>> Listar(Expression<Func<Ciudades, bool>> criterio)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Ciudades
                .AsNoTracking()
                .Where(criterio)
                .ToListAsync();
        }

        public async Task<bool> Eliminar(int detalleId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var detalle = await contexto.EncuestaDetalle
                .Include(d => d.Encuesta)
                .FirstOrDefaultAsync(d => d.DetalleId == detalleId);

            if (detalle != null)
            {
                await AfectarMonto(detalle, false);
                contexto.EncuestaDetalle.Remove(detalle);
                await contexto.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task AfectarMonto(EncuestaDetalle detalle, bool Suma)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var encuesta = await contexto.Encuesta
                .FirstOrDefaultAsync(e => e.EncuestaId == detalle.EncuestaId);

            if (encuesta != null)
            {
                if (Suma)
                {
                    encuesta.Monto -= detalle.Monto;
                }
                else
                {
                    encuesta.Monto += detalle.Monto;
                }

                await contexto.SaveChangesAsync();
            }
        }

}
