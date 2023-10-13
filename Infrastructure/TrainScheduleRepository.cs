using MongoDB.Bson;
using MongoDB.Driver;
using trs_web_service.Models.Domains;
using trs_web_service.Models.Dtos;

namespace trs_web_service.Infrastructure
{
    public class TrainScheduleRepository
    { 
        private readonly IMongoCollection<TrainSchedule> _collection;

        public TrainScheduleRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<TrainSchedule>("TrainShedules");
        }

        // Create Train Schedule
        public async Task CreateAsync(TrainSchedule schedule)
        {
            await _collection.InsertOneAsync(schedule);
        }

        // Get Train Schedule by Reg No
        public async Task<List<TrainSchedule>> GetBySheduleByTrainRegistraionNoAsync(string regNo)
        {
            // Create a filter to find the schedule with the given train reg no
            var filter = Builders<TrainSchedule>.Filter.Eq(x => x.TraingRegistraionNo, regNo) & Builders<TrainSchedule>.Filter.Eq(x => x.IsDelete, false);

            return await _collection.Find(filter).ToListAsync();
        }

        // Get Train Schedule by Id Not Canceled
        public async Task<List<TrainSchedule>> GetBySheduleByTrainRegistraionNoAndNoCancelAsync(string regNo)
        {
            // Create a filter to find the schedule with the given train reg no
            var filter = Builders<TrainSchedule>.Filter.Eq(x => x.IsDelete, false) & Builders<TrainSchedule>.Filter.Eq(x => x.IsCancel, false);

            return await _collection.Find(filter).ToListAsync();
        }

        // Get All Train schedules
        public async Task<List<TrainSchedule>> GetAllTrainShedulesAsync(string tRegNo)
        {
            // Create a filter to match documents where IsDelete is not true and IsCancel not true
            var filter = Builders<TrainSchedule>.Filter.Eq(u => u.IsDelete, false) & Builders<TrainSchedule>.Filter.Eq(x => x.TraingRegistraionNo, tRegNo);

            // Use the filter when querying the collection
            return await _collection.Find(filter).ToListAsync();
        }

        //Get All train Schedules for travelers
        public async Task<List<TrainSchedule>> GetAllTrainShedulesForTravelersAsync()
        {
            // Create a filter to match documents where IsDelete is not true and IsCancel not true
            var filter = Builders<TrainSchedule>.Filter.Eq(u => u.IsDelete, false) & Builders<TrainSchedule>.Filter.Eq(x => x.IsCancel, false);

            // Use the filter when querying the collection
            return await _collection.Find(filter).ToListAsync();
        }

        //Get All Train Schedules By Id
        public async Task<List<TrainSchedule>> GetAllByIdAsync(ObjectId id)
        {
            // Create a filter to match documents where IsDelete is not true and IsCancel not true
            var filter = Builders<TrainSchedule>.Filter.Eq(u => u.Id, id) & Builders<TrainSchedule>.Filter.Eq(x => x.IsDelete, false);

            // Use the filter when querying the collection
            return await _collection.Find(filter).ToListAsync();
        }

        //Update Train Schedules
        public async Task<TrainSchedule> UpdateTrainSchedule(ObjectId id, TrainScheduleReqDto schedule)
        {
                var filter = Builders<TrainSchedule>.Filter.Eq(u => u.Id, id);

                var update = Builders<TrainSchedule>.Update
                    .Set(u => u.DayType, schedule.DayType)
                    .Set(u => u.IsCancel, schedule.IsCancel)
                    .Set(u => u.CancelDates, schedule.CancelDates)
                    .Set(u => u.TrainRouteId, schedule.TrainRouteId)
                    .Set(u => u.TrainStops, schedule.TrainStops)
                    .Set(u => u.StartTime, schedule.StartTime)
                    .Set(u => u.EndTime, schedule.EndTime)
                    .Set(u => u.TrainClasses, schedule.TrainClasses);

                // Find and update
                var updated = await _collection.FindOneAndUpdateAsync(filter, update);

                return updated;
            }

        //Delete Train Schedules
        public async Task DeleteTrainSchedule(ObjectId id)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(u => u.Id, id);
            var update = Builders<TrainSchedule>.Update
                .Set(u => u.IsDelete, true);

            // Find and update the user document
            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        // Cancel Train Schedules
        public async Task CancelShedule(ObjectId id)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(u => u.Id, id);
            var update = Builders<TrainSchedule>.Update
                .Set(u => u.IsCancel, true);

            // Find and update the user document
            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        // Cancel Train Schedules by Train Reg No
        public async Task CancelSheduleByTrainRegNo(string regNo)
        {
            var filter = Builders<TrainSchedule>.Filter.Eq(u => u.TraingRegistraionNo, regNo);
            var update = Builders<TrainSchedule>.Update
                .Set(u => u.IsCancel, true);

            // Find and update the user document
            await _collection.FindOneAndUpdateAsync(filter, update);
        }

    }
}
