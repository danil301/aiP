using aiProj.DAL;
using DAL.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        public ApplicationDbContext _db;

        public ProjectRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Project entity)
        {
            _db.Projects.Add(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(Project entity)
        {
            _db.Projects.Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<Project> Get(int id)
        {
            return await _db.Projects.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Project>> Select()
        {
            return await _db.Projects.ToListAsync();
        }

        public async Task<bool> Update(Project entity)
        {
            _db.Projects.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
