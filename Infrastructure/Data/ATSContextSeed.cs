using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Entities.Admin;
using Microsoft.Extensions.Logging;
using Core.Entities.Processing;

namespace Infrastructure.Data
{
    public class ATSContextSeed
    {
        public static async Task SeedAsync(ATSContext context, ILoggerFactory loggerFactory)
        {
            var loggr = loggerFactory.CreateLogger<ATSContext>();
            try
            {
               if (!context.AssessmentQsBank.Any())         //works
                {
                    var indData = File.ReadAllText("../Infrastructure/Data/SeedData/assessmentQBank.json");
                    var inds = JsonSerializer.Deserialize<List<AssessmentQBank>>(indData);
                    
                    foreach (var item in inds)
                    {
                        context.AssessmentQsBank.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.IndustryTypes.Any())           //works
                {
                    loggr.LogInformation("entered industry types");
                    var indData = File.ReadAllText("../Infrastructure/Data/SeedData/industryType.json");
                    var inds = JsonSerializer.Deserialize<List<IndustryType>>(indData);
                    
                    foreach (var item in inds)
                    {
                        context.IndustryTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.SkillLevels.Any())             //works
                {
                    var levelData = File.ReadAllText("../Infrastructure/Data/SeedData/skillLevel.json");
                    var levels = JsonSerializer.Deserialize<List<SkillLevel>>(levelData);
                    foreach (var item in levels)
                    {
                        context.SkillLevels.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

              // if (!context.Categories.Any())              //works
//                {
                    var catData = File.ReadAllText("../Infrastructure/Data/SeedData/categorycopy.json");
                    var cats = JsonSerializer.Deserialize<List<Category>>(catData);
                    foreach (var item in cats)
                    {
                        context.Categories.Add(item);
                    }
                    await context.SaveChangesAsync();
  //              }
/*
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

*/

                if (!context.ProcessStatuses.Any())
                {
                    var prcsData = File.ReadAllText("../Infrastructure/Data/SeedData/processstatus.json");
                    var prcs = JsonSerializer.Deserialize<List<ProcessStatus>>(prcsData);
                    foreach (var item in prcs)
                    {
                        context.ProcessStatuses.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if (!context.Candidates.Any())              //works
                {
                    var candData = File.ReadAllText("../Infrastructure/Data/SeedData/candidate.json");
                    var cands = JsonSerializer.Deserialize<List<Candidate>>(candData);
                    foreach (var item in cands)
                    {
                        context.Candidates.Add(item);
                    }
                    await context.SaveChangesAsync();
               }
               
                /*
                if (!context.CandidateCategories.Any())     //includes in caandidates
                {
                    var cvData = File.ReadAllText("../Infrastructure/Data/SeedData/candidateCategories.json");
                    var cvs = JsonSerializer.Deserialize<List<CandidateCategory>>(cvData);
                    foreach (var item in cvs)
                    {
                        context.CandidateCategories.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
*/

                if (!context.Customers.Any())
                {
                    var custData = File.ReadAllText("../Infrastructure/Data/SeedData/customer.json");
                    var custs = JsonSerializer.Deserialize<List<Customer>>(custData);
                    foreach (var item in custs)
                    {
                        context.Customers.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            /*
                if (!context.CustomerOfficials.Any())           //inlucded in customers
                {
                    var offData = File.ReadAllText("../Infrastructure/Data/SeedData/customerOfficial.json");
                    var offs = JsonSerializer.Deserialize<List<CustomerOfficial>>(offData);
                    foreach (var item in offs)
                    {
                        context.CustomerOfficials.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
            */


                 if (!context.Roles.Any())      //works
                {
                    var roleData = File.ReadAllText("../Infrastructure/Data/SeedData/roles.json");
                    var rols = JsonSerializer.Deserialize<List<Role>>(roleData);
                    foreach (var item in rols)
                    {
                        context.Roles.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                 if (!context.Employees.Any())      //works
                {
                    var empData = File.ReadAllText("../Infrastructure/Data/SeedData/employee.json");
                    var emps = JsonSerializer.Deserialize<List<Employee>>(empData);
                    foreach (var item in emps)
                    {
                        context.Employees.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
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
            /*
                if (!context.EnquiryItems.Any())        //included in enquiries
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


            
                if (!context.DLForwardToHR.Any())           //works
                {
                    var empData = File.ReadAllText("../Infrastructure/Data/SeedData/dlForwardToHR.json");
                    var emps = JsonSerializer.Deserialize<List<DLForwardToHR>>(empData);
                    foreach (var item in emps)
                    {
                        context.DLForwardToHR.Add(item);
                    }
                    await context.SaveChangesAsync();
                }


                if (!context.ToDos.Any())
                {
                    var todoData = File.ReadAllText("../Infrastructure/Data/SeedData/todo.json");
                    var tos = JsonSerializer.Deserialize<List<ToDo>>(todoData);
                    foreach (var item in tos)
                    {
                        context.ToDos.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                 if (!context.TaskItems.Any())
                {
                    var taskData = File.ReadAllText("../Infrastructure/Data/SeedData/taskItem.json");
                    var tasks = JsonSerializer.Deserialize<List<TaskItem>>(taskData);
                    foreach (var item in tasks)
                    {
                        context.TaskItems.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
         
          if (!context.AssessmentQsBank.Any())
                {
                    var taskData = File.ReadAllText("../Infrastructure/Data/SeedData/assessmentQBank.json");
                    var tasks = JsonSerializer.Deserialize<List<AssessmentQBank>>(taskData);
                    foreach (var item in tasks)
                    {
                        context.AssessmentQsBank.Add(item);
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