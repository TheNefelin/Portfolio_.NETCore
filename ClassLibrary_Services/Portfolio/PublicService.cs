using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using Microsoft.Extensions.Logging;

namespace ClassLibrary_Services.Portfolio
{
    public class PublicService : IPublicService
    {
        private readonly ILogger<PublicService> _logger;

        private readonly IBaseCRUDService<UrlGrpDTO> _urlGrpService;
        private readonly IBaseCRUDService<UrlDTO> _urlService;

        private readonly IBaseCRUDService<ProjectDTO> _projectService;
        private readonly IBaseCRUDService<LanguageDTO> _languageService;
        private readonly IBaseCRUDService<TechnologyDTO> _technologyService;
        private readonly ISimpleCRUDService<ProjectLanguageDTO> _ProLangService;
        private readonly ISimpleCRUDService<ProjectTechnologyDTO> _ProTechService;

        public PublicService(
            ILogger<PublicService> logger,
            IBaseCRUDService<UrlGrpDTO> urlGrpService,
            IBaseCRUDService<UrlDTO> urlService,
            IBaseCRUDService<ProjectDTO> projectService,
            IBaseCRUDService<LanguageDTO> languageService,
            IBaseCRUDService<TechnologyDTO> technologyService,
            ISimpleCRUDService<ProjectLanguageDTO> ProLangService,
            ISimpleCRUDService<ProjectTechnologyDTO> ProTechService)
        {
            _logger = logger;
            _urlGrpService = urlGrpService;
            _urlService = urlService;
            _projectService = projectService;
            _languageService = languageService;
            _technologyService = technologyService;
            _ProLangService = ProLangService;
            _ProTechService = ProTechService;
        }

        public async Task<ResponseApiDTO<IEnumerable<ProjectsDTO>>> GetAllProjectsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var projectTask = _projectService.GetAllAsync(cancellationToken);
                var languageTask = _languageService.GetAllAsync(cancellationToken);
                var technologyTask = _technologyService.GetAllAsync(cancellationToken);
                var plTask = _ProLangService.GetAllAsync(cancellationToken);
                var ptTask = _ProTechService.GetAllAsync(cancellationToken);

                await Task.WhenAll(projectTask, languageTask, technologyTask, plTask, ptTask);

                var project = await projectTask;
                var langauges = await languageTask;
                var technologies = await technologyTask;
                var pls = await plTask;
                var pts = await ptTask;

                if (project.StatusCode != 200 || langauges.StatusCode != 200 || technologies.StatusCode != 200)
                {
                    _logger.LogWarning("Conflicto al obtener los datos");
                    return new ResponseApiDTO<IEnumerable<ProjectsDTO>>
                    {
                        StatusCode = 409,
                        Message = $"Conflicto al obtener los datos"
                    };
                }

                var result = project.Data.Select(pro => new ProjectsDTO
                {
                    Id = pro.Id,
                    Name = pro.Name,
                    ImgUrl = pro.ImgUrl,
                    Languages = pls.Data.Where(pl => pl.Id_Project == pro.Id)
                                        .Select(pl => langauges.Data.FirstOrDefault(l => l.Id == pl.Id_Language) ?? new LanguageDTO())
                                        .Where(l => l != null).ToList(),
                    Technologies = pts.Data.Where(pt => pt.Id_Project == pro.Id)
                                        .Select(pt => technologies.Data.FirstOrDefault(t => t.Id == pt.Id_Technology) ?? new TechnologyDTO())
                                        .Where(t => t != null).ToList()
                }).ToList();

                return new ResponseApiDTO<IEnumerable<ProjectsDTO>>
                {
                    StatusCode = 200,
                    Message = "Ok",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la operación de base de datos en GetAllProjectsAsync.");
                return new ResponseApiDTO<IEnumerable<ProjectsDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<IEnumerable<UrlsGrpsDTO>>> GetAllUrlsGrpsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando la recuperación de grupos de URLs y URLs individuales.");
            try
            {
                var GrpsTask = _urlGrpService.GetAllAsync(cancellationToken);
                var urlsTask = _urlService.GetAllAsync(cancellationToken);

                await Task.WhenAll(GrpsTask, urlsTask);

                var grps = await GrpsTask;
                var urls = await urlsTask;

                if (grps.StatusCode != 200 || urls.StatusCode != 200)
                {
                    _logger.LogWarning("Conflicto al obtener los datos: StatusGrps={StatusGrps}, StatusCodeUrl={StatusCodeUrl}", grps.StatusCode, urls.StatusCode);
                    return new ResponseApiDTO<IEnumerable<UrlsGrpsDTO>>
                    {
                        StatusCode = 409,
                        Message = $"Conflicto: StatusGrps = {grps.StatusCode}, MessageGrps = {grps.Message}, StatusCodeUrl = {urls.StatusCode}, MessageUrl = {urls.Message}"
                    };
                }

                var result = grps.Data.Select(grp => new UrlsGrpsDTO
                {
                    Id = grp.Id,
                    Name = grp.Name,
                    Urls = urls.Data.Where(e => e.Id_UrlGrp == grp.Id).ToList()
                }).ToList();

                _logger.LogInformation("Recuperación de datos completada correctamente. Total grupos recuperados: {count}.", result.Count);
                return new ResponseApiDTO<IEnumerable<UrlsGrpsDTO>>
                {
                    StatusCode = 200,
                    Message = "Ok",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la operación de base de datos en GetAllUrlsGrpsAsync.");
                return new ResponseApiDTO<IEnumerable<UrlsGrpsDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
