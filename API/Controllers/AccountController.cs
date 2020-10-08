using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.Identity;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _appContext;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<AppUser> userManager, AppIdentityDbContext appContext, 
                SignInManager<AppUser> signinManager, ITokenService tokenService,
                IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _signinManager = signinManager;
            _userManager = userManager;
            _appContext = appContext;
        }

        [Authorize]
        [HttpGet]
        // [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(HttpContext.User);
            if (user==null) return NotFound(new ApiResponse(404, "User not found"));
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                //EmployeeId=user.EmployeeId
            };
        }


        // Usermanager is derived from Microsoft.Identity. Usermanager has
        // built in function FindByEmailAsync.
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signinManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse((401)));
            
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
            
        }

    /*
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register(AppUser appUser)
        {

            if (appUser.Address.CustomerId == 0)
            {
                var add = new CustomerAddress
                {
                    CustomerId = appUser.Address.CustomerId, 
                    Address1=appUser.Address.Street,  
                    City=appUser.Address.City,  
                    PIN=appUser.Address.Zipcode,
                    State=appUser.Address.State, 
                    Country=appUser.Address.Country
                };

                var cust = new Customer (appUser.Address.CustomerType, appUser.Address.CompanyName, 
                    appUser.DisplayName, appUser.Address.City, appUser.Email, 
                    appUser.Address.Mobile, appUser.Address.IntroducedBy, add);     

                var custm = await _unitOfWork.Repository<Customer>().AddAsync(cust);

                appUser.Address.CustomerId = cust.Id;
            }
        
            var result = await _userManager.CreateAsync(appUser, appUser.Address.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser),
                EmployeeId = appUser.Address.EmployeeId,
                UserType = Enum.GetName(typeof(enumCustomerType), appUser.Address.CustomerType)
            };
        }
    */

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (registerDto.CustomerId == 0)
            {
                // add a new customer
                var cust = new Customer (registerDto.CustomerType, registerDto.CompanyName, 
                    registerDto.DisplayName, registerDto.City, registerDto.Email, 
                    registerDto.Mobile, registerDto.IntroducedBy);  //, add);     
                var custm = await _unitOfWork.Repository<Customer>().AddAsync(cust);
                registerDto.CustomerId = custm.Id;
                
                // append the customeraddress
                var custAddress = new CustomerAddress(
                    custm.Id, registerDto.AddressType, registerDto.Address1, registerDto.Address2,
                    registerDto.City, registerDto.PIN, registerDto.State, registerDto.District, 
                    registerDto.Country);
                await _unitOfWork.Repository<CustomerAddress>().AddAsync(custAddress);
            } 

            // check if AppUser exists
            var appUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (appUser != null) return BadRequest(new ApiResponse(400, "User already exists"));
            
            var appAdd = new Address{
                FirstName = registerDto.FirstName, LastName = registerDto.FirstName,
                Street= registerDto.Street, City = registerDto.City,
                State = registerDto.State, Country = registerDto.Country,
                Zipcode = registerDto.PIN, EmployeeId = registerDto.EmployeeId,
                CustomerId = registerDto.CustomerId, CompanyName = registerDto.CompanyName,
                OfficialGender = registerDto.Gender, 
                OfficialName = registerDto.FirstName + " " + registerDto.FamilyName,
                OfficialDesignation = registerDto.Designation, IntroducedBy = registerDto.IntroducedBy
            };

            appUser = new AppUser{ Email = registerDto.Email, UserName = registerDto.UserName, 
                PhoneNumber = registerDto.Mobile, DisplayName = registerDto.DisplayName, Address = appAdd };

            var result = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "failed to register the ApplicationUser"));

            return new UserDto
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser),
                EmployeeId = appUser.Address.EmployeeId,
                UserType = Enum.GetName(typeof(enumCustomerType), appUser.Address.CustomerType)
            };
        }


        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressByClaimsPrincipal(HttpContext.User);
            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        /*
        [Authorize]
        [HttpGet("getAllusers")]
        public async Task<IReadOnlyList<AppUser>> GetUsers()
        {
            
        }
        */ 
        
        [Authorize]
        [HttpPut("address")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUserWithAddressByClaimsPrincipal(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("problem updating user address");

        }
    }
}
