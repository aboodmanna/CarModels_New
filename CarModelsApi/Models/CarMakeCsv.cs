namespace CarModelsApi.Service.Models
{
    public class CarMakeCsv
    {
        public int make_id { get; set; }
        public string make_name { get; set; }

        internal static CarMakeCsv ParseCsvCarMakeToCarMake(string line)
        {
            var carMakeCsv = new CarMakeCsv();

            var carLine = line.Split(',');
            carMakeCsv.make_id = Convert.ToInt32(carLine[0]);
            carMakeCsv.make_name = Convert.ToString(carLine[1]).ToLower();

            return carMakeCsv;
        }
    }
}
