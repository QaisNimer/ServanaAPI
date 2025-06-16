using Microsoft.EntityFrameworkCore;
using ServanaAPP.DTOs.GetAllWorkersByCategoryId.Request;
using ServanaAPP.Interfaces;
using ServanaAPP.Models;

namespace ServanaAPP.Services
{
    public class AllWorkersByCategoryIdService: IAllWorkersByCategoryId
    {
        private readonly ServanaDbContext _db;
        public AllWorkersByCategoryIdService(ServanaDbContext servanaDbContext) 
        {
            _db = servanaDbContext;
        }

        //public async Task<List<User>> GetAllWorkersByCategoryId(int CategoryID) {
        //    try
        //    {
        //        var getAllWorkersByCategoryId = await _db.Users
        //            .Where(u => u.CategoryID == CategoryID && u.Role == 3 && u.IsActive)
        //            .ToListAsync();

        //        return getAllWorkersByCategoryId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<List<AllWorkersByCategoryIdDTO>> GetAllWorkersByCategoryId(int categoryId)
        {
            try
            {
                // Step 1: Ratings grouped by WorkerID
                var ratingsGrouped = _db.Ratings
                    .Where(r => r.IsActive)
                    .GroupBy(r => r.WorkerID)
                    .Select(g => new
                    {
                        WorkerID = g.Key,
                        AverageRating = g.Average(r => r.Stars),
                        TotalRatings = g.Count()
                    });

                // Step 2: Join Workers with Ratings
                var result = await _db.Users
                    .Where(u => u.IsActive && u.Role == 3 && u.CategoryID == categoryId)
                    .GroupJoin(
                        ratingsGrouped,
                        user => user.UserID,
                        rating => rating.WorkerID,
                        (user, ratingGroup) => new { user, ratingGroup = ratingGroup.FirstOrDefault() }
                    )
                    .Select(x => new AllWorkersByCategoryIdDTO
                    {
                        FullName = x.user.FullName,
                        ProfileImage = x.user.ProfileImage,
                        PricePerHour = x.user.PricePerHour,
                        AverageRating = x.ratingGroup != null ? Math.Round(x.ratingGroup.AverageRating,1):0 ,
                        TotalRatings = x.ratingGroup != null ? x.ratingGroup.TotalRatings : 0
                    })
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching workers: {ex.Message}");
            }
        }

    }
}
