using Calculator.Engine.Enums;
using System.ComponentModel.DataAnnotations;

namespace Calculator.Engine.Dto;

public record ExpressionDto([Required] double? FirstOperand, double? SecondOperand, [Required] Operators? Operator);
