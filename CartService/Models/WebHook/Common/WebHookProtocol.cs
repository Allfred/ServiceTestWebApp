namespace CartService.Models.WebHook.Common
{   
    public class WebHookProtocol
    {
        public int Id { get; set; }
        public WebHookType WebHookType { get; set; }
        public int ItemId { get; set; } 
        public string ExecutingUri { get; set; }
    }
}