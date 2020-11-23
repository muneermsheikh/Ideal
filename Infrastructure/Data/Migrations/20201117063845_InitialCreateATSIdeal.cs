using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreateATSIdeal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentQsBank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SrNo = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    IsStandardQuestion = table.Column<bool>(nullable: false),
                    AssessmentParameter = table.Column<string>(nullable: false),
                    Question = table.Column<string>(nullable: false),
                    MaxPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQsBank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractReviewItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    TechnicallyFeasible = table.Column<bool>(nullable: false),
                    CommerciallyFeasible = table.Column<bool>(nullable: false),
                    LogisticallyFeasible = table.Column<bool>(nullable: false),
                    VisaAvailable = table.Column<bool>(nullable: false),
                    DocumentationWillBeAvailable = table.Column<bool>(nullable: false),
                    HistoricalStatusAvailable = table.Column<bool>(nullable: false),
                    SalaryOfferedFeasible = table.Column<bool>(nullable: false),
                    ServiceChargesInINR = table.Column<string>(nullable: true),
                    FeeFromClientCurrency = table.Column<string>(nullable: true),
                    FeeFromClient = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    ReviewedOn = table.Column<DateTime>(nullable: false),
                    ReviewedBy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractReviewItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerType = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(maxLength: 100, nullable: false),
                    KnownAs = table.Column<string>(maxLength: 15, nullable: false),
                    IntroducedBy = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    CompanyUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(maxLength: 50, nullable: false),
                    Pin = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CustomerStatus = table.Column<string>(nullable: true),
                    AddedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
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
                name: "DomainSub",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DomainSubName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainSub", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emoluments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVRefId = table.Column<int>(nullable: false),
                    SalaryCurrency = table.Column<string>(nullable: false),
                    BasicSalary = table.Column<int>(nullable: false),
                    WeeklyWorkHours = table.Column<int>(nullable: false, defaultValue: 48)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContractPeriodInMonths = table.Column<int>(nullable: false, defaultValue: 24)
                        .Annotation("Sqlite:Autoincrement", true),
                    Housing = table.Column<int>(nullable: false, defaultValue: 0),
                    HousingAllowance = table.Column<int>(nullable: false),
                    Food = table.Column<int>(nullable: false, defaultValue: 1),
                    FoodAllowance = table.Column<int>(nullable: false),
                    Transport = table.Column<int>(nullable: false, defaultValue: 0),
                    TransportAllowance = table.Column<int>(nullable: false),
                    OtherAllowance = table.Column<int>(nullable: false),
                    OtherAllowanceAmount = table.Column<int>(nullable: false),
                    LeaveEntitlementAfterMonths = table.Column<int>(nullable: false, defaultValue: 23)
                        .Annotation("Sqlite:Autoincrement", true),
                    AnnualLeaveInDays = table.Column<int>(nullable: false),
                    AirportOfBoarding = table.Column<string>(nullable: true),
                    AirportOfDestination = table.Column<string>(nullable: false),
                    OfferAcceptedByCandidate = table.Column<bool>(nullable: false),
                    OfferAcceptedOn = table.Column<DateTime>(nullable: false),
                    OfferRevised = table.Column<bool>(nullable: false),
                    OfferLetterUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emoluments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(nullable: true),
                    SecondName = table.Column<string>(nullable: true),
                    FamilyName = table.Column<string>(nullable: true),
                    KnownAs = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    PassportNo = table.Column<string>(nullable: true),
                    AadharNo = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Designation = table.Column<string>(nullable: false),
                    Department = table.Column<string>(nullable: true),
                    DateOfJoining = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    IsInEmployment = table.Column<bool>(nullable: false),
                    LastDateOfEmployment = table.Column<DateTime>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
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
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndustryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCards",
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
                    SalaryExpectCurrency = table.Column<string>(nullable: true),
                    SalaryExpectation = table.Column<int>(nullable: false),
                    ServiceChargesAcceptable = table.Column<bool>(nullable: false),
                    SuspiciousCandidate = table.Column<bool>(nullable: false),
                    OkToForwardCVToClient = table.Column<bool>(nullable: false),
                    OkToConsider = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeqId = table.Column<int>(nullable: false),
                    Mandatory = table.Column<bool>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NextStatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessStatuses", x => x.Id);
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
                name: "SourceGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceGroup = table.Column<string>(nullable: true),
                    SourceGroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerIndustryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(nullable: false),
                    IndustryTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CustomerId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerIndustryTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerIndustryTypes_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerIndustryTypes_Customers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOfficials",
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
                    Email = table.Column<string>(nullable: false),
                    PersonalEmail = table.Column<string>(nullable: true),
                    PersonalMobile = table.Column<string>(nullable: true),
                    IsValid = table.Column<string>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    AddedOn = table.Column<DateTime>(nullable: false),
                    CustomerId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOfficials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOfficials_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOfficials_Customers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CVForwards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateForwarded = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerOfficialId = table.Column<int>(nullable: false),
                    OfficialEmailId = table.Column<string>(nullable: true),
                    IncludeSalary = table.Column<bool>(nullable: false),
                    IncludeGrade = table.Column<bool>(nullable: false),
                    IncludePhoto = table.Column<bool>(nullable: false),
                    MailSentRef = table.Column<string>(nullable: true),
                    SentMessageToClient = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVForwards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVForwards_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryForwards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryId = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerOfficialId = table.Column<int>(nullable: false),
                    ForwardedOn = table.Column<DateTime>(nullable: false),
                    ForwardedByMode = table.Column<string>(nullable: true),
                    Addressee = table.Column<string>(nullable: true),
                    SentReference = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryForwards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnquiryForwards_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryItemAssessmentQs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    SrNo = table.Column<int>(nullable: false),
                    DomainSubjectId = table.Column<int>(nullable: false),
                    DomainSubId = table.Column<int>(nullable: true),
                    AssessmentParameter = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: true),
                    Mandatory = table.Column<bool>(nullable: false),
                    MaxPoints = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryItemAssessmentQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnquiryItemAssessmentQs_DomainSub_DomainSubId",
                        column: x => x.DomainSubId,
                        principalTable: "DomainSub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicationNo = table.Column<int>(nullable: false),
                    ApplicationDated = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    SecondName = table.Column<string>(nullable: true),
                    FamilyName = table.Column<string>(nullable: false),
                    KnownAs = table.Column<string>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    ReferredById = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: true),
                    PassportNo = table.Column<string>(nullable: false),
                    Ecnr = table.Column<string>(nullable: false),
                    AadharNo = table.Column<string>(nullable: true),
                    MobileNo = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ContactPreference = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: false),
                    Pin = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CandidateStatus = table.Column<int>(nullable: false),
                    LastStatusUpdatedOn = table.Column<DateTime>(nullable: true),
                    LastStatusUpdatedById = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_Employees_LastStatusUpdatedById",
                        column: x => x.LastStatusUpdatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(nullable: false),
                    AddressType = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PIN = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Valid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(nullable: false),
                    SkillName = table.Column<string>(nullable: true),
                    ExpInYears = table.Column<string>(nullable: true),
                    Proficiency = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryId = table.Column<int>(nullable: true),
                    EnquiryItemId = table.Column<int>(nullable: true),
                    OwnerId = table.Column<int>(nullable: false),
                    AssignedToId = table.Column<int>(nullable: false),
                    TaskDate = table.Column<DateTime>(nullable: false),
                    CompleteBy = table.Column<DateTime>(nullable: false),
                    TaskType = table.Column<string>(nullable: false, defaultValue: "Administrative"),
                    TaskDescription = table.Column<string>(maxLength: 250, nullable: false),
                    TaskStatus = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDos_Employees_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToDos_Employees_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Employees",
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
                    SkillLevelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
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
                name: "HRSkillClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(nullable: false),
                    IndustryTypeId = table.Column<int>(nullable: false),
                    SkillLevelId = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRSkillClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRSkillClaims_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRSkillClaims_IndustryTypes_IndustryTypeId",
                        column: x => x.IndustryTypeId,
                        principalTable: "IndustryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRSkillClaims_SkillLevels_SkillLevelId",
                        column: x => x.SkillLevelId,
                        principalTable: "SkillLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enquiries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerId = table.Column<int>(nullable: false),
                    EnquiryNo = table.Column<int>(nullable: false),
                    BasketId = table.Column<string>(nullable: true),
                    EnquiryDate = table.Column<DateTime>(nullable: false),
                    ReadyToReview = table.Column<bool>(nullable: false),
                    EnquiryStatus = table.Column<string>(nullable: false),
                    ProjectManagerId = table.Column<int>(nullable: false),
                    AccountExecutiveId = table.Column<int>(nullable: true),
                    HRExecutiveId = table.Column<int>(nullable: true),
                    LogisticsExecutiveId = table.Column<int>(nullable: true),
                    EnquiryRef = table.Column<string>(nullable: true),
                    CompleteBy = table.Column<DateTime>(nullable: true),
                    ReviewedById = table.Column<int>(nullable: true),
                    EnquiryReviewStatusId = table.Column<int>(nullable: true),
                    ReviewedOn = table.Column<DateTime>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enquiries_CustomerOfficials_AccountExecutiveId",
                        column: x => x.AccountExecutiveId,
                        principalTable: "CustomerOfficials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enquiries_CustomerOfficials_HRExecutiveId",
                        column: x => x.HRExecutiveId,
                        principalTable: "CustomerOfficials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_CustomerOfficials_LogisticsExecutiveId",
                        column: x => x.LogisticsExecutiveId,
                        principalTable: "CustomerOfficials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_Employees_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enquiries_Employees_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CVForwardItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVForwardId = table.Column<int>(nullable: false),
                    SrNo = table.Column<int>(nullable: false),
                    EnquiryId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<int>(nullable: false),
                    ApplicationNo = table.Column<int>(nullable: false),
                    CVRefId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVForwardItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVForwardItems_CVForwards_CVForwardId",
                        column: x => x.CVForwardId,
                        principalTable: "CVForwards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryItemForwarded",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    EnquiryForwardedId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryItemForwarded", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnquiryItemForwarded_EnquiryForwards_EnquiryForwardedId",
                        column: x => x.EnquiryForwardedId,
                        principalTable: "EnquiryForwards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(nullable: false),
                    AttachmentType = table.Column<string>(nullable: true),
                    AttachmentDescription = table.Column<string>(nullable: true),
                    AttachmentUrl = table.Column<string>(nullable: true),
                    UploadedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateCategories_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaskId = table.Column<int>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    QntyConcluded = table.Column<int>(nullable: true),
                    TransactionDetail = table.Column<string>(maxLength: 250, nullable: false),
                    CreateEmailMessage = table.Column<bool>(nullable: false),
                    RemindOn = table.Column<DateTime>(nullable: true),
                    ItemStatus = table.Column<int>(nullable: true),
                    ToDoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskItems_ToDos_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DLForwardToHR",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignedTo = table.Column<int>(nullable: false),
                    AssignedOn = table.Column<DateTime>(nullable: false),
                    EnquiryId = table.Column<int>(nullable: false),
                    ToDoId = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DLForwardToHR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DLForwardToHR_Enquiries_EnquiryId",
                        column: x => x.EnquiryId,
                        principalTable: "Enquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DLForwardToHR_ToDos_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EnquiryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryId = table.Column<int>(nullable: false),
                    ItemOrdered_CategoryItemId = table.Column<int>(nullable: true),
                    ItemOrdered_CategoryName = table.Column<string>(nullable: true),
                    SrNo = table.Column<int>(nullable: false),
                    CategoryItemId = table.Column<int>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    MaxCVsToSend = table.Column<int>(nullable: false),
                    ECNR = table.Column<bool>(nullable: false),
                    AssessmentReqd = table.Column<bool>(nullable: false),
                    EvaluationReqd = table.Column<bool>(nullable: false),
                    HRExecutiveId = table.Column<int>(nullable: true),
                    AssessingHRExecId = table.Column<int>(nullable: true),
                    AssessingSupId = table.Column<int>(nullable: true),
                    AssessingHRMId = table.Column<int>(nullable: true),
                    CompleteBy = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    EnquiryStatus = table.Column<int>(nullable: false),
                    CandidateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Employees_AssessingHRExecId",
                        column: x => x.AssessingHRExecId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Employees_AssessingHRMId",
                        column: x => x.AssessingHRMId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Employees_AssessingSupId",
                        column: x => x.AssessingSupId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnquiryItems_Enquiries_EnquiryId",
                        column: x => x.EnquiryId,
                        principalTable: "Enquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentQs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    EnquiryId = table.Column<int>(nullable: false),
                    QuestionNo = table.Column<int>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    DomainSubject = table.Column<string>(nullable: true),
                    AssessmentParameter = table.Column<string>(nullable: true),
                    Question = table.Column<string>(maxLength: 150, nullable: false),
                    MaxPoints = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentQs_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    CustomerNameAndCity = table.Column<string>(nullable: true),
                    CategoryNameAndRef = table.Column<string>(nullable: true),
                    CandidateId = table.Column<int>(nullable: false),
                    AssessedBy = table.Column<string>(nullable: true),
                    AssessedOn = table.Column<DateTime>(nullable: false),
                    Result = table.Column<string>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    GradeString = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessments_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assessments_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CandidateId = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    ApplicationNo = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    HRExecutiveId = table.Column<int>(nullable: false),
                    SubmittedByHRExecOn = table.Column<DateTime>(nullable: false),
                    ReviewedByHRSup = table.Column<bool>(nullable: true),
                    HRSupervisorId = table.Column<int>(nullable: true),
                    HRSupReviewResult = table.Column<string>(nullable: true),
                    ReviewedByHRSupOn = table.Column<DateTime>(nullable: true),
                    HRManagerId = table.Column<int>(nullable: true),
                    ReviewedByHRM = table.Column<bool>(nullable: true),
                    ReviewedByHRMOn = table.Column<DateTime>(nullable: true),
                    HRMReviewResult = table.Column<string>(nullable: true),
                    CVReferredId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVEvaluations_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVEvaluations_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CVRefs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    EnquiryId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<int>(nullable: false),
                    ApplicationNo = table.Column<int>(nullable: false),
                    grade = table.Column<string>(nullable: true),
                    photourl = table.Column<string>(nullable: true),
                    salaryexpectation = table.Column<string>(nullable: true),
                    HRExecutiveId = table.Column<int>(nullable: false),
                    DateForwarded = table.Column<DateTime>(nullable: false),
                    RefStatus = table.Column<int>(nullable: false),
                    StatusDate = table.Column<DateTime>(nullable: false),
                    SentReference = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CVRefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CVRefs_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CVRefs_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnquiryId = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_JobDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobDescriptions_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Remunerations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVRefId = table.Column<int>(nullable: false),
                    EnquiryId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<int>(nullable: false),
                    OfferLetterDate = table.Column<DateTime>(nullable: false),
                    ContractPeriodInMonths = table.Column<int>(nullable: false),
                    SalaryCurrency = table.Column<string>(maxLength: 3, nullable: true),
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
                    table.PrimaryKey("PK_Remunerations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remunerations_EnquiryItems_EnquiryItemId",
                        column: x => x.EnquiryItemId,
                        principalTable: "EnquiryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssessmentId = table.Column<int>(nullable: false),
                    Assessed = table.Column<bool>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    QuestionNo = table.Column<int>(nullable: false),
                    AssessmentParameter = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    MaxPoints = table.Column<int>(nullable: false),
                    PointsAllotted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentItems_Assessments_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CVRefId = table.Column<int>(nullable: false),
                    ProcessingDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    NextProcessingId = table.Column<int>(nullable: true),
                    attachmentUrl = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processes_CVRefs_CVRefId",
                        column: x => x.CVRefId,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelDecisions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SelectionDate = table.Column<DateTime>(nullable: false),
                    CVRefID = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
                    EnquiryId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<int>(nullable: false),
                    ApplicationNo = table.Column<int>(nullable: false),
                    SelectionRef = table.Column<string>(nullable: true),
                    SelectionResult = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelDecisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelDecisions_CVRefs_CVRefID",
                        column: x => x.CVRefID,
                        principalTable: "CVRefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Travels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProcessId = table.Column<int>(nullable: false),
                    CVRefId = table.Column<int>(nullable: false),
                    BoardingAirport = table.Column<string>(nullable: true),
                    DestinationAirport = table.Column<string>(nullable: true),
                    Airline = table.Column<string>(nullable: true),
                    FlightNo = table.Column<string>(nullable: true),
                    ETD = table.Column<DateTime>(nullable: false),
                    ETA = table.Column<DateTime>(nullable: false),
                    BookedOn = table.Column<DateTime>(nullable: false),
                    PNR = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Travels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Travels_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItems_AssessmentId",
                table: "AssessmentItems",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItems_QuestionNo_AssessmentId",
                table: "AssessmentItems",
                columns: new[] { "QuestionNo", "AssessmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQs_EnquiryItemId",
                table: "AssessmentQs",
                column: "EnquiryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQsBank_AssessmentParameter",
                table: "AssessmentQsBank",
                column: "AssessmentParameter");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_CandidateId",
                table: "Assessments",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_EnquiryItemId",
                table: "Assessments",
                column: "EnquiryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_EnquiryItemId_CandidateId",
                table: "Assessments",
                columns: new[] { "EnquiryItemId", "CandidateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CandidateId",
                table: "Attachments",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateCategories_CandidateId",
                table: "CandidateCategories",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateCategories_CategoryId_CandidateId",
                table: "CandidateCategories",
                columns: new[] { "CategoryId", "CandidateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ApplicationNo",
                table: "Candidates",
                column: "ApplicationNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_LastStatusUpdatedById",
                table: "Candidates",
                column: "LastStatusUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_PassportNo",
                table: "Candidates",
                column: "PassportNo",
                unique: true);

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
                name: "IX_ContractReviewItems_EnquiryItemId",
                table: "ContractReviewItems",
                column: "EnquiryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIndustryTypes_CustomerId1",
                table: "CustomerIndustryTypes",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerIndustryTypes_CustomerId_IndustryTypeId",
                table: "CustomerIndustryTypes",
                columns: new[] { "CustomerId", "IndustryTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOfficials_CustomerId",
                table: "CustomerOfficials",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOfficials_CustomerId1",
                table: "CustomerOfficials",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CVEvaluations_CandidateId",
                table: "CVEvaluations",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CVEvaluations_EnquiryItemId",
                table: "CVEvaluations",
                column: "EnquiryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CVForwardItems_CVForwardId",
                table: "CVForwardItems",
                column: "CVForwardId");

            migrationBuilder.CreateIndex(
                name: "IX_CVForwards_CustomerId",
                table: "CVForwards",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_CandidateId",
                table: "CVRefs",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CVRefs_EnquiryItemId",
                table: "CVRefs",
                column: "EnquiryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DLForwardToHR_EnquiryId",
                table: "DLForwardToHR",
                column: "EnquiryId");

            migrationBuilder.CreateIndex(
                name: "IX_DLForwardToHR_ToDoId",
                table: "DLForwardToHR",
                column: "ToDoId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainSub_DomainSubName",
                table: "DomainSub",
                column: "DomainSubName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emoluments_CVRefId",
                table: "Emoluments",
                column: "CVRefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_EmployeeId",
                table: "EmployeeAddresses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_AccountExecutiveId",
                table: "Enquiries",
                column: "AccountExecutiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiries_CustomerId",
                table: "Enquiries",
                column: "CustomerId");

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
                name: "IX_EnquiryForwards_CustomerId",
                table: "EnquiryForwards",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryForwards_CustomerOfficialId",
                table: "EnquiryForwards",
                column: "CustomerOfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItemAssessmentQs_DomainSubId",
                table: "EnquiryItemAssessmentQs",
                column: "DomainSubId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItemForwarded_EnquiryForwardedId",
                table: "EnquiryItemForwarded",
                column: "EnquiryForwardedId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_AssessingHRExecId",
                table: "EnquiryItems",
                column: "AssessingHRExecId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_AssessingHRMId",
                table: "EnquiryItems",
                column: "AssessingHRMId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_AssessingSupId",
                table: "EnquiryItems",
                column: "AssessingSupId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_CandidateId",
                table: "EnquiryItems",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryItems_EnquiryId_CategoryItemId",
                table: "EnquiryItems",
                columns: new[] { "EnquiryId", "CategoryItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HRSkillClaims_EmployeeId",
                table: "HRSkillClaims",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HRSkillClaims_IndustryTypeId",
                table: "HRSkillClaims",
                column: "IndustryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HRSkillClaims_SkillLevelId",
                table: "HRSkillClaims",
                column: "SkillLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_IndustryTypes_Name",
                table: "IndustryTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCards_EnquiryItemId_CandidateId",
                table: "JobCards",
                columns: new[] { "EnquiryItemId", "CandidateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobDescriptions_EnquiryItemId",
                table: "JobDescriptions",
                column: "EnquiryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Processes_CVRefId",
                table: "Processes",
                column: "CVRefId");

            migrationBuilder.CreateIndex(
                name: "IX_Processes_CVRefId_Status",
                table: "Processes",
                columns: new[] { "CVRefId", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Remunerations_EnquiryItemId",
                table: "Remunerations",
                column: "EnquiryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_EmployeeId",
                table: "Roles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SelDecisions_CVRefID",
                table: "SelDecisions",
                column: "CVRefID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillLevels_Name",
                table: "SkillLevels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_EmployeeId",
                table: "Skills",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SourceGroups_Name",
                table: "SourceGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_SourceGroupId_Name",
                table: "Sources",
                columns: new[] { "SourceGroupId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_TaskId",
                table: "TaskItems",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_ToDoId",
                table: "TaskItems",
                column: "ToDoId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_AssignedToId",
                table: "ToDos",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_OwnerId",
                table: "ToDos",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ToDos_TaskType",
                table: "ToDos",
                column: "TaskType");

            migrationBuilder.CreateIndex(
                name: "IX_Travels_ProcessId",
                table: "Travels",
                column: "ProcessId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentItems");

            migrationBuilder.DropTable(
                name: "AssessmentQs");

            migrationBuilder.DropTable(
                name: "AssessmentQsBank");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "CandidateCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ContractReviewItems");

            migrationBuilder.DropTable(
                name: "CustomerIndustryTypes");

            migrationBuilder.DropTable(
                name: "CVEvaluations");

            migrationBuilder.DropTable(
                name: "CVForwardItems");

            migrationBuilder.DropTable(
                name: "DeliveryMethods");

            migrationBuilder.DropTable(
                name: "DLForwardToHR");

            migrationBuilder.DropTable(
                name: "Emoluments");

            migrationBuilder.DropTable(
                name: "EmployeeAddresses");

            migrationBuilder.DropTable(
                name: "EnquiryItemAssessmentQs");

            migrationBuilder.DropTable(
                name: "EnquiryItemForwarded");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "HRSkillClaims");

            migrationBuilder.DropTable(
                name: "JobCards");

            migrationBuilder.DropTable(
                name: "JobDescriptions");

            migrationBuilder.DropTable(
                name: "ProcessStatuses");

            migrationBuilder.DropTable(
                name: "Remunerations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SelDecisions");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "SourceGroups");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Travels");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "CVForwards");

            migrationBuilder.DropTable(
                name: "DomainSub");

            migrationBuilder.DropTable(
                name: "EnquiryForwards");

            migrationBuilder.DropTable(
                name: "IndustryTypes");

            migrationBuilder.DropTable(
                name: "SkillLevels");

            migrationBuilder.DropTable(
                name: "ToDos");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropTable(
                name: "CVRefs");

            migrationBuilder.DropTable(
                name: "EnquiryItems");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Enquiries");

            migrationBuilder.DropTable(
                name: "CustomerOfficials");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
