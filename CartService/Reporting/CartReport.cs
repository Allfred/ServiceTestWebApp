using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CartService.Logging;
using CartService.Models.Carts;
using Microsoft.Extensions.Logging;

namespace CartService.Reporting
{
    public class CartReport: IReport
    {
        private readonly ILogger<FileLogger> _fileLogger;
        private readonly ICartRepository _cartRepository;
        private string _path;
        
        public CartReport(Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment, ILogger<FileLogger> fileLogger, ICartRepository cartRepository)
        {
            _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
            _cartRepository = cartRepository;

            _path = Path.Combine(hostingEnvironment.ContentRootPath, "Reports");

            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        }

        public async Task Create()
        {
            var reportName = "Report" + DateTime.Today.ToString("dd/MM/yyyy") + ".txt";

            var path = Path.Combine(_path, reportName);

            if (!File.Exists(path))
            {
                var carts= (await _cartRepository.GetAsync()).ToList();
                
                string text = "Report:" + Environment.NewLine;
                
                text += ReportCartsCount(carts);
                
                text += ReportOfCartsWithBonusCount(carts);

                text += ReportOfСartExpiresInDays(carts);
                
                text += ReportOfCartAverageCost(carts);
                
                try
                {
                    File.WriteAllText(path, text);
                    _fileLogger.LogInformation(reportName + " was created!");
                }
                
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _fileLogger.LogInformation("Error!" + reportName + " was not created!");
                }
            }
        }

        private string ReportCartsCount(List<Cart> carts)
        {
            return $"Count of carts: {carts.Count}"+ Environment.NewLine;
        }
        
        private string ReportOfCartsWithBonusCount(List<Cart> carts)
        {
            var count = CalculationsForReporting.CountOfCartsWithBonus(carts);
                
            return $"Count of carts with ForBonusPoints is true: {count}"+ Environment.NewLine;
        }

        private string ReportOfСartExpiresInDays(List<Cart> carts)
        {
            var days = new List<int> {10, 20, 30};

            var result = string.Empty;
            
            foreach (var day in days)
            {
                var count = CalculationsForReporting.CountOfСartExpiresInDays(carts, day);

                result += $"Count of carts expires in {day} days: {count}"+Environment.NewLine;
            }

            return result;
        }

        private string ReportOfCartAverageCost(List<Cart> carts)
        {
            var average = CalculationsForReporting.CalcCartAverageCost(carts);

            return $"Average check of carts: {average}" + Environment.NewLine;
        }
    }
}
