// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sib.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using MongoDB.Driver;

    using Sib.Core.Authentication;
    using Sib.Core.Domain;
    using Sib.Core.Dto;
    using Sib.Core.Interfaces;

    [Authorize, Route("services")]
    public class ServicesController : Controller
    {
        private readonly IServiceRepository serviceRepository;

        private const string TimeFormat = @"hh\:mm";

        public ServicesController(IServiceRepository serviceRepository)
        {
            this.serviceRepository = serviceRepository;
        }

        // GET: /<controller>/
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var servicesCursor = await this.serviceRepository.GetServicesBetween(DateTime.MinValue, DateTime.MaxValue).ConfigureAwait(false);

            var allServices = await servicesCursor.ToListAsync().ConfigureAwait(false);

            var convertedServices =
                allServices.Select(
                    d =>
                        new ServiceModel
                        {
                            Id = d.Id.ToString(),
                            Date = d.Date,
                            Location = d.Location,
                            Start = d.Start.ToString(TimeFormat),
                            End = d.End.ToString(TimeFormat),
                            Work = d.Work
                        });

            return this.View(convertedServices);
        }

        [Route("new"), Authorize(Roles = "Administrator,ADMINISTRATOR")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost, Route("new"), ValidateAntiForgeryToken, Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Create(ServiceModel serviceModel)
        {
            if (ModelState.IsValid)
            {
                var service = new Service
                {
                    Date = serviceModel.Date,
                    End = TimeSpan.Parse(serviceModel.End),
                    Start = TimeSpan.Parse(serviceModel.Start),
                    Location = serviceModel.Location,
                    Work = serviceModel.Work

                };
                await this.serviceRepository.Insert(service).ConfigureAwait(false);
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View("Create", serviceModel);
        }

        [HttpGet, Route("edit/{serviceId}"), Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Edit(string serviceId)
        {
            var service = await this.serviceRepository.FindById(serviceId).ConfigureAwait(false);
            var serviceModel = new ServiceModel
            {
                Id = serviceId,
                Date = service.Date,
                Location = service.Location,
                Start = service.Start.ToString(TimeFormat),
                End = service.End.ToString(TimeFormat),
                Work = service.Work
            };
            return this.View("Edit", serviceModel);
        }

        [HttpPost, Route("update"), ValidateAntiForgeryToken, Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Update(ServiceModel serviceModel)
        {
            var serviceId = serviceModel.Id;
            if (!ModelState.IsValid)
                return this.View(nameof(this.Edit), serviceModel);

            var service = await this.serviceRepository.FindById(serviceModel.Id).ConfigureAwait(false);

            if (service == null)
                throw new ArgumentException("Service being updated was not found",nameof(serviceModel));

            service.Date = serviceModel.Date;
            service.End = TimeSpan.Parse(serviceModel.End);
            service.Start = TimeSpan.Parse(serviceModel.Start);
            service.Location = serviceModel.Location;
            service.Work = serviceModel.Work;

            await this.serviceRepository.Update(service).ConfigureAwait(false);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
