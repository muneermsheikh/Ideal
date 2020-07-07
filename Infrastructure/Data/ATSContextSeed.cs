using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
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

                if (!context.IndustryTypes.Any())
                {
                    loggr.LogInformation("entered industry types");
                    // var indData =
                       // File.ReadAllText(path + @"/Data/SeedData/industryType.json");
                    var indData = File.ReadAllText("../Infrastructure/Data/SeedData/industryType.json");
                    var inds = JsonSerializer.Deserialize<List<IndustryType>>(indData);

                    foreach (var item in inds)
                    {
                        context.IndustryTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

                if (!context.SkillLevels.Any())
                {
                    // var levelData =
                    //    File.ReadAllText(path + @"/Data/SeedData/skillLevel.json");
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
                    // var catData =
                       // File.ReadAllText(path + @"/Data/SeedData/category.json");
                    var catData = File.ReadAllText("../Infrastructure/Data/SeedData/category.json");
                    var cats = JsonSerializer.Deserialize<List<Category>>(catData);

                    foreach (var item in cats)
                    {
                        context.Categories.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ATSContext>();
                logger.LogError(ex.Message, "Error in seeding data");
            }
        }
    }
}