using AutoMapper;
using DemoProject.Application.Mediator;
using DemoProject.Domain.Entities;
using System.Text.Json;

namespace DemoProject.Application.DTOs
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Define our mappings between entities and DTOs here...
        /// </summary>
        public MappingProfile()
        {
            CreateMap<FreeServiceCompany, FreeThirdPartyResponseDTO>();
            CreateMap<PremiumServiceCompany, PremiumThirdPartyResponseDTO>();
            CreateMap<VerificationData, VerificationResponseDTO>();
            CreateMap<VerificationResponseDTO, VerificationData>();

            // custom mapping for a specific case...
            CreateMap<BackendServiceResponse, VerificationResponseDTO>()
                .ForMember(dest => dest.QueryText, opt => opt.MapFrom(src => src.Query))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.Result, opt => opt.MapFrom(src =>
                    SafeSerializeToJson(
                        src.OtherResults != null && src.OtherResults.Any()
                        ? new List<object> { src.Result }.Concat(src.OtherResults).ToList()
                        : src.Result)))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.VerificationId, opt => opt.MapFrom(src => src.VerificationId));
        }

        private static string SafeSerializeToJson(object data)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = false,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                return JsonSerializer.Serialize(data, jsonOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON Serialization Error: {ex.Message}");
                return "{\"error\": \"Failed to serialize result\"}"; 
            }
        }
    }
}
