using DAL.Interfaces;
using Domain;

namespace aiProj.Services
{
    public class ProjectService
    {
        public IProjectRepository _projectRepository;

        private readonly IConfiguration _configuration;

        public ProjectService(IProjectRepository projectRepository, IConfiguration configuration)
        {
            _projectRepository = projectRepository;
            _configuration = configuration;
        }

        public async Task<bool> CreateProject(Project project)
        {
            return await _projectRepository.Create(project);
        }

        public async Task<List<Project>> GetAllUserProjects(string name)
        {
            return _projectRepository.Select().Result.Where(x => x.UserLogin == name).ToList();
        }

        public async Task<bool> DeleteProjectByName(string name)
        {
            var proj = _projectRepository.Select().Result.First(x => x.Name == name);
            return await _projectRepository.Delete(proj);
        }
    }
}
