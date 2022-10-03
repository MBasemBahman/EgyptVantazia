namespace Dashboard.Areas.Dashboard.Models
{
    public class ChartDto
    {
        public string Key { get; set; }

        public string Text { get; set; }

        public string Url { get; set; }

        public ChartTypeEnum Type { get; set; }
    }

    public class DonutChartDto
    {
        public string Key { get; set; }

        public List<string> Labels { get; set; }

        public List<decimal> Series { get; set; }

        public List<string> Urls { get; set; }

        public decimal Total { get; set; }
    }

    public class ColumnChartDto
    {
        public string Key { get; set; }

        public List<string> Categories { get; set; }

        public List<string> Urls { get; set; }

        public List<ColumnChartElementDto> Series { get; set; }
    }

    public class ColumnChartElementDto
    {
        public string Name { get; set; }

        public List<int> Data { get; set; }

        public string Url { get; set; }
    }

    public class DateChartElementDto
    {
        public string Label { get; set; }

        public Dictionary<string, int> Items { get; set; }
    }

    public class ChartItemDto
    {
        public string label { get; set; }
        public decimal value { get; set; }
        public int Id { get; set; }
    }
}
