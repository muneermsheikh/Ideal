using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ReviewedBy = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractReviewItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddresses",
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
                    table.PrimaryKey("PK_CustomerAddresses", x => x.Id);
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
                name: "DomainSubs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DomainSubName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainSubs", x => x.Id);
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
                    PPNo = table.Column<string>(nullable: true),
                    AadharNo = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Designation = table.Column<string>(nullable: false),
                    DOJ = table.Column<DateTime>(nullable: false),
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
                name: "Processings",
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
                    table.PrimaryKey("PK_Processings", x => x.Id);
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
                    SourceGroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerType = table.Column<string>(nullable: false),
                    CustomerName = table.Column<string>(maxLength: 100, nullable: false),
                    KnownAs = table.Column<string>(maxLength: 15, nullable: false),
                    CityName = table.Column<string>(maxLength: 50, nullable: false),
                    IntroducedBy = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    CompanyUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CustomerStatus = table.Column<string>(nullable: false),
                    CustomerAddressId1 = table.Column<int>(nullable: true),
                    CustomerAddressId = table.Column<int>(nullable: true),
                    CustomerOfficialId = table.Column<int>(nullable: true),
                    AddedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerAddresses_CustomerAddressId1",
                        column: x => x.CustomerAddressId1,
                        principalTable: "CustomerAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentQsBank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SrNo = table.Column<int>(nullable: false),
                    DomainSubId = table.Column<int>(nullable: false),
                    IsStandardQuestion = table.Column<bool>(nullable: false),
                    AssessmentParameter = table.Column<string>(nullable: false),
                    Question = table.Column<string>(nullable: false),
                    MaxPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentQsBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentQsBank_DomainSubs_DomainSubId",
                        column: x => x.DomainSubId,
                        principalTable: "DomainSubs",
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
                        name: "FK_EnquiryItemAssessmentQs_DomainSubs_DomainSubId",
                        column: x => x.DomainSubId,
                        principalTable: "DomainSubs",
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
                    KnownAs = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: false),
                    PPNo = table.Column<string>(nullable: false),
                    AadharNo = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
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
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidates_Employees_LastStatusUpdatedById",
                        column: x => x.LastStatusUpdatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddress",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressType = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PIN = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    District = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Valid = table.Column<bool>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAddress_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    email = table.Column<string>(nullable: false),
                    PersonalEmail = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false),
                    AddedOn = table.Column<DateTime>(nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Grades_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
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
                        name: "FK_IndustryTypes_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
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
                        name: "FK_Attachment_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
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
                        name: "FK_DLForwardToHR_ToDos_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    ProjectManagerId = table.Column<int>(nullable: true),
                    AccountExecutiveId = table.Column<int>(nullable: true),
                    HRExecutiveId = table.Column<int>(nullable: true),
                    LogisticsExecutiveId = table.Column<int>(nullable: true),
                    EnquiryRef = table.Column<string>(nullable: true),
                    CompleteBy = table.Column<DateTime>(nullable: true),
                    ReviewedById = table.Column<int>(nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enquiries_Employees_ReviewedById",
                        column: x => x.ReviewedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_CVSource_EnquiryForwards_EnquiryForwardedId",
                        column: x => x.EnquiryForwardedId,
                        principalTable: "EnquiryForwards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVSource_Customers_ReceivedFromAssociateId",
                        column: x => x.ReceivedFromAssociateId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVSource_Candidates_ReceivedFromCandidateId",
                        column: x => x.ReceivedFromCandidateId,
                        principalTable: "Candidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CVSource_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 75, nullable: false),
                    IndustryTypeId = table.Column<int>(nullable: false),
                    SkillLevelId = table.Column<int>(nullable: false),
                    CandidateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
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
                    ECNR = table.Column<bool>(nullable: false),
                    HRExecutiveId = table.Column<int>(nullable: true),
                    AssessingHRExecId = table.Column<int>(nullable: true),
                    AssessingSupId = table.Column<int>(nullable: true),
                    AssessingHRMId = table.Column<int>(nullable: true),
                    CompleteBy = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    CVSourceId = table.Column<int>(nullable: true),
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
                        name: "FK_EnquiryItems_CVSource_CVSourceId",
                        column: x => x.CVSourceId,
                        principalTable: "CVSource",
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
                name: "Assessment",
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
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assessment_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
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
                name: "AssessmentQs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssessmentId = table.Column<int>(nullable: false),
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
                    EnquiryId = table.Column<int>(nullable: false),
                    EnquiryItemId = table.Column<int>(nullable: false),
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
                name: "AssessmentItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssessmentId = table.Column<int>(nullable: false),
                    Assessed = table.Column<bool>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    QuestionNo = table.Column<int>(nullable: false),
                    DomainSubject = table.Column<string>(nullable: true),
                    AssessmentParameter = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    MaxPoints = table.Column<int>(nullable: false),
                    PointsAllotted = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentItem_Assessment_AssessmentId",
                        column: x => x.AssessmentId,
                        principalTable: "Assessment",
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
                name: "IX_AssessmentItem_AssessmentId",
                table: "AssessmentItem",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentItem_QuestionNo_AssessmentId",
                table: "AssessmentItem",
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
                name: "IX_AssessmentQsBank_DomainSubId_SrNo",
                table: "AssessmentQsBank",
                columns: new[] { "DomainSubId", "SrNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_CandidateId",
                table: "Attachment",
                column: "CandidateId");

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
                name: "IX_Candidates_PPNo",
                table: "Candidates",
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
                name: "IX_ContractReviewItems_EnquiryItemId",
                table: "ContractReviewItems",
                column: "EnquiryItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOfficials_CustomerId",
                table: "CustomerOfficials",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerAddressId1",
                table: "Customers",
                column: "CustomerAddressId1");

            migrationBuilder.CreateIndex(
                name: "IX_CVEvaluations_CandidateId",
                table: "CVEvaluations",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CVEvaluations_EnquiryItemId",
                table: "CVEvaluations",
                column: "EnquiryItemId");

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
                name: "IX_DLForwardToHR_ToDoId",
                table: "DLForwardToHR",
                column: "ToDoId");

            migrationBuilder.CreateIndex(
                name: "IX_DomainSubs_DomainSubName",
                table: "DomainSubs",
                column: "DomainSubName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddress_EmployeeId",
                table: "EmployeeAddress",
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
                name: "IX_EnquiryItems_CVSourceId",
                table: "EnquiryItems",
                column: "CVSourceId");

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
                name: "IX_Grades_CustomerId",
                table: "Grades",
                column: "CustomerId",
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
                name: "IX_IndustryTypes_CustomerId",
                table: "IndustryTypes",
                column: "CustomerId");

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
                name: "IX_Processings_CVRefId",
                table: "Processings",
                column: "CVRefId");

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
                name: "IX_SkillLevels_Name",
                table: "SkillLevels",
                column: "Name",
                unique: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentItem");

            migrationBuilder.DropTable(
                name: "AssessmentQs");

            migrationBuilder.DropTable(
                name: "AssessmentQsBank");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ContractReviewItems");

            migrationBuilder.DropTable(
                name: "CVEvaluations");

            migrationBuilder.DropTable(
                name: "DeliveryMethods");

            migrationBuilder.DropTable(
                name: "DLForwardToHR");

            migrationBuilder.DropTable(
                name: "EmployeeAddress");

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
                name: "Processings");

            migrationBuilder.DropTable(
                name: "Remunerations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SourceGroups");

            migrationBuilder.DropTable(
                name: "TaskItems");

            migrationBuilder.DropTable(
                name: "Assessment");

            migrationBuilder.DropTable(
                name: "DomainSubs");

            migrationBuilder.DropTable(
                name: "IndustryTypes");

            migrationBuilder.DropTable(
                name: "SkillLevels");

            migrationBuilder.DropTable(
                name: "ToDos");

            migrationBuilder.DropTable(
                name: "EnquiryItems");

            migrationBuilder.DropTable(
                name: "CVSource");

            migrationBuilder.DropTable(
                name: "Enquiries");

            migrationBuilder.DropTable(
                name: "EnquiryForwards");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "CustomerOfficials");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CustomerAddresses");
        }
    }
}
