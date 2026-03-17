using AdventureWorksDominicana.Data.Context;
using AdventureWorksDominicana.Data.Models;
using Aplicada1.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AdventureWorksDominicana.Services
{
    public class PersonService : IService<Person, int>
    {
        private readonly Contexto _contexto;

        public PersonService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> Guardar(Person entidad)
        {
            entidad.ModifiedDate = DateTime.UtcNow;

            if (entidad.BusinessEntityId == 0)
            {
                entidad.Rowguid = Guid.NewGuid();
                _contexto.People.Add(entidad);
            }
            else
            {
                _contexto.People.Update(entidad);
            }

            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<Person?> Buscar(int id)
        {
            return await _contexto.People.FindAsync(id);
        }

        public async Task<bool> Eliminar(int id)
        {
            var entidad = await _contexto.People.FindAsync(id);
            if (entidad == null)
            {
                return false;
            }

            _contexto.People.Remove(entidad);
            return await _contexto.SaveChangesAsync() > 0;
        }

        public async Task<List<Person>> GetList(Expression<Func<Person, bool>> criterio)
        {
            return await _contexto.People
                .Where(criterio)
                .ToListAsync();
        }
    }
}