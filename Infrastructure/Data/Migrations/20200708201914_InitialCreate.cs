using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerType = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(maxLength: 100, nullable: false),
                    KnownAs = table.Column<string>(maxLength: 15, nullable: false),
                    CityName = table.Column<string>(maxLength: 50, nullable: false),
                    IntroducedBy = table.Column<string>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    CompanyUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CustomerStatus = table.Column<string>(nullable: false),
                    CustomerAddressId = table.Column<int>(nullable: true),
                    CustomerOfficialId = table.Column<int>(nullable: false),
                    AddedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CVEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    HRExecutiveId = table.Column<int>(nullable: false),
                    SubmittedByHRExecOn = table.Column<DateTime>(nullable: false),
                    HRSupervisorId = table.Column<int>(nullable: true),
                    HRSupReviewResult = table.Column<string>(nullable: true),
                    ReviewedByHRSupOn = table.Column<DateTime>(nullable: true),
                    HRManagerId = table.Column<int>(nullable: true),
                    ReviewedByHRMOn = table.Column<DateTime>(nullable: true),
                    HRMReviewResult = table.Column<string>(nullable: true),
                    CVReferredId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVEvaluation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryMethods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortName = table.Column<string>(nullable: true),
                    DeliveryTime = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainSubject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Domain = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainSubject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Person_FirstName = table.Column<string>(nullable: true),
                    Person_SecondName = table.Column<string>(nullable: true),
                    Person_FamilyName = table.Column<string>(nullable: true),
                    Person_KnownAs = table.Column<string>(nullable: true),
                    Person_Gender = table.Column<string>(nullable: true),
                    Person_PPNo = table.Column<string>(nullable: true),
                    Person_AadharNo = table.Column<string>(nullable: true),
                    Person_DOB = table.Column<DateTime>(nullable: true),
                    Designation = table.Column<string>(nullable: false),
                    DOJ = table.Column<DateTime>(nullable: false),
                    EmployeeAddress_AddressType = table.Column<string>(nullable: true),
                    EmployeeAddress_Address1 = table.Column<string>(nullable: true),
                    EmployeeAddress_Address2 = table.Column<string>(nullable: true),
                    EmployeeAddress_City = table.Column<string>(nullable: true),
                    EmployeeAddress_PIN = table.Column<string>(nullable: true),
                    EmployeeAddress_State = table.Column<string>(nullable: true),
                    EmployeeAddress_District = table.Column<string>(nullable: true),
                    EmployeeAddress_Country = table.Column<string>(nullable: true),
                    EmployeeAddress_Valid = table.Column<bool>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    IsInEmployment = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryForwarded",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerOfficialId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    ForwardedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryForwarded", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCard",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobCardDate = table.Column<DateTime>(nullable: false),
                    CandidateId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    PPInPossession = table.Column<bool>(nullable: false),
                    PPIsValid = table.Column<bool>(nullable: false),
                    WillingToEmigrate = table.Column<bool>(nullable: false),
                    WillingToTravelWithinTwoWeeksOfSelection = table.Column<bool>(nullable: false),
                    RemunerationAcceptable = table.Column<bool>(nullable: false),
                    ServiceChargesAcceptable = table.Column<bool>(nullable: false),
                    SuspiciousCandidate = table.Column<bool>(nullable: false),
                    OkToForwardCVToClient = table.Column<bool>(nullable: false),
                    OkToConsider = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobDesc",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    JobDescription = table.Column<string>(nullable: false),
                    QualificationDesired = table.Column<string>(nullable: true),
                    ExperienceDesiredMin = table.Column<int>(nullable: false),
                    ExperienceDesiredMax = table.Column<int>(nullable: false),
                    JobProfileDetails = table.Column<string>(nullable: true),
                    JobProfileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDesc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processing",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVRefId = table.Column<int>(nullable: false),
                    ProcessingDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    NextProcessingId = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Remuneration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    ContractPeriodInMonths = table.Column<int>(nullable: false),
                    SalaryCurrency = table.Column<string>(maxLength: 3, nullable: false),
                    SalaryMin = table.Column<int>(nullable: false),
                    SalaryMax = table.Column<int>(nullable: false),
                    SalaryNegotiable = table.Column<bool>(nullable: false),
                    Housing = table.Column<string>(nullable: false),
                    HousingAllowance = table.Column<int>(nullable: true),
                    Food = table.Column<string>(nullable: false),
                    FoodAllowance = table.Column<int>(nullable: true),
                    Transport = table.Column<string>(nullable: false),
                    TransportAllowance = table.Column<int>(nullable: true),
                    OtherAllowance = table.Column<int>(nullable: true),
                    LeaveAvailableAfterHowmanyMonths = table.Column<int>(nullable: false),
                    LeaveEntitlementPerYear = table.Column<int>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remuneration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceGroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SourceGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(nullable: false),
                    AddressType = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PIN = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Valid = table.Column<bool>(nullable: false),
                    AddedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddress_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOfficial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Mobile2 = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: false),
                    PersonalEmail = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false),
                    AddedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOfficial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOfficial_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(nullable: false),
                    CurrentGrade = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grade_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndustryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndustryTypes_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationNo = table.Column<int>(nullable: false),
                    ApplicationDated = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    FamilyName = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    PPNo = table.Column<string>(nullable: false),
                    CandidateAddress_AddressType = table.Column<string>(nullable: true),
                    CandidateAddress_Address1 = table.Column<string>(nullable: true),
                    CandidateAddress_Address2 = table.Column<string>(nullable: true),
                    CandidateAddress_City = table.Column<string>(nullable: true),
                    CandidateAddress_PIN = table.Column<string>(nullable: true),
                    CandidateAddress_State = table.Column<string>(nullable: true),
                    CandidateAddress_District = table.Column<string>(nullable: true),
                    CandidateAddress_Country = table.Column<string>(nullable: true),
                    CandidateAddress_Valid = table.Column<bool>(nullable: true),
                    CandidateStatus = table.Column<string>(nullable: false),
                    LastStatusUpdatedById = table.Column<int>(nullable: true),
                    LastStatusUpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidate_Employee_LastStatusUpdatedById",
                        column: x => x.LastStatusUpdatedById,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ToDo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    OwnerId = table.Column<int>(nullable: false),
                    AssignedToId = table.Column<int>(nullable: false),
                    TaskDate = table.Column<DateTime>(nullable: false),
                    CompleteBy = table.Column<DateTime>(nullable: false),
                    TaskType = table.Column<string>(nullable: false),
                    TaskDescription = table.Column<string>(nullable: false),
                    TaskStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDo_Employee_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToDo_Employee_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enquiries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryNo = table.Column<int>(nullable: false),
                    EnquiryDate = table.Column<DateTimeOffset>(nullable: false),
                    ShipToAddress_FirstName = table.Column<string>(nullable: true),
                    ShipToAddress_LastName = table.Column<string>(nullable: true),
                    ShipToAddress_Street = table.Column<string>(nullable: true),
                    ShipToAddress_City = table.Column<string>(nullable: true),
                    ShipToAddress_State = table.Column<string>(nullable: true),
                    ShipToAddress_Zipcode = table.Column<string>(nullable: true),
                    DeliveryMethodId = table.Column<int>(nullable: true),
                    EnquiryStatus = table.Column<string>(nullable: false),
                    PaymentIntentId = table.Column<string>(nullable: true),
                    ProjectManagerId = table.Column<int>(nullable: true),
                    AccountExecutiveId = table.Column<int>(nullable: true),
                    HRExecutiveId = table.Column<int>(nullable: true),
                    LogisticsExecutiveId = table.Column<int>(nullable: true),
                    EnquiryRef = table.Column<string>(nullable: true),
                    CompleteBy = table.Column<DateTime>(nullable: false),
                    ReviewedById = table.Column<int>(nullable: true),
                    ReviewedOn = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    BuyerEmail = table.Column<string>(nullable: true),
                    Subtotal = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enquiries_CustomerOfficial_AccountExecutiveId",
                        column: x => x.AccountExecutiveId,
                        principalTable: "CustomerOfficial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_DeliveryMethods_DeliveryMethodId",
                        column: x => x.DeliveryMethodId,
                        principalTable: "DeliveryMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_CustomerOfficial_HRExecutiveId",
                        column: x => x.HRExecutiveId,
                        principalTable: "CustomerOfficial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_CustomerOfficial_LogisticsExecutiveId",
                        column: x => x.LogisticsExecutiveId,
                        principalTable: "CustomerOfficial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_Employee_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_Employee_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(nullable: false),
                    AttachmentType = table.Column<string>(nullable: true),
                    AttachmentDescription = table.Column<string>(nullable: true),
                    UploadedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 75, nullable: false),
                    IndustryTypeId = table.Column<int>(nullable: false),
                    SkillLevelId = table.Column<int>(nullable: false),
                    imageUrl = table.Column<string>(nullable: true),
                    CandidateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categories_IndustryTypes_IndustryTypeId",
                        column: x => x.IndustryTypeId,
                        principalTable: "IndustryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Categories_SkillLevels_SkillLevelId",
                        column: x => x.SkillLevelId,
                        principalTable: "SkillLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVSource",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationId = table.Column<int>(nullable: false),
                    ApplicationNo = table.Column<int>(nullable: false),
                    ReceivedFromAssociateId = table.Column<int>(nullable: true),
                    ReceivedFromCandidateId = table.Column<int>(nullable: true),
                    SourceId = table.Column<int>(nullable: true),
                    EnquiryForwardedId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVSource_EnquiryForwarded_EnquiryForwardedId",
                        column: x => x.EnquiryForwardedId,
                        principalTable: "EnquiryForwarded",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVSource_Customer_ReceivedFromAssociateId",
                        column: x => x.ReceivedFromAssociateId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVSource_Candidate_ReceivedFromCandidateId",
                        column: x => x.ReceivedFromCandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVSource_Source_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaskId = table.Column<int>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    QntyConcluded = table.Column<int>(nullable: true),
                    TransactionDetail = table.Column<string>(nullable: false),
                    CreateEmailMessage = table.Column<bool>(nullable: false),
                    RemindOn = table.Column<DateTime>(nullable: true),
                    ToDoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItem_ToDo_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemOrdered_CategoryItemId = table.Column<int>(nullable: true),
                    ItemOrdered_CategoryName = table.Column<string>(nullable: true),
                    ItemOrdered_Price = table.Column<int>(nullable: true),
                    ItemOrdered_ImageUrl = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    EnquiryId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    MinCVs = table.Column<int>(nullable: false),
                    MaxCVs = table.Column<int>(nullable: false),
                    ECNR = table.Column<bool>(nullable: false),
                    RequireAssessment = table.Column<bool>(nullable: false),
                    HRExecutiveId = table.Column<int>(nullable: true),
                    HRSupervisorId = table.Column<int>(nullable: true),
                    HRManagerId = table.Column<int>(nullable: true),
                    SCFromCandidate = table.Column<string>(nullable: true),
                    FeeFromClientCurrency = table.Column<string>(nullable: true),
                    FeeFromClient = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CompleteBy = table.Column<DateTime>(nullable: false),
                    JobDescId1 = table.Column<int>(nullable: true),
                    JobDescId = table.Column<int>(nullable: false),
                    RemunerationId1 = table.Column<int>(nullable: true),
                    RemunerationId = table.Column<int>(nullable: false),
                    CVSourceId = table.Column<int>(nullable: true),
                    CandidateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_CVSource_CVSourceId",
                        column: x => x.CVSourceId,
                        principalTable: "CVSource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Enquiries_EnquiryId",
                        column: x => x.EnquiryId,
                        principalTable: "Enquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Employee_HRExecutiveId",
                        column: x => x.HRExecutiveId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Employee_HRManagerId",
                        column: x => x.HRManagerId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Employee_HRSupervisorId",
                        column: x => x.HRSupervisorId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_JobDesc_JobDescId1",
                        column: x => x.JobDescId1,
                        principalTable: "JobDesc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Remuneration_RemunerationId1",
                        column: x => x.RemunerationId1,
                        principalTable: "Remuneration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assessment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<int>(nullable: false),
                    Result = table.Column<string>(nullable: false),
                    AssessmentResultId = table.Column<int>(nullable: true),
                    AssessedBy = table.Column<string>(nullable: true),
                    AssessedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessment_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assessment_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentQ",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssessmentId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    QuestionNo = table.Column<int>(nullable: false),
                    Question = table.Column<string>(maxLength: 150, nullable: false),
                    Assessed = table.Column<bool>(nullable: false),
                    MaxPoints = table.Column<int>(nullable: false),
                    PointsGiven = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentQ_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractReviewItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    TechnicallyFeasible = table.Column<bool>(nullable: false),
                    CommerciallyFeasible = table.Column<bool>(nullable: false),
                    LogisticallyFeasible = table.Column<bool>(nullable: false),
                    VisaAvailable = table.Column<bool>(nullable: false),
                    DocumentationWillBeAvailable = table.Column<bool>(nullable: false),
                    HistoricalStatusAvailable = table.Column<bool>(nullable: false),
                    SalaryOfferedFeasible = table.Column<bool>(nullable: false),
                    ServiceChargesInINR = table.Column<string>(nullable: false),
                    FeeFromClientCurrency = table.Column<string>(nullable: true),
                    FeeFromClient = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    ReviewedOn = table.Column<DateTime>(nullable: false),
                    ReviewedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractReviewItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractReviewItem_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_CandidateId",
                table: "Assessment",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_EnquiryItemId",
                table: "Assessment",
                column: "EnquiryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessment_EnquiryItemId_CandidateId",
                table: "Assessment",
                columns: new[] { "EnquiryItemId", "CandidateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQ_EnquiryItemId",
                table: "AssessmentQ",
                column: "EnquiryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CandidateId",
                table: "Attachment",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_ApplicationNo",
                table: "Candidate",
                column: "ApplicationNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_LastStatusUpdatedById",
                table: "Candidate",
                column: "LastStatusUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Candidate_PPNo",
                table: "Candidate",
                column: "PPNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CandidateId",
                table: "Categories",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_IndustryTypeId",
                table: "Categories",
                column: "IndustryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SkillLevelId",
                table: "Categories",
                column: "SkillLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name_SkillLevelId_IndustryTypeId",
                table: "Categories",
                columns: new[] { "Name", "SkillLevelId", "IndustryTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractReviewItem_EnquiryItemId",
                table: "ContractReviewItem",
                column: "EnquiryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddress_CustomerId",
                table: "CustomerAddress",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOfficial_CustomerId",
                table: "CustomerOfficial",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CVSource_EnquiryForwardedId",
                table: "CVSource",
                column: "EnquiryForwardedId");

            migrationBuilder.CreateIndex(
                name: "IX_CVSource_ReceivedFromAssociateId",
                table: "CVSource",
                column: "ReceivedFromAssociateId");

            migrationBuilder.CreateIndex(
                name: "IX_CVSource_ReceivedFromCandidateId",
                table: "CVSource",
                column: "ReceivedFromCandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CVSource_SourceId",
                table: "CVSource",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainSubject_Domain",
                table: "DomainSubject",
                column: "Domain",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_AccountExecutiveId",
                table: "Enquiries",
                column: "AccountExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_DeliveryMethodId",
                table: "Enquiries",
                column: "DeliveryMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_HRExecutiveId",
                table: "Enquiries",
                column: "HRExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_LogisticsExecutiveId",
                table: "Enquiries",
                column: "LogisticsExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_ProjectManagerId",
                table: "Enquiries",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_ReviewedById",
                table: "Enquiries",
                column: "ReviewedById");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryForwarded_CustomerOfficialId",
                table: "EnquiryForwarded",
                column: "CustomerOfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryForwarded_EnquiryItemId",
                table: "EnquiryForwarded",
                column: "EnquiryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_CVSourceId",
                table: "EnquiryItems",
                column: "CVSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_CandidateId",
                table: "EnquiryItems",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_CategoryId",
                table: "EnquiryItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_EnquiryId",
                table: "EnquiryItems",
                column: "EnquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_HRExecutiveId",
                table: "EnquiryItems",
                column: "HRExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_HRManagerId",
                table: "EnquiryItems",
                column: "HRManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_HRSupervisorId",
                table: "EnquiryItems",
                column: "HRSupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_JobDescId1",
                table: "EnquiryItems",
                column: "JobDescId1");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_RemunerationId1",
                table: "EnquiryItems",
                column: "RemunerationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_CustomerId",
                table: "Grade",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTypes_CustomerId",
                table: "IndustryTypes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTypes_Name",
                table: "IndustryTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCard_EnquiryItemId_CandidateId",
                table: "JobCard",
                columns: new[] { "EnquiryItemId", "CandidateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobDesc_EnquiryItemId",
                table: "JobDesc",
                column: "EnquiryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processing_CVRefId",
                table: "Processing",
                column: "CVRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Remuneration_EnquiryItemId",
                table: "Remuneration",
                column: "EnquiryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_EmployeeId",
                table: "Role",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillLevels_Name",
                table: "SkillLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Source_SourceGroupId_Name",
                table: "Source",
                columns: new[] { "SourceGroupId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceGroup_Name",
                table: "SourceGroup",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItem_ToDoId",
                table: "TaskItem",
                column: "ToDoId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDo_AssignedToId",
                table: "ToDo",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDo_OwnerId",
                table: "ToDo",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assessment");

            migrationBuilder.DropTable(
                name: "AssessmentQ");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "ContractReviewItem");

            migrationBuilder.DropTable(
                name: "CustomerAddress");

            migrationBuilder.DropTable(
                name: "CVEvaluation");

            migrationBuilder.DropTable(
                name: "DomainSubject");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "JobCard");

            migrationBuilder.DropTable(
                name: "Processing");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "SourceGroup");

            migrationBuilder.DropTable(
                name: "TaskItem");

            migrationBuilder.DropTable(
                name: "EnquiryItems");

            migrationBuilder.DropTable(
                name: "ToDo");

            migrationBuilder.DropTable(
                name: "CVSource");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Enquiries");

            migrationBuilder.DropTable(
                name: "JobDesc");

            migrationBuilder.DropTable(
                name: "Remuneration");

            migrationBuilder.DropTable(
                name: "EnquiryForwarded");

            migrationBuilder.DropTable(
                name: "Source");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "IndustryTypes");

            migrationBuilder.DropTable(
                name: "SkillLevels");

            migrationBuilder.DropTable(
                name: "CustomerOfficial");

            migrationBuilder.DropTable(
                name: "DeliveryMethods");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
