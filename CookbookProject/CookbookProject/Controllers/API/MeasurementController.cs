using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookProject.Models.Query;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementRepository measurementRepository;

        public MeasurementController(IMeasurementRepository measurementRepository)
        {
            this.measurementRepository = measurementRepository;
        }

        [HttpGet]
        [Route("Recipe/{id}")] // api/Measurement/Recipe/{id}
        public async Task<IEnumerable<QRecipeMeasurement>> GetRecipeMeasurementsByIdAsync([FromRoute] int id)
        {
            return await measurementRepository
                .GetRecipeMeasurementsByIdAsync(id)
                .ConfigureAwait(false);
        }

    }
}
