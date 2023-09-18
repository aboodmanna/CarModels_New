namespace CarModelsApi.Models
{
    public class GetModelsForMakeIdYearResponseModel
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }

        public List<CarModelsModel> Results { get; set; }
    }
}
