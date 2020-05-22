using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CookbookProject.DataTransferObjects;
using CookbookProject.Models;
using CookbookProject.Models.Query;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IWebHostEnvironment environment;
        private readonly IRecipeRepository recipeRepository;
        private readonly ICuisineRepository cuisineRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IIngredientRepository ingredientRepository;
        private readonly IMeasurementRepository measurementRepository;

        public RecipeController(
            IMapper mapper,
            IUserRepository userRepository,
            IWebHostEnvironment environment,
            IRecipeRepository recipeRepository,
            ICuisineRepository cuisineRepository,
            ICategoryRepository categoryRepository,
            IIngredientRepository ingredientRepository,
            IMeasurementRepository measurementRepository)
        {
            this.mapper = mapper;
            this.environment = environment;
            this.userRepository = userRepository;
            this.recipeRepository = recipeRepository;
            this.cuisineRepository = cuisineRepository;
            this.categoryRepository = categoryRepository;
            this.ingredientRepository = ingredientRepository;
            this.measurementRepository = measurementRepository;
        }

        [HttpGet]
        [Route("Category")] // api/Recipe/Category?item=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCategoryAsync([FromQuery] string item)
        {
            return await recipeRepository
                .GetRecipePreviewByCategoryAsync(item)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Cuisine")] // api/Recipe/Cuisine?item=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByCuisineAsync([FromQuery] string item)
        {
            return await recipeRepository
                .GetRecipePreviewByCuisineAsync(item)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Exact")] // api/Recipe/Exact?title=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByExactTitleAsync([FromQuery] string title)
        {
            return await recipeRepository
                .GetRecipePreviewByExactTitleAsync(title)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Contains")] // api/Recipe/Contains?key=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewThatContainsKeyAsync([FromQuery] string key)
        {
            return await recipeRepository
                .GetRecipePreviewThatContainsKeyAsync(key)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Starts")] // api/Recipe/Starts?key=value
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewThatStartsWithKeyAsync([FromQuery] string key)
        {
            return await recipeRepository
                .GetRecipePreviewThatStartsWithKeyAsync(key)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Author/{name}")] // api/Recipe/Author/{name}
        public async Task<IEnumerable<QRecipePreview>> GetRecipePreviewByAuthorAsync([FromRoute] string name)
        {
            return await recipeRepository
                .GetRecipePreviewByAuthorAsync(name)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Full/{id}")] // api/Recipe/Full/{id}
        public async Task<QFullRecipeInfo> GetFullRecipeInfoByIdAsync([FromRoute] int id)
        {
            return await recipeRepository
                .GetFullRecipeInfoByIdAsync(id)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Instructions/{id}")] // api/Recipe/Instructions/{id}
        public async Task<IEnumerable<string>> GetInstructionsByIdAsync([FromRoute] int id)
        {
            return await recipeRepository
                .GetInstructionsByIdAsync(id)
                .ConfigureAwait(false);
        }

        [HttpPost]
        [Route("Insert")] // api/Recipe/Insert
        public async Task<RegularStatus> InsertRecipeAsync(
            [FromForm] string rawRecipe,
            [FromForm] string rawMeasurements,
            [FromForm] IFormFile image)
        {
            var recipeViewModel = JsonConvert.DeserializeObject<RecipeViewModel>(rawRecipe);
            recipeViewModel.Image = image;
            var msViewModelList = JsonConvert.DeserializeObject<IEnumerable<MeasurementViewModel>>(rawMeasurements);

            var isRecipeValid = TryValidateModel(recipeViewModel);
            if (!isRecipeValid)
                return new RegularStatus() { ErrorMessage = "Invalid recipe info" };

            var areMeasurementsValid = TryValidateModel(msViewModelList);
            if (!areMeasurementsValid)
                return new RegularStatus() { ErrorMessage = "Invalid measurements" };

            var categoryId = await categoryRepository
                .InsertIfNecessaryAsync(recipeViewModel.CategoryTitle)
                .ConfigureAwait(false);

            var cuisineId = await cuisineRepository
                .InsertIfNecessaryAsync(recipeViewModel.CuisineTitle)
                .ConfigureAwait(false);

            var userId = await userRepository
                .GetIdByUsernameAsync(recipeViewModel.Username)
                .ConfigureAwait(false);

            var recipe = mapper.Map<Recipe>(recipeViewModel);
            recipe.CategoryId = categoryId;
            recipe.CuisineId = cuisineId;
            recipe.UserId = userId;

            var imageDirectoryPath = environment.WebRootPath + "\\images\\";

            var tryInsertImage = await recipeRepository
                    .InsertRecipeImageAsync(imageDirectoryPath, recipeViewModel.Image)
                    .ConfigureAwait(false);

            recipe.ImagePath = tryInsertImage ? recipeViewModel.Image.FileName : null;

            try
            {
                await recipeRepository
                    .InsertAsync(recipe)
                    .ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                return new RegularStatus() { ErrorMessage = exc.ToString() };
            }

            foreach (var ms in msViewModelList)
            {
                var measurement = mapper.Map<Measurement>(ms);
                measurement.RecipeId = recipe.Id;
                measurement.IngredientId = await ingredientRepository.
                    InsertIfNecessaryAsync(ms.IngredientTitle)
                    .ConfigureAwait(false);

                try
                {
                    await measurementRepository
                        .InsertAsync(measurement)
                        .ConfigureAwait(false);
                }
                catch (Exception)
                { }
            }

            return new RegularStatus() { IsValid = true };
        }

        [HttpPut]
        [Route("Update/{id}")] // api/Recipe/Update/{id}
        public async Task<RegularStatus> UpdateRecipeAsync(
           [FromRoute] int id,
           [FromForm] string rawRecipe,
           [FromForm] string rawMeasurements,
           [FromForm] IFormFile image)
        {
            var recipeViewModel = JsonConvert.DeserializeObject<RecipeViewModel>(rawRecipe);
            recipeViewModel.Image = image;
            var msViewModelList = JsonConvert.DeserializeObject<IEnumerable<MeasurementViewModel>>(rawMeasurements);

            var isRecipeValid = TryValidateModel(recipeViewModel);
            if (!isRecipeValid)
                return new RegularStatus() { ErrorMessage = "Invalid recipe info" };

            var areMeasurementsValid = TryValidateModel(msViewModelList);
            if (!areMeasurementsValid)
                return new RegularStatus() { ErrorMessage = "Invalid measurements" };

            var categoryId = await categoryRepository
                .InsertIfNecessaryAsync(recipeViewModel.CategoryTitle)
                .ConfigureAwait(false);

            var cuisineId = await cuisineRepository
                .InsertIfNecessaryAsync(recipeViewModel.CuisineTitle)
                .ConfigureAwait(false);

            var userId = await userRepository
                .GetIdByUsernameAsync(recipeViewModel.Username)
                .ConfigureAwait(false);

            var recipe = mapper.Map<Recipe>(recipeViewModel);
            recipe.Id = id;
            recipe.CategoryId = categoryId;
            recipe.CuisineId = cuisineId;
            recipe.UserId = userId;

            var imageDirectoryPath = environment.WebRootPath + "\\images\\";

            var tryInsertImage = await recipeRepository
                    .InsertRecipeImageAsync(imageDirectoryPath, recipeViewModel.Image)
                    .ConfigureAwait(false);

            recipe.ImagePath = tryInsertImage ? recipeViewModel.Image.FileName : null;

            try
            {
                await recipeRepository
                    .UpdateAsync(recipe)
                    .ConfigureAwait(false);
            }
            catch(Exception exc)
            {
                return new RegularStatus() { ErrorMessage = exc.ToString() };
            }

            foreach (var ms in msViewModelList)
            {
                var measurement = mapper.Map<Measurement>(ms);
                measurement.Id = ms.Id;
                measurement.RecipeId = recipe.Id;
                measurement.IngredientId = await ingredientRepository.
                    InsertIfNecessaryAsync(ms.IngredientTitle)
                    .ConfigureAwait(false);

                try
                {
                    await measurementRepository
                        .UpdateAsync(measurement)
                        .ConfigureAwait(false);
                }
                catch (Exception)
                { }
            }

            return new RegularStatus() { IsValid = true };
        }

        [HttpDelete]
        [Route("Delete/{id}")] // api/Recipe/Delete/{id}
        public async Task<RegularStatus> DeleteRecipeAsync([FromRoute] int id)
        {
            var existingRecipe = await recipeRepository
                .GetByIdAsync(id)
                .ConfigureAwait(false);

            if (existingRecipe == null)
                return new RegularStatus() { ErrorMessage = "Invalid recipe id" };

            try
            {
                await recipeRepository
                    .DeleteAsync(id)
                    .ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                return new RegularStatus() { ErrorMessage = exc.ToString() };
            }

            return new RegularStatus { IsValid = true };
        }
    }
}
