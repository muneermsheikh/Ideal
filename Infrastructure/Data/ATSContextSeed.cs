using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class ATSContextSeed
    {
        public static async Task SeedAsync(ATSContext context, ILoggerFactory loggerFactory)
        {
            var loggr = loggerFactory.CreateLogger<ATSContext>();
            try
            {
                // var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                loggr.LogInformation("entered ind types");
                if (!context.IndustryTypes.Any())
                {
                    loggr.LogInformation("entered industry types");
                    // var indData =
                       // File.ReadAllText(path + @"/Data/SeedData/industryType.json");
                    var indData = File.ReadAllText("../Infrastructure/Data/SeedData/industryType.json");
                    var inds = JsonSerializer.Deserialize<List<IndustryType>>(indData);
                    
                    loggr.LogWarning("finished industry types json");
                    
                    foreach (var item in inds)
                    {
                        context.IndustryTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.SkillLevels.Any())
                {
                    var levelData = File.ReadAllText("../Infrastructure/Data/SeedData/skillLevel.json");
                    var levels = JsonSerializer.Deserialize<List<SkillLevel>>(levelData);
                    foreach (var item in levels)
                    {
                        context.SkillLevels.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Categories.Any())
                {
                    var catData = File.ReadAllText("../Infrastructure/Data/SeedData/category.json");
                    var cats = JsonSerializer.Deserialize<List<Category>>(catData);
                    foreach (var item in cats)
                    {
                        context.Categories.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.DeliveryMethods.Any())
                {
                    var delData = File.ReadAllText("../Infrastructure/Data/SeedData/deliveryMethod.json");
                    var dels = JsonSerializer.Deserialize<List<DeliveryMethod>>(delData);
                    foreach (var item in dels)
                    {
                        context.DeliveryMethods.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            /*
                if (!context.Enquiries.Any())
                {
                    var enqData = File.ReadAllText("../Infrastructure/Data/SeedData/enquiry.json");
                    var enqs = JsonSerializer.Deserialize<List<Enquiry>>(enqData);
                    foreach (var item in enqs)
                    {
                        context.Enquiries.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.EnquiryItems.Any())
                {
                    var itemData = File.ReadAllText("../Infrastructure/Data/SeedData/enquiryItem.json");
                    var items = JsonSerializer.Deserialize<List<EnquiryItem>>(itemData);
                    foreach (var item in items)
                    {
                        context.EnquiryItems.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            */
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ATSContext>();
                logger.LogError(ex.Message, "Error in seeding data");
            }
        }
    }
}