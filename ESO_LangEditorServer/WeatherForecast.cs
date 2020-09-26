using System;
using System.ComponentModel.DataAnnotations;

namespace ESO_LangEditorServer
{
    public class WeatherForecast
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-DD HH:MM}")]
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
