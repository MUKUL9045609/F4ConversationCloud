using F4ConversationCloud.Application.Common.Helper;
using F4ConversationCloud.Domain.Entities.SuperAdmin;
using F4ConversationCloud.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F4ConversationCloud.Application.Common.Models.OnBoardingRequestResposeModel
{
    public class RegisterUserModel
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? OTP { get; }
        public string PhoneNumber { get; set; }
        public string? FullPhoneNumber { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string PassWord { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; } = true;
        public bool TermsCondition { get; set; }
        public bool EmailOtpVerified { get; set; } = false;
        public ClientRole Role { get; set; }
        public string ClientRoles { get; set; }
        public string Timezone { get; set; }
        public ClientFormStage Stage { get; set; }
        public int UserId { get; set; }
        public string ClientId { get; set; }
        public string CityId { get; set; }
        public string StateId { get; set; }
        public string ZipCode { get; set; }
        public string OptionalAddress { get; set; }
        public string? OrganizationsName { get; set; }
        public ClientRegistrationStatus RegistrationStatus { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string BusinessId { get; set; } = string.Empty;
        public string ClientInfoId { get; set; } = string.Empty;

        public IEnumerable<TimeZoneResponse> TimeZones { get; set; } = new List<TimeZoneResponse>();

    }
    
    public class UserDetailsViewModel
    {
        public string FirstName { get; }
        public string LastName { get;  }
        public string Email { get;  }
        public string PhoneNumber { get;  }
        public string Address { get;  }
        public string Country { get;  }
        public string TimeZone { get;  }
        public string Role { get; }
        public bool TermsCondition { get; set; }
        public ClientFormStage Stage { get; }
        public int UserId { get; set; }

    }
    public class RegisterUserResponse
    {

        public int NewUserId { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class Loginrequest {
        public string Email { get; set; }
        public string PassWord { get; set; }
    }

    public class LoginViewModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }        
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ClientFormStage Stage { get; set; }
        public string Password { get; set; }

    }
    public class LoginResponse
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public LoginViewModel Data { get; set; }
    }

    public class TimeZoneResponse {

        public string name { get; set; }
        public string current_utc_offset { get; set; }
    }
    public class Cities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateId { get; set; }
    }
    public class States
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}

