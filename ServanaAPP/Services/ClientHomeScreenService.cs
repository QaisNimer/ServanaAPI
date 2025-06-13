using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ServanaAPP.DTOs.ClientHomeScreen.Request;
using ServanaAPP.DTOs.ClientHomeScreen.Response;
using ServanaAPP.Helpers.ImageHelpers;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;
using System.Data;

namespace ServanaAPP.Services
{
    public class ClientHomeScreenService : IHomeScreenClient
    {

        private readonly ServanaDbContext _db;

        public ClientHomeScreenService(ServanaDbContext servanaDbContext) 
        { 
            _db = servanaDbContext;
        }

        public async Task<List<Category>> AllCategories()
        {
            try
            {
               var allCategories= await _db.Categories.ToListAsync();
                if (allCategories==null)
                {
                    throw new Exception("No Category Available");
                }
                return allCategories;
            }
            catch (Exception ex)
            {

                throw new Exception($"User Save Error: {ex.Message}");
            }
        }

        public async Task<string> AddCategory(AddCategoryRequestDTO input)
        {
            try
            {
                var addCategory = new Category();
                addCategory.ArabicName = input.ArabicName;
                addCategory.EnglishName = input.EnglishName;
                if (input.CategoryImage != null)
                {

                    string imagePath = await FileUploadHelper.UploadFileAsync(input.CategoryImage, "Uploads/CategoryImages");
                    addCategory.CategoryImage = imagePath;

                }
                else
                {
                    addCategory.CategoryImage = null;
                }
                await _db.AddAsync(addCategory);
                await _db.SaveChangesAsync();
                return "New Category Added Successfully";

            }
            catch (Exception ex)
            {

                return $"User Save Error: {ex.InnerException?.Message ?? ex.Message}";
            }
        }

        public async Task<string> UpdateCategory(ClientUpdateCAtegoryRequestDTO input)
        {
            try
            {
                var updatedCategory = await _db.Categories.Where(c=>c.CategoryID==input.id).SingleOrDefaultAsync();
                if (updatedCategory==null)
                {
                    throw new Exception("Invalid CategoryId");
                }
                updatedCategory.ArabicName = input.ArabicName;
                updatedCategory.EnglishName = input.EnglishName;
                if (input.CategoryImage != null)
                {

                    string imagePath = await FileUploadHelper.UploadFileAsync(input.CategoryImage, "Uploads/CategoryImages");
                    updatedCategory.CategoryImage = imagePath;

                }
                
                 _db.Update(updatedCategory);
                await _db.SaveChangesAsync();
                return "Updated Category Successfully";
            }
            catch (Exception ex)
            {

                return $"User Save Error: {ex.InnerException?.Message ?? ex.Message}";
            }
        }

        public async Task<string> DeleteCategory(int id)
        {
            try
            {
                var deletedCategory = await _db.Categories.Where(c => c.CategoryID == id).SingleOrDefaultAsync();
                if (deletedCategory==null)
                {
                    throw new Exception("Invalid Category Id");
                }
                _db.Remove(deletedCategory);
                await _db.SaveChangesAsync();
                return "Category Deleted Successfully !";
            }
            catch (Exception ex)
            {

                return $"User Save Error: {ex.InnerException?.Message ?? ex.Message}";
            }
        }

        public async Task<List<User>> SearchWorker(string name)
        {
            try
            {
                var searchWorker = await _db.Users.Where(u => u.FullName == name).ToListAsync();
                if (searchWorker==null)
                {
                    throw new Exception("There's No Worker With That Name");
                }
                return searchWorker;

            }
            catch (Exception ex)
            {

                throw new Exception($"User Save Error: {ex.Message}");
            }
        }

        public async Task<List<TopRatedWorkerDTO>> TopRatedWorkers() {
            try
            {
                /*
                 var topWorkers = await (
                 from rating in _db.Ratings
                 where rating.IsActive
                 group rating by rating.WorkerID into g
                 orderby g.Average(r => r.Stars) descending, g.Count() descending
                 select new
                 {
                     WorkerID = g.Key,
                     AverageStars = g.Average(r => r.Stars),
                     TotalRatings = g.Count()
                 } into result
                 join user in _db.Users on result.WorkerID equals user.UserID
                 select new TopRatedWorkerDTO
                 {
                     Worker = user,
                     AverageStars = result.AverageStars,
                     TotalRatings = result.TotalRatings
                 })
                 .Take(4)
                 .ToListAsync();

                 return topWorkers;
                 */

                var topWorkers = await _db.Ratings
                .Where(r => r.IsActive)
                .GroupBy(r => r.WorkerID)
                .Select(g => new
                {
                    WorkerID = g.Key,
                    AverageStars = g.Average(r => r.Stars),
                    TotalRatings = g.Count()
                })
                .OrderByDescending(x => x.AverageStars)
                .ThenByDescending(x => x.TotalRatings)
                .Take(4)
                .Join(_db.Users,
                      r => r.WorkerID,
                      u => u.UserID,
                      (r, u) => new TopRatedWorkerDTO
                      {
                          Worker = u,
                          AverageStars = r.AverageStars,
                          TotalRatings = r.TotalRatings
                      })
                .ToListAsync();

                return topWorkers;

            }
            catch (Exception ex)
            {

                throw new Exception($"User Save Error: {ex.Message}");
            }
        }

    }
}
