namespace UserManagement.Web.Models.Logs;

public class LogListViewModel
{
    public List<LogListItemViewModel> Items { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}
