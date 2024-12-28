namespace BDAS2_Kratky_Horak_Semestralni_Prace.Models
{
    public class HistoryLog
    {

        public int IdLog { get; set; }
        public string TableName { get; set; }
        public string OperationType { get; set; }
        public int RecordId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }

    }
}
