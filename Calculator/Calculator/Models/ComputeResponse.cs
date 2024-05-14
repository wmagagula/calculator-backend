using Calculator.Engine.Dto;

namespace Calculator.WebApi.Models;

public record ComputeResponse(bool IsSuccess, string Message, double? ComputeResult, ExpressionDto? ExpressionDto, DateTime? Date);
